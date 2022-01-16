using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    //[Serializable]
    public class LigneGarantie
    {
        public string Code { get; set; }
        public int? Id { get; set; }
        public string NumeroTarif { get; set; }

        public string LCIValeur { get; set; }
        public string LCIUnite { get; set; }
        public List<Parametre> LCIUnites { get; set; }
        public string LCIType { get; set; }
        public List<Parametre> LCITypes { get; set; }
        public string LCILectureSeule { get; set; }
        public string LCIObligatoire { get; set; }
        public string LienLCIComplexe { get; set; }

        public string FranchiseValeur { get; set; }
        public string FranchiseUnite { get; set; }
        public List<Parametre> FranchiseUnites { get; set; }
        public string FranchiseType { get; set; }
        public List<Parametre> FranchiseTypes { get; set; }
        public string FranchiseLectureSeule { get; set; }
        public string FranchiseObligatoire { get; set; }
        public string FranchiseMini { get; set; }
        public string FranchiseMaxi { get; set; }
        public DateTime? FranchiseDebut { get; set; }
        public DateTime? FranchiseFin { get; set; }
        public string LienFRHComplexe { get; set; }

        public string AssietteValeur { get; set; }
        public string AssietteUnite { get; set; }
        public List<Parametre> AssietteUnites { get; set; }
        public string AssietteType { get; set; }
        public List<Parametre> AssietteTypes { get; set; }
        public string AssietteLectureSeule { get; set; }
        public string AssietteObligatoire { get; set; }

        public string TauxForfaitHTValeur { get; set; }
        public string TauxForfaitHTUnite { get; set; }
        public List<Parametre> TauxForfaitHTUnites { get; set; }
        public string TauxForfaitHTMinimum { get; set; }
        public string TauxForfaitHTLectureSeule { get; set; }
        public string TauxForfaitHTObligatoire { get; set; }


        public string ConcurrenceValeur { get; set; }
        public string ConcurrenceUnite { get; set; }
        public List<Parametre> ConcurrenceUnites { get; set; }
        public string ConcurrenceType { get; set; }
        public List<Parametre> ConcurrenceTypes { get; set; }

        public string UniteLCINew { get; set; }
        public List<Parametre> UnitesLCINew { get; set; }
        public string TypeLCINew { get; set; }
        public List<Parametre> TypesLCINew { get; set; }
        public string UniteFranchiseNew { get; set; }
        public List<Parametre> UnitesFranchiseNew { get; set; }
        public string TypeFranchiseNew { get; set; }
        public List<Parametre> TypesFranchiseNew { get; set; }
        public string UniteConcurrence { get; set; }
        public List<Parametre> UnitesConcurrence { get; set; }
        public string TypeConcurrence { get; set; }
        public List<Parametre> TypesConcurrence { get; set; }

        public string Niveau { get; set; }

        public LigneGarantie()
        {
            LCIUnites = new List<Parametre>();
            LCITypes = new List<Parametre>();
            FranchiseUnites = new List<Parametre>();
            FranchiseTypes = new List<Parametre>();
            AssietteUnites = new List<Parametre>();
            AssietteTypes = new List<Parametre>();
            TauxForfaitHTUnites = new List<Parametre>();
            ConcurrenceUnites = new List<Parametre>();
            ConcurrenceTypes = new List<Parametre>();
            UnitesLCINew = new List<Parametre>();
            TypesLCINew = new List<Parametre>();
            UnitesFranchiseNew = new List<Parametre>();
            TypesFranchiseNew = new List<Parametre>();
            UnitesConcurrence = new List<Parametre>();
            TypesConcurrence = new List<Parametre>();
        }
    }
}
