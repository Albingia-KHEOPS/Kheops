using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Parametres;
using System.Data.Linq.Mapping;
using OP.WSAS400.DTO.Offres;

namespace OP.WSAS400.DTO.Traite
{
    [DataContract]
    public class TraiteDto : _Traite_Base
    {
        [DataMember]
        public bool LCIGenerale { get; set; }

        [DataMember]
        [Column(Name = "LCIVALEUR")]
        public Double LCIValeur { get; set; }

        [DataMember]
        [Column(Name = "LCIUNITE")]
        public string LCIUnite { get; set; }

        [DataMember]
        public List<ParametreDto> LCIUnites { get; set; }

        [DataMember]
        [Column(Name = "LCITYPE")]
        public string LCIType { get; set; }

        [DataMember]
        public List<ParametreDto> LCITypes { get; set; }

        [Column(Name = "LCIINDEXEE")]
        public string SLCIIndexee { get; set; }
        [DataMember]
        public bool LCIIndexee { get; set; }

        [DataMember]
        [Column(Name = "NOMTRAITE")]
        public string NomTraite { get; set; }

        [Column(Name = "EFFETDEB")]
        public Int32 SDebutEffet { get; set; }
        [DataMember]
        public DateTime? DDebutEffet { get; set; }

        [Column(Name = "EFFETFIN")]
        public Int32 SFinEffet { get; set; }
        [DataMember]
        public DateTime? DFinEffet { get; set; }

        [DataMember]
        public TraiteInfoRsqVenDto TraiteInfoRsqVen { get; set; }

        [DataMember]
        [Column(Name = "ENGAGEMENTTOTAL")]
        public Double EngagementTotal { get; set; }

        [DataMember]
        [Column(Name = "PARTALB")]
        public Double PartAlb { get; set; }

        [DataMember]
        [Column(Name = "ENGAGEMENTALB")]
        public Double EngagementAlbingia { get; set; }

        [DataMember]
        [Column(Name = "FAMREASSU")]
        public string FamilleReassurance { get; set; }

        [DataMember]
        [Column(Name = "COMMENTFORCE")]
        public string CommentForce { get; set; }

        [DataMember]
        [Column(Name = "LIENCOMPLEXE")]
        public Int64 LienComplexeLCI { get; set; }

        [DataMember]
        [Column(Name = "LIBCOMPLEXE")]
        public string LibComplexeLCI { get; set; }

        [DataMember]
        [Column(Name = "CODECOMPLEXE")]
        public string CodeComplexeLCI { get; set; }

        [DataMember]
        public OffreDto Offre { get; set; }
       
        [DataMember]
        public int IdVentilation { get; set; }

        [DataMember]
        public int TotalSMP { get; set; }
  
        public TraiteDto()
        {
            this.LCIUnites = new List<ParametreDto>();
            this.LCITypes = new List<ParametreDto>();
            this.TraiteInfoRsqVen = new TraiteInfoRsqVenDto();
           
        }
    }
}
