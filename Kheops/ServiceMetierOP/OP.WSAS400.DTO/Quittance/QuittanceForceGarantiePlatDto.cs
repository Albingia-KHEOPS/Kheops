using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Quittance
{
    public class QuittanceForceGarantiePlatDto
    {
        [Column(Name = "CODEFOR")]
        public Int64 CodeFor { get; set; }
        [Column(Name = "FORMLETTRE")]
        public string FormLettre { get; set; }
        [Column(Name = "FORMDESC")]
        public string FormDesc { get; set; }
        [Column(Name = "CODERSQ")]
        public int CodeRsq { get; set; }
        [Column(Name = "RSQDESC")]
        public string RsqDesc { get; set; }
        [Column(Name = "CODEGAR")]
        public string CodeGarantie { get; set; }
        [Column(Name = "LIBGAR")]
        public string LibGarantie { get; set; }
        [Column(Name = "CATNAT")]
        public string CatNat { get; set; }
        [Column(Name = "MONTANT")]
        public double MontantCal { get; set; }
        [Column(Name = "CODETAXE")]
        public string CodeTaxe { get; set; }
        [Column(Name = "LIBTAXE")]
        public string LibTaxe { get; set; }
    }
}
