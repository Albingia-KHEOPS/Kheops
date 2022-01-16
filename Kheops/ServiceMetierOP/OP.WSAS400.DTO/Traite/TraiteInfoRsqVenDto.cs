using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.WSAS400.DTO.Traite
{
    public class TraiteInfoRsqVenDto : _Traite_Base 
    {
        public List<TraiteRisqueDto> TraiteRisques { get; set; }
        public List<TraiteVentilationDto> TraiteVentilations { get; set; }
    }
}
