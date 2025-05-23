using System.ComponentModel.DataAnnotations;

namespace ImmoConnect_BE.ViewModel
{
    public class LoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Mot_De_Passe { get; set; }
    }
}
