using ProjetConcoursAspCore.Areas.Admin.ViewModel;
using ProjetConcoursAspCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetConcoursAspCore.Areas.Admin.Repository
{
    public interface IEtudiant3aRepository
    {
        Etudiant3a Get(int id);
        Etudiant3a Get(int? id);
        void Update(Etudiant3a etudiant);
        void Remove(Etudiant3a etudiant);
        IEnumerable<Etudiant3a> GetListPreselectionneByFiliere(int? FiliereID);
        IEnumerable<Etudiant3a> GetListPreselectionneByFiliere(int FiliereID);
        IEnumerable<Etudiant3a> GetListAfterDelibByFiliere(int? FiliereID);
        System.Threading.Tasks.Task DeliberationMethodAsync(int Filiere, DeliberationViewModel model);
        IEnumerable<Etudiant3a> GetListAdmisByFiliere(int? FiliereID);
        IEnumerable<Etudiant3a> GetListAttenteByFiliere(int? FiliereID);
        Task<bool> SendEmailPreselectionAsync(int id, int? FiliereID);
        Task<bool> SendEmailAsync(int id, int? FiliereID);
		Etudiant3a GetByNumDossier(string NumDossier);
		public IEnumerable<Etudiant3a> GetAllCandidatsDataNumDossier();
		Etudiant3a GetAllCandidatsData2(int? id);
		Etudiant3a GetAllCandidatsData2(int id);
		IEnumerable<Etudiant3a> GetAllCandidatsData();
        void Save();
        IEnumerable<Etudiant3a> GetAllCandidatsDataSupp();
        Etudiant3a GetSup(int id);
        Etudiant3a GetSup(int? id);
        IEnumerable<Etudiant3a> GetPreselection(int Filiere, string Diplome);
		int ListeDUT();
		int ListeDEUG();
		int ListeDEUST();
		int ListeLF();
		int ListeLP();
		int ListeGINFDUT();
		int ListeGINDDUT();
		int ListeGPMCDUT();
		int ListeGTRDUT();
		int ListeGINFDEUG();
		int ListeGINDDEUG();
		int ListeGPMCDEUG();
		int ListeGTRDEUG();
		int ListeGINFDEUST();
		int ListeGINDDEUST();
		int ListeGPMCDEUST();
		int ListeGTRDEUST();
		int ListeGINFLP();
		int ListeGINDLP();
		int ListeGPMCLP();
		int ListeGTRLP();
		int ListeGINFLF();
		int ListeGINDLF();
		int ListeGPMCLF();
		int ListeGTRLF();
		int ListeGINFDUTC();
		int ListeGINDDUTC();
		int ListeGPMCDUTC();
		int ListeGTRDUTC();
		int ListeGINFDEUGC();
		int ListeGINDDEUGC();
		int ListeGPMCDEUGC();
		int ListeGTRDEUGC();
		int ListeGINFDEUSTC();
		int ListeGINDDEUSTC();
		int ListeGPMCDEUSTC();
		int ListeGTRDEUSTC();
		int ListeGINFLPC();
		int ListeGINDLPC();
		int ListeGPMCLPC();
		int ListeGTRLPC();
		int ListeGINFLFC();
		int ListeGINDLFC();
		int ListeGPMCLFC();
		int ListeGTRLFC();


        int CountTotale();
        int CountInfo();
        int CountIndus();
        int CountProcede();
        int CountTelecom();
        int CountTotaleSupprime();
        public string Chart();

    }
}
