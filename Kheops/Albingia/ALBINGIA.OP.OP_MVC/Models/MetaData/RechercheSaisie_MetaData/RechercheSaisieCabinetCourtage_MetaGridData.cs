using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ALBINGIA.OP.OP_MVC.Models.MetaData.SaisieCreation_MetaData;
using System.ComponentModel.DataAnnotations;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.RechercheSaisie_MetaData
{
    public class RechercheSaisieCabinetCourtage_MetaGridData : List<CabinetCourtage_JSON_MetaData>
    {
        [Display(Name = "Nom Cabinet")]
        public string NomCabinetRecherche { get; set; }
        public int NbCount { get; set; }
        public RechercheSaisieCabinetCourtage_MetaGridData() : base() { }
        public RechercheSaisieCabinetCourtage_MetaGridData(int capacity) : base(capacity) { }
        public RechercheSaisieCabinetCourtage_MetaGridData(IEnumerable<CabinetCourtage_JSON_MetaData> items) : base(items) { }
    }
}