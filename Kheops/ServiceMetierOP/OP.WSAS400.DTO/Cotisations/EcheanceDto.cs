using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Cotisations
{
    public class EcheanceDto
    {
        [Column(Name = "DATEECHA")]
        public Int16 DateEcheanceAnnee { get; set; }
        [Column(Name = "DATEECHM")]
        public Int16 DateEcheanceMois { get; set; }
        [Column(Name = "DATEECHJ")]
        public Int16 DateEcheanceJour { get; set; }
        [Column(Name = "POURCENTAGEPRIME")]
        public Double PourcentagePrime { get; set; }
        [Column(Name = "POURCENTAGECALCULE")]
        public Double PourcentageCalcule { get; set; }
        [Column(Name = "MONTANT")]
        public Double Montant { get; set; }
        [Column(Name = "MONTANTCALCULE")]
        public Double MontantCalcule { get; set; }
        [Column(Name = "FRAISACCESSOIRE")]
        public Int32 FraisAccessoire { get; set; }
        [Column(Name = "APPLIQUETAXEATTENTAT")]
        public string AppliqueTaxeAttentat { get; set; }
        [Column(Name = "NUMPRIME")]
        public Int32 NumPrime { get; set; }
        
    }
}
