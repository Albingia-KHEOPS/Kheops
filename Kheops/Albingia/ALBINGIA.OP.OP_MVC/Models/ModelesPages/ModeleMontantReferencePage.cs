using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesMontantReference;
using EmitMapper;
using OP.WSAS400.DTO.MontantReference;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleMontantReferencePage : MetaModelsBase
    {
        [Display(Name = "Périodicité")]
        public string Periodicite { get; set; }
        [Display(Name = "Ech. principale")]
        public string EcheancePrincipale { get; set; }
        [Display(Name = "Prochaine éch.")]
        public DateTime? ProchaineEcheance { get; set; }
        [Display(Name = "Période du")]
        public DateTime? PeriodeDeb { get; set; }
        [Display(Name = "au")]
        public DateTime? PeriodeFin { get; set; }
        [Display(Name = "Frais accessoires")]
        public string TypeFraisAccessoires { get; set; }
        [Display(Name = "Montant (€)")]
        public decimal Montant { get; set; }
        [Display(Name = "Taxe attentat")]
        public bool TaxeAttentat { get; set; }
        public bool MontantForce { get; set; }
        [Display(Name = "Commentaires")]
        public string CommentForce { get; set; }
        public Int64 CodeCommentaire { get; set; }
        [Display(Name = "ISACQUIS")]
        public string IsAcquis { get; set; }

        public List<ModeleMontantReference> MontantsReference { get; set; }
        public List<AlbSelectListItem> ListeTypesFraisAcc { get; set; }

        public double TotalMntCalcule { get; set; }
        public double TotalMntForce { get; set; }
        [Display(Name = "Provisionnel")]
        public string TotalMntProvi { get; set; }
        public double TotalMntAcquis { get; set; }

        public int DureeGarantie { get; set; }
        public string UniteTemps { get; set; }

        public static explicit operator ModeleMontantReferencePage(MontantReferenceDto modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<MontantReferenceDto, ModeleMontantReferencePage>().Map(modele);
        }

        public static MontantReferenceDto LoadDto(ModeleMontantReferencePage modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleMontantReferencePage, MontantReferenceDto>().Map(modele);
        }

    }
}