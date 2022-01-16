using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.AffaireNouvelle
{
    [DataContract]
    public class FormVolAffNouvFormDto
    {
        [DataMember]
        public Int64 GuidForm { get; set; }
        [DataMember]
        public Int64 CodeForm { get; set; }
        [DataMember]
        public string LettreForm { get; set; }
        [DataMember]
        public string DescFormule { get; set; }
        [DataMember]
        public bool CheckRow { get; set; }
        [DataMember]
        public List<FormVolAffNouvOptDto> Options { get; set; }
    }
}
