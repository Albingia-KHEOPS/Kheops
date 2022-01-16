using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;

namespace Albingia.Kheops.DTO {
    public class LigneModeleISDto {
        private string libelle;
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string ParentCode { get; set; }
        public ProprieteISDto Propriete { get; set; }
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
        public int TypePresentation { get; set; }

        public bool IsReadonly { get; set; }
        public bool IsMandatory { get; set; }
        public bool HasScriptAffichage { get; set; }
        public bool HasScriptControle { get; set; }
        public Dictionary<string, string> ListeValeurs { get; set; }
        public Dictionary<string, string> ListeUnites { get; set; }
    }
}