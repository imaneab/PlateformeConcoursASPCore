using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjetConcoursAspCore.Data;
using ProjetConcoursAspCore.Repositories;
using ProjetConcoursAspCore.ViewModel;

namespace ProjetConcoursAspCore.Controllers
{
	public class AuthController : Controller
	{
        private readonly ILogger<AuthController> logger;
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _hostingEnvironment;
        private StudentRepository studentRepository;

        public static string CalculateMD5Hash(string input)
        {
			// step 1, calculate MD5 hash from input
			System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
        public AuthController(ILogger<AuthController> logger, ApplicationDbContext context, IWebHostEnvironment environment)
		{
            this.logger = logger;
            _context = context;
            _hostingEnvironment = environment;

            studentRepository  = new StudentRepository(_context);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
		{
            return View("~/Views/Home/login.cshtml", new LoginPageViewModel(null));
		}

		// GET: AuthController
		[HttpPost]
		[ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string cne,string cin,string password, string ReturnUrl = null)
		{

            this.logger.LogInformation(String.Format("URL {0}",ReturnUrl));
            if (ValidateLogin(cne, cin, password))
            {
                var claims = new List<Claim>
                {
                    new Claim("cne", cne,ClaimValueTypes.String),
                    new Claim("role", "Member",ClaimValueTypes.String),
                    new Claim("IsMember", "true",ClaimValueTypes.String)
                };
                var userIdentity = new ClaimsIdentity(claims, Startup.AuthSchema);
                var userPrincipal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(Startup.AuthSchema, userPrincipal);
                if (Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    return Redirect("/");
                }
            }

            return View("~/Views/Home/login.cshtml",
                new LoginPageViewModel(
                    "Vos informations d'identification ne sont pas correctes.",
                    cne,
                    cin
                    ));
        }

        public IActionResult Denied()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            foreach (var cookie in HttpContext.Request.Cookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
        public IActionResult AccessDenied(string returnUrl = null)
        {
            return Content("Access Denied");
        }
        private bool ValidateLogin(string cne, string cin, string password)
		{
            var etudiant = _context.Etudiants.FirstOrDefault(e => e.Cne.Equals(cne));
            
            if (etudiant == null || cne == null || cin == null || password == null)
                return false;
            if (etudiant.Cin == null || etudiant.Password == null)
                return false;
            return etudiant.Cin.Equals(cin) && etudiant.Password.Equals(CalculateMD5Hash(password));
		}
	}
}
