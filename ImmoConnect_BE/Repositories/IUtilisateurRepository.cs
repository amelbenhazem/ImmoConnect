using ImmoConnect_BE.Models;

namespace ImmoConnect_BE.Repositories
{
    public interface IUtilisateurRepository
    {
        Task<Utilisateur> AjouterAsync(Utilisateur utilisateur);
        Task<Utilisateur> ObtenirParEmailAsync(string email);
        Task<Utilisateur> ObtenirParIdAsync(int id);
        Task<Utilisateur> MettreAJourAsync(Utilisateur utilisateur);
        Task<List<Utilisateur>> ObtenirTousAsync();

    }
}
