using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Condition
{
    public class ConditionGarantieLigneDto
    {
        [Column(Name = "NIVEAU")]
        public Int64 Niveau { get; set; }
        [Column(Name = "PERE")]
        public Int64 Pere { get; set; }
        [Column(Name = "ORIGINE")]
        public Int64 Origine { get; set; }
        [Column(Name = "IDLIGNE1")]
        public Int64 IdLigne1 { get; set; }
        [Column(Name = "IDLIGNE2")]
        public Int64 IdLigne2 { get; set; }
        [Column(Name = "IDSEQUENCE")]
        public Int64 IdSequence { get; set; }
        [Column(Name = "CODE")]
        public string Code { get; set; }
        [Column(Name = "DESCRIPTION")]
        public string Description { get; set; }
        [Column(Name = "COULEUR1")]
        public string Couleur1 { get; set; }
        [Column(Name = "COULEUR2")]
        public string Couleur2 { get; set; }
        [Column(Name = "LCIVALEUR")]
        public double LCIValeur { get; set; }
        [Column(Name = "LCIUNITE")]
        public string LCIUnite { get; set; }
        [Column(Name = "LCITYPE")]
        public string LCIType { get; set; }
        [Column(Name = "LIENLCI")]
        public Int64 LienLCI { get; set; }
        [Column(Name = "CODECOMPLEXELCI")]
        public Int64 CodeComplexeLCI { get; set; }
        [Column(Name = "LCILECTURESEULE")]
        public string LCILectureSeule { get; set; }
        [Column(Name = "LCIOBLIGATOIRE")]
        public string LCIObligatoire { get; set; }
        [Column(Name = "FRANCHISEVALEUR")]
        public double FranchiseValeur { get; set; }
        [Column(Name = "FRANCHISEUNITE")]
        public string FranchiseUnite { get; set; }
        [Column(Name = "FRANCHISETYPE")]
        public string FranchiseType { get; set; }
        [Column(Name = "LEINFRH")]
        public Int64 LienFRH { get; set; }
        [Column(Name = "CODECOMPLEXEFRH")]
        public Int64 CodeComplexeFRH { get; set; }
        [Column(Name = "FRANCHISELECTURESEULE")]
        public string FranchiseLectureSeule { get; set; }
        [Column(Name = "FRANCHISEOBLIGATOIRE")]
        public string FranchiseObligatoire { get; set; }
        [Column(Name = "ASSIETTEVALEUR")]
        public double AssietteValeur { get; set; }
        [Column(Name = "ASSIETTEUNITE")]
        public string AssietteUnite { get; set; }
        [Column(Name = "ASSIETTETYPE")]
        public string AssietteType { get; set; }
        [Column(Name = "ASSIETTELECTURESEULE")]
        public string AssietteLectureSeule { get; set; }
        [Column(Name = "ASSIETTEOBLIGATOIRE")]
        public string AssietteObligatoire { get; set; }
        [Column(Name = "NUMTARIF")]
        public Int64 NumTarif { get; set; }
        [Column(Name = "PRIMEVALEUR")]
        public double PrimeValeur { get; set; }
        [Column(Name = "PRIMEUNITE")]
        public string PrimeUnite { get; set; }
        [Column(Name = "PRIMEMINI")]
        public double PrimeMini { get; set; }
        [Column(Name = "PRIMELECTURESEULE")]
        public string PrimeLectureSeule { get; set; }
        [Column(Name = "PRIMEOBLIGATOIRE")]
        public string PrimeObligatoire { get; set; }
        [Column(Name = "CODEBLOC")]
        public Int64 CodeBloc { get; set; }
    }
}
