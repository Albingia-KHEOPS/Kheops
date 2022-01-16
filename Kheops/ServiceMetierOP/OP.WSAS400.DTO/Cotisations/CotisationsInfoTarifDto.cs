using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Cotisations
{
    [DataContract]
    public class CotisationsInfoTarifDto
    {
        [DataMember]
        public string NomGarantie { get; set; }
        [DataMember]
        public List<CotisationsTarifDto> Tarifs { get; set; }
    }
}
