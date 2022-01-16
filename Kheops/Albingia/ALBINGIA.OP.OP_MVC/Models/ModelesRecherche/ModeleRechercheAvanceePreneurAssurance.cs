using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using System.ComponentModel.DataAnnotations;
using ALBINGIA.OP.OP_MVC.Models.ModeleTransverse;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesRecherche
{
    [Serializable]
    public class ModeleRechercheAvanceePreneurAssurance : MetaModelsBase
    {
        public List<ModeleCommonCabinetPreneur> PreneursAssurance;
        [Display(Name="Nom du Preneur")]
        public string NomPreneurAssuranceRecherche;
        public int NbCount { get; set; }
        public int StartLigne { get; set; }
        public int EndLigne { get; set; }
        public int PageNumber { get; set; }
        public int LineCount { get; set; }
        public string Order { get; set; }
        public int By { get; set; }
              
    }
}