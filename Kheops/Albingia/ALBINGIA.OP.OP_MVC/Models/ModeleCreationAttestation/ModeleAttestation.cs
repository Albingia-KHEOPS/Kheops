using ALBINGIA.OP.OP_MVC.Models.ModelesCreationAvenant;
using EmitMapper;
using OP.WSAS400.DTO.Attestation;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleCreationAttestation
{
    public class ModeleAttestation
    {
        public List<ModeleAvenantAlerte> Alertes { get; set; }


        public static explicit operator ModeleAttestation(AttestationDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<AttestationDto, ModeleAttestation>().Map(modeleDto);
        }

        public static AttestationDto LoadDto(ModeleAttestation modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleAttestation, AttestationDto>().Map(modele);
        }
    }
}