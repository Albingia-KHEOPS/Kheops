using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamTemplateNomenclature
{
    public class ModeleDetailsTemplateNomenclature
    {
        public List<AlbSelectListItem> Branches { get; set; }
        public ModeleListeCibles ModeleCibles { get; set; }
        public string Branche { get; set; }
        public string Cible { get; set; }
        public string Libelle { get; set; }
        public string ModeSaisie { get; set; }
        public Int64 GuidId { get; set; }
    }
}