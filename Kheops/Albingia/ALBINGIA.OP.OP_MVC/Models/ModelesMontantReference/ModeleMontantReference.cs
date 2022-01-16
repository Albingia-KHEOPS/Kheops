using EmitMapper;
using OP.WSAS400.DTO.MontantReference;
using System;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesMontantReference
{
    public class ModeleMontantReference
    {
        public string LettreForm { get; set; }
        public Int64 CodeForm { get; set; }
        public string LibFormule { get; set; }
        public Int64 CodeRsq { get; set; }
        public string LibRisque { get; set; }
        [Display(Name = "Montant réf. calculé")]
        public decimal MontantCalcule { get; set; }
        [Display(Name = "Montant réf. forcé")]
        public decimal MontantForce { get; set; }
        [Display(Name = "Provisionnel")]
        public string MontantProvisionnel { get; set; }
        [Display(Name = "Montant acquis")]
        public decimal MontantAcquis { get; set; }
        public string ChkMntAcquis { get; set; }
        
        public static explicit operator ModeleMontantReference(MontantReferenceInfoDto modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<MontantReferenceInfoDto, ModeleMontantReference>().Map(modele);
        }

        public static MontantReferenceInfoDto LoadDto(ModeleMontantReference modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleMontantReference, MontantReferenceInfoDto>().Map(modele);
        }
    }
}