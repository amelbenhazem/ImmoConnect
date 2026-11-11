using ImmoConnect_BE.Helpers;
using ImmoConnect_BE.Models;
using ImmoConnect_BE.Repositories;
using ImmoConnect_BE.ViewModel;
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

        public async Task<UtilisateurResponseDto> RegisterAsync(UtilisateurDto dto)
        {
            var existingUser = await _repo.ObtenirParEmailAsync(dto.Email);
            if (existingUser != null)
                throw new Exception("Cet email est déjà utilisé.");

            var utilisateur = new Utilisateur
            {
                Nom = dto.Nom,
                Prenom = dto.Prenom,
                Email = dto.Email,
                Mot_De_Passe = PasswordHelper.Hash(dto.Mot_De_Passe),
                Role = Role.Client
            };

            var created = await _repo.AjouterAsync(utilisateur);
            return PasswordHelper.ToPublic(created);
        }

        public async Task<UtilisateurResponseDto> LoginAsync(LoginDto dto)
        {
            var utilisateur = await _repo.ObtenirParEmailAsync(dto.Email);
            if (utilisateur == null || !PasswordHelper.Verify(dto.Mot_De_Passe, utilisateur.Mot_De_Passe))
            {
                throw new Exception("Email ou mot de passe incorrect.");
            }

            return PasswordHelper.ToPublic(utilisateur);
        }

        public async Task<UtilisateurResponseDto> UpdateUtilisateurAsync(int id, UtilisateurDto dto)
        {
            var utilisateur = await _repo.ObtenirParIdAsync(id);
            if (utilisateur == null)
                throw new Exception("Utilisateur non trouvé.");

            utilisateur.Nom = dto.Nom;
            utilisateur.Prenom = dto.Prenom;
            utilisateur.Email = dto.Email;
            if (!string.IsNullOrWhiteSpace(dto.Mot_De_Passe))
            {
                utilisateur.Mot_De_Passe = PasswordHelper.Hash(dto.Mot_De_Passe);
            }

            var updated = await _repo.MettreAJourAsync(utilisateur);
            return PasswordHelper.ToPublic(updated);
        }

        public async Task<List<UtilisateurResponseDto>> ObtenirTousAsync()
        {
            var users = await _repo.ObtenirTousAsync();
            return users.Select(PasswordHelper.ToPublic).ToList();
        }

        public async Task<UtilisateurResponseDto?> ObtenirParIdAsync(int id)
        {
            var user = await _repo.ObtenirParIdAsync(id);
            return user == null ? null : PasswordHelper.ToPublic(user);
        }
    }
}
