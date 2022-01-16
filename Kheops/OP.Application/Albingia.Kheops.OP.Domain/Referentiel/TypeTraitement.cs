using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.Extensions;

namespace Albingia.Kheops.OP.Domain.Referentiel {
    public class TypeTraitement : RefParamValue {
        public TraitementAffaire Type => Code.ParseCode<TraitementAffaire>();
    }
}
