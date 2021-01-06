using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjetConcoursAspCore.Data;
using ProjetConcoursAspCore.Models;
using ProjetConcoursAspCore.ViewModel;

namespace ProjetConcoursAspCore.Controllers
{
    
    public class HomeController : Controller
    {
        
        private readonly ApplicationDbContext _context;
        
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["ID_Filiere"] = new SelectList(_context.Filieres, "ID", "Titre");
            return View();
        }

        //// POST: Etudiants/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Index([Bind("ID,Nom,Prenom,Cne,Cin,Password,Email,Civilite,NumDossier,Etat,IsDeleted,DateNaiss,Nationnalite,Tel,Etablissement,ID_Filiere")] Etudiant etudiant)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(etudiant);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["ID_Filiere"] = new SelectList(_context.Filieres, "ID", "Titre", etudiant.ID_Filiere);
        //    return View(etudiant);
        //}


        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.ListFil = new SelectList(_context.Filieres, "ID", "Titre");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Etudiant etudiant)

        {
            var nom = etudiant.Nom;
                if (ModelState.IsValid)
            {
                
                return RedirectToAction(nameof(Index));
            }
            //ViewData["ID_Filiere"] = new SelectList(_context.Filieres, "ID", "Titre", etudiant.ID_Filiere);
            return View(etudiant);
        }

        public IActionResult create()
        {
            return View();
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

        public string RandomPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }
        // Generate a random number between two numbers    
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        // Generate a random string with a given size    
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
    }
}
