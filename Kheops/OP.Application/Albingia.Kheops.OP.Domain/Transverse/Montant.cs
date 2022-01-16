using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Transverse {
    public class Montant {
        public decimal ValeurHorsTaxe { get; set; }
        public decimal Taxe { get; set; }

        /// <summary>
        /// Gets or sets the value of the amount. Should be value+tax
        /// </summary>
        public decimal Valeur { get; set; }
    }
}
