using ProjetConcoursAspCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetConcoursAspCore.ViewModel
{
    public class RegisterViewModel
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Cne { get; set; }
        public string Cin { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Nationnalite { get; set; }
        public string Civilite { get; set; }
        public DateTime DateNaiss { get; set; }
        public string Etablissement { get; set; }
        public int FiliereID { get; set; }
        public int Bac { get; set; }
        [Range(1, 20)]
        public int S1 { get; set; }
        [Range(1, 20)]
        public int S2 { get; set; }
        [Range(1, 20)]
        public int S3 { get; set; }
        [Range(1, 20)]
        public int S4 { get; set; }
        [Range(1, 20)]
        public int S5 { get; set; }
        [Range(1, 20)]
        public int S6 { get; set; }
        public bool Redoublant1 { get; set; }
        public bool Redoublant2 { get; set; }
        public bool Redoublant3 { get; set; }
        public string Titre { get; set; }
        public int Annee1 { get; set; }
        public int Annee2 { get; set; }
        public int Annee3 { get; set; }
        public int AnneeBac { get; set; }
        //public string fichier { get; set; }

        
    }
}
