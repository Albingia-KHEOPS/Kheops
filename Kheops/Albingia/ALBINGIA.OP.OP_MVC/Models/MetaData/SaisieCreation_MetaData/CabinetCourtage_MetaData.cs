using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.SaisieCreation_MetaData
{
    public class CabinetCourtage_MetaData : _SaisieCreation_MetaData_Base
    {
        [Display(Name = "Code *")]
        [Required(ErrorMessage = "Veuillez choisir un cabinet de courtage.")]
        public int? CodeCabinetCourtage { get; set; }

        [Display(Name = "Nom du cabinet")]
        public string NomCabinetCourtage { get; set; }

        public string CodeInterlocuteur { get; set; }

        [Display(Name = "Interlocuteur")]
        public string NomInterlocuteur { get; set; }

        [Display(Name = "Type")]
        public string Type { get; set; }

        [Display(Name = "Référence")]
        public string Reference { get; set; }

        public bool EditMode { get; set; }
        public bool CopyMode { get; set; }


        public CabinetCourtage_MetaData()
        {
            this.CodeCabinetCourtage = null;
            this.NomCabinetCourtage = String.Empty;
            this.NomInterlocuteur = String.Empty;
            this.Type = String.Empty;
            this.Reference = String.Empty;
        }
    }
}