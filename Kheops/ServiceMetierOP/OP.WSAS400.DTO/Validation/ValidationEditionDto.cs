using OP.WSAS400.DTO.DocumentGestion;
using OP.WSAS400.DTO.Engagement;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Validation
{
    [DataContract]
    public class ValidationEditionDto
    {
        [DataMember]
        public List<DocumentGestionDocDto> ListeDocuments { get; set; }

        [DataMember]
        public List<EngagementTraiteDto> EngagementsTraites { get; set; }

        [Column(Name = "C100HT")]
        [DataMember]
        public double Cot100HTAvecCatNat { get; set; }

        [Column(Name = "C100CATNAT")]
        [DataMember]
        public double Cot100CatNat { get; set; }

        [Column(Name = "C100TTC")]
        [DataMember]
        public double Cot100TTC { get; set; }

        [Column(Name = "ALBHT")]
        [DataMember]
        public double CotAlbHT { get; set; }

        [Column(Name = "ALBCATNAT")]
        [DataMember]
        public double CotAlbCatNat { get; set; }

        [Column(Name = "ALBTTC")]
        [DataMember]
        public double CotAlbTTC { get; set; }

        [Column(Name = "TRACEEMISS")]
        [DataMember]
        public string TraceEmiss { get; set; }

        [Column(Name = "REGULEHT")]
        [DataMember]
        public double CotReguleHT { get; set; }

        [Column(Name = "REGULECATNAT")]
        [DataMember]
        public double CotReguleCatNat { get; set; }

        [Column(Name = "REGULETTC")]
        [DataMember]
        public double CotReguleTTC { get; set; }

        [Column(Name = "TRACEEMISSREGULE")]
        [DataMember]
        public string TraceEmissRegule { get; set; }


    }
}
