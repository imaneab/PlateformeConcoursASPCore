using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjetConcoursAspCore.Models;
using ProjetConcoursAspCore.Areas.Admin.ViewModel;
using Microsoft.AspNetCore.Authorization;
using ProjetConcoursAspCore.Areas.Admin.Repository;

namespace ProjetConcoursAspCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminPolicy")]
    public class HomeController : Controller
    {
        private IEtudiant3aRepository repository3;
        private IEtudiant4aRepository repository4;
        private IFiliereRepository FilRepo;
        private readonly ILogger<HomeController> _logger;
        public HomeController(IEtudiant3aRepository repository3, IEtudiant4aRepository repository4, IFiliereRepository FilRepo, ILogger<HomeController> logger)
        {
            _logger = logger;
            this.repository3 = repository3;
            this.repository4 = repository4;
            this.FilRepo = FilRepo;
        }
        //[Authorize]
        public ActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel()
            {
                Preselectionne3Info = repository3.GetListPreselectionneByFiliere(1),
                Preselectionne3Indus = repository3.GetListPreselectionneByFiliere(2),
                Preselectionne3Procede = repository3.GetListPreselectionneByFiliere(3),
                Preselectionne3Telecom = repository3.GetListPreselectionneByFiliere(4),

                Preselectionne4Info = repository4.GetListPreselectionneByFiliere(1),
                Preselectionne4Indus = repository4.GetListPreselectionneByFiliere(2),
                Preselectionne4Procede = repository4.GetListPreselectionneByFiliere(3),
                Preselectionne4Telecom = repository4.GetListPreselectionneByFiliere(4),
                // nombre des inscrits en 3ieme année
                NbrTotal3 = repository3.CountTotale(),
                // nombre des inscrits en 3ieme année informatique
                NbrInfo3 = repository3.CountInfo(),
                // nombre des inscrits en 3ieme année industriel
                NbrIndus3 = repository3.CountIndus(),
                // nombre des inscrits en 3ieme année Procedes
                NbrProcede3 = repository3.CountProcede(),
                // nombre des inscrits en 3ieme année Telecom
                NbrTelecom3 = repository3.CountTelecom(),
                // nombre des 3ieme annee supprimés
                NbrTotalSupp3 = repository3.CountTotaleSupprime(),



                // nombre des inscrits en 4ieme année
                NbrTotal4 = repository4.CountTotale(),
                // nombre des inscrits en 3ieme année informatique
                NbrInfo4 = repository4.CountInfo(),
                // nombre des inscrits en 3ieme année industriel
                NbrIndus4 = repository4.CountIndus(),
                // nombre des inscrits en 3ieme année Procedes
                NbrProcede4 = repository4.CountProcede(),
                // nombre des inscrits en 3ieme année Telecom
                NbrTelecom4 = repository4.CountTelecom(),
                // nombre des 3ieme annee supprimés
                NbrTotalSupp4 = repository4.CountTotaleSupprime(),



                //nombre totale des utilisateurs
                NbrTotal = repository3.CountTotale()+repository4.CountTotale(),

                //Chart
                Datapoints = repository3.Chart(),

            };
            //return Content(Convert.ToString(homeViewModel.NbrTotal4));
            return View(homeViewModel);
        }
        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        //public IActionResult Index()
        //{
        //    return View();
        //}

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