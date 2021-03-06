﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetConcoursAspCore.Areas.Admin.ViewModel
{
    public class CorrectionViewModel
    {
        public int ID { get; set; }
        public string NumDossier { get; set; }
        public string Cin { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Diplome { get; set; }
        public string Filiere { get; set; }
        [Range(1, 20, ErrorMessage = "Entrer une note valide")]
        public double? NoteMath { get; set; }
        public double? NoteSpecialite { get; set; }



    }
}
