using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Engagement
{
    [DataContract]
    public class ModeleEngTraitDTO
    {
        [DataMember]
        public string CodeEngagement { get; set; }
        [DataMember]
        public Int64 ValeurEngagement { get; set; }
    }
}
