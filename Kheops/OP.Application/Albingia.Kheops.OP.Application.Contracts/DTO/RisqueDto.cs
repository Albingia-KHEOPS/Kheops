using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;

namespace Albingia.Kheops.DTO {
    public class RisqueDto
    {
        public AffaireId AffaireId { get; set; }
        public int Numero { get; set; }
        public string Designation { get; set; }
        public List<ObjetDto> Objets { get; set; } = new List<ObjetDto>();
        public Cible Cible { get; set; }
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public Branche Branche { get; set; }
        public string  SousBranche { get; set; }
        public bool ParticipationBeneficiaire { get; set; }
        public bool BonnificationNonSinistre { get; set; }
        public bool ARegulariser { get; set; }
        public bool BonnificationNonSinistreIncendie { get; set; }
        public int NumeroAvenantCreation { get; set; }
        public int NumeroAvenantModification { get; set; }
        public DateTime? DateDebutImplicite { get; set; }
        public DateTime? DateFinImplicite { get; set; }
        public RegimeTaxe RegimeTaxe { get; set; }
        public bool AllowCANAT { get; set; }
        public int Code { get; set; }
    }
}
