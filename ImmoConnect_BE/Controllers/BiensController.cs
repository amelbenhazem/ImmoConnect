using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ImmoConnect_BE.Data;
using ImmoConnect_BE.Models;
using ImmoConnect_BE.ViewModel;

namespace ImmoConnect_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BiensController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BiensController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Biens
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BienDetailsDto>>> GetBiens()
        {
            var biens = await _context.BiensImmobiliers
                .Include(b => b.Categorie)
                .Include(b => b.Utilisateur)
                .Select(b => new BienDetailsDto
                {
                    Id_bien = b.Id_bien,
                    Titre = b.Titre,
                    Description = b.Description,
                    Prix = b.Prix,
                    Adresse = b.Adresse,
                    Image_url = b.Image_url,
                    Statut = b.Statut,
                    Date_ajout = b.Date_ajout,
                    CategorieNom = b.Categorie.Nom,
                    AgentNom = b.Utilisateur.Nom,
                    AgentPrenom = b.Utilisateur.Prenom,
                    AgentEmail = b.Utilisateur.Email
                })
                .ToListAsync();

            return Ok(biens);
        }

        // GET: api/Biens/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<BienDetailsDto>> GetBien(int id)
        {
            var bien = await _context.BiensImmobiliers
                .Include(b => b.Categorie)
                .Include(b => b.Utilisateur)
                .Where(b => b.Id_bien == id)
                .Select(b => new BienDetailsDto
                {
                    Id_bien = b.Id_bien,
                    Titre = b.Titre,
                    Description = b.Description,
                    Prix = b.Prix,
                    Adresse = b.Adresse,
                    Image_url = b.Image_url,
                    Statut = b.Statut,
                    Date_ajout = b.Date_ajout,
                    CategorieNom = b.Categorie.Nom,
                    AgentNom = b.Utilisateur.Nom,
                    AgentPrenom = b.Utilisateur.Prenom,
                    AgentEmail = b.Utilisateur.Email
                })
                .FirstOrDefaultAsync();

            if (bien == null)
            {
                return NotFound();
            }

            return bien;
        }

        // POST: api/Biens
        [HttpPost]
        public async Task<ActionResult<BienImmobilier>> PostBien(BienImmobilier bien)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.BiensImmobiliers.Add(bien);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBien), new { id = bien.Id_bien }, bien);
        }

        // PUT: api/Biens/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBien(int id, BienImmobilier bien)
        {
            if (id != bien.Id_bien)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(bien).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BienExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Biens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBien(int id)
        {
            var bien = await _context.BiensImmobiliers.FindAsync(id);
            if (bien == null)
            {
                return NotFound();
            }

            _context.BiensImmobiliers.Remove(bien);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BienExists(int id)
        {
            return _context.BiensImmobiliers.Any(e => e.Id_bien == id);
        }
    }
} 