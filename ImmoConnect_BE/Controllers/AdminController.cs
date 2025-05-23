using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ImmoConnect_BE.Data;
using ImmoConnect_BE.Models;
using ImmoConnect_BE.ViewModel;
using System.Linq;
using System.Threading.Tasks;

namespace ImmoConnect_BE.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            var dashboard = new AdminDashboardViewModel
            {
                TotalUsers = await _context.Utilisateurs.CountAsync(),
                TotalProperties = await _context.BiensImmobiliers.CountAsync(),
                TotalCategories = await _context.Categories.CountAsync(),
                TotalVisitRequests = await _context.DemandesVisite.CountAsync(),
                RecentProperties = await _context.BiensImmobiliers
                    .Include(b => b.Categorie)
                    .Include(b => b.Utilisateur)
                    .OrderByDescending(b => b.Date_ajout)
                    .Take(5)
                    .ToListAsync(),
                RecentVisitRequests = await _context.DemandesVisite
                    .Include(d => d.BienImmobilier)
                    .Select(d => new DemandeVisite
                    {
                        Id_demande = d.Id_demande,
                        Nom_client = d.Nom_client,
                        Email_client = d.Email_client,
                        Date_souhaitee = d.Date_souhaitee,
                        Message = d.Message,
                        Date_demande = d.Date_demande,
                        Statut = d.Statut,
                        Id_bien = d.Id_bien,
                        BienImmobilier = d.BienImmobilier
                    })
                    .OrderByDescending(d => d.Date_demande)
                    .Take(5)
                    .ToListAsync()
            };

            return View(dashboard);
        }

        // GET: Admin/Users
        public async Task<IActionResult> Users()
        {
            var users = await _context.Utilisateurs.ToListAsync();
            return View(users);
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Categories()
        {
            var categories = await _context.Categories
                .Include(c => c.BiensImmobiliers)
                .ToListAsync();
            return View(categories);
        }

        // GET: Admin/Properties
        public async Task<IActionResult> Properties()
        {
            var properties = await _context.BiensImmobiliers
                .Include(b => b.Categorie)
                .Include(b => b.Utilisateur)
                .ToListAsync();
            return View(properties);
        }

        // GET: Admin/VisitRequests
        public async Task<IActionResult> VisitRequests()
        {
            var visitRequests = await _context.DemandesVisite
                .Include(d => d.BienImmobilier)
                .ToListAsync();
            return View(visitRequests);
        }

        // POST: Admin/DeleteUser/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Utilisateurs.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Utilisateurs.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Users));
        }

        // POST: Admin/DeleteCategory/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Categories));
        }

        // POST: Admin/DeleteProperty/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            var property = await _context.BiensImmobiliers.FindAsync(id);
            if (property == null)
            {
                return NotFound();
            }

            _context.BiensImmobiliers.Remove(property);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Properties));
        }

        // POST: Admin/DeleteVisitRequest/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVisitRequest(int id)
        {
            var visitRequest = await _context.DemandesVisite.FindAsync(id);
            if (visitRequest == null)
            {
                return NotFound();
            }

            _context.DemandesVisite.Remove(visitRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(VisitRequests));
        }
    }
} 