using ALBINGIA.Framework.Common.Extensions;

namespace Albingia.Kheops.OP.Domain.Referentiel {
    public class Branche : RefValue {
        public static readonly Branche RisquesSpeciaux = new Branche { Code = "RS", Libelle = "Risques spéciaux", LibelleLong = string.Join(" ", nameof(RisquesSpeciaux).CutKamelCase()).ToUpper() };
        public static readonly Branche IndividuelleAccident = new Branche { Code = "IA", Libelle = "Ind. accident", LibelleLong = string.Join(" ", nameof(IndividuelleAccident).CutKamelCase()).ToUpper() };
        public static readonly Branche ResponcabiliteCivile = new Branche { Code = "RC", Libelle = "RC", LibelleLong = string.Join(" ", nameof(ResponcabiliteCivile).CutKamelCase()).ToUpper() };
        public static readonly Branche ArtEtPrecieux = new Branche { Code = "AP", Libelle = "Art et Précieux", LibelleLong = string.Join(" ", nameof(ArtEtPrecieux).CutKamelCase()).ToUpper() };
        public static readonly Branche Multirisques = new Branche { Code = "MR", Libelle = "Multirisques", LibelleLong = string.Join(" ", nameof(Multirisques).CutKamelCase()).ToUpper() };
        public static readonly Branche Incendie = new Branche() { Code = "IN", Libelle = nameof(Incendie), LibelleLong = nameof(Incendie) };
    }
}