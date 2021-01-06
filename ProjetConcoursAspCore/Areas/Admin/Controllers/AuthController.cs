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
using ProjetConcoursAspCore.Areas.Admin.ViewModel;
using ProjetConcoursAspCore.Data;
using ProjetConcoursAspCore.Repositories;

namespace ProjetConcoursAspCore.Areas.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
	{
        private readonly ILogger<AuthController> logger;
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _hostingEnvironment;
        private StudentRepository studentRepository;

   //     public static string CalculateMD5Hash(string input)
   //     {
			//// step 1, calculate MD5 hash from input
			//System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
   //         byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
   //         byte[] hash = md5.ComputeHash(inputBytes);

   //         // step 2, convert byte array to hex string
   //         StringBuilder sb = new StringBuilder();
   //         for (int i = 0; i < hash.Length; i++)
   //         {
   //             sb.Append(hash[i].ToString("X2"));
   //         }
   //         return sb.ToString();
   //     }



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
            return View(new AdminViewModel(null));
		}

        // GET: AuthController
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string login, string password, string ReturnUrl = null)
        {

            this.logger.LogInformation(String.Format("URL {0}", ReturnUrl));
            if (ValidateLogin(login, password))
            {
                var claims = new List<Claim>
                {
                    new Claim("login", login,ClaimValueTypes.String),
                    new Claim("role", "Admin",ClaimValueTypes.String),
                    new Claim("IsAdmin", "true",ClaimValueTypes.String)
                };
                var userIdentity = new ClaimsIdentity(claims, Startup.AuthSchema);
                var userPrincipal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(Startup.AuthSchema, userPrincipal);
                if (Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect("/admin/");
                }
                else
                {
                    return Redirect("/admin");
                }
            }

            return RedirectToAction("index");
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            foreach (var cookie in HttpContext.Request.Cookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }
            await HttpContext.SignOutAsync();
            return Redirect("/admin/auth");
        }
        public IActionResult AccessDenied(string returnUrl = null)
        {
            return Content("Access Denied");
        }
        private bool ValidateLogin(string Login, string password)
		{
            var etudiant = _context.Admins.FirstOrDefault(e => e.Login.Equals(Login));
            
            if (etudiant == null || Login == null || password == null)
                return false;
            if (etudiant.Password == null)
                return false;
            return etudiant.Login.Equals(Login) && etudiant.Password.Equals(password);
		}
	}
}
