using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModelePrisePositionPage : MetaModelsBase
    {
        public List<AlbSelectListItem> ListeMotifsAttente { get; set; }
        public List<AlbSelectListItem> ListeMotifsRefus { get; set; }
        public string MotifAttente { get; set; }
        public string MotifRefus { get; set; }
    }
}