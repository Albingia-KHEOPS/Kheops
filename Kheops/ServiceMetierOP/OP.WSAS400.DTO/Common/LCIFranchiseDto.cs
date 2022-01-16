using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Common
{
    public class LCIFranchiseDto
    {        
        [DataMember]
        [Column(Name = "VALEUR")]
        public Double Valeur { get; set; }
        [DataMember]
        [Column(Name = "UNITE")]
        public string Unite { get; set; }
        [DataMember]
        [Column(Name = "TYPE")]
        public string Type { get; set; }
        [DataMember]
        [Column(Name = "ISINDEXE")]
        public string IsIndexe { get; set; }
        [DataMember]
        [Column(Name = "LIENCOMPLEXE")]
        public Int64 LienComplexe { get; set; }
        [DataMember]
        [Column(Name = "CODECOMPLEXE")]
        public string CodeComplexe { get; set; }
        [DataMember]
        [Column(Name = "LIBCOMPLEXE")]
        public string LibComplexe { get; set; }
    }
}
