using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.AffaireNouvelle
{
    [DataContract]
    public class FormVolAffNouvRecapDto
    {
        [DataMember]
        public string CodeContrat { get; set; }
        [DataMember]
        public string VersionContrat { get; set; }
        [DataMember]
        public List<FormVolAffNouvRsqDto> Risques { get; set; }
        [DataMember]
        public List<FormVolAffNouvFormDto> Formules { get; set; }
        [DataMember]
        public Int64 CountGarTar { get; set; }
    }
}
