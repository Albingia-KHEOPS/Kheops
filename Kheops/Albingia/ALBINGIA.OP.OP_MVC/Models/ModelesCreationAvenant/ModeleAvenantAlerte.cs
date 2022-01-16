using EmitMapper;
using OP.WSAS400.DTO.Avenant;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesCreationAvenant
{
    public class ModeleAvenantAlerte
    {
        public string TypeBloquante { get; set; }
        public string MessageAlerte { get; set; }
        public string TypeAlerte { get; set; }
        public string LienAlerte { get; set; }
        public string LienMessage { get; set; }
        public static explicit operator ModeleAvenantAlerte(AvenantAlerteDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<AvenantAlerteDto, ModeleAvenantAlerte>().Map(modeleDto);
        }

        public static AvenantAlerteDto LoadDto(ModeleAvenantAlerte modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleAvenantAlerte, AvenantAlerteDto>().Map(modele);
        }
    }
}