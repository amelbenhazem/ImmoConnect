using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ImmoConnect_BE.Data;
using ImmoConnect_BE.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ImmoConnect_BE.Controllers
{
    [Authorize]
    public class DemandesVisiteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DemandesVisiteController> _logger;

        public DemandesVisiteController(ApplicationDbContext context, ILogger<DemandesVisiteController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: DemandesVisite
        public async Task<IActionResult> Index(string searchString, DateTime? dateDebut, DateTime? dateFin)
        {
            var demandes = _context.DemandesVisite
                .Include(d => d.BienImmobilier)
                .ThenInclude(b => b.Utilisateur)
                .AsQueryable();

            // Filtrage par recherche
            if (!string.IsNullOrEmpty(searchString))
            {
                demandes = demandes.Where(d => d.Nom_client.Contains(searchString) || 
                                             d.Email_client.Contains(searchString) || 
                                             d.Message.Contains(searchString));
            }

            // Filtrage par date
            if (dateDebut.HasValue)
            {
                demandes = demandes.Where(d => d.Date_souhaitee >= dateDebut.Value);
            }
            if (dateFin.HasValue)
            {
                demandes = demandes.Where(d => d.Date_souhaitee <= dateFin.Value);
            }

            // Si l'utilisateur n'est pas admin, ne montrer que les demandes pour ses biens
            if (!User.IsInRole("Admin"))
            {
                demandes = demandes.Where(d => d.BienImmobilier.Utilisateur.Email == User.Identity.Name);
            }

            ViewBag.SearchString = searchString;
            ViewBag.DateDebut = dateDebut;
            ViewBag.DateFin = dateFin;

            return View(await demandes.OrderByDescending(d => d.Date_demande).ToListAsync());
        }

        // GET: DemandesVisite/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var demandeVisite = await _context.DemandesVisite
                .Include(d => d.BienImmobilier)
                .ThenInclude(b => b.Utilisateur)
                .FirstOrDefaultAsync(m => m.Id_demande == id);

            if (demandeVisite == null)
            {
                return NotFound();
            }

            // Vérifier si l'utilisateur est autorisé à voir cette demande
            if (!User.IsInRole("Admin") && demandeVisite.BienImmobilier.Utilisateur.Email != User.Identity.Name)
            {
                return Forbid();
            }

            return View(demandeVisite);
        }

        // GET: DemandesVisite/Create
        public IActionResult Create(int? bienId)
        {
            if (bienId.HasValue)
            {
                ViewBag.BienId = bienId;
                ViewBag.BienTitre = _context.BiensImmobiliers
                    .Where(b => b.Id_bien == bienId)
                    .Select(b => b.Titre)
                    .FirstOrDefault();
            }
            else
            {
                ViewBag.Biens = _context.BiensImmobiliers.ToList();
            }
            return View();
        }

        // POST: DemandesVisite/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nom_client,Email_client,Date_souhaitee,Heure_demande,Message,Id_bien,Statut")] DemandeVisite demandeVisite)
        {
            _logger.LogInformation($"Received DemandeVisite with Id_bien: {demandeVisite.Id_bien}");

            if (ModelState.IsValid)
            {
                try
                {
                    demandeVisite.Date_demande = DateTime.Now;
                    _context.Add(demandeVisite);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log the exception (using a logging framework like Serilog, NLog, etc.)
                    // For simplicity, we'll just add a model error here.
                    ModelState.AddModelError("", "Une erreur est survenue lors de l'ajout de la demande de visite. Veuillez réessayer.");
                    // Potentially log the full exception details for debugging
                    _logger.LogError(ex, "Error creating visit request");
                }
            }
            else
            {
                // Log validation errors
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        _logger.LogError($"Validation Error: {error.ErrorMessage}");
                    }
                }
                ModelState.AddModelError("", "Veuillez corriger les erreurs de validation.");
            }

            // If ModelState is not valid or save failed, repopulate ViewBag.Biens for the view
            if (ViewBag.Biens == null)
            {
                 ViewBag.Biens = _context.BiensImmobiliers.ToList();
            }
            
            // If bienId was passed, repopulate ViewBag.BienId and ViewBag.BienTitre
             if (demandeVisite.Id_bien != 0)
            {
                ViewBag.BienId = demandeVisite.Id_bien;
                 ViewBag.BienTitre = _context.BiensImmobiliers
                    .Where(b => b.Id_bien == demandeVisite.Id_bien)
                    .Select(b => b.Titre)
                    .FirstOrDefault();
            }

            return View(demandeVisite);
        }

        // GET: DemandesVisite/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var demandeVisite = await _context.DemandesVisite
                .Include(d => d.BienImmobilier)
                .FirstOrDefaultAsync(m => m.Id_demande == id);

            if (demandeVisite == null)
            {
                return NotFound();
            }

            // Vérifier si l'utilisateur est autorisé à modifier cette demande
            if (!User.IsInRole("Admin") && demandeVisite.BienImmobilier.Utilisateur.Email != User.Identity.Name)
            {
                return Forbid();
            }

            return View(demandeVisite);
        }

        // POST: DemandesVisite/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_demande,Nom_client,Email_client,Date_souhaitee,Heure_demande,Message,Id_bien,Statut")] DemandeVisite demandeVisite)
        {
            if (id != demandeVisite.Id_demande)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingDemande = await _context.DemandesVisite
                        .Include(d => d.BienImmobilier)
                        .FirstOrDefaultAsync(d => d.Id_demande == id);

                    if (existingDemande == null)
                    {
                        return NotFound();
                    }

                    // Vérifier si l'utilisateur est autorisé à modifier cette demande
                    if (!User.IsInRole("Admin") && existingDemande.BienImmobilier.Utilisateur.Email != User.Identity.Name)
                    {
                        return Forbid();
                    }

                    // Préserver la date de demande
                    demandeVisite.Date_demande = existingDemande.Date_demande;

                    _context.Entry(existingDemande).CurrentValues.SetValues(demandeVisite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DemandeVisiteExists(demandeVisite.Id_demande))
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
                     ModelState.AddModelError("", "Une erreur est survenue lors de la modification de la demande de visite. Veuillez réessayer.");
                     // _logger.LogError(ex, "Error editing visit request");
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
             // If ModelState is not valid or save failed, ensure the BienImmobilier is loaded for the view
             if (demandeVisite.Id_bien != 0 && demandeVisite.BienImmobilier == null)
            {
                 demandeVisite.BienImmobilier = await _context.BiensImmobiliers.FirstOrDefaultAsync(b => b.Id_bien == demandeVisite.Id_bien);
            }

            return View(demandeVisite);
        }

        // GET: DemandesVisite/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var demandeVisite = await _context.DemandesVisite
                .Include(d => d.BienImmobilier)
                .ThenInclude(b => b.Utilisateur)
                .FirstOrDefaultAsync(m => m.Id_demande == id);

            if (demandeVisite == null)
            {
                return NotFound();
            }

            // Vérifier si l'utilisateur est autorisé à supprimer cette demande
            if (!User.IsInRole("Admin") && demandeVisite.BienImmobilier.Utilisateur.Email != User.Identity.Name)
            {
                return Forbid();
            }

            return View(demandeVisite);
        }

        // POST: DemandesVisite/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var demandeVisite = await _context.DemandesVisite
                .Include(d => d.BienImmobilier)
                .FirstOrDefaultAsync(d => d.Id_demande == id);

            if (demandeVisite == null)
            {
                return NotFound();
            }

            // Vérifier si l'utilisateur est autorisé à supprimer cette demande
            if (!User.IsInRole("Admin") && demandeVisite.BienImmobilier.Utilisateur.Email != User.Identity.Name)
            {
                return Forbid();
            }

            _context.DemandesVisite.Remove(demandeVisite);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DemandeVisiteExists(int id)
        {
            return _context.DemandesVisite.Any(e => e.Id_demande == id);
        }
    }
} 