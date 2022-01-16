using Albingia.Kheops.OP.Domain.Affaire;

namespace Albingia.Kheops.OP.Domain.Document
{
    public class Destinataire
    {
        public TypeDestinataire Type { get; set; }

        public Courtier Courtier { get; set; }
        public Assure Assure { get; set; }
        public bool IsPrincipal { get; set; }
    }
}