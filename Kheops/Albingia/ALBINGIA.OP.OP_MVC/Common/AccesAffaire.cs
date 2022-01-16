using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albingia.Mvc.Common {
    public class AccesAffaire : IEquatable<AccesAffaire> {
        public string Code { get; set; }
        public int Version { get; set; }
        public int? Avenant { get; set; }
        /// <summary>
        /// Gets the type of Affaire. Optional in context, code and version suffice
        /// </summary>
        public string Type { get; set; }
        public AccesOrigine ModeAcces { get; set; }
        public Guid TabGuid { get; set; }
        public bool VerrouillageEffectue { get; private set; }
        public AccesAffaire() { }
        public AccesAffaire(AccesOrigine modeAcces) {
            VerrouillageEffectue = false;
            Code = string.Empty;
            Version = 0;
            Avenant = null;
            ModeAcces = modeAcces;
            TabGuid = Guid.Empty;
        }
        public void Valider() {
            VerrouillageEffectue = true;
        }

        public override bool Equals(object obj) {
            return obj is AccesAffaire a ? Equals(a) : base.Equals(obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (((((
                    17 + (Code ?? string.Empty).GetHashCode()
                    * 23) + Version.GetHashCode()
                    * 23) + TabGuid.GetHashCode()
                    * 23) + ModeAcces.GetHashCode()));
            }
        }

        public bool Equals(AccesAffaire other) {
            return other?.Code == Code
                && other?.Version == Version
                && other?.TabGuid == TabGuid
                && other?.ModeAcces == ModeAcces;
        }
    }
}