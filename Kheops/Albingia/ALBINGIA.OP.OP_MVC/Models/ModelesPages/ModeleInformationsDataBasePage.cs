using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleInformationDatabase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    [Serializable]
    public class ModeleInformationsDataBasePage : MetaModelsBase
    {
        public IList<ModeleColumnInfo> Colonnes { get; set; }

        [Display(Name ="Environnement:")]
        public string Environnement { get; set; }

        [Display(Name = "Table:")]
        public string Table { get; set; }

    }
}