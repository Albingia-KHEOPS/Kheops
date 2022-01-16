using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Ecran.ModifierOffre
{
    [DataContract]
    public class ModifierOffreGetQueryDto : _ModifierOffre_Base, IQuery
    {
        [DataMember]
        public string CodeOffre { get; set; }
        [DataMember]
        public int Version { get; set; }
        [DataMember]
        public string Type { get; set; }

    }
}
