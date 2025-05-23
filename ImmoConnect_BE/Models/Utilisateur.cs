using System.ComponentModel.DataAnnotations;

namespace ImmoConnect_BE.Models
{
    public enum Role
    {
        Admin,
        Client
    }

    public class Utilisateur
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nom { get; set; }

        [Required]
        public string Prenom { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Mot_De_Passe { get; set; }

        [Required]
        public Role Role { get; set; }
    }
}
