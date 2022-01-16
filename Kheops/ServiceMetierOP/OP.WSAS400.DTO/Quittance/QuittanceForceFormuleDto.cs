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
    public class QuittanceForceFormuleDto
    {
        [DataMember]
        public List<QuittanceForceInfoFormuleDto> ListFormule { get; set; }
    }
}
