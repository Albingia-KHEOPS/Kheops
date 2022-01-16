using ALBINGIA.OP.OP_MVC.Models.ModeleTransverse;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesRecherche
{
    [Serializable]
    public class ModeleRechercheAvancee 
    {
        public List<ModeleCommonCabinetPreneur> CabinetsPreneurs;
        public string CodeCabinetRecherche;
        public string NomCabinetRecherche;
        public int NbCount { get; set; }
        public int StartLigne { get; set; }
        public int EndLigne { get; set; }
        public int PageNumber { get; set; }
        public int LineCount { get; set; }
        public string Order { get; set; }
        public int By { get; set; }
        [Display(Name = "Nom du Preneur")]
        public string NomPreneurAssuranceRecherche;
        public string CodePreneurAssuranceRecherche;
        public string Contexte { get; set; }
        public string CPPreneurAssuranceRecherche { get; set; }
        public bool IsSinglePage => NbCount <= MvcApplication.PAGINATION_PAGE_SIZE;

        public bool IsLastPage { get; internal set; }
    }
}