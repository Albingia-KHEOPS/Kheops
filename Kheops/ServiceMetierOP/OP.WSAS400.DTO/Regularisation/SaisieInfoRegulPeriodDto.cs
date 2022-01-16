using OP.WSAS400.DTO.Offres.Parametres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class SaisieInfoRegulPeriodDto
    {
        [DataMember]
        public SaisieGarInfoDto LignePeriodRegul { get; set; }
        [DataMember]
        public List<ParametreDto> UnitesDef { get; set; }
        [DataMember]
        public List<ParametreDto> CodesTaxesDef { get; set; }
        [DataMember]
        public List<ParametreDto> UnitesPrev { get; set; }
        [DataMember]
        public List<ParametreDto> CodesTaxesPrev { get; set; }

        [DataMember]
        public int CodeFormule { get; set; }

        [DataMember]
        public int CodeOption { get; set; }

        [DataMember]
        public string CodeGarantie { get; set; }

        [DataMember]
        public string Libelle { get; set; }
    }
}
