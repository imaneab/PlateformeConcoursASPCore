using Microsoft.EntityFrameworkCore;
using ProjetConcoursAspCore.Areas.Admin.ViewModel;
using ProjetConcoursAspCore.Data;
using ProjetConcoursAspCore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProjetConcoursAspCore.Areas.Admin.Repository
{
    public class Etudiant4aRepository : IEtudiant4aRepository
    {
        private readonly ApplicationDbContext db;
        public Etudiant4aRepository(ApplicationDbContext context)
        {
            db = context;
        }
        
        public void Update(Etudiant4a etudiant)
        {
            db.Entry(etudiant).State = EntityState.Modified;
            db.SaveChanges();
        }
        public IEnumerable<Etudiant4a> GetListAfterDelibByFiliere(int? FiliereID)
        {
            if (FiliereID == null)
            {
                return null;
            }
            return db.Etudiants4a.Include(e => e.Etudiant).Include("Etudiant.Diplome").Include("Etudiant.Note").Include("Etudiant.Filiere")
            .Where(e => e.EtudiantId == e.Etudiant.ID
            && e.Etudiant.ID_Filiere == FiliereID
            && e.EtudiantId == e.Etudiant.Note.EtudiantNoteId
            && e.EtudiantId == e.Etudiant.Diplome.EtudiantDiplomeId
            && e.Etudiant.Etat != "inscrit"
            && !string.IsNullOrEmpty(e.Etudiant.NumDossier)
            )
            .ToList();
        }
        public IEnumerable<Etudiant4a> GetListPreselectionneByFiliere(int FiliereID)
        {
            return db.Etudiants4a.Include(e => e.Etudiant).Include("Etudiant.Diplome").Include("Etudiant.Note").Include("Etudiant.Filiere")
                .Where(e => e.EtudiantId == e.Etudiant.ID
                && e.Etudiant.ID_Filiere == FiliereID
                && e.EtudiantId == e.Etudiant.Note.EtudiantNoteId
                && e.EtudiantId == e.Etudiant.Diplome.EtudiantDiplomeId
                && e.Etudiant.Etat == "preselectionne"
                && !string.IsNullOrEmpty(e.Etudiant.NumDossier))
                .ToList();
        }
        public IEnumerable<Etudiant4a> GetListPreselectionneByFiliere(int? FiliereID)
        {
            if (FiliereID == null)
            {
                return null;
            }
            return db.Etudiants4a.Include(e => e.Etudiant).Include("Etudiant.Diplome").Include("Etudiant.Note").Include("Etudiant.Filiere")
                .Where(e => e.EtudiantId == e.Etudiant.ID
                && e.Etudiant.ID_Filiere == FiliereID
                && e.EtudiantId == e.Etudiant.Note.EtudiantNoteId
                && e.EtudiantId == e.Etudiant.Diplome.EtudiantDiplomeId
                && e.Etudiant.Etat == "preselectionne"
                && !string.IsNullOrEmpty(e.Etudiant.NumDossier))
                .ToList();
        }

        public async System.Threading.Tasks.Task DeliberationMethodAsync(int Filiere, Deliberation4ViewModel model)
        {
            int nbrAdmis = model.NbrPlaces - model.ListeAtt;
            List<Etudiant4a> AllEtudiants = GetListPreselectionneByFiliere(Filiere)
                .OrderBy(e => e.Etudiant.Note.Specialite).ToList();

            if (AllEtudiants.Count >= model.NbrPlaces)
            {
                AllEtudiants = AllEtudiants.Take(model.NbrPlaces).ToList();
                List<Etudiant4a> ListAdmis = AllEtudiants.Take(nbrAdmis).ToList();
                List<Etudiant4a> ListAttentes = AllEtudiants.GetRange(nbrAdmis, model.ListeAtt);
                foreach (var item in ListAdmis)
                {
                    item.Etudiant.Etat = "admis";
                    db.SaveChanges();
                    //await SendEmailAsync(item.EtudiantId, item.Etudiant.ID_Filiere);
                }
                foreach (var item in ListAttentes)
                {
                    item.Etudiant.Etat = "attente";
                    db.SaveChanges();
                }
            }

        }
        public IEnumerable<Etudiant4a> GetListAdmisByFiliere(int? FiliereID)
        {
            if (FiliereID == null)
            {
                return null;
            }
            return db.Etudiants4a.Include(e => e.Etudiant).Include("Etudiant.Diplome").Include("Etudiant.Note").Include("Etudiant.Filiere")
                .Where(e => e.EtudiantId == e.Etudiant.ID
                && e.Etudiant.ID_Filiere == FiliereID
                && e.EtudiantId == e.Etudiant.Note.EtudiantNoteId
                && e.EtudiantId == e.Etudiant.Diplome.EtudiantDiplomeId
                && e.Etudiant.Etat == "admis"
                && !string.IsNullOrEmpty(e.Etudiant.NumDossier)
                ).OrderByDescending(e => (e.Etudiant.Note.NoteMath + e.Etudiant.Note.Specialite) / 2)
                .ToList();
        }
        public IEnumerable<Etudiant4a> GetListAttenteByFiliere(int? FiliereID)
        {
            if (FiliereID == null)
            {
                return null;
            }
            return db.Etudiants4a.Include(e => e.Etudiant).Include("Etudiant.Diplome").Include("Etudiant.Note").Include("Etudiant.Filiere")
                .Where(e => e.EtudiantId == e.Etudiant.ID
                && e.Etudiant.ID_Filiere == FiliereID
                && e.EtudiantId == e.Etudiant.Note.EtudiantNoteId
                && e.EtudiantId == e.Etudiant.Diplome.EtudiantDiplomeId
                && e.Etudiant.Etat == "attente"
                && !string.IsNullOrEmpty(e.Etudiant.NumDossier)
                ).OrderByDescending(e => (e.Etudiant.Note.NoteMath + e.Etudiant.Note.Specialite) / 2)
                .ToList();
        }

        public async System.Threading.Tasks.Task<bool> SendEmailPreselectionAsync(int id, int? FiliereID)
        {
            var client = new HttpClient();
            string nomFiliere = db.Filieres.Find(FiliereID).Titre;
            string url = "http://20.41.74.71:3000/api/email/preselection/" + id + "?json={filiere:\"" + nomFiliere + "\"}";
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("token", "vDdzzWaXf8866Gbsdzfz788221Afd");
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;

        }

        public async System.Threading.Tasks.Task<bool> SendEmailAsync(int id, int? FiliereID)
        {
            var client = new HttpClient();
            string nomFiliere = db.Filieres.Find(FiliereID).Titre;
            string url = "http://20.41.74.71:3000/api/email/accepted/" + id + "?json={filiere:\"" + nomFiliere + "\"}";
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("token", "vDdzzWaXf8866Gbsdzfz788221Afd");
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;

        }

		public IEnumerable<Etudiant4a> GetAllCandidatsData()
		{
			return db.Etudiants4a.Include("Etudiant").Include("Etudiant.Diplome").Include("Etudiant.Note").Include("Etudiant.Filiere").Where(e => e.EtudiantId == e.Etudiant.ID
			&& e.EtudiantId == e.Etudiant.Note.EtudiantNoteId
			&& e.EtudiantId == e.Etudiant.Diplome.EtudiantDiplomeId && e.Etudiant.IsDeleted == false).ToList();
		}

		public Etudiant4a Get(int id)
        {

            return db.Etudiants4a.Include(e => e.Etudiant).Include("Etudiant.Diplome").Include("Etudiant.Note").Include("Etudiant.Filiere").Where(e => e.ID == id && e.EtudiantId == e.Etudiant.ID
             && e.EtudiantId == e.Etudiant.Note.EtudiantNoteId
            && e.EtudiantId == e.Etudiant.Diplome.EtudiantDiplomeId).FirstOrDefault();
        }
        public Etudiant4a Get(int? id)
        {

            return db.Etudiants4a.Include(e => e.Etudiant).Include("Etudiant.Diplome").Include("Etudiant.Note").Include("Etudiant.Filiere").Where(e => e.ID == id && e.EtudiantId == e.Etudiant.ID
             && e.EtudiantId == e.Etudiant.Note.EtudiantNoteId
            && e.EtudiantId == e.Etudiant.Diplome.EtudiantDiplomeId).FirstOrDefault();
        }

        public void Save()
        {
            db.SaveChanges();
        }

		//Recherche Corbeil
		public IEnumerable<Etudiant4a> GetAllCandidatsDataSupp()
		{

			return db.Etudiants4a.Include(e => e.Etudiant).Include("Etudiant.Diplome").Include("Etudiant.Note").Include("Etudiant.Filiere").Where(e => e.EtudiantId == e.Etudiant.ID
			 && e.EtudiantId == e.Etudiant.Note.EtudiantNoteId
			 && e.EtudiantId == e.Etudiant.Diplome.EtudiantDiplomeId && e.Etudiant.IsDeleted == true).ToList();
		}

		//Retourner

		public Etudiant4a GetSup(int id)
        {
            return db.Etudiants4a.Include(e => e.Etudiant).Include("Etudiant.Diplome").Include("Etudiant.Note").Include("Etudiant.Filiere").Where(e => e.ID == id && e.EtudiantId == e.Etudiant.ID
             && e.EtudiantId == e.Etudiant.Note.EtudiantNoteId
            && e.EtudiantId == e.Etudiant.Diplome.EtudiantDiplomeId && e.Etudiant.IsDeleted == true).FirstOrDefault();
        }
        public Etudiant4a GetSup(int? id)
        {

            return db.Etudiants4a.Include(e => e.Etudiant).Include("Etudiant.Diplome").Include("Etudiant.Note").Include("Etudiant.Filiere").Where(e => e.ID == id && e.EtudiantId == e.Etudiant.ID
             && e.EtudiantId == e.Etudiant.Note.EtudiantNoteId
            && e.EtudiantId == e.Etudiant.Diplome.EtudiantDiplomeId && e.Etudiant.IsDeleted == true).FirstOrDefault();
        }

        public void Remove(Etudiant4a etudiant)
        {

            var dip = db.Diplomes.Include(e => e.Etudiant).Where(e => e.EtudiantDiplomeId == etudiant.EtudiantId && e.Etudiant.IsDeleted == true).FirstOrDefault();
            db.Diplomes.Remove(dip);
            var note = db.Notes.Include(e => e.Etudiant).Where(e => e.EtudiantNoteId == etudiant.EtudiantId && e.Etudiant.IsDeleted == true).FirstOrDefault();
            db.Notes.Remove(note);
            var et = db.Etudiants.Where(e => e.ID == etudiant.EtudiantId && e.IsDeleted == true).FirstOrDefault();
            db.Etudiants.Remove(et);
            db.SaveChanges();
        }

        //Preselection

        public IEnumerable<Etudiant4a> GetPreselection(int Filiere, string Diplome)
        {
            return db.Etudiants4a.Include(e => e.Etudiant).Include("Etudiant.Diplome").Include("Etudiant.Note").Include("Etudiant.Filiere").Where(
                p => p.Etudiant.IsDeleted == false && p.Etudiant.ID_Filiere == Filiere && p.Etudiant.Diplome.Titre == Diplome).ToList();
        }
        
		//*******************statistique*****************************/

		public int ListeDUT()
		{
			var ListeDUT = (from e in db.Etudiants4a
							from D in db.Diplomes
							where e.EtudiantId == D.EtudiantDiplomeId && D.Titre == "DUT"
							select e);
			return ListeDUT.Count();
		}

		public int ListeDEUG()
		{
			var ListeDEUG = (from e in db.Etudiants4a
							 from D in db.Diplomes
							 where e.EtudiantId == D.EtudiantDiplomeId && D.Titre == "DEUG"
							 select e);
			return ListeDEUG.Count();
		}

		public int ListeDEUST()
		{
			var ListeDEUST = (from e in db.Etudiants4a
							  from D in db.Diplomes
							  where e.EtudiantId == D.EtudiantDiplomeId && D.Titre == "DEUST"
							  select e);
			return ListeDEUST.Count();
		}

		public int ListeLF()
		{
			var ListeLF = (from e in db.Etudiants4a
						   from D in db.Diplomes
						   where e.EtudiantId == D.EtudiantDiplomeId && D.Titre == "LF"
						   select e);
			return ListeLF.Count();
		}

		public int ListeLP()
		{
			var ListeLP = (from e in db.Etudiants4a
						   from D in db.Diplomes
						   where e.EtudiantId == D.EtudiantDiplomeId && D.Titre == "LP"
						   select e);
			return ListeLP.Count();
		}

		public int ListeGINFLP()
		{
			var ListeGINFLP = (from e in db.Etudiants4a
							   from D in db.Diplomes
							   where e.EtudiantId== D.EtudiantDiplomeId && D.Titre == "LP" && e.Etudiant.ID_Filiere == 1
							   select e);
			return ListeGINFLP.Count();
		}

		public int ListeGINDLP()
		{
			var ListeGINDLP = (from e in db.Etudiants4a
							   from D in db.Diplomes
							   where e.EtudiantId== D.EtudiantDiplomeId && D.Titre == "LP" && e.Etudiant.ID_Filiere == 2
							   select e);
			return ListeGINDLP.Count();
		}

		public int ListeGPMCLP()
		{
			var ListeGPMCLP = (from e in db.Etudiants4a
							   from D in db.Diplomes
							   where e.EtudiantId== D.EtudiantDiplomeId && D.Titre == "LP" && e.Etudiant.ID_Filiere == 3
							   select e);
			return ListeGPMCLP.Count();
		}

		public int ListeGTRLP()
		{
			var ListeGTRLP = (from e in db.Etudiants4a
							  from D in db.Diplomes
							  where e.EtudiantId== D.EtudiantDiplomeId && D.Titre == "LP" && e.Etudiant.ID_Filiere == 4
							  select e);
			return ListeGTRLP.Count();
		}

		public int ListeGINFLF()
		{
			var ListeGINFLF = (from e in db.Etudiants4a
							   from D in db.Diplomes
							   where e.EtudiantId== D.EtudiantDiplomeId && D.Titre == "LF" && e.Etudiant.ID_Filiere == 1
							   select e);
			return ListeGINFLF.Count();
		}

		public int ListeGINDLF()
		{
			var ListeGINDLF = (from e in db.Etudiants4a
							   from D in db.Diplomes
							   where e.EtudiantId== D.EtudiantDiplomeId && D.Titre == "LF" && e.Etudiant.ID_Filiere == 2
							   select e);
			return ListeGINDLF.Count();
		}

		public int ListeGPMCLF()
		{
			var ListeGPMCLF = (from e in db.Etudiants4a
							   from D in db.Diplomes
							   where e.EtudiantId== D.EtudiantDiplomeId && D.Titre == "LF" && e.Etudiant.ID_Filiere == 3
							   select e);
			return ListeGPMCLF.Count();
		}

		public int ListeGTRLF()
		{
			var ListeGTRLF = (from e in db.Etudiants4a
							  from D in db.Diplomes
							  where e.EtudiantId== D.EtudiantDiplomeId && D.Titre == "LF" && e.Etudiant.ID_Filiere == 4
							  select e);
			return ListeGTRLF.Count();
		}
		//******************************** zayda *****************************//
		public int ListeGINFLST()
		{
			var ListeGINFLF = (from e in db.Etudiants4a
							   from D in db.Diplomes
							   where e.EtudiantId== D.EtudiantDiplomeId && D.Titre == "LST" && e.Etudiant.ID_Filiere == 1
							   select e);
			return ListeGINFLF.Count();
		}

		public int ListeGINDLST()
		{
			var ListeGINDLF = (from e in db.Etudiants4a
							   from D in db.Diplomes
							   where e.EtudiantId== D.EtudiantDiplomeId && D.Titre == "LST" && e.Etudiant.ID_Filiere == 2
							   select e);
			return ListeGINDLF.Count();
		}

		public int ListeGPMCLST()
		{
			var ListeGPMCLF = (from e in db.Etudiants4a
							   from D in db.Diplomes
							   where e.EtudiantId== D.EtudiantDiplomeId && D.Titre == "LST" && e.Etudiant.ID_Filiere == 3
							   select e);
			return ListeGPMCLF.Count();
		}

		public int ListeGTRLST()
		{
			var ListeGTRLF = (from e in db.Etudiants4a
							  from D in db.Diplomes
							  where e.EtudiantId== D.EtudiantDiplomeId && D.Titre == "LST" && e.Etudiant.ID_Filiere == 4
							  select e);
			return ListeGTRLF.Count();
		}

		public int ListeGINFLSTC()
		{
			var ListeGINFLPC = (from e in db.Etudiants4a
								from D in db.Diplomes
								where e.EtudiantId== D.EtudiantDiplomeId && D.Titre == "LST" && e.Etudiant.ID_Filiere == 1
								&& e.Etudiant.Etat == "preselectionne"
								select e);
			return ListeGINFLPC.Count();
		}

		public int ListeGINDLSTC()
		{
			var ListeGINDLPC = (from e in db.Etudiants4a
								from D in db.Diplomes
								where e.EtudiantId== D.EtudiantDiplomeId && D.Titre == "LST" && e.Etudiant.ID_Filiere == 2
								&& e.Etudiant.Etat == "preselectionne"
								select e);
			return ListeGINDLPC.Count();
		}

		public int ListeGPMCLSTC()
		{
			var ListeGPMCLPC = (from e in db.Etudiants4a
								from D in db.Diplomes
								where e.EtudiantId== D.EtudiantDiplomeId && D.Titre == "LST" && e.Etudiant.ID_Filiere == 3
								&& e.Etudiant.Etat == "preselectionne"
								select e);
			return ListeGPMCLPC.Count();
		}

		public int ListeGTRLSTC()
		{
			var ListeGTRLPC = (from e in db.Etudiants4a
							   from D in db.Diplomes
							   where e.EtudiantId== D.EtudiantDiplomeId && D.Titre == "LST" && e.Etudiant.ID_Filiere == 4
							   && e.Etudiant.Etat == "preselectionne"
							   select e);
			return ListeGTRLPC.Count();
		}

		//*********************************************************************//

		public int ListeGINFLPC()
		{
			var ListeGINFLPC = (from e in db.Etudiants4a
								from D in db.Diplomes
								where e.EtudiantId== D.EtudiantDiplomeId && D.Titre == "LP" && e.Etudiant.ID_Filiere == 1
								&& e.Etudiant.Etat == "preselectionne"
								select e);
			return ListeGINFLPC.Count();
		}

		public int ListeGINDLPC()
		{
			var ListeGINDLPC = (from e in db.Etudiants4a
								from D in db.Diplomes
								where e.EtudiantId== D.EtudiantDiplomeId && D.Titre == "LP" && e.Etudiant.ID_Filiere == 2
								&& e.Etudiant.Etat == "preselectionne"
								select e);
			return ListeGINDLPC.Count();
		}

		public int ListeGPMCLPC()
		{
			var ListeGPMCLPC = (from e in db.Etudiants4a
								from D in db.Diplomes
								where e.EtudiantId== D.EtudiantDiplomeId && D.Titre == "LP" && e.Etudiant.ID_Filiere == 3
								&& e.Etudiant.Etat == "preselectionne"
								select e);
			return ListeGPMCLPC.Count();
		}

		public int ListeGTRLPC()
		{
			var ListeGTRLPC = (from e in db.Etudiants4a
							   from D in db.Diplomes
							   where e.EtudiantId== D.EtudiantDiplomeId && D.Titre == "LP" && e.Etudiant.ID_Filiere == 4
							   && e.Etudiant.Etat == "preselectionne"
							   select e);
			return ListeGTRLPC.Count();
		}

		public int ListeGINFLFC()
		{
			var ListeGINFLFC = (from e in db.Etudiants4a
								from D in db.Diplomes
								where e.EtudiantId== D.EtudiantDiplomeId && D.Titre == "LF" && e.Etudiant.ID_Filiere == 1
								&& e.Etudiant.Etat == "preselectionne"
								select e);
			return ListeGINFLFC.Count();
		}

		public int ListeGINDLFC()
		{
			var ListeGINDLFC = (from e in db.Etudiants4a
								from D in db.Diplomes
								where e.EtudiantId== D.EtudiantDiplomeId && D.Titre == "LF" && e.Etudiant.ID_Filiere == 2
								&& e.Etudiant.Etat == "preselectionne"
								select e);
			return ListeGINDLFC.Count();
		}


		public int ListeGPMCLFC()
		{
			var ListeGPMCLFC = (from e in db.Etudiants4a
								from D in db.Diplomes
								where e.EtudiantId== D.EtudiantDiplomeId && D.Titre == "LF" && e.Etudiant.ID_Filiere == 3
								&& e.Etudiant.Etat == "preselectionne"
								select e);
			return ListeGPMCLFC.Count();
		}

		public int ListeGTRLFC()
		{
			var ListeGTRLFC = (from e in db.Etudiants4a
							   from D in db.Diplomes
							   where e.EtudiantId== D.EtudiantDiplomeId && D.Titre == "LF" && e.Etudiant.ID_Filiere == 4
							   && e.Etudiant.Etat == "preselectionne"
							   select e);
			return ListeGTRLFC.Count();
		}
		public Etudiant4a GetByNumDossier(string NumDossier)
		{
			return db.Etudiants4a.Where(e => e.Etudiant.NumDossier == NumDossier).FirstOrDefault();
		}
		//Rechereche pour N°Dossier
		public IEnumerable<Etudiant4a> GetAllCandidatsDataNumDossier()
		{
			return db.Etudiants4a.Include("Etudiant.Diplome").Include("Etudiant.Note").Include("Etudiant.Filiere").Where(e => e.EtudiantId == e.Etudiant.ID
		 && e.Etudiant.Etat == "preselectionne"
		 && e.EtudiantId == e.Etudiant.Note.EtudiantNoteId
		 && e.EtudiantId == e.Etudiant.Diplome.EtudiantDiplomeId)
		 .OrderBy(e => e.Etudiant.Cne)
		 .ToList();

		}
		public Etudiant4a GetAllCandidatsData2(int? id)
		{

			return db.Etudiants4a.Include(e => e.Etudiant).Where(e => e.Etudiant.ID == id && e.Etudiant.IsDeleted == false && e.Etudiant.Etat == "preselectionne").FirstOrDefault();
		}

		public Etudiant4a GetAllCandidatsData2(int id)
		{

			return db.Etudiants4a.Include(e => e.Etudiant).Where(e => e.Etudiant.ID == id && e.Etudiant.IsDeleted == false && e.Etudiant.Etat == "preselectionne").FirstOrDefault();
		}

        public int CountTotale()
        {
            int nbrTotale4 = db.Etudiants4a.Count();
            return nbrTotale4;
        }


        // nombre des inscrits en 3ieme année informatique

        public int CountInfo()
        {
            int nbrInfo4 = db.Etudiants4a.Where(s => s.Etudiant.ID_Filiere == 1).Count();
            return nbrInfo4;
        }

        // nombre des inscrits en 4ieme année industriel

        public int CountIndus()
        {
            int nbrIndus4 = db.Etudiants4a.Where(s => s.Etudiant.ID_Filiere == 2).Count();
            return nbrIndus4;
        }

        // nombre des inscrits en 4ieme année Procedes

        public int CountProcede()
        {
            int nbrProcede4 = db.Etudiants4a.Where(s => s.Etudiant.ID_Filiere == 3).Count();
            return nbrProcede4;
        }

        // nombre des inscrits en 4ieme année Telecom

        public int CountTelecom()
        {
            int nbrTelecom4 = db.Etudiants4a.Where(s => s.Etudiant.ID_Filiere == 4).Count();
            return nbrTelecom4;
        }

        // nombre des 4ieme annee supprimés (ya encore un probleme ici !! mabghach i tester l egalité d bool)

        public int CountTotaleSupprime()
        {
            int nbrTotale4Supprime = db.Etudiants4a.Where(s => s.Etudiant.IsDeleted == true).Count();
            return nbrTotale4Supprime;

        }

    }
}
