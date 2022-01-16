using Albingia.Kheops.OP.Domain.Affaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.DTO {
    public class InformationSpecifiqueDto {
        public AffaireId AffaireId { get; set; }
        public int NumeroRisque { get; set; }
        public int NumeroObjet { get; set; }
        public int NumeroOption { get; set; }
        public int NumeroFormule { get; set; }
        public string Cle { get; set; }
        public LigneModeleISDto ModeleLigne { get; set; }
        public InfoSpeValeurDto Valeur { get; set; }
    }
}
