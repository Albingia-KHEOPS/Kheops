using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.FormuleGarantie
{
    [DataContract]
    public class GarantieDetailInfoDto
    {
        [DataMember]
        [Column(Name="CODEGAR")]
        public string CodeGarantie { get; set; }
        [DataMember]
        [Column(Name = "LIBGAR")]
        public string LibGarantie { get; set; }
        [DataMember]
        [Column(Name="TYPECONTROLE")]
        public string CodeTypeControle { get; set; }
        [DataMember]
        [Column(Name = "LIBTYPECONTROLE")]
        public string LibTypeControle { get; set; }
        [DataMember]
        [Column(Name = "GROUPEALT")]
        public int GroupeAlternatif { get; set; }
        [DataMember]
        [Column(Name = "TYPEAPPLI")]
        public string TypeApplication { get; set; }
        [DataMember]
        [Column(Name="FRHVAL")]
        public Double FranchiseValeur { get; set; }
        [DataMember]
        [Column(Name = "FRHUNIT")]
        public string FranchiseUnite { get; set; }
        [DataMember]
        [Column(Name = "FRHBASE")]
        public string FranchiseType { get; set; }
        [DataMember]
        [Column(Name = "FRHMODI")]
        public string FranchiseMAJ { get; set; }
        [DataMember]
        [Column(Name = "FRHOBLI")]
        public string FranchiseOblig { get; set; }

        [DataMember]
        [Column(Name = "LCIVAL")]
        public Double LCIValeur { get; set; }
        [DataMember]
        [Column(Name = "LCIUNIT")]
        public string LCIUnite { get; set; }
        [DataMember]
        [Column(Name = "LCIBASE")]
        public string LCIType { get; set; }
        [DataMember]
        [Column(Name = "LCIMODI")]
        public string LCIMAJ { get; set; }
        [DataMember]
        [Column(Name = "LCIOBLI")]
        public string LCIOblig { get; set; }

        [DataMember]
        [Column(Name = "ASSVAL")]
        public Double AssietteValeur { get; set; }
        [DataMember]
        [Column(Name = "ASSUNIT")]
        public string AssietteUnite { get; set; }
        [DataMember]
        [Column(Name = "ASSBASE")]
        public string AssietteType { get; set; }
        [DataMember]
        [Column(Name = "ASSMODI")]
        public string AssietteMAJ { get; set; }
        [DataMember]
        [Column(Name = "ASSOBLI")]
        public string AssietteOblig { get; set; }

        [DataMember]
        [Column(Name = "PRIVAL")]
        public Double PrimeValeur { get; set; }
        [DataMember]
        [Column(Name = "PRIUNIT")]
        public string PrimeUnite { get; set; }
        [DataMember]
        [Column(Name = "PRIBASE")]
        public string PrimeType { get; set; }
        [DataMember]
        [Column(Name = "PRIMODI")]
        public string PrimeMAJ { get; set; }
        [DataMember]
        [Column(Name = "PRIOBLI")]
        public string PrimeOblig { get; set; }
    }
}
