using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailService.Models;
using EmailService.ViewModel;

namespace EmailService.Templates
{
	public class AcceptedTemplate : IEmailTemplate
	{
		private IRazorViewToStringRenderer razor;
		private dynamic json;

		public AcceptedTemplate(IRazorViewToStringRenderer razor, dynamic json)
		{
			this.razor = razor;
			this.json = json;
		}
		public object DbProcess(ApplicationContext db, object model)
		{
			Etudiant et = (Etudiant)model;
			et.Etat = "admis";
			db.Update(et);
			db.SaveChanges();
			return null;
		}

		public string GetTitle()
		{
			return "Vous avez réussi le concour";
		}

		public string GetView()
		{
			return "accepted";
		}

		public Task<string> Render(int user_id, ApplicationContext db, object model)
		{
			string filiere = json.filiere;
			if(filiere==null)
			{
				throw new Exception("Filiere field is required");
			}
			this.DbProcess(db, model);
			Etudiant et = (Etudiant)model;
			AcceptedViewModel vm = new AcceptedViewModel();
			vm.FullName = et.NomComplet;
			vm.Filiere = filiere;
			return razor.RenderViewToStringAsync(GetView(), vm);
		}
	}
}
