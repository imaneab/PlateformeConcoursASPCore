using ProjetConcoursAspCore.Areas.Admin.ViewModel;
using ProjetConcoursAspCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetConcoursAspCore.Areas.Admin.Repository
{
    public interface IEtudiant4aRepository
    {
        Etudiant4a Get(int id);
        Etudiant4a Get(int? id);
        void Update(Etudiant4a etudiant);
        void Remove(Etudiant4a etudiant);
        IEnumerable<Etudiant4a> GetListPreselectionneByFiliere(int? FiliereID);
        IEnumerable<Etudiant4a> GetListPreselectionneByFiliere(int FiliereID);
        IEnumerable<Etudiant4a> GetListAfterDelibByFiliere(int? FiliereID);
        System.Threading.Tasks.Task DeliberationMethodAsync(int Filiere, Deliberation4ViewModel model);
        IEnumerable<Etudiant4a> GetListAdmisByFiliere(int? FiliereID);
        IEnumerable<Etudiant4a> GetListAttenteByFiliere(int? FiliereID);
        Task<bool> SendEmailPreselectionAsync(int id, int? FiliereID);
        Task<bool> SendEmailAsync(int id, int? FiliereID);
        IEnumerable<Etudiant4a> GetAllCandidatsData();
        void Save();
        IEnumerable<Etudiant4a> GetAllCandidatsDataNumDossier();
        Etudiant4a GetByNumDossier(string NumDossier);
        IEnumerable<Etudiant4a> GetAllCandidatsDataSupp();
        Etudiant4a GetAllCandidatsData2(int? id);
        Etudiant4a GetAllCandidatsData2(int id);
        Etudiant4a GetSup(int id);
        Etudiant4a GetSup(int? id);
        IEnumerable<Etudiant4a> GetPreselection(int Filiere, string Diplome);
        int ListeDUT();
        int ListeDEUG();
        int ListeDEUST();
        int ListeLF();
        int ListeLP();
        int ListeGINFLP();
        int ListeGINDLP();
        int ListeGPMCLP();
        int ListeGTRLP();
        int ListeGINFLF();
        int ListeGINDLF();
        int ListeGPMCLF();
        int ListeGTRLF();
        int ListeGINFLPC();
        int ListeGINDLPC();
        int ListeGPMCLPC();
        int ListeGTRLPC();
        int ListeGINFLFC();
        int ListeGINDLFC();
        int ListeGPMCLFC();
        int ListeGTRLFC();
        int ListeGINFLST();
        int ListeGINDLST();
        int ListeGPMCLST();
        int ListeGTRLST();
        int ListeGINFLSTC();
        int ListeGINDLSTC();
        int ListeGPMCLSTC();
        int ListeGTRLSTC();
        int CountTotale();
        int CountInfo();
        int CountIndus();
        int CountProcede();
        int CountTelecom();
        int CountTotaleSupprime();
    }
}
