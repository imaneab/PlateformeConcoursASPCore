using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetConcoursAspCore.Data;
using ProjetConcoursAspCore.Models;
using ProjetConcoursAspCore.Repositories;
using ProjetConcoursAspCore.ViewModel;

namespace ProjetConcoursAspCore.Controllers
{
	[Authorize(Policy = "MemberPolicy")]
	public class ProfileController : Controller
	{
		private readonly ApplicationDbContext _context;
		private StudentRepository studentRepository;
		private IWebHostEnvironment _hostingEnvironment;
		public ProfileController(ApplicationDbContext context, IWebHostEnvironment environment)
		{
			_context = context;

			studentRepository = new StudentRepository(_context);
			_hostingEnvironment = environment;
		}
		// GET: ProfileController
		[Authorize]
		public ActionResult Index()
		{
			var user = GetCurrentUser();
			//cne
			//role
			//ViewBag.ListFil = new SelectList(_context.Filieres, "ID", "Titre");
			ViewBag.ListFil = _context.Filieres.ToList<Filiere>();

			return View(user);
		}

		private String GetClaimValue(System.Security.Claims.Claim[] claims, String key)
		{
			foreach (var c in claims)
			{
				if (c.Type.Equals(key))
					return c.Value;
			}
			return null;
		}
		private Etudiant GetCurrentUser()
		{
			System.Security.Claims.ClaimsPrincipal currentUser = this.User;
			String cne = GetClaimValue(currentUser.Claims.ToArray(), "cne");
			return studentRepository.GetEtudiantByCNE(cne);
		}

		// POST: ProfileController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(Etudiant etudiant, string Choixdannee, IFormFile file, IFormFile photo, string x)
		{
			if (true /*ModelState.IsValid*/)
			{
				int f = Convert.ToInt32(x);

				var originEtudiant = _context.Etudiants.FirstOrDefault(e => e.ID == etudiant.ID);
				if(originEtudiant == null)
					return RedirectToAction("Index");
				var note = _context.Notes.FirstOrDefault(n=>n.EtudiantNoteId == originEtudiant.ID);
				var diplome = _context.Diplomes.FirstOrDefault(d=>d.EtudiantDiplomeId == originEtudiant.ID);

				originEtudiant.ID_Filiere = f;
				originEtudiant.Nom = etudiant.Nom;
				originEtudiant.Prenom = etudiant.Prenom;
				originEtudiant.Email = etudiant.Email;
				originEtudiant.Tel = etudiant.Tel;
				originEtudiant.Nationnalite = etudiant.Nationnalite;
				originEtudiant.Civilite = etudiant.Civilite;
				originEtudiant.DateNaiss = etudiant.DateNaiss;
				originEtudiant.Etablissement = etudiant.Etablissement;
				//originEtudiant.Nom = etudiant.Nom;

				note.NoteBac = etudiant.Note.NoteBac;
				note.S1 = etudiant.Note.S1;
				note.S2 = etudiant.Note.S2;
				note.S3 = etudiant.Note.S3;
				note.S4 = etudiant.Note.S4;
				note.S5 = etudiant.Note.S5;
				note.S6 = etudiant.Note.S6;

				note.Redoublant1 = etudiant.Note.Redoublant1;
				note.Redoublant2 = etudiant.Note.Redoublant2;
				note.Redoublant3 = etudiant.Note.Redoublant3;


				diplome.AnneeBac = etudiant.Diplome.AnneeBac;
				diplome.Annee1 = etudiant.Diplome.Annee1;
				diplome.Annee2 = etudiant.Diplome.Annee2;
				diplome.Annee3 = etudiant.Diplome.Annee3;
				diplome.Titre = etudiant.Diplome.Titre;

				_context.Entry(originEtudiant).State = EntityState.Modified;
				_context.Entry(note).State = EntityState.Modified;
				_context.Entry(diplome).State = EntityState.Modified;

				if (file != null)
				{
					//delete old file first
					try
					{
						System.IO.File.Delete(Path.Combine(_hostingEnvironment.WebRootPath, "uploads", diplome.fichier));
					}
					catch (Exception ex)
					{

					}

					string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
					string pic = Path.GetFileName(file.FileName);
					string newName = timeStamp + "_" + pic;
					diplome.fichier = newName;
					//string path = Path.Combine(Server.MapPath("~/images/"), newName);
					var path = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
					// file is uploaded
					if (file.Length > 0)
					{
						var filePath = Path.Combine(path, newName); //file.FileName
						using (var fileStream = new FileStream(filePath, FileMode.Create))
						{
							await file.CopyToAsync(fileStream);

							fileStream.Close();
						}
					}
				}

				if (photo != null)
				{
					//delete old file first
					try
					{
						System.IO.File.Delete(Path.Combine(_hostingEnvironment.WebRootPath, "uploads", originEtudiant.picture));
					}
					catch (Exception ex)
					{

					}

					string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
					string pic = Path.GetFileName(photo.FileName);
					string newName = timeStamp + "_" + pic;
					originEtudiant.picture = newName;
					//string path = Path.Combine(Server.MapPath("~/images/"), newName);
					var path = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
					// file is uploaded
					if (photo.Length > 0)
					{
						var filePath = Path.Combine(path, newName); //file.FileName
						using (var fileStream = new FileStream(filePath, FileMode.Create))
						{
							await photo.CopyToAsync(fileStream);
							fileStream.Close();
						}
					}
				}

				/*if (Choixdannee == "3eme")
				{
					_context.Etudiants4a.Remove(
						_context.Etudiants4a.Where<Etudiant4a>(e=>e.EtudiantId==etudiant.ID).FirstOrDefault()
						);
					studentRepository.InsertStudent3emeAnnee(etudiant);

				}
				else if (Choixdannee.Equals("4eme"))
				{
					_context.Etudiants3a.Remove(
						_context.Etudiants3a.Where<Etudiant3a>(e => e.EtudiantId == etudiant.ID).FirstOrDefault()
						);
					studentRepository.InsertStudent4emeAnnee(etudiant);
				}*/

				_context.SaveChanges();
			}
			return RedirectToAction("Index");
		}

		// GET: ProfileController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: ProfileController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
