using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.NavigationArbre
{
    [DataContract]
    public class RisqueDto
    {
        [Column(Name = "JERSQ")]
        [DataMember]
        public int Numero { get; set; }

        [Column(Name = "JECCH")]
        [DataMember]
        public int Code { get; set; }

        [Column(Name = "KABDESC")]
        [DataMember]
        public string Descriptif { get; set; }

        [DataMember]
        public List<FormuleDto> Formules;

        [DataMember]
        public string TagRisque { get; set; }

        [DataMember]
        public bool isBadDate {get;set;}

        public RisqueDto()
        {
            Numero = 0;
            Code = 0;
            Descriptif = string.Empty;
            Formules = new List<FormuleDto>();
        }
    }
}
