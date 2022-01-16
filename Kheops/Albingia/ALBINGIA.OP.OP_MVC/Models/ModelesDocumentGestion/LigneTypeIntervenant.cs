using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesDocumentGestion
{
    public class LigneTypeIntervenant
    {       
        public string idDestinataire { get; set; }
        public string CodeSelectedIntervenant { get; set; }
        public string NomSelectedIntervenant { get; set; }
        public bool IsFromAffaire { get; set; }

        public List<AlbSelectListItem> ListeTypesIntervenant { get; set; }
        public string SelectedTypeIntervenant { get; set; }
    }
}