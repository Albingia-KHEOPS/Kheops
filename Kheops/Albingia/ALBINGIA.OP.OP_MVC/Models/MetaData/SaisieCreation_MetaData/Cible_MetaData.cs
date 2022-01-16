using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ALBINGIA.Framework.Common.Extensions;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.SaisieCreation_MetaData
{
    public class Cible_MetaData : ModelsBase
    {
        [Display(Name = "Cible")]
        public String Cible { get; set; }
        public List<AlbSelectListItem> Cibles { get; set; }

        public Cible_MetaData()
        {
            Cible = String.Empty;
            Cibles = new List<AlbSelectListItem>();
        }
    }
}