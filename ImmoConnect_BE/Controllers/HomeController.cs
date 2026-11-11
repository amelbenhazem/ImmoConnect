using ImmoConnect_BE.Data;
using ImmoConnect_BE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ImmoConnect_BE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        public IActionResult Index()
        {
            ViewBag.Categories = _context.Categories.ToList();
            var biens = _context.BiensImmobiliers
                .OrderByDescending(b => b.Date_ajout)
                .ToList();

            return View(biens);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
