using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Common
{
    [DataContract]
    public class IdObjetSituationDto : IdContratDto
    {
        [DataMember]
        [Column(Name = "SIT")]
        public string State { get; set; }

        [DataMember]
        [Column(Name = "ID")]
        public int Id { get; set; }
    }
}
