using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.GestionIntervenants
{
    [DataContract]
    public class IntervenantsInfoDto
    {
        [DataMember]
        public bool IsAvenantModificationLocale { get; set; }
        [DataMember]
        public List<IntervenantDto> Intervenants { get; set; }
    }
}
