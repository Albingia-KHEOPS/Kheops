using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Cotisations
{
    [DataContract]
    public class CotisationsDto : Cotisations_Base
    {
        [DataMember]
        public CotisationInfoGarantieDto GarantiesInfo { get; set; }
        [DataMember]
        public string CommentForce { get; set; }
        [DataMember]
        public bool MontantForce { get; set; }
        [DataMember]
        public bool TraceCC { get; set; }
        
        public bool isChecked { get; set; }

        public CotisationsDto()
        {
            this.GarantiesInfo = new CotisationInfoGarantieDto();
        }
    }
}
