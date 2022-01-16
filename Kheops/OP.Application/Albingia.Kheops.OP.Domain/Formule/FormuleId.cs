using Albingia.Kheops.OP.Domain.Affaire;

namespace Albingia.Kheops.OP.Domain.Formule
{
    public class FormuleId : AffaireId
    {
        public FormuleId() { }

        public FormuleId(AffaireId affaire) {
            CodeAffaire = affaire.CodeAffaire;
            IsHisto = affaire.IsHisto;
            NumeroAliment = affaire.NumeroAliment;
            NumeroAvenant = affaire.NumeroAvenant;
            TypeAffaire = affaire.TypeAffaire;
        }

        public int NumeroFormule { get; set; }
    }

}