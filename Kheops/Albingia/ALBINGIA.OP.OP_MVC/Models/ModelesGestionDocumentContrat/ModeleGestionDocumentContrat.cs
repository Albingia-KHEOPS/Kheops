using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesGestionDocumentContrat
{
    public class ModeleGestionDocumentContrat
    {
        public string CodeContrat { get; set; }
        public string VersionContrat { get; set; }
        public List<AlbSelectListItem> ListFiltreTypeDocument { get; set; }
        public string FiltreDefautTypeDocument { get; set; }
        public List<AlbSelectListItem> ListFiltreLotEdition { get; set; }
        public string FiltreDefautLotEdition { get; set; }
        public List<ModeleLigneDocument> ListDocuments { get; set; }
    }
}