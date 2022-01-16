using System.Collections.Generic;
using Albingia.Kheops.OP.Domain.Referentiel;
using Albingia.Kheops.OP.Domain.Transverse;

namespace Albingia.Kheops.OP.Domain.Document
{
    public class DocumentGenere : DocumentBase
    {
        public class IdComparer : IEqualityComparer<DocumentGenere>
        {
            public bool Equals(DocumentGenere x, DocumentGenere y) => x?.IdDoc == y?.IdDoc;


            public int GetHashCode(DocumentGenere obj) => obj?.IdDoc.GetHashCode() ?? 0;

        }
        public DocumentGenere()
        {
            Service = "PRODU";
        }
        // KEQNTA
        public NatureGeneration NatureGeneration { get; set; }
        // KEQENVU ...
        public UpdateMetadata Envoi { get; set; }
        // KEQDIMP
        public bool IsImprimable { get; set; }

        public bool IsAjoute { get; set; }
        public string CodeClause { get; set; }
        public bool IsAccompagnement { get; set; }
        public bool IsEnCourt { get; set; }
        public EtapeGeneration Etape { get; set; }
        public LienDocument Lien { get; set; }
        public long ParentId { get; set; }
        public bool IsLibre { get; set; }
        public bool IsTransforme { get; set; }
        public int VersionClause { get; set; }
        public string Service { get; set; }
        public StatutGeneration StatutGeneration { get; set; }
        public DocumentExterne DocumentExterne { get; set; }
    }
}