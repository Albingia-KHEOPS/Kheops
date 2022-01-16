using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Condition
{
    public class EnsembleGarantiePlatDto
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
        public Double LciValeur { get; set; }
        [Column(Name = "LCIUNITE")]
        public string LciUnite { get; set; }
        [Column(Name = "LCITYPE")]
        public string LciType { get; set; }
        [Column(Name = "LIENLCI")]
        public Int64 LienLCI { get; set; }
        [Column(Name = "CODECOMPLEXELCI")]
        public string CodeComplexeLCI { get; set; }
        [Column(Name = "LIBLCICOMPLEXE")]
        public string LibLCIComplexe { get; set; }
        [Column(Name = "LCILECTURESEULE")]
        public string LciLectureSeule { get; set; }
        [Column(Name = "LCIOBLIGATOIRE")]
        public string LciObligatoire { get; set; }
        [Column(Name = "FRANCHISEVALEUR")]
        public Double FranchiseValeur { get; set; }
        [Column(Name = "FRANCHISEUNITE")]
        public string FranchiseUnite { get; set; }
        [Column(Name = "FRANCHISETYPE")]
        public string FranchiseType { get; set; }
        [Column(Name = "LIENFRH")]
        public Int64 LienFRH { get; set; }
        [Column(Name = "CODECOMPLEXEFRH")]
        public string CodeComplexeFRH { get; set; }
        [Column(Name = "LIBFRHCOMPLEXE")]
        public string LibFRHComplexe { get; set; }
        [Column(Name = "FRANCHISELECTURESEULE")]
        public string FranchiseLectureSeule { get; set; }
        [Column(Name = "FRANCHISEOBLIGATOIRE")]
        public string FranchiseObligatoire { get; set; }
        [Column(Name = "ASSIETTEVALEUR")]
        public Double AssietteValeur { get; set; }
        [Column(Name = "ASSIETTEUNITE")]
        public string AssietteUnite { get; set; }
        [Column(Name = "ASSIETTETYPE")]
        public string AssietteType { get; set; }
        [Column(Name = "ASSIETTELECTURESEULE")]
        public string AssietteLectureSeule { get; set; }
        [Column(Name = "ASSIETTEOBLIGATOIRE")]
        public string AssietteObligatoire { get; set; }
        [Column(Name="ASSIETTEALIMAUTO")]
        public string AssietteAlimAuto { get; set; }
        [Column(Name="REPORTINVEN")]
        public string ReportInven { get; set; }
        [Column(Name = "NUMEROTARIF")]
        public Int64 NumeroTarif { get; set; }

        public decimal PrimeValeurTemp { get; set; }
        public decimal PrimeValeur { get; set; }
        public decimal PrimeValeurOrigine { get; set; }

        [Column(Name = "PRIMEUNITE")]
        public string PrimeUnite { get; set; }
        [Column(Name = "PRIMEMINI")]
        public Double PrimeMini { get; set; }
        [Column(Name = "PRIMELECTURESEULE")]
        public string PrimeLectureSeule { get; set; }
        [Column(Name = "PRIMEOBLIGATOIRE")]
        public string PrimeObligatoire { get; set; }
        [Column(Name = "CODEBLOC")]
        public Int64 CodeBloc { get; set; }
        [Column(Name = "CVOLET")]
        public Int64 CVolet { get; set; }
        [Column(Name = "LVOLET")]
        public string LVolet { get; set; }
        [Column(Name = "CBLOC")]
        public Int64 CBloc { get; set; }
        [Column(Name = "LBLOC")]
        public string LBloc { get; set; }

        [Column(Name = "TRIVOLET")]
        public Single TriVolet { get; set; }
        [Column(Name = "TRIBLOC")]
        public Single TriBloc { get; set; }
        //[Column(Name = "TRIVOLET")]
        //public Int64 TriVolet { get; set; }
        //[Column(Name = "TRIBLOC")]
        //public Int64 TriBloc { get; set; }
        [Column(Name = "TRIDATE")]
        public Int64 TriDate { get; set; }
        [Column(Name = "TRIGAR")]
        public string TriGar { get; set; }

        [Column(Name="DATEDEB")]
        public string DateDeb { get; set; }
        [Column(Name = "DATEFIN")]
        public Int64 DateFin { get; set; }
        [Column(Name = "HEUREFIN")]
        public Int64 HeureFin { get; set; }
        [Column(Name = "DUREE")]
        public Int64 Duree { get; set; }
        [Column(Name="UNITDUREE")]
        public string UnitDuree { get; set; }
        [Column(Name = "ATTENTATGAREAT")]
        public string AttentatGareat { get; set; }
    }
}
