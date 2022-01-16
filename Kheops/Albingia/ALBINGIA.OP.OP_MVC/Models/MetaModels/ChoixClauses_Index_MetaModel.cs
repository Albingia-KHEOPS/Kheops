using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.ModelesClauses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.MetaModels
{
    [Serializable]
    public class ChoixClauses_Index_MetaModel : MetaModelsBase
    {
        public string txtParamRedirect { get; set; }

        public string CodeOffre { get; set; }
        public string Version { get; set; }
        public string Type { get; set; }
        public string RisqueObj { get; set; } //Savoir si il y a un objet renseigné ou pas.  
        public bool HasRisques { get; set; }    
        public string CodeFormule { get; set; }
        public string CodeOption { get; set; }
        public string LibelleFormule { get; set; }
        public string LettreLibelleFormule { get; set; }
        public string ModeAvt { get; set; }
        public string ReguleId { get; set; }
        public string TypeAvt { get; set; }

        
        // Contrat
        [Display(Name = "Contrat")]
        public string ContratIdentification { get; set; }
        [Display(Name = "Cible")]
        public string ContratCible { get; set; }

        // Garantie
        [Display(Name = "Garantie")]
        public string GarantieDescriptif { get; set; }
        [Display(Name = "Cible")]
        public string GarantieCible { get; set; }

        // Condition
        [Display(Name = "Condition")]
        public string ConditionDescriptif { get; set; }
        [Display(Name = "Cible")]
        public string ConditionCible { get; set; }

        // Risque
        [Display(Name = "Risque")]
        public string RisqueDescriptif { get; set; }
        [Display(Name = "Cible")]
        public string RisqueCible { get; set; }

        // Objet
        [Display(Name = "Objet")]
        public List<AlbSelectListItem> ObjetDescriptif { get; set; }
        [Display(Name = "Volet")]
        public List<AlbSelectListItem> Volet { get; set; }
        [Display(Name = "Bloc")]
        public List<AlbSelectListItem> Bloc { get; set; }

        //Clauses
        public ModeleChoixClause ChoixClauseIntermediaire { get; set; }

        public ChoixClauses_Index_MetaModel()
        {
            ContratIdentification = String.Empty;
            ContratCible = String.Empty;
            RisqueDescriptif = String.Empty;
            RisqueCible = String.Empty;
            ObjetDescriptif = new List<AlbSelectListItem>();
            Volet = new List<AlbSelectListItem>();
            Bloc = new List<AlbSelectListItem>();                   
        }
    }
}
