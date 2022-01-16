using System.Collections.Generic;
using System.Linq;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesConnexite
{
    public class ModeleEngagement
    {
        //ExistEngCnx=true si le contrat a une connexité de type engagement
        public bool ExistEngCnx { get; set; }

        public string ContratTraite { get; set; }
        public string NumConnexiteEngagement { get; set; }
        public long CodeObservationEngagement { get; set; }
        public string ObservationEngagement { get; set; }
        public long IdeConnexiteEngagement { get; set; }
        public List<ModeleLigneConnexite> ConnexitesEngagement { get; set; }

        public List<ModelePeriodeDeConnexite> PeriodesDeConnexites
        {
            get; set; 
        }

        public List<string> ListeDeTraites
        {
            get
            {
                var result = new List<string>();

                if (ConnexitesEngagement == null || !ConnexitesEngagement.Any())
                    return result;

                var allTraites = new List<ModeleEngTrait>();
                
                ConnexitesEngagement.ForEach(i =>
                                                 {
                                                     if(i.LstEngagmentTraite != null)
                                                        allTraites.AddRange(i.LstEngagmentTraite);
                                                 });
                allTraites.Add(new ModeleEngTrait {CodeEngagement = "X"});
                allTraites.Add(new ModeleEngTrait {CodeEngagement = "Z"});
                allTraites.Add(new ModeleEngTrait {CodeEngagement = "W"});


                return allTraites.Select(i => i.CodeEngagement).Distinct().ToList();
            }
        }

        public bool IsConnexiteReadOnly { get; set; }
        public string TypeAffichage { get;set;}
    }
}