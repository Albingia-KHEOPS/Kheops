using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleCopyParamClausePage : MetaModelsBase
    {
        public string Source { get; set; }
        public List<AlbSelectListItem> Sources { get; set; }
        public string Destination { get; set; }
        public List<AlbSelectListItem> Destinations { get; set; }
    }
}