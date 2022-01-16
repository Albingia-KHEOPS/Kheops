using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Application.Port.Driver.DTO {
    public class ValeursUniteDto {
        public decimal ValeurOrigine { get; set; }
        public decimal ValeurActualise { get; set; }
        public decimal ValeurTravail { get; set; }
        public string CodeUnite { get; set; }
        public string CodeBase { get; set; }
        public long? IdCPX { get; set; }
    }
}
