using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Engagement
{
    public class EngagementTraiteDto : _Engagement_Base
    {
        public string FamilleTraite { get; set; }
        public string NomTraite { get; set; }
        public string EngagementTotal { get; set; }
        public string EngagementAlbingia { get; set; }
        public string SMPTotal { get; set; }
        public string SMPAlbingia { get; set; }
    }
}
