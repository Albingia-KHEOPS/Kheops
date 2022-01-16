using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.AssuresAdditionnels
{
    [DataContract]
    public class AssuresNonRefDto
    {
        [DataMember]
        [Column(Name = "CODEQUALITE1")]
        public string CodeQualite1 { get; set; }
        [DataMember]
        [Column(Name = "LIBQUALITE1")]
        public string LibQualite1 { get; set; }
        [DataMember]
        [Column(Name = "CODEQUALITE2")]
        public string CodeQualite2 { get; set; }
        [DataMember]
        [Column(Name = "LIBQUALITE2")]
        public string LibQualite2 { get; set; }
        [DataMember]
        [Column(Name = "CODEQUALITE3")]
        public string CodeQualite3 { get; set; }
        [DataMember]
        [Column(Name = "LIBQUALITE3")]
        public string LibQualite3 { get; set; }
        [DataMember]
        [Column(Name = "QUALITEAUTRE")]
        public string QualiteAutre { get; set; }
        [DataMember]
        public List<ParametreDto> Qualites1 { get; set; }
        [DataMember]
        public List<ParametreDto> Qualites2 { get; set; }
        [DataMember]
        public List<ParametreDto> Qualites3 { get; set; }
    }
}
