using ImmoConnect_BE.Models;
using System.ComponentModel.DataAnnotations;

namespace ImmoConnect_BE.ViewModels
{
    public class UtilisateurDto
    {
        [Required]
        public string Nom { get; set; }

        [Required]
        public string Prenom { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Mot_De_Passe { get; set; }

        public Role Role { get; set; } = Role.Client;
    }
}

