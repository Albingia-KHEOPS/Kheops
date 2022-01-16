using System;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class PeriodeReguleDto
    {
        [Column(Name = "DATEDEBINT")]
        public int? DateDebInt  { get; set; }
        [Column(Name = "DATEFININT")]
        public int? DateFinInt { get; set; }

        [DataMember]
        public DateTime? DateDeb { get; set; }
        [DataMember]
        public DateTime? DateFin { get; set; }


    }
}
