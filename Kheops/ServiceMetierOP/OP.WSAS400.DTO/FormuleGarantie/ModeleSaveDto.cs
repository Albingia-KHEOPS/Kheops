using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.FormuleGarantie
{
    [DataContract]
    public class ModeleSaveDto
    {
        [DataMember]
        public string GuidId { get; set; }
        [DataMember]
        public List<ModeleNiv1SaveDto> Modeles { get; set; }
    }
}
