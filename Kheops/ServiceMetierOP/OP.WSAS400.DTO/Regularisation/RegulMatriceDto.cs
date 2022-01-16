using System;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class RegulMatriceDto
    {
        [DataMember]
        [Column(Name = "RISQUEID")]
        public int RisqueId { get; set; }

        [DataMember]
        [Column(Name = "FORMULE")]
        public int Formule { get; set; }

        [DataMember]
        [Column(Name = "GARID")]
        public long GarId { get; set; }

        [DataMember]
        [Column(Name = "GARLIB")]
        public string GarLabel { get; set; }

        [DataMember]
        [Column(Name = "SITRSQ")]
        public string RisqueStatus { get; set; }
    }
}
