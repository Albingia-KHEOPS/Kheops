using OP.WSAS400.DTO.Offres.Parametres;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.ParametreInventaires
{
    [DataContract]
    public class ParamInventairesDto
    {
        [DataMember]
        [Column(Name = "GUIDID")]
        public Int64 GuidId { get; set; }
        [DataMember]
        [Column(Name = "CODE")]
        public string Code { get; set; }
        [DataMember]
        [Column(Name = "LIBELLE")]
        public string Libelle { get; set; }
        [DataMember]
        [Column(Name = "KAGTMAP")]
        public Int32 Kagtmap { get; set; }
        [DataMember]
        [Column(Name = "CODEFILTRE")]
        public Int64 CodeFiltre { get; set; }
        [DataMember]
        public List<ParametreDto> Filtres { get; set; }
    }
}
