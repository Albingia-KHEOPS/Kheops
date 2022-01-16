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
    public class TypologieDto
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Libelle { get; set; }
        [DataMember]
        public string Lien { get; set; }
        [DataMember]
        public string ValeurFiltre { get; set; }
        [DataMember]
        public Int32 Niveau { get; set; }
        [DataMember]
        public List<ParametreDto> Valeurs { get; set; }
    }
}
