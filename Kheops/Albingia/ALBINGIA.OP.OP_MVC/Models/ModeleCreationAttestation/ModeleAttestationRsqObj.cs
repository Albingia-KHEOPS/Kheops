using ALBINGIA.OP.OP_MVC.Models.ModelesRisque;
using EmitMapper;
using OP.WSAS400.DTO.Attestation;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleCreationAttestation
{
    public class ModeleAttestationRsqObj
    {
        public string LotId { get; set; }
        public string Erreur { get; set; }
        public string PeriodeDeb { get; set; }
        public string PeriodeFin { get; set; }

        public List<ModeleRisque> Risques { get; set; }

        public static explicit operator ModeleAttestationRsqObj(AttestationSelRsqDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<AttestationSelRsqDto, ModeleAttestationRsqObj>().Map(modeleDto);
        }

        public static AttestationSelRsqDto LoadDto(ModeleAttestationRsqObj modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleAttestationRsqObj, AttestationSelRsqDto>().Map(modele);
        }
    }
}