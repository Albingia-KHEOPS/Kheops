using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Adresses
{
    [DataContract]
    public class AdressePlatDto
    {
        [DataMember]
        [Column(Name = "NUMEROCHRONO")]
        public Int32 NumeroChrono { get; set; }
        [DataMember]
        [Column(Name ="MATHEX")]
        public string MatriculeHexavia { get; set; }
        [DataMember]
        [Column(Name = "BATIMENT")]
        public string Batiment { get; set; }
        [DataMember]
        [Column(Name = "NUMEROVOIE")]
        public Int32 NumeroVoie { get; set; } = 0;
        [DataMember]
        [Column(Name = "NUMEROVOIE2")]
        public string NumeroVoie2 { get; set; }
        [DataMember]
        [Column(Name = "EXTENSIONVOIE")]
        public string ExtensionVoie { get; set; }
        [DataMember]
        [Column(Name = "NOMVOIE")]
        public string NomVoie { get; set; }
        [DataMember]
        [Column(Name = "BOITEPOSTALE")]
        public string BoitePostale { get; set; }
        [DataMember]
        public string CodeInsee { get; set; }
        [DataMember]
        [Column(Name = "DEPARTEMENT")]
        public string Departement { get; set; }
        [DataMember]
        [Column(Name = "NOMVILLE")]
        public string NomVille { get; set; }
        [DataMember]
        [Column(Name = "CODEPOSTAL")]
        public Int32 CodePostal { get; set; } = 0;
        [DataMember]
        [Column(Name = "NOMCEDEX")]
        public string NomCedex { get; set; }
        [DataMember]
        [Column(Name = "CODEPOSTALCEDEX")]
        public Int32 CodePostalCedex { get; set; } = 0;
        [DataMember]
        [Column(Name = "TYPECEDEX")]
        public string TypeCedex { get; set; }
        [DataMember]
        [Column(Name = "CODEPAYS")]
        public string CodePays { get; set; }
        [DataMember]
        [Column(Name = "NOMPAYS")]
        public string NomPays { get; set; }

        [DataMember]
        public string CodePostalString { get; set; }

        [DataMember]
        public decimal? Latitude { get; set; }

        [DataMember]
        public decimal? Longitude { get; set; }
    }
}