using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.DocumentGestion
{
    [DataContract]
    public class DocumentGestionDocDto {
        [DataMember]
        public Int64 LotId { get; set; }

        [DataMember]
        public Int64 DocId { get; set; }

        [DataMember]
        public List<DocumentGestionDocInfoDto> ListDocInfos { get; set; }

        [DataMember]
        public bool FirstGeneration { get; set; }
    }
}
