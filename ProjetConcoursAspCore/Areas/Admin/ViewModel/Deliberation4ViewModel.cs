using ProjetConcoursAspCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetConcoursAspCore.Areas.Admin.ViewModel
{
    public class Deliberation4ViewModel
    {
        public int NbrPlaces { get; set; }
        public int ListeAtt { get; set; }
        public Deliberation4ViewModel()
        {
            NbrPlaces = 1;
            ListeAtt = 1;
        }
    }
}
