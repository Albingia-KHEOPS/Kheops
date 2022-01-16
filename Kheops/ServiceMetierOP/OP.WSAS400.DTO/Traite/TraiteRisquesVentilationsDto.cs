using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace OP.WSAS400.DTO.Traite
{
    public class TraiteRisquesVentilationsDto : _Traite_Base
    {
        [Column(Name = "RISQUE")]
        public int CodeRisque { get; set; }

        [Column(Name = "DESCRRSQ")]
        public string DescrRsq { get; set; }

        [Column(Name = "NOMVENTILATION")]
        public string NomVentilation { get; set; }

        [Column(Name = "IDVENTILATION")]
        public Int64 IdVentilation { get; set; }

        [Column(Name = "SMP")]
        public Int64 SMP { get; set; }

        public long SMPForce { get; set; }

        [Column(Name = "LCI")]
        public Int64 LCI { get; set; }

        [Column(Name = "ENGAGEMENTFORCE")]
        public Int64 EngagementForce { get; set; }

        public long Engagement { get; set; }

        [Column(Name = "RSQSEL")]
        public string RisqueSel { get; set; }

        [Column(Name = "ENGAGEMENTVENTCALCULE")]
        public Int64 EngagementVentilationCalcule { get; set; }

        [Column(Name = "ENGAGEMENTVENTFORCE")]
        public Int64 EngagementVentilationForce { get; set; }

        [Column(Name = "VENTSEL")]
        public string VentilationSel { get; set; }
    }
}
