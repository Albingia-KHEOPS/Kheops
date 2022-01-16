using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Engagement
{
    public class EngagementPlatDto
    {
        [Column(Name = "LCIVALEUR")]
        public double LciValeur { get; set; }
        [Column(Name = "LCIUNITE")]
        public string LciUnite { get; set; }
        [Column(Name = "LCIBASE")]
        public string LciBase { get; set; }
        [Column(Name = "LCIINDEXEE")]
        public string LciIndexee { get; set; }
        [Column(Name = "NATURECONTRAT")]
        public string NatureContrat { get; set; }
        [Column(Name = "NATURECONTRATLIB")]
        public string NatureContratLib { get; set; }
        [Column(Name = "PARTALBINGIA")]
        public float PartAlbingia { get; set; }
        [Column(Name = "COUVERTURE")]
        public Int64 Couverture { get; set; }
        [Column(Name = "FAMTRAITE")]
        public string FamilleTraite { get; set; }
        [Column(Name = "LIBTRAITE")]
        public string LibTraite { get; set; }
        
        public long EngagementTotal { get; set; }
        public long EngagmentAlbingia { get; set; }
        public long SMPTotal { get; set; }

        [Column(Name = "CATNATTOTAL")]
        public double CATNATTotal { get; set; }
        [Column(Name = "CATNATALBINGIA")]
        public double CATNATAlbingia { get; set; }
        [Column(Name = "MONTANTFORCE")]
        public string MontantForce { get; set; }
        [Column(Name = "COMMENTFORCE")]
        public string CommentForce { get; set; }
        [Column(Name = "LIENCOMPLEXE")]
        public Int64 LienComplexeLCI { get; set; }
        [Column(Name = "LIBCOMPLEXE")]
        public string LibComplexeLCI { get; set; }
        [Column(Name = "CODECOMPLEXE")]
        public string CodeComplexeLCI { get; set; }
        [Column(Name = "DATEDEB")]
        public Int64 DateDeb { get; set; }
        [Column(Name = "DATEFIN")]
        public Int64 DateFin { get; set; }
    }
}
