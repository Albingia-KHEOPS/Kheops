using EmitMapper;
using OP.WSAS400.DTO.Engagement;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleEngagements
{
    [Serializable]
    public class ModeleEngagementTraite
    {
        public string FamilleTraite { get; set; }
        public string NomTraite { get; set; }
        public string EngagementTotal { get; set; }
        public string EngagementAlbingia { get; set; }
        public string SMPTotal { get; set; }
        public string SMPAlbingia { get; set; }

        public static explicit operator ModeleEngagementTraite(EngagementTraiteDto TraiteDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<EngagementTraiteDto, ModeleEngagementTraite>().Map(TraiteDto);
        }

        public static EngagementTraiteDto LoadDto(ModeleEngagementTraite modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleEngagementTraite, EngagementTraiteDto>().Map(modele);
        }
    }
}