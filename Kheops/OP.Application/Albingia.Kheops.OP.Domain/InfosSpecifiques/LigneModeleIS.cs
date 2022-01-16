using Albingia.Kheops.OP.Domain.Referentiel;
using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Albingia.Kheops.OP.Domain.InfosSpecifiques {
    public class LigneModeleIS {
        private string libelle;
        public ProprieteIS Propriete { get; set; }
        public int Id { get; set; }
        public int ParentId { get; set; }
        public LigneModeleIS Parent { get; set; }
        public string EvenementParent { get; set; }
        public string ComportementSiParent { get; set; }
        public string Code { get; set; }
        public string Libelle {
            get => this.libelle.IsEmptyOrNull() ? (Propriete?.Libelle ?? string.Empty) : this.libelle;
            set => this.libelle = value;
        }
        public int Ordre { get; set; }
        public string SQLSelectParametres { get; set; }
        public int NombreSautsLignes { get; set; }
        public string CodeConcpept { get; set; }
        public string CodeFamille { get; set; }
        public int TypePresentation { get; set; }

        public bool IsReadonly { get; set; }
        public bool IsMandatory { get; set; }
        public bool HasScriptAffichage { get; set; }
        public bool HasScriptControle { get; set; }
        public string CodeConditions { get; set; }
        public IDictionary<string, string> ListeValeurs { get; set; }
        public List<(string code, string libelle)> ListeUnites { get; set; }
        public bool HasUnite => ListeUnites.AnyNotNull();
    }
}