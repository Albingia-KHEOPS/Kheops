using EmitMapper;
using OP.WSAS400.DTO.Engagement;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleEngagements
{
    [Serializable]
    public class ModeleTraite
    {
        public int IdTraite { get; set; }
        public int IdEngagement { get; set; }
        public string CodeTraite { get; set; }
        public string LibelleTraite { get; set; }
        public string EngagementAlbingia { get; set; }
        public string EngagementTotal { get; set; }

        public static List<ModeleTraite> GetEngagementsTraites(List<EngagementConnexiteTraiteDto> engCnxTraites)
        {
            var engagementsConnexiteTraites = new List<ModeleTraite>();
            if (engCnxTraites != null)
                foreach (var engTraite in engCnxTraites)
                {
                    engagementsConnexiteTraites.Add((ModeleTraite)engTraite);
                }
            return engagementsConnexiteTraites;
        }

        public static explicit operator ModeleTraite(EngagementConnexiteTraiteDto TraiteDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<EngagementConnexiteTraiteDto, ModeleTraite>().Map(TraiteDto);
        }
    }
}