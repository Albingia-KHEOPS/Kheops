using Albingia.Kheops.OP.Domain.Referentiel;
using Albingia.Kheops.OP.Domain.Transverse;

namespace Albingia.Kheops.OP.Domain.Document
{
    public class DocumentExterne
    {
        public long IdDoc { get; set; }
        public string Nom { get; set; }
        public string Chemin { get; set; }
        public string Libelle { get; set; }
        public TypoDocument TypoDocument { get; set; }
        // KERREF
        public string Reference { get; set; }
    }
}