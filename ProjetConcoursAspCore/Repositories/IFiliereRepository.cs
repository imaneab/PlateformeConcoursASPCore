using ProjetConcoursAspCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetConcoursAspCore.Repositories
{
	public interface IFiliereRepository
	{
		List<Filiere> All();
	}
}
