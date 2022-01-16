using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamNomenclature
{
    public class ModeleDetailsNomenclature
    {
        public string Branche { get; set; }
        public string Cible { get; set; }
        public string Libelle { get; set; }
        public string GuidId { get; set; }

        public List<AlbSelectListItem> ListeConcepts01 { get; set; }
        public List<AlbSelectListItem> ListeFamilles01 { get; set; }
        public List<AlbSelectListItem> ListeConcepts02 { get; set; }
        public List<AlbSelectListItem> ListeFamilles02 { get; set; }
        public List<AlbSelectListItem> ListeConcepts03 { get; set; }
        public List<AlbSelectListItem> ListeFamilles03 { get; set; }
        public List<AlbSelectListItem> ListeConcepts04 { get; set; }
        public List<AlbSelectListItem> ListeFamilles04 { get; set; }

        public string Concept01 { get; set; }
        public string Famille01 { get; set; }
        public string Concept02 { get; set; }
        public string Famille02 { get; set; }
        public string Concept03 { get; set; }
        public string Famille03 { get; set; }
        public string Concept04 { get; set; }
        public string Famille04 { get; set; }

        public List<ModeleLigneDetails> ListeLignesDetails { get; set; }
    }
}