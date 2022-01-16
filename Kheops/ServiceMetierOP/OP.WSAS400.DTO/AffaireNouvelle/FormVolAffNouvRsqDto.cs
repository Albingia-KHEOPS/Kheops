using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.AffaireNouvelle
{
    [DataContract]
    public class FormVolAffNouvRsqDto
    {
        [DataMember]
        public Int64 CodeRisque { get; set; }
        [DataMember]
        public string DescRisque { get; set; }
        [DataMember]
        public List<FormVolAffNouvFormDto> Formules { get; set; }
    }
}
