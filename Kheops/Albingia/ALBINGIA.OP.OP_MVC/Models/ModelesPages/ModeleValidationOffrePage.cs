using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleEngagements;
using ALBINGIA.OP.OP_MVC.Models.ModelesValidationOffre;
using ALBINGIA.OP.OP_MVC.Models.Regularisation;
using EmitMapper;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Regularisation;
using OP.WSAS400.DTO.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleValidationOffrePage : MetaModelsBase, IRegulModel
    {
        [Display(Name = "Offre complète ?")]
        public string OffreComplete { get; set; }
        public List<AlbSelectListItem> OffreCompletes { get; set; }

        [Display(Name = "Motif")]
        public string Motif { get; set; }
        public List<AlbSelectListItem> Motifs { get; set; }

        [Display(Name = "Pouvoir requis")]
        public string ValidationRequise { get; set; }

        public List<ModeleCritereValidation> Criteres { get; set; }

        public string IsDocEdit { get; set; }

        public bool IsControleOk { get; set; }

        public List<ModeleEngagementTraite> Traites { get; set; }

        public ModeleEditionDocuments Docs { get; set; }

        public string EtatDossier { get; set; }
        #region champs supplémentaires offre
        public int DateStatistique { get; set; }
        public Int64 CodeObservation { get; set; }
        public string Observation { get; set; }
        public string DelegationOffre { get; set; }
        public string SecteurOffre { get; set; }
        public double MontantReference { get; set; }
        public string Validable { get; set; }
        #endregion

        #region champs supplémentaires contrat

        public double MontantReferenceCalcule { get; set; }
        public double MontantReferenceForce { get; set; }
        public double MontantReferenceAcquis { get; set; }
        public string DelegationApporteur { get; set; }
        public string SecteurApporteur { get; set; }
        public string DelegationGestionnaire { get; set; }
        public string SecteurGestionnaire { get; set; }
        #endregion

        public string EtatRegule { get; set; }
        public string IsDocGener { get; set; }

        public Int64 DateAccordInt { get; set; }
        public DateTime? DateAccord { get; set; }

        public bool IsChekedEcheance { get; set; }

        public RegularisationContext Context { get; set; }

        public IdContratDto IdContrat
        {
            get { return new IdContratDto { CodeOffre = CodePolicePage, Version = Int32.Parse(VersionPolicePage), Type = TypePolicePage }; }
        }

        public long RgId
        {
            get
            {
                Int64.TryParse(InformationGeneraleTransverse.GetAddParamValue(AddParamValue, AlbParameterName.REGULEID), out long id);
                
                return id;
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

        public static explicit operator ModeleValidationOffrePage(ValidationDto validationtDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ValidationDto, ModeleValidationOffrePage>().Map(validationtDto);
        }

        public static ValidationDto LoadDto(ModeleValidationOffrePage modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleValidationOffrePage, ValidationDto>().Map(modele);
        }
    }
}