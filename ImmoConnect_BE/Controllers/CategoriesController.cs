using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ImmoConnect_BE.Data;
using ImmoConnect_BE.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ImmoConnect_BE.Controllers
{
    [Authorize(Roles = "Admin")] // Assuming only Admin can manage categories
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.Include(c => c.BiensImmobiliers).ToListAsync();
            return View(categories);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorie = await _context.Categories
                .Include(c => c.BiensImmobiliers)
                .FirstOrDefaultAsync(m => m.Id_categorie == id);

            if (categorie == null)
            {
                return NotFound();
            }

            return View(categorie);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id_categorie,Nom,Description")] Categorie categorie)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(categorie);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log the exception for debugging
                    // _logger.LogError(ex, "Error creating category");
                    ModelState.AddModelError("", "Une erreur est survenue lors de la création de la catégorie. Veuillez réessayer.");
                }
            }
             else
            {
                 ModelState.AddModelError("", "Veuillez corriger les erreurs de validation.");
            }

            return View(categorie);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorie = await _context.Categories.FindAsync(id);
            if (categorie == null)
            {
                return NotFound();
            }
            return View(categorie);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_categorie,Nom,Description")] Categorie categorie)
        {
            if (id != categorie.Id_categorie)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categorie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategorieExists(categorie.Id_categorie))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                     // Log the exception
                     ModelState.AddModelError("", "Une erreur est survenue lors de la modification de la catégorie. Veuillez réessayer.");
                     // _logger.LogError(ex, "Error editing category");
                }

                if(ModelState.IsValid)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
             else
            {
                 ModelState.AddModelError("", "Veuillez corriger les erreurs de validation.");
            }
            return View(categorie);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorie = await _context.Categories
                .Include(c => c.BiensImmobiliers)
                .FirstOrDefaultAsync(m => m.Id_categorie == id);

            if (categorie == null)
            {
                return NotFound();
            }

            return View(categorie);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categorie = await _context.Categories.FindAsync(id);
            if (categorie != null)
            {
                _context.Categories.Remove(categorie);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CategorieExists(int id)
        {
            return _context.Categories.Any(e => e.Id_categorie == id);
        }
    }
} 