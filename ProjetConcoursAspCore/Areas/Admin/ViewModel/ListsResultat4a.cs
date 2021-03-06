﻿using ProjetConcoursAspCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetConcoursAspCore.Areas.Admin.ViewModel
{
    public class ListsResultat4a
    {
        public int? FiliereId { get; set; }
        public string Titre { get; set; }
        public List<Etudiant4a> ListAdmis { get; set; }
        public List<Etudiant4a> ListAttente { get; set; }
        public ListsResultat4a()
        {
            ListAdmis = new List<Etudiant4a>();
            ListAttente = new List<Etudiant4a>();
        }
    }
}
