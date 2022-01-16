using OP.WSAS400.DTO.FormuleGarantie;
using OP.WSAS400.DTO.Offres.Risque;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class RegularisationGarInfoDto
    {
        [DataMember]
        public string ErreurStr { get; set; }
        [DataMember]
        public LigneRegularisationGarantieDto RegulPeriodDetail { get; set; }
        [DataMember]
        public RisqueDto AppliqueRegule { get; set; }
        [DataMember]
        public List<LigneMouvementDto> ListMvtPeriod { get; set; }
        [DataMember]
        public List<LigneMouvtGarantieDto> ListPeriodRegulGar { get; set; }
        [DataMember]
        public Int64 MouvementPeriodeDebMin { get; set; }
        [DataMember]
        public Int64 MouvementPeriodeFinMax { get; set; }

        [DataMember]
        public GarantieInfo GarantieInfo { get; set; }


    }
}
