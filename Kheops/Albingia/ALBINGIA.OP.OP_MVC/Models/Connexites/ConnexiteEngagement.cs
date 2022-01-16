using ALBINGIA.Framework.Business;
using ALBINGIA.OP.OP_MVC.Models.Connexites;
using OP.WSAS400.DTO.Engagement;
using System.Collections.Generic;

namespace ALBINGIA.OP.OP_MVC.Models
{
    public class ConnexiteEngagement : ConnexiteBase {
        public override TypeConnexite Type { get => TypeConnexite.Engagement; set { } }
        public string CodeEtat { get; set; }
        public string CodeSituation { get; set; }
        public List<ValeurEngagement> Valeurs { get; set; }

        public override void CopyFrom(ContratConnexeDto contratConnexe) {
            base.CopyFrom(contratConnexe);
            CodeEtat = contratConnexe.Etat;
            CodeSituation = contratConnexe.Situation;
            Valeurs = new List<ValeurEngagement>();
        }
    }
}