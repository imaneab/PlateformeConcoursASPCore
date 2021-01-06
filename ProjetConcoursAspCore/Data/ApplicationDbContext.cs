using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjetConcoursAspCore.Models;

namespace ProjetConcoursAspCore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
     

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Etudiant> Etudiants { get; set; }
        public DbSet<Etudiant3a> Etudiants3a { get; set; }
        public DbSet<Etudiant4a> Etudiants4a { get; set; }
        //   public DbSet<AncienEpreuve> AncienEpreuves { get; set; }
        public DbSet<Diplome> Diplomes { get; set; }
        public DbSet<Upload> Uploads { get; set; }
        public DbSet<Filiere> Filieres { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Parametre> Parametres { get; set; }
        public DbSet<File> files { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Etudiant>()
                .HasIndex(u => u.Cin)
                .IsUnique(true);
            builder.Entity<Etudiant>()
                .HasIndex(u => u.Cne)
                .IsUnique(true);
            builder.Entity<Etudiant>()
                .HasIndex(u => u.Email)
                .IsUnique(true);
            builder.Entity<Admin>()
                .HasIndex(u => u.Login)
                .IsUnique(true);
            base.OnModelCreating(builder);

        }
    }
}
