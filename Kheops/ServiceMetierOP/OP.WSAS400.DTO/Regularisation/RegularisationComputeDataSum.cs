using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class RegularisationComputeDataSum
    {
        [DataMember]
        [Column(Name = "SRIM")]
        public decimal RistourneAnticipee {get; set;}

        [Column(Name = "SIDF")]
        [DataMember]
        public decimal IndemnitesEtFrais { get; set; }

        [Column(Name = "SREC")]
        [DataMember]
        public decimal Recours { get; set; }

        [Column(Name = "SPRO")]
        [DataMember]
        public decimal Provisions { get; set; }

        [Column(Name = "SPRE")]
        [DataMember]
        public decimal Previsions { get; set; }

        [Column(Name = "EMD")]
        [DataMember]
        public decimal CotisationPeriode { get; set; }

        [Column(Name = "COT")]
        [DataMember]
        public decimal CotisationsRetenues { get; set; }

        [Column(Name = "SCHG")]
        [DataMember]
        public decimal ChargementSinistres { get; set; }

        [Column(Name = "MHC")]
        [DataMember]
        public decimal MontantCalcule { get; set; }

        [Column(Name = "RGID")]
        [DataMember]
        public long RgId { get; set; }

        [Column(Name = "RCH")]
        [DataMember]
        public decimal ReportCharges { get; set; }
    }
}
