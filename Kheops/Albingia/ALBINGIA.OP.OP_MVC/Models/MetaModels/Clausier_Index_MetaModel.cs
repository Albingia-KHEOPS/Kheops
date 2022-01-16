using Albingia.Common;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.MetaModels
{
    [Serializable]
    public class Clausier_Index_MetaModel : MetaModelsBase
    {
        public override string TabGuid {
            get {
                return base.TabGuid;
            }
            set {
                if (AllParameters == null) { AllParameters = AlbParameters.Parse(null); }
                AllParameters[PageParamContext.TabGuidKey] = value;
            }
        }
        public string CodeOffre { get; set; }
        public string Version { get; set; }
        public string Type { get; set; }
        public string Etape { get; set; }
        public string Branche { get; set; }

        [Display(Name = "Risque / Objet :")]
        public string TypeInventaire { get; set; }

        [Display(Name = "Titre")]
        public string Titre { get; set; }

        [Display(Name = "Mot Clé")]
        public string MotCle { get; set; }
        public List<AlbSelectListItem> MotCles { get; set; }
        public SelectListItemMetaModel ContexteList { get; set; }
        public List<ClausierRecherche_MetaModel> Clauses { get; set; }

        public Clausier_Index_MetaModel()
        {
            CodeOffre = string.Empty;
            Version = string.Empty;
            Type = string.Empty;
            Etape = string.Empty;
            TypeInventaire = string.Empty;
            Titre = string.Empty;
            MotCle = string.Empty;
            MotCles = new List<AlbSelectListItem>();
            Branche = string.Empty;
            Clauses = new List<ClausierRecherche_MetaModel>();
            ContexteList = new SelectListItemMetaModel();
        }
    }
}