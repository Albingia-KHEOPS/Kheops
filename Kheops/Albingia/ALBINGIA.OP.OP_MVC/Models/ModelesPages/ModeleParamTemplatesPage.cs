using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamTemplates;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleParamTemplatesPage : MetaModelsBase
    {
        public List<ModeleLigneTemplate> ListeTemplates { get; set; }
        public List<AlbSelectListItem> ListeTypesTemplate { get; set; }
        public string SelectedTypeTemplate { get; set; }
    }
}