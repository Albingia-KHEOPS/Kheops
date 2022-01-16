using System;
using System.Collections.Generic;

using System.Data.Linq.Mapping;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Avenant;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.Regularisation
{ 
    [DataContract] // CODERSQ, KHVKDEID IDGAR, KDEGARAN CODEGAR
    public class RegularisationSimplifieeDto
    {
        [DataMember]
        [Column(Name = "IDLOT")]
        public long LotId { get; set; }

        [DataMember]
        [Column(Name = "CODEGAR")]
        public string CodeGar { get; set; }

        [DataMember]
        [Column(Name = "IDGAR")]
        public Int64 IdGar { get; set; }

        [DataMember]
        [Column(Name = "CODERSQ")]
        public Int32 IdRsq { get; set; }

        [DataMember]
        public bool IsGarRC { get; set; }

        [DataMember]
        public long SequenceGar { get; set; }
    }
}
