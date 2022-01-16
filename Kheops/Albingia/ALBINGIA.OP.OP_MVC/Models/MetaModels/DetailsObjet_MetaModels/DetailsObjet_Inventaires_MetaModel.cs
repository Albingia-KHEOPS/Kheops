using OP.WSAS400.DTO.Offres.Risque.Inventaire;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.MetaModels.DetailsObjet_MetaModels
{
    [Serializable]
    public class DetailsObjet_Inventaires_MetaModel : MetaModelsBase
    {
        public string CodeOffre { get; set; }
        public int? Version { get; set; }
        public int CodeRisque { get; set; }
        public int CodeObjet { get; set; }
        public List<Inventaire_MetaModel> Inventaires { get; set; }

        public DetailsObjet_Inventaires_MetaModel()
        {
            CodeOffre = string.Empty;
            Version = null;
            Inventaires = new List<Inventaire_MetaModel>();
        }

        public DetailsObjet_Inventaires_MetaModel Load(List<InventaireDto> inventaires, String codeOffre, int? version, int codeRisque, int codeObjet)
        {
            var toReturn = new DetailsObjet_Inventaires_MetaModel();

            toReturn.CodeOffre = codeOffre;
            toReturn.Version = version;
            toReturn.CodeRisque = codeRisque;
            toReturn.CodeObjet = codeObjet;

            inventaires.ForEach(elem => toReturn.Inventaires.Add((new Inventaire_MetaModel()).Load(elem)));

            return toReturn;
        }
    }
}