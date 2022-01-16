using Albingia.Kheops.OP.Domain.Parametrage;
using ALBINGIA.Framework.Common.Extensions;
using System;

namespace Albingia.Kheops.OP.Domain.Referentiel
{
    public static class RefValueExtendsion {
        public static bool IsSameCodeAs(this RefValue value, string code) {
            return (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(value?.Code))
                || code.Trim() == value?.Code?.Trim();
        }
    }
    public abstract class RefValue : IFiltered, IEquatable<RefValue>
    {
        public string Code { get; set; } = "";
        public string Libelle { get; set; } = "";
        public string LibelleLong { get; set; } = "";
        public string LibelleSuppose => LibelleLong.OrDefault(Libelle.OrDefault(Code ?? string.Empty));
        public string CodeFiltre { get; set; } = "";
        public Filtre Filtre { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is RefValue) || this.GetType() != obj.GetType()) { return false; }

            var val = (RefValue)obj;

            return  this.Equals(val);
        }

        public static bool operator ==(RefValue a, RefValue b) {
            return object.ReferenceEquals(a, b) || (a is null && b is null) || a.Equals(b) ;
        }
        public static bool operator !=(RefValue a, RefValue b)
        {
            return !(a == b );
        }
        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }

        public override string ToString()
        {
            return this.Libelle;
        }

        public bool Equals(RefValue val)
        {
            return this.GetType() == val?.GetType() &&  val?.Code == this.Code;
        }

        public void CopyTo<T>(T value) where T : RefValue {
            if (value == default(T)) {
                return;
            }
            value.Code = Code;
            value.CodeFiltre = CodeFiltre;
            if (Filtre != null) {
                value.Filtre = new Filtre() { Code = Filtre.Code, Description = Filtre.Description, Id = Filtre.Id, Lines = Filtre.Lines };
            }
            value.Libelle = Libelle;
            value.LibelleLong = LibelleLong;
        }
    }
}
