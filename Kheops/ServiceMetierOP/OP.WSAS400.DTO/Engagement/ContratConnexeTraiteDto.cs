using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Engagement
{
    public class ContratConnexeTraiteDto
    {
        [DataMember]
        [Column(Name = "NUMCONNEXITE")]
        public Int32 NumConnexite { get; set; }

        [DataMember]
        [Column(Name = "IDECONNEXITE")]
        public Int64 IdeConnexite { get; set; }

        [DataMember]
        [Column(Name = "NUMCONTRAT")]
        public String NumContrat { get; set; }

        [DataMember]
        [Column(Name = "VERSIONCONTRAT")]
        public Int16 VersionContrat { get; set; }
        
        [DataMember]
        [Column(Name = "DATEDEBUTENGAGEMENT")]
        public Int64 DateDebutEngagement { get; set; }

        [DataMember]
        [Column(Name = "DATEFINENGAGEMENT")]
        public Int64 DateFinEngagement { get; set; }
        
        [DataMember]
        [Column(Name = "IDENGAGEMENT")]
        public Int64 IdEngagement { get; set; }
    }
}
