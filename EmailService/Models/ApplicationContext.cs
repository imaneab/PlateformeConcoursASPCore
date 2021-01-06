using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailService.Models
{
	public class ApplicationContext : DbContext
	{
		public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
		{
		}
		public DbSet<Etudiant> Etudiants { get; set; }
		public DbSet<Etudiant3a> Etudiants3a { get; set; }
		public DbSet<Etudiant4a> Etudiants4a { get; set; }
		public DbSet<Diplome> Diplomes { get; set; }
		public DbSet<Upload> Uploads { get; set; }
		public DbSet<Filiere> Filieres { get; set; }
		public DbSet<Note> Notes { get; set; }
	}
}
