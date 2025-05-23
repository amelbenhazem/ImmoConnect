using Microsoft.EntityFrameworkCore;
using ImmoConnect_BE.Models;

namespace ImmoConnect_BE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<BienImmobilier> BiensImmobiliers { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<DemandeVisite> DemandesVisite { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration de la conversion de l'énumération Role
            modelBuilder.Entity<Utilisateur>()
                .Property(u => u.Role)
                .HasConversion<string>();

            // Configuration des relations
            modelBuilder.Entity<BienImmobilier>()
                .HasOne(b => b.Categorie)
                .WithMany(c => c.BiensImmobiliers)
                .HasForeignKey(b => b.Id_categorie);

            modelBuilder.Entity<BienImmobilier>()
                .HasOne(b => b.Utilisateur)
                .WithMany()
                .HasForeignKey(b => b.Id_utilisateur);

            modelBuilder.Entity<DemandeVisite>()
                .HasOne(d => d.BienImmobilier)
                .WithMany(b => b.DemandesVisite)
                .HasForeignKey(d => d.Id_bien);
        }
    }
}

