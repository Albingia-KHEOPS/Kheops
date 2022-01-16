using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamTemplateNomenclature
{
    public class ModeleListeCibles
    {     
        public String Cible { get; set; }
        public List<AlbSelectListItem> Cibles { get; set; }
        public string ModeSaisie { get; set; }
    }
}