using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetConcoursAspCore.Data;
using ProjetConcoursAspCore.Models;

namespace ProjetConcoursAspCore.Controllers
{
    public class FileController : Controller
    {
        private readonly ApplicationDbContext db;

        public FileController(ApplicationDbContext context)
        {
            db = context;
        }



        public ActionResult Index()
        {
            //Context2 db = new Context2();
            List<Filiere> filieres = new List<Filiere>();
            return View(db.Filieres.ToList());
        }

        public ActionResult Get(int ID)
        {

            ProjetConcoursAspCore.Models.File file = db.files.Find(ID);
            string s = Convert.ToBase64String(file.Content);
            //return Content(Convert.ToString(file.Content.Length));
            var contentType = "APPLICATION/octet-stream";
            return File(file.Content, contentType, file.Name);
        }


            // la liste des fichires d'une filiere
            public ActionResult ListFilesParFiliere(int? ID)
        {

            // List<Models.File> listfiles = db.Filieres.Where(f => f.ID== ID).Single().files.ToList();
            List<ProjetConcoursAspCore.Models.File> listfiles = db.files.Where(f => f.Id_fil == ID).ToList();

            ViewData["ID"] = ID;
            return View(listfiles);
        }


       




    }
}