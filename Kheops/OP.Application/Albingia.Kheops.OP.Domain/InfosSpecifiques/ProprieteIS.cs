using Albingia.Kheops.OP.Domain.Referentiel;
using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Albingia.Kheops.OP.Domain.InfosSpecifiques {
    public class ProprieteIS {
        public const string CleQuestionMedical = "QuestionMedical";
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
        public List<(string code, string libelle)> ListeUnites { get; set; }
        public bool HasUnite => ListeUnites.AnyNotNull();
    }
}