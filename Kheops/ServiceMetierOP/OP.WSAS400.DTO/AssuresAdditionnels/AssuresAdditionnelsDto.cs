using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.AssuresAdditionnels
{
    [DataContract]
    public class AssuresAdditionnelsDto
    {
        [DataMember]
        public Int32 CodeError { get; set; }
        [DataMember]
        public List<AssuresRefDto> AssuresReference { get; set; }
        [DataMember]
        public List<AssuresNonRefDto> AssuresNonReference { get; set; }
        [DataMember]
        public DateTime? DateEffetAvenantModificationLocale { get; set; }
        [DataMember]
        public bool IsAvenantModificationLocale { get; set; }
    }
}
