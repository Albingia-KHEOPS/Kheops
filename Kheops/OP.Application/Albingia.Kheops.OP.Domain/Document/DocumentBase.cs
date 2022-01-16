using Albingia.Kheops.OP.Domain.Referentiel;
using Albingia.Kheops.OP.Domain.Transverse;

namespace Albingia.Kheops.OP.Domain.Document
{
    public class DocumentBase
    {
        //KEMTYENV
        public TypeEnvoi TypeEnvoi { get; set; }
        // KEMORD
        public short NumeroOrdre { get; set; }
        // KEMID
        public long IdDocLot { get; set; }
        // KEQID / KEMTYPL
        public long IdDoc { get; set; }
        // KEQSTU ...
        public SituationDocumentLot SituationLot { get; set; }
        public SituationClause SituationDoc { get; set; }
        public UpdateMetadata SituationMetadata { get; set; }
        // KEMIDS / KEMTYDS
        public Destinataire Destinataire {get; set;}
        // KEQNOM/KERNOM
        public string Nom { get; set; }
        // KEQCHM/KERCHM
        public string Chemin { get; set; }
        //KEMTAMP / KEQTEDI
        public Tampon Tampon { get; set; }
        public string Libelle { get; set; }
        public TypoDocument TypoDocument { get; set; }


    }
}