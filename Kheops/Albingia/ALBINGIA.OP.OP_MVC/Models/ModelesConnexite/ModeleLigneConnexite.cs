using OP.WSAS400.DTO.Engagement;
using System.Collections.Generic;
using System.Linq;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesConnexite
{
    public class ModeleLigneConnexite
    {
        public string GuidId { get; set; }
        public string Contrat { get; set; }
        public string Branche { get; set; }
        public string LibelleBranche { get; set; }
        public string Cible { get; set; }
        public string LibelleCible { get; set; }
        public string Commentaire { get; set; }
        public long CodeObservation { get; set; }
        public string Preneur { get; set; }
        public string PreneurInfo { get; set; }
        public long IdeConnexite { get; set; }
        public string DescriptionContrat { get; set; }
        public string Etat { get; set; }
        public string Situation { get; set; }
        public string CodeEngagement { get; set; }
        public List<ModeleEngTrait> LstEngagmentTraite { get; set; }

        public static List<ModeleLigneConnexite> LoadContratsConnexes(List<ContratConnexeDto> contratsConnexesDto)
        {
            var contratsConnexes = new List<ModeleLigneConnexite>();


            if (contratsConnexesDto != null)
                foreach (var contratConnexeDto in contratsConnexesDto)
                {
                    contratsConnexes.Add(new ModeleLigneConnexite
                   {
                       IdeConnexite = contratConnexeDto.IdeConnexite,
                       GuidId = contratConnexeDto.NumContrat + "_" + contratConnexeDto.VersionContrat + "_" + contratConnexeDto.TypeContrat + "_" + contratConnexeDto.NumConnexite + "_" + contratConnexeDto.CodeTypeConnexite + "_" + contratConnexeDto.IdeConnexite,
                       Contrat = contratConnexeDto.NumContrat + "-" + contratConnexeDto.VersionContrat,
                       Branche = contratConnexeDto.CodeBranche,
                       LibelleBranche = contratConnexeDto.LibelleBranche,
                       Cible = contratConnexeDto.CodeCible,
                       LibelleCible = contratConnexeDto.LibelleCible,
                       Commentaire = contratConnexeDto.Observation,
                       CodeObservation = contratConnexeDto.CodeObservation,
                       Preneur = contratConnexeDto.CodePreneur + "-" + contratConnexeDto.NomPreneur,
                       PreneurInfo = contratConnexeDto.NomPreneur + " " + contratConnexeDto.Adresse1 + " " + contratConnexeDto.Adresse2 + " " + contratConnexeDto.Departement + contratConnexeDto.CodePostal + " " + contratConnexeDto.Ville,
                       DescriptionContrat = contratConnexeDto.DescriptionContrat,
                       Etat = contratConnexeDto.Etat,
                       Situation = contratConnexeDto.Situation

                   });
                }
            return contratsConnexes;
        }

        public static List<ModeleLigneConnexite> LoadContratsConnexesEng(List<ContratConnexeDto> contratsConnexesDto)
        {
            var contratsConnexes = new List<ModeleLigneConnexite>();
            if (contratsConnexesDto != null)
            {
                var grouped = contratsConnexesDto.GroupBy(i => i.NumContrat);
                foreach (var item in grouped)
                {
                    var contratConnexeDto = item.ElementAt(0);

                    var ligne = new ModeleLigneConnexite
                    {
                        IdeConnexite = contratConnexeDto.IdeConnexite,
                        GuidId = contratConnexeDto.NumContrat + "_" + contratConnexeDto.VersionContrat + "_" + contratConnexeDto.TypeContrat + "_" + contratConnexeDto.NumConnexite + "_" + contratConnexeDto.CodeTypeConnexite + "_" + contratConnexeDto.IdeConnexite,
                        Contrat = contratConnexeDto.NumContrat + "-" + contratConnexeDto.VersionContrat,
                        Branche = contratConnexeDto.CodeBranche,
                        LibelleBranche = contratConnexeDto.LibelleBranche,
                        Cible = contratConnexeDto.CodeCible,
                        LibelleCible = contratConnexeDto.LibelleCible,
                        Commentaire = contratConnexeDto.Observation,
                        CodeObservation = contratConnexeDto.CodeObservation,
                        Preneur = contratConnexeDto.CodePreneur + "-" + contratConnexeDto.NomPreneur,
                        PreneurInfo = contratConnexeDto.NomPreneur + " " + contratConnexeDto.Adresse1 + " " + contratConnexeDto.Adresse2 + " " + contratConnexeDto.Departement + contratConnexeDto.CodePostal + " " + contratConnexeDto.Ville,
                        DescriptionContrat = contratConnexeDto.DescriptionContrat,
                        Etat = contratConnexeDto.Etat,
                        Situation = contratConnexeDto.Situation,
                       
                    };


                    if(item != null )
                    {
                        ligne.LstEngagmentTraite = item.Select(i => new ModeleEngTrait
                        {
                            CodeEngagement = i.CodeEngagement ?? string.Empty,
                            ValeurEngagement = i.ValeurEngagement,
                        }).ToList();   
                    }
                    

                    contratsConnexes.Add(ligne);               
                }
            }
            return contratsConnexes;

        }

    }
}