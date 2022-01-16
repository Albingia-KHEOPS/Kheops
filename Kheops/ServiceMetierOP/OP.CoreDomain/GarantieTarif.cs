using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    //[Serializable]
    public class GarantieTarif
    {
        public string GuidId { get; set; }
        public string Type { get; set; }
        public string CodeOffre { get; set; }
        public string Version { get; set; }
        public string CodeFormule { get; set; }
        public string CodeOption { get; set; }
        public string CodeGarantie { get; set; }
        public string GuidGarantie { get; set; }
        public int NumTarif { get; set; }

        public string LCIModifiable { get; set; }
        public string LCIObligatoire { get; set; }
        public string LCIValeurOrigine { get; set; }
        public string LCIValeurActualisee { get; set; }
        public string LCIValeurTravail { get; set; }
        public string LCIUnite { get; set; }
        public string LCIBase { get; set; }
        public string LCIExpressionComplexe { get; set; }

        public string FranchiseModifiable { get; set; }
        public string FranchiseObligatoire { get; set; }
        public string FranchiseValeurOrigine { get; set; }
        public string FranchiseValeurActualisee { get; set; }
        public string FranchiseValeurTravail { get; set; }
        public string FranchiseUnite { get; set; }
        public string FranchiseBase { get; set; }
        public string FranchiseExpressionComplexe { get; set; }

        public string FranchiseMiniValeurOrigine { get; set; }
        public string FranchiseMiniValeurActualisee { get; set; }
        public string FranchiseMiniValeurTravail { get; set; }
        public string FranchiseMiniUnite { get; set; }
        public string FranchiseMiniBase { get; set; }

        public string FranchiseMaxiValeurOrigine { get; set; }
        public string FranchiseMaxiValeurActualisee { get; set; }
        public string FranchiseMaxiValeurTravail { get; set; }
        public string FranchiseMaxiUnite { get; set; }
        public string FranchiseMaxiBase { get; set; }

        public string PrimeModifiable { get; set; }
        public string PrimeObligatoire { get; set; }
        public string PrimeValeurOrigine { get; set; }
        public string PrimeValeurActualisee { get; set; }
        public string PrimeValeurTravail { get; set; }
        public string PrimeUnite { get; set; }
        public string PrimeBase { get; set; }

        public string MontantBase { get; set; }
        public string PrimeProvisionnelle { get; set; }
    }
}
