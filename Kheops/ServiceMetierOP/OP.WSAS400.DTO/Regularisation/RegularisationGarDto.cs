using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Offres.Risque;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class RegularisationGarDto
    {
        [DataMember]
        public RisqueDto Risque { get; set; }
        [DataMember]
        public List<RegularisationGarantieDto> Garanties { get; set; }

        [DataMember]
        public PeriodeReguleDto PeriodeRegule { get; set; }
    }
}
