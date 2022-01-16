using ALBINGIA.Framework.Common.Business;
using System;

namespace ALBINGIA.Framework.Common {
    [Serializable]
    public class BlacklistAlert {
        public IPartner Partner { get; set; }
        public Blacklisting Status { get; set; }
        public string AlertMessage => Status == Blacklisting.NotSuspicious ? string.Empty 
            : (AlbEnumInfoValue.GetEnumInfo(Partner.Type) + " : Entité suspecte en cours de vérification par la conformité Albingia. Veuillez contacter la Direction Juridique.");
    }
}
