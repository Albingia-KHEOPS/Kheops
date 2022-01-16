using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Volet;
using OP.WSAS400.DTO.FormuleGarantie;

namespace OP.WSAS400.DTO.MatriceGarantie
{
    [DataContract]
    public class MatriceGarantieDto
    {
        [DataMember]
        public Int32 CodeError { get; set; }
        [DataMember]
        public List<RisqueDto> Risques { get; set; }
        [DataMember]
        public List<DtoVolet> Volets { get; set; }

        [DataMember]
        public bool FormGen { get; set; }
        [DataMember]
        public bool HasFormGen { get; set; }
        [DataMember]
        public FormuleDto Formule { get; set; }
        [DataMember]
        public bool AddFormule { get; set; }
         [DataMember]
        public string formuleOption { get; set; }
    }
}
