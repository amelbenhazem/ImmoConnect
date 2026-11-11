using ImmoConnect_BE.Helpers;
using ImmoConnect_BE.ViewModel;
using ImmoConnect_BE.ViewModels;

namespace ImmoConnect_BE.Services
{
    public interface ICompteService
    {
        Task<UtilisateurResponseDto> RegisterAsync(UtilisateurDto utilisateurDto);
        Task<UtilisateurResponseDto> LoginAsync(LoginDto loginDto);
        Task<UtilisateurResponseDto> UpdateUtilisateurAsync(int id, UtilisateurDto dto);
        Task<List<UtilisateurResponseDto>> ObtenirTousAsync();
        Task<UtilisateurResponseDto?> ObtenirParIdAsync(int id);
    }
}
