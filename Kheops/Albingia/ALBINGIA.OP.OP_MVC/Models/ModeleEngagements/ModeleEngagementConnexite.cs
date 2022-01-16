using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.Engagement;
using OP.WSAS400.DTO.Offres.Parametres;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleEngagements
{
    public class ModeleEngagementConnexite
    {
        public string IdEngagaement { get; set; }
        public string NumContrat { get; set; }
        public string Statut { get; set; }
        public string DateDeb { get; set; }
        public string DateFin { get; set; }
        public List<ModeleTraite> Traites { get; set; }
        //La liste de tous les traités independamment du contrat
        public List<ModeleTraite> AllTraites { get; set; }
        public List<ParametreDto> ListCodeTraites { get; set; }

        public static List<ModeleEngagementConnexite> GetEngagements(List<EngagementConnexiteDto> engCnxDto)
        {    
            var engagementsConnexites = new List<ModeleEngagementConnexite>();
            if (engCnxDto != null)
                foreach (var engCnx in engCnxDto)
                {
                    var dateDeb = AlbConvert.ConvertIntToDate(int.Parse(engCnx.DateDebutEngagement.ToString()));
                    var dateFin = AlbConvert.ConvertIntToDate(int.Parse(engCnx.DateFinEngagement.ToString()));
                    engagementsConnexites.Add(new ModeleEngagementConnexite
                    {
                        IdEngagaement = engCnx.IdEngagement.ToString(),
                        DateDeb = dateDeb.HasValue ? dateDeb.Value.ToShortDateString() : string.Empty,
                        DateFin = dateFin.HasValue ? dateFin.Value.ToShortDateString() : string.Empty,                       
                        Statut=engCnx.ModeUtilise+"-"+engCnx.ModeActif
                    });              
                }
            return engagementsConnexites;
        }
    }
}