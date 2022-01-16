using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ALBINGIA.OP.OP_MVC.Models.MetaData.SaisieCreation_MetaData;
using System.ComponentModel.DataAnnotations;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.RechercheSaisie_MetaData
{
    public class RechercheSaisiePreneurAssurance_MetaGridData : List<PreneurAssurance_JSON_MetaData>
    {
        [Display(Name = "Nom Preneur Assurance")]
        public string NomPreneurAssuranceRecherche { get; set; }
        public int NbCount { get; set; }
        public RechercheSaisiePreneurAssurance_MetaGridData() : base() { }
        public RechercheSaisiePreneurAssurance_MetaGridData(int capacity) : base(capacity) { }
        public RechercheSaisiePreneurAssurance_MetaGridData(IEnumerable<PreneurAssurance_JSON_MetaData> items) : base(items) { }
    }
}