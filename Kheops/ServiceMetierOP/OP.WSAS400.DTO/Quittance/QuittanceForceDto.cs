using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Quittance
{
    [DataContract]
    public class QuittanceForceDto
    {
        [DataMember]
        public QuittanceForceTotalDto ForceTotal { get; set; }
        [DataMember]
        public QuittanceForceFormuleDto ForceFormule { get; set; }

    }
}
