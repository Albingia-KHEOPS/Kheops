using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Albingia.Kheops.OP.DataAdapter {
    public class UserRepository : IUserRepository {
        private readonly KUsrDrtRepository kUsrDrtRepository;
        private readonly UtilisateurRepository utilisateurRepository;
        public UserRepository(KUsrDrtRepository kUsrDrtRepository, UtilisateurRepository utilisateurRepository) {
            this.kUsrDrtRepository = kUsrDrtRepository;
            this.utilisateurRepository = utilisateurRepository;
        }
        public ProfileKheops GetProfile(string userId) {
            if (userId is null) {
                throw new ArgumentNullException(nameof(userId));
            }
            if (!ProfileKheops.UserIdRegex.IsMatch(userId)) {
                throw new ArgumentException(nameof(userId));
            }
            var user = this.utilisateurRepository.Get(userId);
            var utilisateur = new UtilisateurKheops {
                Id = userId,
                Nom = user.Utnom,
                Prenom = user.Utpnm,
                Famille = user.Utfut.IsEmptyOrNull() ? FamilleUtilisateur.Interne : (Enum.TryParse(user.Utfut.Trim().Substring(1), true, out FamilleUtilisateur f) ? f : 0)
            };
            var fonctions = (user.Utgep == Booleen.Oui.AsCode() ? FonctionContrat.GestionnaireProduction : 0)
                | (user.Utges == Booleen.Oui.AsCode() ? FonctionContrat.GestionnaireSinistre : 0)
                | (user.Utsou == Booleen.Oui.AsCode() ? FonctionContrat.Souscripteur : 0);
            var droits = this.kUsrDrtRepository.GetAccesBranches(userId);

            if (droits.Any(x => x.Khrbra == "**")) {
                var allbr = droits.First(x => x.Khrbra == "**");
                return new ProfileKheops() {
                    Branches = new[] { (allbr.Khrtyd[0], allbr.Khrbra) },
                    Utilisateur = utilisateur,
                    FonctionsContrat = fonctions
                };
            }
            else {
                return new ProfileKheops(userId) {
                    Branches = droits.Select(x => (x.Khrtyd[0], x.Khrbra)).ToArray(),
                    Utilisateur = utilisateur,
                    FonctionsContrat = fonctions
                };
            }
        }
    }
}
