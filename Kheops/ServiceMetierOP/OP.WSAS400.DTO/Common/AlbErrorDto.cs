using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Common
{
    [DataContract]
    public class AlbErrorDto
    {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Label { get; set; }
    }
}
