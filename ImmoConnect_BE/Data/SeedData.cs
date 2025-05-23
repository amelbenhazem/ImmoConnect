using ImmoConnect_BE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ImmoConnect_BE.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Vérifier si des catégories existent déjà
                if (context.Categories.Any())
                {
                    return; // La base de données a déjà été initialisée
                }

                // Ajouter des catégories
                context.Categories.AddRange(
                    new Categorie { Nom = "Appartement" },
                    new Categorie { Nom = "Maison" },
                    new Categorie { Nom = "Terrain" },
                    new Categorie { Nom = "Bureau" },
                    new Categorie { Nom = "Local commercial" }
                );

                // Ajouter un utilisateur admin
                var adminUser = new Utilisateur
                {
                    Nom = "Admin",
                    Prenom = "System",
                    Email = "admin@immoconnect.com",
                    Mot_De_Passe = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    Role = Role.Admin
                };
                context.Utilisateurs.Add(adminUser);

                // Ajouter un agent immobilier
                var agentUser = new Utilisateur
                {
                    Nom = "Agent",
                    Prenom = "Test",
                    Email = "agent@immoconnect.com",
                    Mot_De_Passe = BCrypt.Net.BCrypt.HashPassword("Agent123!"),
                    Role = Role.Client
                };
                context.Utilisateurs.Add(agentUser);

                context.SaveChanges();

                // Ajouter quelques biens immobiliers
                context.BiensImmobiliers.AddRange(
                    new BienImmobilier
                    {
                        Titre = "Bel appartement centre-ville",
                        Description = "Magnifique appartement de 80m² au centre-ville",
                        Prix = 250000,
                        Adresse = "123 Rue du Commerce, 75001 Paris",
                        Image_url = "https://example.com/appart1.jpg",
                        Statut = "à vendre",
                        Date_ajout = DateTime.Now,
                        Id_categorie = 1,
                        Id_utilisateur = agentUser.Id
                    },
                    new BienImmobilier
                    {
                        Titre = "Maison familiale avec jardin",
                        Description = "Belle maison de 150m² avec jardin de 500m²",
                        Prix = 450000,
                        Adresse = "45 Avenue des Fleurs, 75016 Paris",
                        Image_url = "https://example.com/maison1.jpg",
                        Statut = "à vendre",
                        Date_ajout = DateTime.Now,
                        Id_categorie = 2,
                        Id_utilisateur = agentUser.Id
                    }
                );

                context.SaveChanges();
            }
        }
    }
} 