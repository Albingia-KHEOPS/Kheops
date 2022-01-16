using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Cotisations
{
    public class EcheancierDto
    {
        public Double ComptantHT { get; set; }
        public Double PrimeComptant { get; set; }
        public Int32 FraisAccessoire { get; set; }
        public List<EcheanceDto> Echeances { get; set; }

        public string PeriodeDebut { get; set; }   
        public double PrimeHT { get; set; }
        public double FraisAccessoiresHT { get; set; }
        public bool TaxeAttentat { get; set; }

        public bool IsModeSaisieParMontant { get; set; }
    }
}
