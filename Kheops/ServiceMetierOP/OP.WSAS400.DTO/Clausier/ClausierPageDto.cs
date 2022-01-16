using System.Collections.Generic;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.Clausier
{
    [DataContract]
    public class ClausierPageDto
    {
        [DataMember]
        public List<ParametreDto> Rubriques { get; set; }
        [DataMember]
        public List<ParametreDto> SousRubriques { get; set; }
        [DataMember]
        public List<ParametreDto> Sequences { get; set; }
        [DataMember]
        public List<ParametreDto> MotsCles1 { get; set; }
        [DataMember]
        public List<ParametreDto> MotsCles2 { get; set; }
        [DataMember]
        public List<ParametreDto> MotsCles3 { get; set; }
        [DataMember]
        public List<ParametreDto> Contextes { get; set; }
    }
}
