using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.GestionNomenclature
{
    [DataContract]
    public class GestionNomenclatureDto
    {
        [DataMember]
        public string Typologie { get; set; }
        [DataMember]
        public List<ParametreDto> Typologies { get; set; }
        [DataMember]
        public string Branche { get; set; }
        [DataMember]
        public List<ParametreDto> Branches { get; set; }
        [DataMember]
        public string Cible { get; set; }
        [DataMember]
        public List<ParametreDto> Cibles { get; set; }

        [DataMember]
        public List<NomenclatureDto> Nomenclatures { get; set; }
    }
}
