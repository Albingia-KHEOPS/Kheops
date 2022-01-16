using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Traite
{
    public class TraiteVentilationDto : _Traite_Base
    {
        public string NomVentilation { get; set; }
        public long IdVentilation { get; set; }
        public string SMP { get; set; }
        public string SMPF { get; set; }
        public string SMPAlbingia { get; set; }
        public string LCI { get; set; }
        public string Engagement { get; set; }
        public bool RisqueSel { get; set; }
        public string EngagementVentilationCalcule { get; set; }
        public string EngagementVentilationForce { get; set; }
        public string CapitauxTotaux { get; set; }
        public string CapitauxAlbingia { get; set; }
        public bool VentilationSel { get; set; }
        public int? Position { get; set; }
    }
}
