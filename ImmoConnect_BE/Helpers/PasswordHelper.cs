using ImmoConnect_BE.Models;

namespace ImmoConnect_BE.Helpers
{
    public static class PasswordHelper
    {
        public static string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool Verify(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(storedHash) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            if (storedHash.StartsWith("$2"))
            {
                return BCrypt.Net.BCrypt.Verify(password, storedHash);
            }

            // Anciens comptes en clair (avant unifié BCrypt)
            return storedHash == password;
        }

        public static UtilisateurResponseDto ToPublic(Utilisateur user)
        {
            return new UtilisateurResponseDto
            {
                Id = user.Id,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }
    }

    public class UtilisateurResponseDto
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
