using System;
using System.Collections.Generic;

namespace OP.CoreDomain
{
    ////[Serializable]
    public class FormuleGarantieDetails
    {
        public string CodeGarantie { get; set; }
        public string Garantie { get; set; }
        public string LibelleGarantie { get; set; }

        public string Caractere { get; set; }
        public string NatureStd { get; set; }
        public string Nature { get; set; }
        public List<Parametre> Natures { get; set; }
        public DateTime? DateDebStd { get; set; }
        public TimeSpan? HeureDebStd { get; set; }
        public DateTime? DateFinStd { get; set; }
        public TimeSpan? HeureFinStd { get; set; }
        public DateTime? DateDeb { get; set; }
        public TimeSpan? HeureDeb { get; set; }
        public DateTime? DateFin { get; set; }
        public TimeSpan? HeureFin { get; set; }
        public string Duree { get; set; }
        public string DureeUnite { get; set; }
        public List<Parametre> DureeUnites { get; set; }
        public bool GarantieIndexe { get; set; }
        public bool LCI { get; set; }
        public bool Franchise { get; set; }
        public bool Assiette { get; set; }
        public bool CATNAT { get; set; }
        public bool InclusMontant { get; set; }
        public string Application { get; set; }
        public List<Parametre> Applications { get; set; }
        public string TypeEmission { get; set; }
        public List<Parametre> TypesEmission { get; set; }
        public string CodeTaxe { get; set; }
        public List<Parametre> CodesTaxe { get; set; }
        public string DefGarantie { get; set; }
        public string AlimAssiette { get; set; }
        public List<Parametre> AlimAssiettes { get; set; }

        public string GarantieIndexeW { get; set; }
        public string LCIW { get; set; }
        public string FranchiseW { get; set; }
        public string AssietteW { get; set; }
        public string CATNATW { get; set; }
        
        public string TypeControleDate { get; set; }

    }
}
