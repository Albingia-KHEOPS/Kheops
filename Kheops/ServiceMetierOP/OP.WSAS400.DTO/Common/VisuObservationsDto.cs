using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Common
{
    public class VisuObservationsDto
    {
        [DataMember]
        [Column(Name = "OBSVGEN")]
        public string ObsvInfoGen { get; set; }

        [DataMember]
        [Column(Name = "OBSVCOMM")]
        public string ObsvCotisation { get; set; }

        [DataMember]
        [Column(Name = "OBSVENG")]
        public string ObsvEngagement { get; set; }

        [DataMember]
        [Column(Name = "OBSVMNTREF")]
        public string ObsvMntRef { get; set; }
        [DataMember]
        [Column(Name = "OBSVREFGEST")]
        public string  ObsvRefGest { get; set; }
        [DataMember]
        [Column(Name = "IDENG")]
        public string IdEng { get; set; }
        
    }
}
