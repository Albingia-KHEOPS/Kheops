using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Inventaires
{
    [DataContract]
    public class CPVilleDto
    {
        [DataMember]
        [Column(Name="CP")]
        public string CodePostal { get; set; }
        [DataMember]
        [Column(Name = "VILLE")]
        public string Ville { get; set; }
    }
}
