using System;
using System.ComponentModel.DataAnnotations;

namespace ImmoConnect_BE.ViewModel
{
    public class DemandeVisiteDto
    {
        [Required(ErrorMessage = "Le nom du client est obligatoire")]
        public string Nom_client { get; set; }

        [Required(ErrorMessage = "L'email du client est obligatoire")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string Email_client { get; set; }

        [Required(ErrorMessage = "La date souhaitée est obligatoire")]
        [DataType(DataType.Date)]
        public DateTime Date_souhaitee { get; set; }

        public string Message { get; set; }

        [Required(ErrorMessage = "L'identifiant du bien est obligatoire")]
        public int Id_bien { get; set; }
    }
} 