using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Engagement
{
    [DataContract]
    public class InfoContratConnexeDto
    {
        [DataMember]
        public string NumeroConnexite { get; set; }
        [DataMember]
        public List<ContratConnexeDto> ContratsConnexes { get; set; }
    }
}
