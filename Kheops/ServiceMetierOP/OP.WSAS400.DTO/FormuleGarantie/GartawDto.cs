using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.FormuleGarantie
{
    public class GartawDto
    {
        [Column(Name = "GUIDID")]
        public Int64 GuidId { get; set; }
        [Column(Name = "TYPE")]
        public String Type { get; set; }
        [Column(Name = "CODEOFFRE")]
        public String CodeOffre { get; set; }
        [Column(Name = "VERSION")]
        public Int32 Version { get; set; }
        [Column(Name = "CODEFORMULE")]
        public Int32 CodeFormule { get; set; }
        [Column(Name = "CODEOPTION")]
        public Int32 CodeOption { get; set; }
        [Column(Name = "CODEGARANTIE")]
        public String CodeGarantie { get; set; }
        [Column(Name = "GUIDGARANTIE")]
        public Int64 GuidGarantie { get; set; }
        [Column(Name = "NUMTARIF")]
        public Int32 NumTarif { get; set; }
        [Column(Name = "LCIMODIFIABLE")]
        public String LCImodifiable { get; set; }
        [Column(Name = "LCIOBLIGATOIRE")]
        public String LCIobligatoire { get; set; }
        [Column(Name = "LCIVALEURORIG")]
        public Double LCIValeurOrig { get; set; }
        [Column(Name = "LCIVALEURACTU")]
        public Double LCIValeurActu { get; set; }
        [Column(Name = "LCIVALEURTRAVAIL")]
        public Double LCIValeurTravail { get; set; }
        [Column(Name = "LCIUNITE")]
        public String LCIUnite { get; set; }
        [Column(Name = "LCIBASE")]
        public String LCIBase { get; set; }
        [Column(Name = "LCIEXPRCOMPLEXE")]
        public Int64 LCIExprComplexe { get; set; }
        [Column(Name = "FRANCHISEMODIFIABLE")]
        public String FranchiseModifiable { get; set; }
        [Column(Name = "FRANCHISEOBLIGATOIRE")]
        public String FranchiseObligatoire { get; set; }
        [Column(Name = "FRANCHISEVALEURORIG")]
        public Double FranchiseValeurOrig { get; set; }
        [Column(Name = "FRANCHISEVALEURACTU")]
        public Double FranchiseValeurActu { get; set; }
        [Column(Name = "FRANCHISEVALEURTRAVAIL")]
        public Double FranchiseValeurTravail { get; set; }
        [Column(Name = "FRANCHISEUNITE")]
        public String FranchiseUnite { get; set; }
        [Column(Name = "FRANCHISEBASE")]
        public String FranchiseBase { get; set; }
        [Column(Name = "FRANCHISEEXPRCOMPLEXE")]
        public Int64 FranchiseExprComplexe { get; set; }
        [Column(Name = "FRANCHISEMINIVALORIG")]
        public Double FranchiseMiniValOrig { get; set; }
        [Column(Name = "FRANCHISEMINIVALACTU")]
        public Double FranchiseMiniValActu { get; set; }
        [Column(Name = "FRANCHISEMINIVALTRAVAIL")]
        public Double FranchiseMiniValTravail { get; set; }
        [Column(Name = "FRANCHISEMINIUNITE")]
        public String FranchiseMiniUnite { get; set; }
        [Column(Name = "FRANCHISEMINIBASE")]
        public String FranchiseMiniBase { get; set; }
        [Column(Name = "FRANCHISEMAXIVALORIG")]
        public Double FranchiseMaxiValOrig { get; set; }
        [Column(Name = "FRANCHISEMAXIVALACTU")]
        public Double FranchiseMaxiValActu { get; set; }
        [Column(Name = "FRANCHISEMAXIVALTRAVAIL")]
        public Double FranchiseMaxiValTravail { get; set; }
        [Column(Name = "FRANCHISEMAXIUNITE")]
        public String FranchiseMaxUnite { get; set; }
        [Column(Name = "FRANCHISEMAXIBASE")]
        public String FranchiseMaxBase { get; set; }      
        [Column(Name = "PRIMEMODIFIABLE")]
        public String PrimeModifiable { get; set; }
        [Column(Name = "PRIMEOBLIGATOIRE")]
        public String PrimeObligatoire { get; set; }
        [Column(Name = "PRIMEVALORIG")]
        public Decimal PrimeValOrig { get; set; }
        [Column(Name = "PRIMEVALACTU")]
        public Decimal PrimeValActu { get; set; }
        [Column(Name = "PRIMEVALTRAVAIL")]
        public Decimal PrimeValTravail { get; set; }
        [Column(Name = "PRIMEUNITE")]
        public String PrimeUnite { get; set; }
        [Column(Name = "PRIMEBASE")]
        public String PrimeBase { get; set; }
        [Column(Name = "MONTANTBASE")]
        public Double MontantBase { get; set; }
        [Column(Name = "PRIMEPROVISIONNELLE")]
        public Double PrimeProvisionnelle { get; set; }
    }
}
