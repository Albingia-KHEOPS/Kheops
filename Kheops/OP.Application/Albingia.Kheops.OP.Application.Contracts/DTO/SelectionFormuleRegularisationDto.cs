using Albingia.Kheops.OP.Domain.Regularisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.DTO {
    public class SelectionFormuleRegularisationDto : SelectionRegularisationDto {
        public override PerimetreSelectionRegul Perimetre => PerimetreSelectionRegul.Formule;
        public override int NumeroObjet => 0;
    }
}
