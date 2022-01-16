using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OP.WSAS400.DTO.FormuleGarantie
{
    [DataContract]
    public class ParamNatGarDto
    {
        [Column(Name = "CARACTERE")]
        [DataMember]
        public string Caractere { get; set; }
        [Column(Name = "NATURE")]
        [DataMember]
        public string Nature { get; set; }
        [Column(Name = "NATUREPARAMCHECKED")]
        [DataMember]
        public string NatureParamChecked { get; set; }
        [Column(Name = "NATUREPARAMNONCHECKED")]
        [DataMember]
        public string NatureParamNoChecked { get; set; }
    }
}
