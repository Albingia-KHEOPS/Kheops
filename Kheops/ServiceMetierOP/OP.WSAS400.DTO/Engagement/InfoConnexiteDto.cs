using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Engagement
{
    [DataContract]
    public class InfoConnexiteDto
    {
        [DataMember]
        public List<TypeConnexiteDto> TypesConnexite { get; set; }
    }
}
