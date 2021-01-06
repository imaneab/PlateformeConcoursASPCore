using ProjetConcoursAspCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetConcoursAspCore.Areas.Admin.Repository
{
    public interface IFiliereRepository
    {
        IEnumerable<Filiere> Get();
        Filiere Get(int id);
        string GetFiliereTitre(int id);
        string GetFiliereTitre(int? id);
        void Add(Filiere filiere);
        void Update(Filiere filiere);
        void Remove(Filiere filiere);
        void Save();
    }
}
