using ImmoConnect_BE.Models;
using ImmoConnect_BE.ViewModels;
using ImmoConnect_BE.ViewModel;

namespace ImmoConnect_BE.Services
{
    public interface ICompteService
    {
        Task<Utilisateur> RegisterAsync(UtilisateurDto utilisateurDto);
        Task<Utilisateur> LoginAsync(LoginDto loginDto);
        Task<Utilisateur> UpdateUtilisateurAsync(int id, UtilisateurDto dto);
        Task<List<Utilisateur>> ObtenirTousAsync();
        Task<Utilisateur> ObtenirParIdAsync(int id);
    }
}
