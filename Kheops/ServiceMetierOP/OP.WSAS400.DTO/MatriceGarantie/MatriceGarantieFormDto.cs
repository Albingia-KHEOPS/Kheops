using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.FormuleGarantie;

namespace OP.WSAS400.DTO.MatriceGarantie
{
    [DataContract]
    public class MatriceGarantieFormDto
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Designation { get; set; }
        [DataMember]
        public List<FormuleGarantieDto> Volets { get; set; }
    }
}
