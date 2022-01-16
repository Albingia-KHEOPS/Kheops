using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Cotisations
{
    [DataContract]
    public class CotisationsTarifDto
    {
        [DataMember]    
        public string CodeTarif { get; set; }
        [DataMember]
        public string LCIValeur { get; set; }
        [DataMember]
        public string LCIUnite { get; set; }
        [DataMember]
        public string FranchiseValeur { get; set; }
        [DataMember]
        public string FranchiseUnite { get; set; }
        [DataMember]
        public string TauxValeur { get; set; }
        [DataMember]
        public string TauxUnite { get; set; }
    }
}
