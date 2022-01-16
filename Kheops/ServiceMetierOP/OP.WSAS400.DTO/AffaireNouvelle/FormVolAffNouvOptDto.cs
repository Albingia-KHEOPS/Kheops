using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.AffaireNouvelle
{
    [DataContract]
    public class FormVolAffNouvOptDto
    {
        [DataMember]
        public Int64 GuidOpt { get; set; }
        [DataMember]
        public Int64 CodeOpt { get; set; }
        [DataMember]
        public bool CheckRow { get; set; }
        [DataMember]
        public List<FormVolAffNouvVolDto> Volets { get; set; }
    }
}
