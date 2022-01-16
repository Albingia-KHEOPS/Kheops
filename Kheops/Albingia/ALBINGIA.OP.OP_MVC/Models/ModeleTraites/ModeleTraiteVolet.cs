using EmitMapper;
using OP.WSAS400.DTO.Traite;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleTraites
{
    [Serializable]
    public class ModeleTraiteVentilation
    {
        public string NomVentilation { get; set; }
        public Int64 IdVentilation { get; set; }

        public string SMP { get; set; }
        public string SMPF { get; set; }
        public string SMPAlbingia { get; set; }
        public string LCI { get; set; }
        public string Engagement { get; set; }
        public bool RisqueSel { get; set; }

        public string EngagementVentilationCalcule { get; set; }
        public string EngagementVentilationForce { get; set; }

        public string CapitauxTotaux { get; set; }
        public string CapitauxAlbingia { get; set; }

        public bool VentilationSel { get; set; }

        public int? Position { get; set; }

        public static explicit operator ModeleTraiteVentilation(TraiteVentilationDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<TraiteVentilationDto, ModeleTraiteVentilation>().Map(modeleDto);
        }

        public static TraiteVentilationDto LoadDto(ModeleTraiteVentilation modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleTraiteVentilation, TraiteVentilationDto>().Map(modele);
        }

    }
}