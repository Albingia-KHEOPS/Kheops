using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.DTO {
    public class RelanceDto {
        public int CodeOffre { get; set; }
        public int Version { get; set; }
        public Branche Branche { get; set; }
        public string Descriptif { get; set; }
        public DateTime DateValidation { get; set; }
        public Utilisateur Souscripteur { get; set; }
        public Utilisateur Gestionnaire { get; set; }
        public int DelaisRelanceJours { get; set; }
        public Courtier CourtierGestionnaire { get; set; }
        public MotifSituation MotifSituation { get; set; }
        public SituationAffaire Situation { get; set; }
        public bool IsAttenteDocCourtier { get; set; }
    }
}
