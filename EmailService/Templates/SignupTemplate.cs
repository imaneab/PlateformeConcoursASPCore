using EmailService.Models;
using EmailService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailService.Templates
{
	public class SignupTemplate : IEmailTemplate
	{
		private IRazorViewToStringRenderer razor;
		private dynamic json;

		public SignupTemplate(IRazorViewToStringRenderer razor,dynamic json)
		{
			this.razor = razor;
			this.json = json;
		}
		public string GetView()
		{
			return "signup";
		}
		public string GetTitle()
		{
			return "Vérification d'inscription";
		}
		public Task<string> Render(int user_id, ApplicationContext db,object model)
		{
			Etudiant etudiant = (Etudiant)model;
			string password = (string)this.DbProcess(db, etudiant);
			SignupViewModel vm = new SignupViewModel();
			vm.Email = etudiant.Email;
			vm.Password = password;
			vm.FullName = etudiant.NomComplet;
			return razor.RenderViewToStringAsync(GetView(), vm);
		}

		public Object DbProcess(ApplicationContext db, object model)
		{
			Etudiant etudiant = (Etudiant)model;
			string password = Utils.Utils.RandomString(8);
			etudiant.Password = Utils.Utils.CalculateMD5Hash(password);
			etudiant.Etat = "Inscrit";
			db.Update(etudiant);
			db.SaveChanges();
			return password;
		}
	}
}
