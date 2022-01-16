using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesClauses;
using ALBINGIA.OP.OP_MVC.Models.Regularisation;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Regularisation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleChoixClausesPage : MetaModelsBase, IRegulModel
    {
        public string txtParamRedirect { get; set; }

        public string CodeOffre { get; set; }
        public string Version
        {
            get { return _versionNum.StringValue(); }
            set
            {
                if (int.TryParse(value, out var val))
                {
                    _versionNum = val;
                }
                else { _versionNum = null; }
            }
        }
        private int? _versionNum;
        public int VersionNum => _versionNum.GetValueOrDefault();
        public string Type { get; set; }
        public string RisqueObj { get; set; } //Savoir si il y a un objet renseigné ou pas.        
        public string CodeFormule { get; set; }
        public string CodeOption { get; set; }
        public string CodeRisque { get; set; }
        public string LibelleFormule { get; set; }
        public string LettreLibelleFormule { get; set; }
        public string ModAvt { get; set; }
        public string ReguleId { get; set; }
        public string TypeAvt
        {
            get { return ActeGestion; }
            set { ActeGestion = value; }
        }

        // Contrat
        [Display(Name = "Contrat")]
        public string ContratIdentification { get; set; }
        [Display(Name = "Cible")]
        public string ContratCible { get; set; }
        public string ContratCibleLib { get; set; }

        // Garantie
        [Display(Name = "Formule")]
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
        public string RisqueCibleLib { get; set; }

        // Objet
        [Display(Name = "Objet")]
        public List<AlbSelectListItem> ObjetDescriptif { get; set; }
        [Display(Name = "Volet")]
        public List<AlbSelectListItem> Volet { get; set; }
        [Display(Name = "Bloc")]
        public List<AlbSelectListItem> Bloc { get; set; }

        //Clauses
        public ModeleChoixClause ChoixClauseIntermediaire { get; set; }

        public RegularisationContext Context { get; set; }

        public IdContratDto IdContrat
        {
            get { return new IdContratDto { CodeOffre = CodePolicePage, Version = Int32.Parse(VersionPolicePage), Type = TypePolicePage }; }
        }

        public long RgId
        {
            get
            {
                Int64.TryParse(ReguleId, out var rgId);
                return rgId;
            }
        }

        public long LotId
        {
            get
            {
                Int64.TryParse(InformationGeneraleTransverse.GetAddParamValue(AddParamValue, AlbParameterName.LOTID), out long id);
                return id;

            }
        }

        public InfosBaseDto InfosBaseAffaire { get; internal set; }
    }
}