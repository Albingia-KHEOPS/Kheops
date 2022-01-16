using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class ConfirmSaisieReguleDto
    {
        [DataMember]
        [Column(Name = "CODEGAR")]
        public string CodeGar { get; set; }
        [DataMember]
        [Column(Name = "LIBGAR")]
        public string LibGar { get; set; }
        [DataMember]
        [Column(Name = "MNTHT")]
        public Double MntHT { get; set; }
        //[DataMember]
        //[Column(Name = "MNTTAXE")]
        //public Double MntTaxe { get; set; }
        //[DataMember]
        //[Column(Name = "CODEGAR")]
        //public Double CodeGar { get; set; }
        [DataMember]
        [Column(Name = "ATTENTATHT")]
        public Double AttentatHT { get; set; }
    }
}
