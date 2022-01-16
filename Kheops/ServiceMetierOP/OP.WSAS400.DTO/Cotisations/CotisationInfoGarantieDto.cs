using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Cotisations
{
    [DataContract]
    public class CotisationInfoGarantieDto 
    {
        [DataMember]
        public List<CotisationGarantieDto> Garanties { get; set; }

        [DataMember]
        public string SubCotisationHT { get; set; }
        [DataMember]
        public string SubCotisationTaxe { get; set; }
        [DataMember]
        public string SubCotisationTTC { get; set; }

        [DataMember]
        public CotisationCanatGareatDto Catnat { get; set; }
        [DataMember]
        public CotisationCanatGareatDto Gareat { get; set; }

        [DataMember]
        public CotisationCommissionDto Commission { get; set; }

        [DataMember]
        public string CoefCom { get; set; }
        [DataMember]
        public decimal TotalHorsFraisHT { get; set; }
        [DataMember]
        public decimal TotalHorsFraisTaxe { get; set; }
        [DataMember]
        public decimal TotalHorsFraisTTC { get; set; }
        [DataMember]
        public decimal FraisHT { get; set; }
        [DataMember]
        public decimal FraisTaxe { get; set; }
        [DataMember]
        public decimal FGATaxe { get; set; }
        [DataMember]
        public string FGATTC { get; set; }
        [DataMember]
        public decimal TotalHT { get; set; }
        [DataMember]
        public string TotalTaxe { get; set; }
        [DataMember]
        public string TotalTTC { get; set; }
        [DataMember]
        public string TypePart { get; set; }
        [DataMember]
        public string TypePeriode { get; set; }
        [DataMember]
        public string NatureContrat { get; set; }

        public CotisationInfoGarantieDto()
        {
            this.Garanties = new List<CotisationGarantieDto>();
            this.Catnat = new CotisationCanatGareatDto();
            this.Gareat = new CotisationCanatGareatDto();
            this.Commission = new CotisationCommissionDto();
        }
    }
}
