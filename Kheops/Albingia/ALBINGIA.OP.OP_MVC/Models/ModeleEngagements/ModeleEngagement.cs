using OP.WSAS400.DTO.Offres.Parametres;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleEngagements
{
    public class ModeleEngagement
    {
        public List<ModeleEngagementConnexite> Engagements { get; set; }
        public List<ModeleTraite> AllTraitesEngagements { get; set; }
        public List<ParametreDto> ListCodeTraites { get; set; }
    }
}