using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Branches;

namespace OP.WSAS400.DTO.Clause
{
    [DataContract]
    public class ClausierGetQueryDto : _Clause_Base
    {
        [DataMember]
        public string Titre { get; set; }

        [DataMember]
        public string MotsCle { get; set; }

        [DataMember]
        public string Branche { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string IdOffre { get; set; }

        [DataMember]
        public int Version { get; set; }

        [DataMember]
        public string Etape { get; set; }
    }
}
