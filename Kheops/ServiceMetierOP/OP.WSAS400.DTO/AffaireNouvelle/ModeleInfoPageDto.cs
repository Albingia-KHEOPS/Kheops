using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.AffaireNouvelle
{
    [DataContract]
    public class ModeleInfoPageCommissionDto
    {
        [DataMember]
        public List<CourtierDto> LstCourties { get; set; }
        [DataMember]
        public CommissionCourtierDto CommissionsStd { get; set; }
    }
}
