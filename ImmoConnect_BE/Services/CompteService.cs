using ImmoConnect_BE.Models;
using ImmoConnect_BE.Models;
using ImmoConnect_BE.Repositories;
using ImmoConnect_BE.Repositories;
using ImmoConnect_BE.ViewModel;
using ImmoConnect_BE.ViewModels;
using ImmoConnect_BE.ViewModels;

namespace ImmoConnect_BE.Services
{
    public class CompteService : ICompteService
    {
        private readonly IUtilisateurRepository _repo;

        public CompteService(IUtilisateurRepository repo)
        {
            _repo = repo;
        }

        public async Task<Utilisateur> RegisterAsync(UtilisateurDto dto)
        {
            var existingUser = await _repo.ObtenirParEmailAsync(dto.Email);
            if (existingUser != null)
                throw new Exception("Cet email est déjà utilisé.");

            var utilisateur = new Utilisateur
            {
                Nom = dto.Nom,
                Prenom = dto.Prenom,
                Email = dto.Email,
                Mot_De_Passe = dto.Mot_De_Passe, // 🔐 À chiffrer avec BCrypt idéalement
                Role = dto.Role
            };

            return await _repo.AjouterAsync(utilisateur);
        }
        public async Task<Utilisateur> LoginAsync(LoginDto dto)
        {
            var utilisateur = await _repo.ObtenirParEmailAsync(dto.Email);
            if (utilisateur == null || utilisateur.Mot_De_Passe != dto.Mot_De_Passe)
            {
                throw new Exception("Email ou mot de passe incorrect.");
            }

            return utilisateur;
        }
        public async Task<Utilisateur> UpdateUtilisateurAsync(int id, UtilisateurDto dto)
        {
            var utilisateur = await _repo.ObtenirParIdAsync(id);
            if (utilisateur == null)
                throw new Exception("Utilisateur non trouvé.");

            utilisateur.Nom = dto.Nom;
            utilisateur.Prenom = dto.Prenom;
            utilisateur.Email = dto.Email;
            utilisateur.Mot_De_Passe = dto.Mot_De_Passe;
            utilisateur.Role = dto.Role;

            return await _repo.MettreAJourAsync(utilisateur);
        }
        public async Task<List<Utilisateur>> ObtenirTousAsync()
        {
            return await _repo.ObtenirTousAsync();
        }

        public async Task<Utilisateur> ObtenirParIdAsync(int id)
        {
            return await _repo.ObtenirParIdAsync(id);
        }


    }
}

