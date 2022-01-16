using EmitMapper;
using OP.WSAS400.DTO.Avenant;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesCreationAvenant
{
    public class ModeleAvenant
    {
        public List<ModeleAvenantAlerte> Alertes { get; set; }
        public ModeleAvenantModification AvenantModif { get; set; }


        public static explicit operator ModeleAvenant(AvenantDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<AvenantDto, ModeleAvenant>().Map(modeleDto);
        }

        public static AvenantDto LoadDto(ModeleAvenant modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleAvenant, AvenantDto>().Map(modele);
        }
    }
}