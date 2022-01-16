using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Engagement
{
    [DataContract]
    public class TypeConnexiteDto
    {
        [DataMember]
        public string CodeConnexite { get; set; }
        [DataMember]
        public string LibConnexite { get; set; }
        [DataMember]
        public List<ConnexiteDto> Connexites { get; set; }
    }
}
