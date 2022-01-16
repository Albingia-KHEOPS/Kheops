using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OP.WSAS400.DTO.Offres.Risque;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.FormuleGarantie;

namespace OP.WSAS400.DTO.MatriceRisque
{
    [DataContract]
    public class MatriceRisqueDto
    {
        [DataMember]
        public List<RisqueDto> Risques { get; set; }
        [DataMember]
        public bool FormGen { get; set; }
        [DataMember]
        public bool HasFormGen { get; set; }
        [DataMember]
        public FormuleDto Formule { get; set; }
        [DataMember]
        public bool AddFormule { get; set; }
        [DataMember]
        public bool CopyRisque { get; set; }
        [DataMember]
        public string   formuleOption { get; set; }
    }
}
