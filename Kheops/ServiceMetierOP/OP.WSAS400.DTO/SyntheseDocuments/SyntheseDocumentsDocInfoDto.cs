using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.SyntheseDocuments
{
    [DataContract]
    public class SyntheseDocumentsDocInfoDto
    {
        [DataMember]
        public Int64 DocId { get; set; }
        [DataMember]
        public string Document { get; set; }
        [DataMember]
        public Int64 NbExemp { get; set; }
        [DataMember]
        public string Imprim { get; set; }
    }
}
