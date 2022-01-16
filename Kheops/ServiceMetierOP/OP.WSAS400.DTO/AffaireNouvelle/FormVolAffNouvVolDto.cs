using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.AffaireNouvelle
{
    [DataContract]
    public class FormVolAffNouvVolDto
    {
        [DataMember]
        public Int64 GuidVolet { get; set; }
        [DataMember]
        public string CodeVolet { get; set; }
        [DataMember]
        public string DescVolet { get; set; }
        [DataMember]
        public bool CheckRow { get; set; }
    }
}
