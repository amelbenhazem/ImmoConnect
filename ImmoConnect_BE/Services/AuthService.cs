using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ImmoConnect_BE.Models;
using ImmoConnect_BE.ViewModels;
using ImmoConnect_BE.Data;
using ImmoConnect_BE.Helpers;

namespace ImmoConnect_BE.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthResult> SignInAsync(string email, string password, bool rememberMe)
        {
            var user = await _context.Utilisateurs
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !PasswordHelper.Verify(password, user.Mot_De_Passe))
            {
                return new AuthResult { Succeeded = false, Errors = new[] { "Tentative de connexion invalide" } };
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.GivenName, user.Prenom ?? string.Empty),
                new Claim(ClaimTypes.Surname, user.Nom ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = rememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            await _httpContextAccessor.HttpContext!.SignInAsync(
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
                Role = Role.Client,
                Mot_De_Passe = PasswordHelper.Hash(model.Password)
            };

            _context.Utilisateurs.Add(user);
            await _context.SaveChangesAsync();

            return new AuthResult { Succeeded = true };
        }

        public async Task SignOutAsync()
        {
            await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
