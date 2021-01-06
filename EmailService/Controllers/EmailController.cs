using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmailService.Filters;
using EmailService.Models;
using EmailService.Templates;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
namespace EmailService.Controllers
{
	[Route("api/[controller]")]
	[CheckTokenFilter]
    [ApiController]
    public class EmailController : ControllerBase
    {
		private readonly ApplicationContext db;
		private readonly IRazorViewToStringRenderer _renderer;
		private IEmailTemplate template;
		private TemplateFactory templateFactory;
		private readonly Services.EmailService _emailService;
		public EmailController(IRazorViewToStringRenderer renderer, ApplicationContext _db)
		{
			_renderer = renderer;
			_emailService = new Services.EmailService();
			templateFactory = new TemplateFactory(renderer);
			this.db = _db;
		}
		[HttpGet("{type}/{user_id}")]
		public async Task<ActionResult<String>> send(string type,int user_id,string json = "{}")
		{
			Console.WriteLine("Email Service at here \n#############################################################");
			dynamic stuff = JObject.Parse(json);
			Etudiant et = await db.FindAsync<Etudiant>(user_id);
			if (et == null)
				throw new KeyNotFoundException(user_id + " User not found");

			Console.WriteLine(et.Email);

			template = templateFactory.GetTempalte(type, stuff);
			_emailService.send(et.Email, template.GetTitle(), await template.Render(user_id,db,et));
			//return await template.Render(user_id,db,et);
			return "ok";
		}
	}
}