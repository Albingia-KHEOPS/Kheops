using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ALBINGIA.Framework.Common.Extensions;


namespace ALBINGIA.OP.OP_MVC.Models.MetaData.SaisieCreation_MetaData
{
    public class InformationSaisie_MetaData_Cibles : ModelsBase
    {
        //[Required(ErrorMessage = "Veuillez choisir une cible.")]
        [Display(Name = "Cible")]
        
        public String Cible { get; set; }
        public List<AlbSelectListItem> Cibles { get; set; }

        public bool CopyMode { get; set; }

        public InformationSaisie_MetaData_Cibles()
        {
            Cible = String.Empty;
            Cibles = new List<AlbSelectListItem>();
        }
    }
}