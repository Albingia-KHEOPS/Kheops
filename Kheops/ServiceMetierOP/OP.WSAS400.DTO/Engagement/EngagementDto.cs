using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OP.WSAS400.DTO.Offres.Parametres;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres;

namespace OP.WSAS400.DTO.Engagement
{
    [DataContract]
    public class EngagementDto : _Engagement_Base
    {
        [DataMember]
        public bool LCIGenerale { get; set; }
        [DataMember]
        public string LCIValeur { get; set; }
        [DataMember]
        public string LCIUnite { get; set; }
        [DataMember]
        public List<ParametreDto> LCIUnites { get; set; }
        [DataMember]
        public string LCIType { get; set; }
        [DataMember]
        public List<ParametreDto> LCITypes { get; set; }
        [DataMember]
        public bool LCIIndexee { get; set; }

        [DataMember]
        public string Nature { get; set; }
        [DataMember]
        public string PartAlb { get; set; }
        [DataMember]
        public string Couverture { get; set; }

        [DataMember]
        public List<EngagementTraiteDto> Traites { get; set; }

        [DataMember]
        public string BaseTotale { get; set; }
        [DataMember]
        public string BaseAlb { get; set; }
        [DataMember]
        public bool MontantForce { get; set; }
        [DataMember]
        public string CommentForce { get; set; }

        [DataMember]
        public string LienComplexeLCI{ get; set; }
        [DataMember]
        public string LibComplexeLCI { get; set; }
        [DataMember]
        public string CodeComplexeLCI { get; set; }

        [DataMember]
        public DateTime? DateDeb { get; set; }
        [DataMember]
        public DateTime? DateFin { get; set; }

        public EngagementDto()
        {
            this.LCIGenerale = false;
            this.LCIValeur = string.Empty;
            this.LCIUnite = string.Empty;
            this.LCIUnites = new List<ParametreDto>();
            this.LCIType = string.Empty;
            this.LCITypes = new List<ParametreDto>();
            this.LCIIndexee = false;

            this.Nature = string.Empty;
            this.PartAlb = string.Empty;
            this.Couverture = string.Empty;

            this.Traites = new List<EngagementTraiteDto>();

            this.BaseTotale = string.Empty;
            this.BaseAlb = string.Empty;
        }
    }
}
