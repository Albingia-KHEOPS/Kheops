using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using Albingia.Kheops.OP.Domain.Transverse;

namespace Albingia.Kheops.OP.Domain.Document
{
    public class LotDocument
    {
        public long IdLot { get; set; }
        public AffaireId AffaireId { get; set; }
        public int AA { get; set; }
        public int Numero { get; set; }
        //TODO : trouver la bonne énumération, il manque AFFNOUV
        public string ActeDeGestion { get; set; }
        public int NumeroActeDeGestion { get; set; }
        public string Libelle { get; set; }

        public ICollection<DocumentGenere> Documents { get; private set; } = new List<DocumentGenere>();

        public UpdateMetadata SituationMetadata { get; set; }
        public UpdateMetadata Creation { get; set; }
        public UpdateMetadata MiseAJour { get; set; }
        public SituationClause Situation { get; set; }

        public bool IsWorkData { get; set; }
    }
}
