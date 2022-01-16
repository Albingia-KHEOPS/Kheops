using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleCreationAttestation
{
    public class ModeleAttestationRisque
    {
        public string Erreur { get; set; }
        public string Code { get; set; }
        public string CodesObj { get; set; }
        public List<ModeleAttestationObjet> Objets { get; set; }
        public List<ModeleAttestationFormule> Formules { get; set; }

        //public static explicit operator ModeleAttestationRisque(AttestationRisqueDto modeleDto)
        //{
        //    return ObjectMapperManager.DefaultInstance.GetMapper<AttestationRisqueDto, ModeleAttestationRisque>().Map(modeleDto);
        //}

        //public static AttestationRisqueDto LoadDto(ModeleAttestationRisque modele)
        //{
        //    return ObjectMapperManager.DefaultInstance.GetMapper<ModeleAttestationRisque, AttestationRisqueDto>().Map(modele);
        //}
    }
}