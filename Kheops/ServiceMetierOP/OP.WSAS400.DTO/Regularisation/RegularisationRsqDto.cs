using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Offres.Risque;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class RegularisationRsqDto
    {
        [DataMember]
        public string CodeErreurAvt { get; set; }
        [DataMember]
        public string ErreurAvt { get; set; }
        [DataMember]
        public long ReguleId { get; set; }
        [DataMember]
        public List<RisqueDto> Risques { get; set; }
        [DataMember]
        public PeriodeReguleDto PeriodeRegule { get; set; }


    }
}
