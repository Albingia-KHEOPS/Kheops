using System;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.OffresRapide
{
    [DataContract]
    public class OffreRapideInfoDto
    {
        [DataMember]
        [Column(Name = "CODEOFFRE")]
        public string CodeOffre { get; set; }
        [DataMember]
        [Column(Name = "CODETYPEOFFRE")]
        public string CodeTypeOffre { get; set; }
        [DataMember]
        [Column(Name = "VERSION")]
        public int Version { get; set; }
        [DataMember]
        [Column(Name = "CODEAVN")]
        public int CodeAvenant { get; set; }
        [DataMember]
        [Column(Name = "TYPETRAITEMENT")]
        public string TypeTraitement { get; set; }
        [Column(Name = "DATESAISIE")]
        public long? DateDeSaisie { get; set; }
        [DataMember]
        [Column(Name = "CODEBRANCHE")]
        public string CodeBranche { get; set; }
        [DataMember]
        [Column(Name = "LIBBRANCHE")]
        public string LibelleBranche { get; set; }
        [DataMember]
        [Column(Name = "CODECIBLE")]
        public string CodeCible { get; set; }
        [DataMember]
        [Column(Name = "LIBCIBLE")]
        public string LibelleCible { get; set; }
        [DataMember]
        [Column(Name = "CODEETAT")]
        public string CodeEtat { get; set; }
        [DataMember]
        [Column(Name = "LIBETAT")]
        public string LibellleEtat { get; set; }
        [DataMember]
        [Column(Name = "CODESIT")]
        public string CodeSituation { get; set; }
        [DataMember]
        [Column(Name = "SITUATIONLIB")]
        public string LibelleSituation { get; set; }
        [DataMember]
        [Column(Name = "DESCRIPTIF")]
        public string Descriptif { get; set; }
        [DataMember]
        [Column(Name = "CODEPERIODICITE")]
        public string CodePeriodicite { get; set; }
        [DataMember]
        [Column(Name = "LIBPERIODICITE")]
        public string LibelllePeriodicite { get; set; }
        [DataMember]
        [Column(Name = "USERCREA")]
        public string UserCrea { get; set; }
        [DataMember]
        [Column(Name = "USERMAJ")]
        public string UserMaj { get; set; }
        [DataMember]
        public DateTime? DateDeSaisieDate { get; set; }

    }
}
