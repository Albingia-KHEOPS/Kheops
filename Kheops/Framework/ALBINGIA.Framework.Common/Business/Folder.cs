using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace ALBINGIA.Framework.Common {
    [DataContract]
    public class Folder :ICloneable {
        public Folder() {
            CodeOffre = string.Empty;
            NumeroAvenant = 0;
            Type = string.Empty;
            Version = 0;
            OriginalArray = null;
        }

        public Folder(string[] data) : this() {
            if (data?.Any() != true) { return; }
            OriginalArray = new string[data.Length];
            data.CopyTo(OriginalArray, 0);
            CodeOffre = data[0];
            Version = data.Length > 1 ? (int.TryParse(data[1], out int v) ? v : default(int)) : default(int);
            Type = data.Length < 3 || data[2].IsEmptyOrNull() ? string.Empty : data[2].Substring(0, 1);
        }

        public Folder(string codeAffaire, int version, char type, int numeroAvenant) : this(codeAffaire,version,type)
        {
            var paramErrors = new HashSet<string>();
            if (numeroAvenant < 0)
            {
                paramErrors.Add($"{nameof(version)} is invalid");
            }
            if (paramErrors.Any())
            {
                throw new ArgumentException(string.Join(Environment.NewLine, paramErrors));
            }
            this.NumeroAvenant = numeroAvenant;
        }
        public Folder(string codeAffaire, int version, char type) {
            var paramErrors = new HashSet<string>();
            if (!Regex.IsMatch(codeAffaire, @"^\s*[A-Z0-9]+$") || codeAffaire.Length > 9) {
                paramErrors.Add($"{nameof(codeAffaire)} is invalid");
            }
            if (version < 0) {
                paramErrors.Add($"{nameof(version)} is invalid");
            }
            if (!type.IsIn('O', 'P')) {
                paramErrors.Add($"{nameof(type)} is invalid");
            }
            if (paramErrors.Any()) {
                throw new ArgumentException(string.Join(Environment.NewLine, paramErrors));
            }
            CodeOffre = codeAffaire;
            Type = type.ToString();
            Version = version;
        }

        [DataMember]
        public virtual string CodeOffre { get; set; }

        [DataMember]
        public virtual int Version { get; set; }

        [DataMember]
        public virtual string Type { get; set; }

        [DataMember]
        public virtual int NumeroAvenant { get; set; }

        public virtual string Identifier {
            get {
                var s = new List<string>() {
                    CodeOffre,
                    Version.ToString(),
                    Type
                };
                return string.Join("_", s.Where(val => val.ContainsChars()));
            }
        }

        public override bool Equals(object obj) {
            return obj is Folder f
                && f.CodeOffre == CodeOffre
                && f.Version == Version
                && f.Type == Type
                && f.NumeroAvenant == NumeroAvenant;
        }

        public override int GetHashCode() {
            unchecked {
                return (((
                    17 + (CodeOffre ?? "").GetHashCode()
                    * 23) + Version.GetHashCode()
                    * 23) + (Type ?? "").GetHashCode()
                    * 23) + NumeroAvenant.GetHashCode();
            }
        }

        public virtual string BuildId(string separator = "_") {
            var s = new List<string>() {
                    CodeOffre,
                    Version.ToString(),
                    Type
                };
            return string.Join(separator, s);
        }

        public object Clone() {
            return this.MemberwiseClone();
        }

        public virtual string FullIdentifier {
            get {
                var id = Identifier;
                if (id.IsEmptyOrNull()) {
                    return string.Empty;
                }
                return id + '_' + NumeroAvenant.ToString();
            }
        }

        public string[] OriginalArray { get; }
    }
}