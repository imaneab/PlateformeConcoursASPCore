using ProjetConcoursAspCore.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjetConcoursAspCore.Data;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace ProjetConcoursAspCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminPolicy")]
    public class FileController : Controller
    {
        private readonly ApplicationDbContext db;
        public FileController(ApplicationDbContext context)
        {
            db = context;
        }
        //1er view :: contient table des filiere avec (button liste des epreuves -> vers une table des pdf)+(button vers form d'insertion des pdf)    
        public ActionResult Index()
        {
            //Context2 db = new Context2();
            List<Filiere> filieres = new List<Filiere>();
            return View(db.Filieres.ToList());
        }


        public ActionResult delete(int Id, int Id_fil)
        {
            ProjetConcoursAspCore.Models.File file = db.files.Find(Id);
            db.files.Remove(file);
            db.SaveChanges();
            // return RedirectToAction("ListFilesParFiliere", Id_fil);
            return RedirectToAction("ListFilesParFiliere", new { ID = Id_fil });

        }
        //return RedirectToAction("URL" , new { id = yourID



        // la liste des fichires d'une filiere
        public ActionResult ListFilesParFiliere(int? ID)
        {

            // List<Models.File> listfiles = db.Filieres.Where(f => f.ID== ID).Single().files.ToList();
            List<ProjetConcoursAspCore.Models.File> listfiles = db.files.Where(f => f.Id_fil == ID).ToList();

            ViewData["ID"] = ID;
            return View(listfiles);
        }


        public ActionResult AddFile()
        {
            ViewBag.filiere = new SelectList(db.Filieres, "ID", "Titre");
            return View();
        }


        //upload file
        public ActionResult Get(int ID)
        {

            ProjetConcoursAspCore.Models.File file = db.files.Find(ID);
            string s = Convert.ToBase64String(file.Content);
            //return Content(Convert.ToString(file.Content.Length));
            var contentType = "APPLICATION/octet-stream";
            return File(file.Content, contentType, file.Name);
            //If file exists....
            ViewBag.Id_fil = new SelectList(db.Filieres, "Id_filiere", "Nom_filiere");
            MemoryStream ms = new MemoryStream(file.Content, 0, 0, true, true);
            //Response.ContentType = file.Type;
            //Response.Headers.Add("content-disposition", "attachment;filename=" + file.Name);
            //Response.Clear();
            //var contentType = "APPLICATION/octet-stream";

            //Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            //Response.OutputStream.Flush();
            //Response.End();
            ViewBag.filieres = new SelectList(db.Filieres, "ID", "Nom_fil");


            //return new FileStreamResult(Response.OutputStream, file.Type);
            return File(ms, contentType,file.Name);
            return new FileStreamResult(ms, new MediaTypeHeaderValue("text/csv"))
            {
                FileDownloadName = file.Name
            };

        }


        //add file in database

        //[HttpPost]
        //public ActionResult Add(IFormFile uploadFile)
        //{
        //    return Content(Annee);
        //    if (uploadFile != null && uploadFile.Length > 0)
        //    {
        //        //Context2 db = new Context2();
        //        ProjetConcoursAspCore.Models.File file = new ProjetConcoursAspCore.Models.File();
        //        int lengthFile = uploadFile.Length;
        //        byte[] tempFile = new byte[uploadFile.Length];
        //        string nameFile = uploadFile.FileName;
        //        string typeFile = uploadFile.ContentType;
        //        int Annee = uploadFile.ContentLength;


        //        uploadFile.InputStream.Read(tempFile, 0, lengthFile);
        //        file.Content = tempFile;
        //        file.Name = nameFile;
        //        file.Length = lengthFile;
        //        file.Type = typeFile;
        //        file.Annee = Annee;
        //        // file.id_fil = Convert.ToInt32(Request.Form["id_fil"]);

        //        file.Id_fil = Convert.ToInt32(Request.Form["ID"]);// bach anbdliha!
        //        file.Titre_concours = Request.Form["Titre_concours"];
        //        file.Annee = Convert.ToInt32(Request.Form["Annee"]);

        //        db.files.Add(file);
        //        db.SaveChanges();

        //    }

        //    return RedirectToAction("Index");

        //}

        [HttpPost]
        public async Task<ActionResult> AddFile(IFormFile Upfile)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (Upfile == null || Upfile.Length == 0)
                return Content("file not selected");
            ProjetConcoursAspCore.Models.File file = new ProjetConcoursAspCore.Models.File();
            long lengthFile = Upfile.Length;
            byte[] tempFile = new byte[Upfile.Length];
            string nameFile = Upfile.FileName;
            string typeFile = Upfile.ContentType;
            //int Annee = Convert.ToInt32( AnneeP);
            var ms = new MemoryStream();
            await Upfile.CopyToAsync(ms);
            var fileBytes = ms.ToArray();
            string s = Convert.ToBase64String(fileBytes);
            //return Content(s);
            file.Content =fileBytes;
            // act on the Base64 data

            file.Name = nameFile;
            file.Length =Convert.ToInt32(lengthFile);
            file.Type = typeFile;
            //file.Annee = Annee;
            // file.id_fil = Convert.ToInt32(Request.Form["id_fil"]);

            file.Id_fil = Convert.ToInt32(Request.Form["ID"]);// bach anbdliha!
            file.Titre_concours = Request.Form["Titre_concours"];
            file.Annee = Convert.ToInt32(Request.Form["Annee"]);

            db.files.Add(file);
            db.SaveChanges();


            return RedirectToAction("Index");
        }



    }
}