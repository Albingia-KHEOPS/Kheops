using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.SuiviDocuments
{
    [DataContract]
    public class SuiviDocumentsListeDto
    {
        [DataMember]
        public List<SuiviDocumentsLotDto> SuiviDocumentsListeLot { get; set; }
        [DataMember]
        public List<SuiviDocumentsPlatDto> SuiviDocumentsPlat { get; set; }
        [DataMember]
        public Int32 CountLine { get; set; }
        [DataMember]
        public Int32 StartLine { get; set; }
        [DataMember]
        public Int32 EndLine { get; set; }
        [DataMember]
        public Int32 PageNumber { get; set; }
    }
}
