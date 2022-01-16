using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Quittance
{
    [DataContract]
    public class QuittanceForceInfoGarantieDto
    {
        [DataMember]
        public string CodeGarantie { get; set; }
        [DataMember]
        public string LibGarantie { get; set; }
        [DataMember]
        public bool CatNat { get; set; }
        [DataMember]
        public double MontantCal { get; set; }
        [DataMember]
        public string CodeTaxe { get; set; }
        [DataMember]
        public string LibTaxe { get; set; }
    }
}
