using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamCible
{
    public class EditBSC
    {
        public string GuidId { get; set; }
        public List<AlbSelectListItem> Branches { get; set; }
        public string Branche { get; set; }

        public List<AlbSelectListItem> SousBranches { get; set; }
        public string SousBranche { get; set; }

        public List<AlbSelectListItem> Categories { get; set; }
        public string Categorie { get; set; }
    }
}