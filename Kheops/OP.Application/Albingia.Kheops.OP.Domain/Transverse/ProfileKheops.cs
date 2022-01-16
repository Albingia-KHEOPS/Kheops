
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Albingia.Kheops.OP.Domain {
    public class ProfileKheops {
        public static readonly Regex UserIdRegex = new Regex(@"^[\w\-]{1,10}", RegexOptions.Singleline | RegexOptions.Compiled);

        public ProfileKheops() {
            ShowImpayesOnStartup = true;
            Branches = new (char, string)[0];
        }
        public ProfileKheops(string userId) : this() {
            Utilisateur = new UtilisateurKheops { Id = userId };
        }
        public UtilisateurKheops Utilisateur { get; set; }
        public bool ShowImpayesOnStartup { get; set; }
        public (char droit, string branche)[] Branches { get; set; }
        public FonctionContrat FonctionsContrat { get; set; }

        public void Update(ProfileKheops profile, IEnumerable<ProfileKheopsData> specificUpdate) {
            if (!specificUpdate?.Any() ?? true) {
                return;
            }
            foreach (var data in specificUpdate) {
                switch (data) {
                    case ProfileKheopsData.None:
                        break;
                    case ProfileKheopsData.ShowImpayesOnStartup:
                        ShowImpayesOnStartup = profile.ShowImpayesOnStartup;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
