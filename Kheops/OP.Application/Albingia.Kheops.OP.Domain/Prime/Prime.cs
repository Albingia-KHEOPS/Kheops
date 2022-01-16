using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain {
    public class Prime {
        virtual public PrimeId Id { get; set; }
        virtual public decimal MontantHT { get; set; }
        virtual public decimal MontantTT { get; set; }
        virtual public int NumeroEcheance { get; set; }
        virtual public DateTime DateEcheance { get; set; }
        virtual public TypeRelance TypeRelance { get; set; }
    }
}
