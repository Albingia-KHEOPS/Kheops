using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesConnexite
{
    public class ModeleDetailsConnexitesEngPeriode
    {
        public Int64 Code { get; set; }
        public bool Actif { get; set; }
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public bool Utilise { get; set; }
        public bool IsReadOnly { get; set; }
        public List<ModeleEngTrait> LstEngagmentTraite { get; set; }

       
    }
}