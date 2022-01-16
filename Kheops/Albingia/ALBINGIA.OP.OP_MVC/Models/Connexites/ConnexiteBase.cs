using ALBINGIA.Framework.Business;
using ALBINGIA.Framework.Common;
using OP.WSAS400.DTO.Engagement;
using System;

namespace ALBINGIA.OP.OP_MVC.Models
{
    public class ConnexiteBase {
        public ConnexiteBase() { }
        public ConnexiteBase(ContratConnexeDto contratConnexe) {
            CopyFrom(contratConnexe);
        }
        public int Numero { get; set; }
        public Folder Folder { get; set; }
        public string NomContrat { get; set; }
        public string CodeBranche { get; set; }
        public string CodeSousBranche { get; set; }
        public string CodeCategorie { get; set; }
        public string CodeCible { get; set; }
        public string Commentaires { get; set; }
        public string Preneur { get; set; }
        public int CodeObservation { get; set; }
        public string Observation { get; set; }
        public virtual TypeConnexite Type { get { return 0; } set { } }

        public virtual void CopyFrom(ContratConnexeDto contratConnexe) {
            if (contratConnexe == null) {
                throw new ArgumentNullException(nameof(contratConnexe));
            }
            Numero = contratConnexe.NumConnexite;
            Folder = new Folder(contratConnexe.NumContrat, contratConnexe.VersionContrat, contratConnexe.TypeContrat[0]);
            NomContrat = contratConnexe.DescriptionContrat;
            CodeBranche = contratConnexe.CodeBranche;
            CodeCible = contratConnexe.CodeCible;
            CodeCategorie = contratConnexe.CodeCategorie;
            CodeSousBranche = contratConnexe.CodeSousBranche;
            CodeObservation = (int)contratConnexe.CodeObservation;
            Commentaires = contratConnexe.Observation;
            Preneur = contratConnexe.NomPreneur;
        }
    }
}