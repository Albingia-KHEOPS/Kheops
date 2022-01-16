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
    public class GarantiePeriodeDto
    {
        [DataMember]
        [Column(Name = "IDGARANTIE")]
        public Int64 IdGarantie { get; set; }

        [DataMember]
        public DateTime? DateDebut { get; set; }

        [DataMember]
        public DateTime? DateFin { get; set; }

        [DataMember]
        public bool IsCreateAvn { get; set; }

        [DataMember]
        public bool IsUpdateAvn { get; set; }

        [Column(Name = "DATEDEBINT")]
        public decimal? DateDebutInt { get; set; }

        [Column(Name = "DATEFININT")]
        public decimal? DateFinInt { get; set; }

        [Column(Name = "ISCREATEAVN")]
        public Int16 CreateAvn { get; set; }

        [Column(Name = "ISUPDATEAVN")]
        public Int16 UpdateAvn { get; set; }
    }
}
