using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleParamCibleRecupPage : MetaModelsBase
    { 
        public string Type { get; set; }
        public string Erreur { get; set; }
        public string CodeOffre { get; set; }
        public string Version { get; set; }

        public string CodeCibleRecup { get; set; }
        public string CodeCibleRecupLabel { get; set; }
        public string Cible { get; set; }
        public List<AlbSelectListItem> Cibles { get; set; }

        public bool MultiObj { get; set; }
    }
}