using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Traite
{
    [DataContract]
    public class TraiteRisqueDto : _Traite_Base
    {
        [DataMember]
        public int CodeRisque { get; set; }
        [DataMember]
        public string DescrRsq { get; set; }
        [DataMember]
        public List<TraiteVentilationDto> TraiteVentilations { get; set; }

        public TraiteRisqueDto()
        {
            this.TraiteVentilations = new List<TraiteVentilationDto>();
        }
    }
}
