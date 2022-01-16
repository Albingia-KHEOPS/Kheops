using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.ModelesLCIFranchise;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleConditionsGarantie
{
    [Serializable]
    public class ModeleConditionsInfosContrat
    {
        [Display(Name = "LCI Gale")]
        public bool IsLCI { get; set; }
        public string LCI { get; set; }
        [Display(Name = "Unité")]
        public string UniteLCI { get; set; }
        public List<AlbSelectListItem> UnitesLCI { get; set; }
        [Display(Name = "Type")]
        public string TypeLCI { get; set; }
        public List<AlbSelectListItem> TypesLCI { get; set; }
        [Display(Name = "Indexée")]
        public bool IsIndexeLCI { get; set; }

        [Display(Name = "Franchise Gale")]
        public bool IsFranchise { get; set; }
        public string Franchise { get; set; }
        [Display(Name = "Unité")]
        public string UniteFranchise { get; set; }
        public List<AlbSelectListItem> UnitesFranchise { get; set; }
        [Display(Name = "Type")]
        public string TypeFranchise { get; set; }
        public List<AlbSelectListItem> TypesFranchise { get; set; }
        [Display(Name = "Indexée")]
        public bool IsIndexeFranchise { get; set; }

        [Display(Name = "Expresssion de l'assiette")]
        public string ExpAssiette { get; set; }

        #region LCIFranchise     
        public ModeleLCIFranchise LCIGenerale { get; set; }
        public ModeleLCIFranchise FranchiseGenerale { get; set; }
        public ModeleLCIFranchise LCIRisque { get; set; }
        public ModeleLCIFranchise FranchiseRisque { get; set; }
        #endregion
    }
}