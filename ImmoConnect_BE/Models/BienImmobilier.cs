using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImmoConnect_BE.Models
{
    public class BienImmobilier
    {
        [Key]
        public int Id_bien { get; set; }

        [Required(ErrorMessage = "Le titre est obligatoire")]
        [StringLength(100, ErrorMessage = "Le titre ne peut pas dépasser 100 caractères")]
        public string Titre { get; set; }

        [Required(ErrorMessage = "La description est obligatoire")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Le prix est obligatoire")]
        [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être supérieur à 0")]
        public decimal Prix { get; set; }

        [Required(ErrorMessage = "L'adresse est obligatoire")]
        public string Adresse { get; set; }

        public string? Image_url { get; set; }

        [Required(ErrorMessage = "Le statut est obligatoire")]
        public string Statut { get; set; }

        public DateTime Date_ajout { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "La catégorie est obligatoire")]
        public int Id_categorie { get; set; }

        [Required(ErrorMessage = "L'utilisateur est obligatoire")]
        public int Id_utilisateur { get; set; }

        // Navigation properties
        [ForeignKey("Id_categorie")]
        public virtual Categorie? Categorie { get; set; }

        [ForeignKey("Id_utilisateur")]
        public virtual Utilisateur? Utilisateur { get; set; }

        public virtual ICollection<DemandeVisite>? DemandesVisite { get; set; }
    }
} 