using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.DocumentGestion
{
    [DataContract]
    public class CourrierTypeDto
    {
        [DataMember]
        [Column(Name = "GUIDID")]
        public Int64 GuidId { get; set; }

        [DataMember]
        [Column(Name = "CODE")]
        public string Code { get; set; }

        [DataMember]
        [Column(Name = "LIBELLE")]
        public string Libelle { get; set; }
    }
}
