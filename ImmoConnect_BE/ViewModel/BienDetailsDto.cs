using System;

namespace ImmoConnect_BE.ViewModel
{
    public class BienDetailsDto
    {
        public int Id_bien { get; set; }
        public string Titre { get; set; }
        public string Description { get; set; }
        public decimal Prix { get; set; }
        public string Adresse { get; set; }
        public string Image_url { get; set; }
        public string Statut { get; set; }
        public DateTime Date_ajout { get; set; }
        public string CategorieNom { get; set; }
        public string AgentNom { get; set; }
        public string AgentPrenom { get; set; }
        public string AgentEmail { get; set; }
    }
} 