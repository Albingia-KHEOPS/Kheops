using Albingia.Kheops.OP.Domain;
using Albingia.Kheops.OP.Domain.Affaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Application.Port.Driver.DTO {
    public class ConditionsAffaireDto {
        public AffaireId AffaireId { get; set; }
        public TarifAffaire LCI { get; set; }
        public TarifAffaire Franchise { get; set; }
        public TarifGeneral LCIRisque { get; set; }
        public TarifGeneral FranchiseRisque { get; set; }
    }
}
