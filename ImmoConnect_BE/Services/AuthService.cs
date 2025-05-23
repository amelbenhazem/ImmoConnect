using System;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ImmoConnect_BE.Models;
using ImmoConnect_BE.ViewModels;
using ImmoConnect_BE.Data;

namespace ImmoConnect_BE.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<Utilisateur> _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(
            ApplicationDbContext context, 
            IPasswordHasher<Utilisateur> passwordHasher,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthResult> SignInAsync(string email, string password, bool rememberMe)
        {
            var user = await _context.Utilisateurs
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return new AuthResult { Succeeded = false, Errors = new[] { "Utilisateur non trouvé" } };
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Mot_De_Passe, password);
            if (result == PasswordVerificationResult.Failed)
            {
                return new AuthResult { Succeeded = false, Errors = new[] { "Mot de passe incorrect" } };
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = rememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return new AuthResult { Succeeded = true };
        }

        public async Task<AuthResult> SignUpAsync(SignUpViewModel model)
        {
            if (await _context.Utilisateurs.AnyAsync(u => u.Email == model.Email))
            {
                return new AuthResult { Succeeded = false, Errors = new[] { "Cet email est déjà utilisé" } };
            }

            var user = new Utilisateur
            {
                Email = model.Email,
                Nom = model.Nom,
                Prenom = model.Prenom,
                Role = Role.Client
            };

            user.Mot_De_Passe = _passwordHasher.HashPassword(user, model.Password);

            _context.Utilisateurs.Add(user);
            await _context.SaveChangesAsync();

            return new AuthResult { Succeeded = true };
        }

        public async Task SignOutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
} 