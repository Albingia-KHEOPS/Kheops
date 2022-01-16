using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.FormuleGarantie
{
    [DataContract]
    public class ModeleNiv2SaveDto
    {
        [DataMember]
        public bool MAJ { get; set; }
        [DataMember]
        public string GuidGarantie { get; set; }
        [DataMember]
        public string Caractere { get; set; }
        [DataMember]
        public string Nature { get; set; }
        [DataMember]
        public string NatureParam { get; set; }
        [DataMember]
        public List<ModeleNiv3SaveDto> Modeles { get; set; }
        [DataMember]
        public string ParamNatMod { get; set; }
    }
}
