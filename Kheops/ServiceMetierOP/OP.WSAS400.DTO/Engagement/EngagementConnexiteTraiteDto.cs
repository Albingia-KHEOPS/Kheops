using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Engagement
{
    public class EngagementConnexiteTraiteDto
    {
        [DataMember]
        [Column(Name = "IDTRAITE")]
        public Int64 IdTraite { get; set; }
        [DataMember]
        [Column(Name = "IDENGAGEMENT")]
        public Int64 IdEngagement { get; set; }
        [DataMember]
        [Column(Name = "CODETRAITE")]
        public String CodeTraite { get; set; }
        [DataMember]
        [Column(Name = "LIBELLETRIATE")]
        public String LibelleTraite { get; set; }
        [DataMember]
        [Column(Name = "ENGAGEMENTALBINGIA")]
        public Int64 EngagementAlbingia { get; set; }
        [DataMember]
        [Column(Name = "ENGAGEMENTTOTAL")]
        public Int64 EngagementTotal { get; set; }
    }
}
