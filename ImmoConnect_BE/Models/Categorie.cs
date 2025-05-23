using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ImmoConnect_BE.Models
{
    public class Categorie
    {
        [Key]
        public int Id_categorie { get; set; }

        [Required(ErrorMessage = "Le nom de la catégorie est obligatoire")]
        [StringLength(50, ErrorMessage = "Le nom ne peut pas dépasser 50 caractères")]
        public string Nom { get; set; }

        [StringLength(500, ErrorMessage = "La description ne peut pas dépasser 500 caractères")]
        public string? Description { get; set; }

        // Navigation property
        public virtual ICollection<BienImmobilier> BiensImmobiliers { get; set; }

        // Constructor to initialize collection navigation properties
        public Categorie()
        {
            BiensImmobiliers = new HashSet<BienImmobilier>();
        }
    }
}