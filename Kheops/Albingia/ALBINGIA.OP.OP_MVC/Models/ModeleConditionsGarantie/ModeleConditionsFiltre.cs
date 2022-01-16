using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleConditionsGarantie
{
    public class ModeleConditionsFiltre
    {
        public string FiltreType { get; set; }
        public string FiltreValue { get; set; }
        public List<AlbSelectListItem> FiltreListe { get; set; }
    }
}