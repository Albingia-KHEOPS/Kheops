using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.Quittance
{
    [DataContract]
    public class QuittanceForceGarantieDto
    {
        [DataMember]
        public QuittanceForceInfoFormuleDto Formule { get; set; }
        [DataMember]
        public List<QuittanceForceInfoGarantieDto> ListGaranties { get; set; }
        [DataMember]
        public List<ParametreDto> CodesTaxe { get; set; }
        [DataMember]
        public string CodeTaxe { get; set; }
    }
}
