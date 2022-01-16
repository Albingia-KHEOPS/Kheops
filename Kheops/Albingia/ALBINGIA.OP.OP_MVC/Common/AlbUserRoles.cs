using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models;
using OPServiceContract.ISaisieCreationOffre;
using System.Collections.Generic;
using System.Linq;
namespace ALBINGIA.OP.OP_MVC.Common
{
    public class AlbUserRoles
    {
        #region Méthodes publiques
        public static bool GetValiditeOffre(Folder folder)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                return client.Channel.OffreEstValide(folder.CodeOffre, folder.Version.ToString(), folder.Type, folder.NumeroAvenant.ToString());
            }
        }

        /// <summary>
        /// Met à jour le profil utilisateur
        /// </summary>
        /// <param name="tabGuid">Guid session</param>
        /// <param name="userName">Nom Utilisateur connecté</param>
        /// <param name="offre">numéro offre/contrat</param>
        /// <param name="version">version</param>
        /// <param name="type">type O/P</param>
        /// <param name="lockUser">Utilisateur vérrouillant l'offre</param>
        /// <param name="firstScreen">Premier ecran</param>
        /// <param name="isValideFolder">Si l'offre ou le contrat est valide</param>
        /// <param name="readOnlyFolder">Si On est en lecture seule</param>
        /// <param name="ignoreReadOnly">Forcer à Ignorer le readOnly </param>
        /// <param name="modeConsultationEcran">Définit si le mode d'affichage de l'écran est en consultation ou en modification</param>
        /// <param name="forceReadOnly">Forcer le readOnly</param>

        public static AlbUserProfileTask SetUserProfil(string tabGuid, string userName, string offre, string version, string type,
            string lockUser, bool firstScreen, bool isValideFolder, bool readOnlyFolder, bool forceReadOnly = false, bool ignoreReadOnly = false, bool modeConsultationEcran = false, string numAvn = "0", bool modifHorsAvenant = false, string currentFlow = null, FlowAccessMode flowInitAction = default)
        {
            var key = $"{tabGuid}_{userName}_{offre}_{version}_{type}_{numAvn}";
            var lockKey = new FolderKey(offre, int.Parse(version), type, int.Parse(numAvn), userName, tabGuid);

            if (AlbSessionHelper.CurrentFolders == null)
            {
                AlbSessionHelper.CurrentFolders = new Dictionary<FolderKey, AlbProjectInfo> {
                    {
                        lockKey,
                        MakeNewUserInfo(tabGuid, userName, offre, version, type,numAvn,lockUser,firstScreen,isValideFolder, readOnlyFolder,modeConsultationEcran , modifHorsAvenant)
                    }
                };
            }
            else
            {
                AlbSessionHelper.CurrentFolders.TryGetValue(lockKey, out var folderInCache);
                if (folderInCache != null)
                {
                    if (readOnlyFolder || forceReadOnly)
                    {
                        folderInCache.ReadOnlyFolder = true;
                        return AlbUserProfileTask.ReadOnly;
                    }
                    if (ignoreReadOnly)
                    {
                        folderInCache.ReadOnlyFolder = false;
                        return AlbUserProfileTask.None;
                    }
                    if (modeConsultationEcran)
                    {
                        folderInCache.ReadOnlyFolder = true;
                        folderInCache.ModeConsultationEcran = modeConsultationEcran;
                    }
                    if (folderInCache.ModeConsultationEcran)
                    {
                        return AlbUserProfileTask.ReadOnly;
                    }
                    if (folderInCache.LockUser == userName && !folderInCache.ReadOnlyFolder)
                    {
                        return AlbUserProfileTask.None;
                    }
                }

                var userLockFolderElem = AlbSessionHelper.CurrentFolders.Where(x =>
                    x.Key.User == userName
                    && x.Key.CodeOffre == offre && x.Key.Version == int.Parse(version) && x.Key.Type == type && x.Key.NumeroAvenant == int.Parse(numAvn)
                    && !x.Value.ReadOnlyFolder && !x.Value.ReadOnlyUser);

                var tabGuidElem = AlbSessionHelper.CurrentFolders.Where(x => x.Key.TabGuid == tabGuid);

                if (userLockFolderElem.Any() && !tabGuidElem.Any())
                {
                    if (!userLockFolderElem.Any(x => x.Key.TabGuid.ContainsChars()))
                    {
                        userLockFolderElem.ToList().ForEach(x => AlbSessionHelper.CurrentFolders.Remove(x.Key));
                        AlbSessionHelper.CurrentFolders.Add(lockKey, MakeNewUserInfo(tabGuid, userName, offre, version, type, numAvn, lockUser, firstScreen, isValideFolder, readOnlyFolder, modeConsultationEcran, modifHorsAvenant));
                        return AlbUserProfileTask.None;
                    }
                    else
                    {
                        // ARA - 3213
                        // Si lecture seule et que c'est une modification, alors ignorer lecture seule
                        var isModifFolder = userLockFolderElem.Where(x =>
                               x.Value?.ModeConsultationEcran != true
                            && x.Value?.ReadOnlyFolder != true
                            && x.Value?.ReadOnlyUser != true
                        );
                        if (isModifFolder != null && isModifFolder.Any())
                        {
                            AlbSessionHelper.CurrentFolders.Add(lockKey, MakeNewUserInfo(tabGuid, userName, offre, version, type, numAvn, lockUser, firstScreen, isValideFolder, readOnlyFolder, modeConsultationEcran, modifHorsAvenant));
                            return AlbUserProfileTask.None;
                        }
                        else
                        {
                            AlbSessionHelper.CurrentFolders.Add(lockKey, MakeNewUserInfo(tabGuid, userName, offre, version, type, numAvn, lockUser, firstScreen, isValideFolder, true, true, modifHorsAvenant));
                            return AlbUserProfileTask.ReadOnly;
                        }
                    }
                }
                if (folderInCache == null)
                {
                    AlbSessionHelper.CurrentFolders.Add(lockKey, MakeNewUserInfo(tabGuid, userName, offre, version, type, numAvn, lockUser, firstScreen, isValideFolder, readOnlyFolder, modeConsultationEcran, modifHorsAvenant));
                }
            }
            return !AlbSessionHelper.CurrentFolders[lockKey].ReadOnlyFolder
                && !AlbSessionHelper.CurrentFolders[lockKey].ModeConsultationEcran
                ? AlbUserProfileTask.None : AlbUserProfileTask.ReadOnly;
        }
        /// <summary>
        /// L'offre ou le contrat est en lecture seule
        /// </summary>
        /// <param name="tabGuid">Guid de l'onglet du browser</param>
        /// <param name="userName">Utilisateur</param>
        /// <param name="offre">Code offre / Contrat</param>
        /// <param name="version">Version</param>
        /// <param name="type"></param>
        /// <param name="numAvn">Numéro Avenant</param>
        /// <returns></returns>
        public static bool IsReadOnlyFolder(string tabGuid, string userName, string offre, string version, string type, string numAvn)
        {
            if (AlbSessionHelper.CurrentFolders.TryGetValue(new FolderKey(offre, int.Parse(version), type, int.Parse(numAvn), userName, tabGuid), out var folder))
            {
                return (folder?.ReadOnlyFolder).GetValueOrDefault();
            }
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="guidTab"></param>
        /// <param name="userName"></param>
        /// <param name="offre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="numAvn"></param>
        /// <param name="lockUser"></param>
        /// <param name="firstScreen"></param>
        /// <param name="ifValideFolder"></param>
        /// <param name="readOnlyFolder"></param>
        /// <param name="modeConsultationEcran"></param>
        /// <param name="modifHorsAvenant"> </param>
        /// <returns></returns>
        public static AlbProjectInfo MakeNewUserInfo(string guidTab, string userName, string offre, string version, string type, string numAvn, string lockUser, bool firstScreen, bool ifValideFolder, bool readOnlyFolder, bool modeConsultationEcran = false, bool modifHorsAvenant = false, string currentFlow = null, FlowAccessMode flowInitAction = default)
        {
            var projInfo = new AlbProjectInfo
            {
                TabGuid = guidTab,
                Id = guidTab + "_" + userName + "_" + offre + "_" + version + "_" + type + "_" + numAvn,
                Folder = offre,
                UserName = userName,
                ReadOnlyFolder =
                  !string.IsNullOrEmpty(offre) && !string.IsNullOrEmpty(version) && GetFolderRole(offre, version, type, numAvn, userName, lockUser, ifValideFolder, readOnlyFolder),
                ReadOnlyUser = GetUserRole(userName),
                LockUser = lockUser,
                FirstScreen = firstScreen,
                ModeConsultationEcran = modeConsultationEcran,
                ModifHorsAvenant = modifHorsAvenant,
                CurrentFlow = currentFlow,
                FlowInitAction = (currentFlow != null ? flowInitAction : FlowAccessMode.ReadOnly).ToString()
            };
            return projInfo;
        }
        #endregion
        #region Méthode privées

        private static bool GetUserRole(string userName)
        {
            return false;
        }
        private static bool GetFolderRole(string offre, string version, string type, string numAvn, string userName, string lockUser, bool ifValideFolder, bool readOnlyFolder = false)
        {
            var offreValide = ifValideFolder;

            if (readOnlyFolder)
            {
                return true;
            }
            var offreVerouillee = userName != lockUser && AlbTransverse.IsOffreVerrrouille(offre, version, type, numAvn);

            return offreValide || offreVerouillee;
        }
        #endregion
    }
}
