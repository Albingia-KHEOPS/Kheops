using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    public class Risque
    {
        public int Code { get; set; }
        public String Descriptif { get; set; }
        public Cible Cible { get; set; }
        public String Designation { get; set; }
        public DateTime? EntreeGarantie { get; set; }
        public TimeSpan? HeureEntree { get; set; }
        public DateTime? SortieGarantie { get; set; }
        public TimeSpan? HeureSortie { get; set; }
        public int? Valeur { get; set; }
        public Parametre Unite { get; set; }
        public Parametre Type { get; set; }
        public string ValeurHT { get; set; }
        public int CodeObjet { get; set; }
        public bool RisqueIndexe { get; set; }
        public bool LCI { get; set; }
        public bool Franchise { get; set; }
        public bool Assiette { get; set; }
        public string RegimeTaxe { get; set; }
        public bool CATNAT { get; set; }
        public List<Objet> Objets { get; set; }
        public int ChronoDesi { get; set; }
        public string ReportValeur { get; set; }
        public string ReportObligatoire { get; set; }
        public int IdAdresseRisque { get; set; }
        public Adresse AdresseRisque { get; set; }
        public string CodeApe { get; set; }
        public string Nomenclature1 { get; set; }
        public string CodeTre { get; set; }
        public string CodeClasse { get; set; }
        public string Territorialite { get; set; }

        public string TypeRisque { get; set; }
        public string TypeMateriel { get; set; }
        public string NatureLieux { get; set; }
        public DateTime? DateEntreeDescr { get; set; }
        public DateTime? DateSortieDescr { get; set; }
    }
}
