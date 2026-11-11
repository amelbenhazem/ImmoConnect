using ImmoConnect_BE.Data;
using ImmoConnect_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace ImmoConnect_BE.Repositories
{
    public class UtilisateurRepository : IUtilisateurRepository
    {
        private readonly ApplicationDbContext _context;

        public UtilisateurRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Utilisateur> AjouterAsync(Utilisateur utilisateur)
        {
            _context.Utilisateurs.Add(utilisateur);
            await _context.SaveChangesAsync();
            return utilisateur;
        }

        public async Task<Utilisateur> ObtenirParEmailAsync(string email)
        {
            return await _context.Utilisateurs.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<Utilisateur> ObtenirParIdAsync(int id)
        {
            return await _context.Utilisateurs.FindAsync(id);
        }

        public async Task<Utilisateur> MettreAJourAsync(Utilisateur utilisateur)
        {
            _context.Utilisateurs.Update(utilisateur);
            await _context.SaveChangesAsync();
            return utilisateur;
        }
        public async Task<List<Utilisateur>> ObtenirTousAsync()
        {
            return await _context.Utilisateurs.ToListAsync();
        }

    }
}
