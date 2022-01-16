using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.OP.OP_MVC.Models.ModelesGestUtilisateurs;
using OP.WSAS400.DTO.GestUtilisateurs;
using OPServiceContract.ICommon;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ALBINGIA.Framework.Common.Constants;
using System.Collections;

namespace ALBINGIA.OP.OP_MVC.Common {
    public static class CacheUserRights {
        /// <summary>
        /// Tous les User Rights existant dans la BD
        /// </summary>
        private static readonly List<ModeleUtilisateurBranche> _allUserRights = new List<ModeleUtilisateurBranche>();
        public static IEnumerable<ModeleUtilisateurBranche> AllUserRights {
            get {
                if (!_allUserRights.Any()) {
                    SetUserRights();
                }
                return _allUserRights;
            }

        }
        public static List<ModeleUtilisateurBranche> UserRights {
            get {
                if (!_allUserRights.Any(elem => elem.Utilisateur == AlbSessionHelper.ConnectedUser)) {
                    SetUserRights();
                }
                return _allUserRights.Where(elem => elem.Utilisateur == AlbSessionHelper.ConnectedUser).ToList();
            }

        }
        public static bool IsUserAdmin {
            get {
                if (!_allUserRights.Any(elem => elem.Utilisateur == AlbSessionHelper.ConnectedUser)) {
                    SetUserRights();
                }
                return _allUserRights.Any(elem => elem.Utilisateur == AlbSessionHelper.ConnectedUser && (elem.TypeDroit == TypeDroit.A.ToString() || elem.TypeDroit == TypeDroit.M.ToString()));
            }
        }

        public static void InitCacheUserRights() {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>()) {
                var serviceContext = client.Channel;
                serviceContext.GetUserRights();
            }
            _allUserRights.Clear();
        }

        private static List<UtilisateurBrancheDto> SetUserRightsDto() {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>()) {
                var serviceContext = client.Channel;
                return serviceContext.GetUserRights();
            }
        }
        
        public static void SetUserRights() {
            lock (((ICollection)_allUserRights).SyncRoot) {
                if (!_allUserRights.Any(elem => elem.Utilisateur == AlbSessionHelper.ConnectedUser))
                {
                    var allUserRightsDto = SetUserRightsDto();
                    _allUserRights.Clear();
                    _allUserRights.AddRange(allUserRightsDto.Select(u => (ModeleUtilisateurBranche)u));
                }
            }
        }
    }
}