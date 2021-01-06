using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetConcoursAspCore.Data;
using ProjetConcoursAspCore.Kafka;
using ProjetConcoursAspCore.Models;
using ProjetConcoursAspCore.Repositories;
using ProjetConcoursAspCore.ViewModel;

namespace ProjetConcoursAspCore.Controllers
{
	public class RegisterController : Controller
	{
		private readonly ApplicationDbContext _context;
		private IWebHostEnvironment _hostingEnvironment;
		private ProducerWrapper _producerWrapper;

		public RegisterController(ApplicationDbContext context, IWebHostEnvironment environment, ProducerWrapper producerWrapper)
		{
			_context = context;
			_hostingEnvironment = environment;
			this._producerWrapper = producerWrapper;
		}
		[HttpGet]
		public IActionResult Create()
		{


			if (TempData["Message"] != null)
			{
				ViewBag.Message = TempData["Message"].ToString();
			}
			ViewBag.ListFil = new SelectList(_context.Filieres, "ID", "Titre");
			return View();
		}

		public async System.Threading.Tasks.Task<bool> SendEmailAsync(int id)
		{
			string Baseurl = "https://localhost:44344/";
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Baseurl);

				client.DefaultRequestHeaders.Clear();
				//Define request data format  
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Add("token", "vDdzzWaXf8866Gbsdzfz788221Afd");

				//Sending request to find web api REST service resource GetAllEmployees using HttpClient  
				HttpResponseMessage Res = await client.GetAsync("api/email/signup/" + id);

				//Checking the response is successful or not which is sent using HttpClient  
				if (Res.IsSuccessStatusCode)
				{
					//Storing the response details recieved from web api   
					var response = Res.Content.ReadAsStringAsync().Result;

					//Deserializing the response recieved from web api and storing into the Employee list  
					if (Res.ToString().Equals("ok")) return true;

				}
				return false;
			}

		}


		// POST: Etudiant/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Etudiant etudiant, string Choixdannee, IFormFile file, IFormFile photo, string x)
		{
			StudentRepository studentRepository = new StudentRepository(_context);
			try
			{
				if (ModelState.IsValid)
				{
					etudiant.Etat = "Inscrit";
					int f = Convert.ToInt32(x);
					etudiant.ID_Filiere = f;
					if (Choixdannee == "3eme")
					{
						studentRepository.InsertStudent3emeAnnee(etudiant);

					}
					else if (Choixdannee.Equals("4eme"))
					{
						studentRepository.InsertStudent4emeAnnee(etudiant);

					}
					if (file != null)
					{
						string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
						string pic = Path.GetFileName(file.FileName);
						string newName = timeStamp + "_" + pic;
						etudiant.Diplome.fichier = newName;
						//string path = Path.Combine(Server.MapPath("~/images/"), newName);
						var path = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
						// file is uploaded
						if (file.Length > 0)
						{
							var filePath = Path.Combine(path, newName);
							using (var fileStream = new FileStream(filePath, FileMode.Create))
							{
								await file.CopyToAsync(fileStream);

							}

						}
						
						//await SendEmailAsync(etudiant.ID);


					}
					if (photo != null)
					{
						string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
						string pic = Path.GetFileName(photo.FileName);
						string newName = timeStamp + "_" + pic;
						etudiant.picture = newName;
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
					 _context.SaveChanges();

					await _producerWrapper.writeMessage(
							new MessageToSend(
							etudiant.ID,
							EmailTypes.SIGNUP
							));
					TempData["Message"] = "success";
					return Redirect("~/auth");
				}
			}
			catch (Exception)
			{
				TempData["Message"] = "error";
				return RedirectToAction("Create");
			}


			return RedirectToAction("Create");


		}
	}
}