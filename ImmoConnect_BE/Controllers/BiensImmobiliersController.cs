using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ImmoConnect_BE.Data;
using ImmoConnect_BE.Models;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace ImmoConnect_BE.Controllers
{
    [Authorize]
    public class BiensImmobiliersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BiensImmobiliersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> IndexA(string searchString, int? categorieId, decimal? minPrix, decimal? maxPrix, string statut)
        {
            return await ListBiens(searchString, categorieId, minPrix, maxPrix, statut, mineOnly: false);
        }

        public async Task<IActionResult> Index(string searchString, int? categorieId, decimal? minPrix, decimal? maxPrix, string statut)
        {
            var mineOnly = !User.IsInRole("Admin");
            return await ListBiens(searchString, categorieId, minPrix, maxPrix, statut, mineOnly);
        }

        private async Task<IActionResult> ListBiens(string searchString, int? categorieId, decimal? minPrix, decimal? maxPrix, string statut, bool mineOnly)
        {
            var biens = _context.BiensImmobiliers
                .Include(b => b.Categorie)
                .Include(b => b.Utilisateur)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                biens = biens.Where(b => b.Titre.Contains(searchString) ||
                                       b.Description.Contains(searchString) ||
                                       b.Adresse.Contains(searchString));
            }

            if (categorieId.HasValue)
            {
                biens = biens.Where(b => b.Id_categorie == categorieId.Value);
            }

            if (minPrix.HasValue)
            {
                biens = biens.Where(b => b.Prix >= minPrix.Value);
            }
            if (maxPrix.HasValue)
            {
                biens = biens.Where(b => b.Prix <= maxPrix.Value);
            }

            if (!string.IsNullOrEmpty(statut))
            {
                biens = biens.Where(b => b.Statut == statut);
            }

            if (mineOnly)
            {
                biens = biens.Where(b => b.Utilisateur.Email == User.Identity.Name);
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.SearchString = searchString;
            ViewBag.CategorieId = categorieId;
            ViewBag.MinPrix = minPrix;
            ViewBag.MaxPrix = maxPrix;
            ViewBag.Statut = statut;
            ViewBag.ListAction = mineOnly ? "Index" : "IndexA";

            return View("Index", await biens.ToListAsync());
        }

        // GET: BiensImmobiliers/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bienImmobilier = await _context.BiensImmobiliers
                .Include(b => b.Categorie)
                .Include(b => b.Utilisateur)
                .FirstOrDefaultAsync(m => m.Id_bien == id);

            if (bienImmobilier == null)
            {
                return NotFound();
            }

            return View(bienImmobilier);
        }

        // GET: BiensImmobiliers/Create
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        // POST: BiensImmobiliers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Titre,Description,Prix,Adresse,Image_url,Statut,Id_categorie")] BienImmobilier bienImmobilier)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Récupérer l'utilisateur connecté
                    var user = await _context.Utilisateurs.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "Utilisateur non trouvé");
                        ViewBag.Categories = await _context.Categories.ToListAsync();
                        return View(bienImmobilier);
                    }

                    // Initialiser les propriétés de navigation
                    bienImmobilier.Utilisateur = user;
                    bienImmobilier.Id_utilisateur = user.Id;
                    bienImmobilier.Date_ajout = DateTime.Now;
                    bienImmobilier.DemandesVisite = new List<DemandeVisite>();

                    // Récupérer la catégorie
                    var categorie = await _context.Categories.FindAsync(bienImmobilier.Id_categorie);
                    if (categorie == null)
                    {
                        ModelState.AddModelError("Id_categorie", "Catégorie non trouvée");
                        ViewBag.Categories = await _context.Categories.ToListAsync();
                        return View(bienImmobilier);
                    }
                    bienImmobilier.Categorie = categorie;

                    _context.Add(bienImmobilier);
                    var result = await _context.SaveChangesAsync();
                    
                    if (result > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Erreur lors de l'enregistrement du bien");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Une erreur est survenue : {ex.Message}");
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(bienImmobilier);
        }

        // GET: BiensImmobiliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bienImmobilier = await _context.BiensImmobiliers
                .Include(b => b.Utilisateur)
                .FirstOrDefaultAsync(b => b.Id_bien == id);
            if (bienImmobilier == null)
            {
                return NotFound();
            }

            // Vérifier si l'utilisateur est autorisé à modifier ce bien
            if (!User.IsInRole("Admin") && bienImmobilier.Utilisateur?.Email != User.Identity.Name)
            {
                return Forbid();
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(bienImmobilier);
        }

        // POST: BiensImmobiliers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_bien,Titre,Description,Prix,Adresse,Image_url,Statut,Id_categorie")] BienImmobilier bienImmobilier)
        {
            if (id != bienImmobilier.Id_bien)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingBien = await _context.BiensImmobiliers
                        .Include(b => b.Utilisateur)
                        .FirstOrDefaultAsync(b => b.Id_bien == id);
                    if (existingBien == null)
                    {
                        return NotFound();
                    }

                    // Vérifier si l'utilisateur est autorisé à modifier ce bien
                    if (!User.IsInRole("Admin") && existingBien.Utilisateur?.Email != User.Identity.Name)
                    {
                        return Forbid();
                    }

                    // Préserver les champs qui ne doivent pas être modifiés
                    bienImmobilier.Id_utilisateur = existingBien.Id_utilisateur;
                    bienImmobilier.Date_ajout = existingBien.Date_ajout;

                    _context.Entry(existingBien).CurrentValues.SetValues(bienImmobilier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BienImmobilierExists(bienImmobilier.Id_bien))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = _context.Categories.ToList();
            return View(bienImmobilier);
        }

        // GET: BiensImmobiliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bienImmobilier = await _context.BiensImmobiliers
                .Include(b => b.Categorie)
                .Include(b => b.Utilisateur)
                .FirstOrDefaultAsync(m => m.Id_bien == id);

            if (bienImmobilier == null)
            {
                return NotFound();
            }

            // Vérifier si l'utilisateur est autorisé à supprimer ce bien
            if (!User.IsInRole("Admin") && bienImmobilier.Utilisateur?.Email != User.Identity.Name)
            {
                return Forbid();
            }

            return View(bienImmobilier);
        }

        // POST: BiensImmobiliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bienImmobilier = await _context.BiensImmobiliers
                .Include(b => b.Utilisateur)
                .FirstOrDefaultAsync(b => b.Id_bien == id);
            if (bienImmobilier == null)
            {
                return NotFound();
            }

            // Vérifier si l'utilisateur est autorisé à supprimer ce bien
            if (!User.IsInRole("Admin") && bienImmobilier.Utilisateur?.Email != User.Identity.Name)
            {
                return Forbid();
            }

            _context.BiensImmobiliers.Remove(bienImmobilier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BienImmobilierExists(int id)
        {
            return _context.BiensImmobiliers.Any(e => e.Id_bien == id);
        }
    }
} 