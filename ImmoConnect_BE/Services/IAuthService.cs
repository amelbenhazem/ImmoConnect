using System.Threading.Tasks;
using ImmoConnect_BE.ViewModels;

namespace ImmoConnect_BE.Services
{
    public interface IAuthService
    {
        Task<AuthResult> SignInAsync(string email, string password, bool rememberMe);
        Task<AuthResult> SignUpAsync(SignUpViewModel model);
        Task SignOutAsync();
    }

    public class AuthResult
    {
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
    }
} 