using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using OP.WSAS400.DTO.Offres.Risque.Inventaire;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesDetailsObjet
{
    public class ModeleInventaireObjet : MetaModelsBase
    {
        public string CodeOffre { get; set; }
        public int? Version { get; set; }
        public int CodeRisque { get; set; }
        public int CodeObjet { get; set; }
        public List<ModeleInventaire> Inventaires { get; set; }


        public ModeleInventaireObjet Load(List<InventaireDto> inventaires, String codeOffre, int? version, int codeRisque, int codeObjet)
        {
            var toReturn = new ModeleInventaireObjet();

            toReturn.CodeOffre = codeOffre;
            toReturn.Version = version;
            toReturn.CodeRisque = codeRisque;
            toReturn.CodeObjet = codeObjet;

            toReturn.Inventaires = new List<ModeleInventaire>();

            inventaires.ForEach(elem => toReturn.Inventaires.Add((new ModeleInventaire()).Load(elem)));

            return toReturn;
        }
    }
}