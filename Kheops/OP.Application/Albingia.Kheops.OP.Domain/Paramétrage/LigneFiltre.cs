using Albingia.Kheops.OP.Domain.Referentiel;
using System;

namespace Albingia.Kheops.OP.Domain.Parametrage
{
    public class LigneFiltre
    {
        public bool IsInclusion { get; set; }
        public bool IsExclusion { get; set; }
        public string Cible { get; set; }
        public string Branche { get; set; }
        public object Ordre { get; set; }
        public long IdCible { get; set; }

        internal bool DoesApply(CibleFiltre cible)
        {
            return
               (String.IsNullOrWhiteSpace(Branche) || Branche == "*" || cible.CodeBranche == Branche)
               && (String.IsNullOrWhiteSpace(Cible) || Cible == "*" || cible.CodeCible == Cible || cible.CodeCible == IdCible.ToString());
        }
        internal bool Includes(CibleFiltre cible)
        {
            return IsInclusion && DoesApply(cible);
        }
        internal bool Excludes(CibleFiltre cible)
        {
            return IsInclusion && DoesApply(cible);
        }
    }
}