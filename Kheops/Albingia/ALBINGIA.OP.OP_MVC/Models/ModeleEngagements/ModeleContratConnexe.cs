using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.Engagement;
using OP.WSAS400.DTO.Offres.Parametres;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleEngagements
{
    public class ModeleContratConnexe
    {
        public long IdeConnexite { get; set; }
        public string IdEngagaement { get; set; }
        public string NumContrat { get; set; }     
        public string DateDeb { get; set; }
        public string DateFin { get; set; }
        public List<ModeleTraite> Traites { get; set; }
        //La liste de tous les traités independamment du contrat
        public List<ModeleTraite> AllTraites { get; set; }
        public List<ParametreDto> ListCodeTraites { get; set; }

        public static List<ModeleContratConnexe> GetContratsConnexes(List<ContratConnexeTraiteDto> contratCnxDto)
        {
            var engagementsConnexites = new List<ModeleContratConnexe>();
            if (contratCnxDto != null)
                foreach (var contratCnx in contratCnxDto)
                {
                    var dateDeb = AlbConvert.ConvertIntToDate(int.Parse(contratCnx.DateDebutEngagement.ToString()));
                    var dateFin = AlbConvert.ConvertIntToDate(int.Parse(contratCnx.DateFinEngagement.ToString()));
                    engagementsConnexites.Add(new ModeleContratConnexe
                    {
                        IdeConnexite=contratCnx.IdeConnexite,
                        IdEngagaement = contratCnx.IdEngagement.ToString(),
                        DateDeb = dateDeb.HasValue ? dateDeb.Value.ToShortDateString() : string.Empty,
                        DateFin = dateFin.HasValue ? dateFin.Value.ToShortDateString() : string.Empty,
                        NumContrat = contratCnx.NumContrat + "-" + contratCnx.VersionContrat
                    });
                }
            return engagementsConnexites;
        }
    }
}