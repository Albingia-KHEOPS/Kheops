using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.MatriceFormule
{
    [DataContract]
    public class MatriceFormuleOptDto
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Designation { get; set; }
        [DataMember]
        public string Icone { get; set; }
        [DataMember]
        public Int32 NumeroChrono { get; set; }
        [DataMember]
        public string Anomalie { get; set; }
    }
}
