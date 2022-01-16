using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Offres
{
    [DataContract]
    public class TestDto
    {
        [DataMember]
        [Column(Name = "P_OUT")]
        public Int64 P_OUT { get; set; }
    }
}
