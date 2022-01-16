using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.ParametreFiltre
{
    public class ModeleLigneBrancheCibleDto
    {
        [DataMember]
        [Column(Name = "IDPAIRE")]
        public Int64 GuidIdPaire { get; set; }

        [DataMember]
        [Column(Name = "ACTION")]
        public string Action { get; set; }

        [DataMember]
        [Column(Name = "BRANCHE")]
        public string Branche { get; set; }

        [DataMember]
        [Column(Name = "CIBLE")]
        public string Cible { get; set; }               

    }
}
