using Albingia.Kheops.OP.Domain.InfosSpecifiques;
using System.Collections.Generic;

namespace Albingia.Kheops.DTO {
    public class ProprieteISDto {
        public string Code { get; set; }
        public string Libelle { get; set; }
        public string Description { get; set; }
        public string Observations { get; set; }
        public string Type { get; set; }
        public string TypeConvertion { get; set; }
        public int TypePresentation { get; set; }
        public TypeAffichage TypeUIControl { get; set; }
        public int Longueur { get; set; }
        public int NbDecimales { get; set; }

        public bool IsMapped { get; set; }
        public bool IsMandatory { get; set; }
        public bool HasScriptAffichage { get; set; }
        public bool HasScriptControle { get; set; }
        public Dictionary<string, string> ListeUnites { get; set; }
        public bool HasUnite { get; set; }
    }
}