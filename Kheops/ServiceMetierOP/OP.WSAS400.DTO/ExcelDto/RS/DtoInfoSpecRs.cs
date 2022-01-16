using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OP.WSAS400.DTO.ExcelDto.RS
{
    [DataContract]
    public class DtoInfoSpecRs
    {
        [DataMember]
        public Entete AlbEntetes { get; set; }
        [DataMember]
        public Objets AlbObjets { get; set; }
        [DataMember]
        public Garanties AlbGaranties { get; set; }
        [DataMember]
        public string AlbCells { get; set; }
    }
}
