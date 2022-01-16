using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.SyntheseDocuments
{
    [DataContract]
    public class SyntheseDocumentsDocDto
    {
        [DataMember]
        public Int64 LotId { get; set; }
        [DataMember]
        public Int64 Ordre { get; set; }
        [DataMember]
        public string TypeDestinataire { get; set; }
        [DataMember]
        public Int64 DestinataireId { get; set; }
        [DataMember]
        public string TypeEnvoi { get; set; }
        [DataMember]
        public string LibEnvoi { get; set; }
        [DataMember]
        public List<SyntheseDocumentsDocInfoDto> DocInfos { get; set; }
    }
}
