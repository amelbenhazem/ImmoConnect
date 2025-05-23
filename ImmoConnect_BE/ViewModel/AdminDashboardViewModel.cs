using System.Collections.Generic;
using ImmoConnect_BE.Models;

namespace ImmoConnect_BE.ViewModel
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalProperties { get; set; }
        public int TotalCategories { get; set; }
        public int TotalVisitRequests { get; set; }
        public List<BienImmobilier> RecentProperties { get; set; }
        public List<DemandeVisite> RecentVisitRequests { get; set; }
    }
} 