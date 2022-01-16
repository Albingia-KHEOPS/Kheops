using Albingia.Kheops.OP.Domain.Affaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.DTO
{
    public class ObjetDto
    {
        public AffaireId AffaireId { get; set; }
        public int NumRisque { get; set; }
        public int Code { get; set; }
        public long Designation { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Unite { get; set; }
        public long Valeur { get; set; }
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public DateTime? DateDebutImplicite { get; set; }
        public DateTime? DateFinImplicite { get; set; }
    }
}
