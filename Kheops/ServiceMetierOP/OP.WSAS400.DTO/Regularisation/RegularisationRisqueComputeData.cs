using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public  class RegularisationRisqueComputeData
    {
        [Column(Name = "KILPBT")]
        [DataMember]
        public int TauxAppel { get; set; }

        [Column(Name = "KILPBA")]
        [DataMember]
        public int NbAnnees { get; set; }

        [Column(Name = "KILPBR")]
        [DataMember]
        public int Ristourne { get; set; }

        [Column(Name = "KILRIA")]
        [DataMember]
        public int RistourneAnticipee { get; set; }

        [Column(Name = "KILPBS")]
        [DataMember]
        public int PrcSeuilSP { get; set; }

        [Column(Name = "KILPBP")]
        [DataMember]
        public int PrcCotisationRetenue { get; set; }

        [Column(Name = "KILEMH")]
        [DataMember]
        public decimal CotisationPeriode { get; set; }

        [Column(Name = "KILCOT")]
        [DataMember]
        public decimal CotisationsRetenues { get; set; }

        [Column(Name = "KILSCHG")]
        [DataMember]
        public decimal ChargementSinistres { get; set; }
    }
}
