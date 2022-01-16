using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Models.Common;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    [Serializable]
    public class ModeleLogTracePage : MetaModelsBase
    {
        //public List<ModeleLogTrace> LogTraces;
        public List<LogTrace> LogTraces;

        [Display(Name = "Date de début:")]
        public DateTime? DateDebutFiltre;
        [Display(Name = "Date de fin:")]
        public DateTime? DateFinFiltre;

        [Display(Name = "Type:")]
        public string CodeType { get; set; }
        public List<AlbSelectListItem> CodesType { get; set; }
        [Display(Name = "Contient:")]
        public string MotCle { get; set; }

       
    }
}