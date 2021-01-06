using ProjetConcoursAspCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetConcoursAspCore.ViewModel
{
	public class ProfileViewModel
	{
		public Etudiant etudiant{ get; set; }


		public ProfileViewModel(Etudiant etudiant)
		{
			this.etudiant = etudiant;
		}
	}
}
