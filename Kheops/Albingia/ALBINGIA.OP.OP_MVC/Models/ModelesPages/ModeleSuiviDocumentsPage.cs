using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesSuiviDocuments;
using OP.WSAS400.DTO.SuiviDocuments;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleSuiviDocumentsPage : MetaModelsBase
    {
        public SuiviDocFiltreDto Filtre { get; set; }

        public string DisplayType { get; set; }
        public string Situation { get; set; }
        public List<AlbSelectListItem> Situations { get; set; }
        public string TypeDestinataire { get; set; }
        public List<AlbSelectListItem> TypesDestinataire { get; set; }
        public SuiviDocumentsListe ListSuiviDocuments { get; set; }
        public bool DisplaySearch { get; set; }
        public string ModeAffichage { get; set; }
        public List<AlbSelectListItem> ModeAffichages { get; set; }
        public bool readOnly { get; set; }
    }
}