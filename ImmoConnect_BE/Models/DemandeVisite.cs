using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImmoConnect_BE.Models
{
    public class DemandeVisite
    {
        [Key]
        public int Id_demande { get; set; }

        [Required(ErrorMessage = "Le nom du client est obligatoire")]
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
        public string Nom_client { get; set; }

        [Required(ErrorMessage = "L'email du client est obligatoire")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string Email_client { get; set; }

        [Required(ErrorMessage = "La date souhaitée est obligatoire")]
        [DataType(DataType.Date)]
        public DateTime Date_souhaitee { get; set; }

        [Required(ErrorMessage = "L'heure souhaitée est obligatoire")]
        [DataType(DataType.Time)]
        public TimeSpan Heure_demande { get; set; }

        public string Message { get; set; }

        public DateTime Date_demande { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Le statut est obligatoire")]
        public string Statut { get; set; } = "En attente";

        [Required(ErrorMessage = "Le bien immobilier est obligatoire")]
        public int Id_bien { get; set; }

        // Navigation property
        [ForeignKey("Id_bien")]
        public virtual BienImmobilier? BienImmobilier { get; set; }
    }
} 