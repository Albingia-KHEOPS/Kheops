using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.ParametreInventaires
{
    [DataContract]
    public class ParamInventairesListDto
    {
        [DataMember]
        public List<ParamInventairesDto> Inventaires { get; set; }
        [DataMember]
        public int ReturnValue { get; set; }
    }
}
