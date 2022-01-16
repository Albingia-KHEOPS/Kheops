using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Avenant
{
    [DataContract]
    public class AvenantAlerteDto
    {
        [DataMember]
        [Column(Name="TYPEBLOC")]
        public string TypeBloquante { get; set; }
        [DataMember]
        [Column(Name = "MSGALERT")]
        public string MessageAlerte { get; set; }
        [DataMember]
        [Column(Name="TYPEALERT")]
        public string TypeAlerte { get; set; }
        [DataMember]
        [Column(Name = "LIENALERT")]
        public string LienAlerte { get; set; }
    }
}
