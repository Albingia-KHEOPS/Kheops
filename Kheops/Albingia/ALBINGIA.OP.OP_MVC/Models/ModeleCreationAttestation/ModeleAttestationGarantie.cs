using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.Attestation;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleCreationAttestation
{
    public class ModeleAttestationGarantie
    {
        public string LotId { get; set; }
        public string Erreur { get; set; }
        public string PeriodeDeb { get; set; }
        public string PeriodeFin { get; set; }

        public string FiltreGarantie { get; set; }
        public List<AlbSelectListItem> FiltresGarantie { get; set; }

        public string  FiltreRisque { get; set; }
        public List<AlbSelectListItem> FiltresRisque { get; set; }

        public List<ModeleAttestationRisque> Risques { get; set; }

        public static explicit operator ModeleAttestationGarantie(AttestationSelGarDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<AttestationSelGarDto, ModeleAttestationGarantie>().Map(modeleDto);
        }

        public static AttestationSelGarDto LoadDto(ModeleAttestationGarantie modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleAttestationGarantie, AttestationSelGarDto>().Map(modele);
        }
    }
}