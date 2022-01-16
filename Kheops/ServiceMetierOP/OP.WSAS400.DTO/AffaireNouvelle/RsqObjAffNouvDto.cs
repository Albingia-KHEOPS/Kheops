using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.AffaireNouvelle
{
    [DataContract]
    public class RsqObjAffNouvDto
    {
        [DataMember]
        public string CodeOffre { get; set; }
        [DataMember]
        public Int64 Version { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string CodeContrat { get; set; }
        [DataMember]
        public Int64 VersionContrat { get; set; }

        [DataMember]
        public List<RsqObjAffNouvRowDto> ListRsqObj { get; set; }

    }
}
