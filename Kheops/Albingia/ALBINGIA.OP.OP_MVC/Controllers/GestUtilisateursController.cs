using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesGestUtilisateurs;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO.GestUtilisateurs;
using OPServiceContract.IAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using OP.DataAccess;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class GestUtilisateursController : ControllersBase<ModeleGestUtilisateursPage>
    {
        //
        // GET: /GestUtilisateurs/
        [AlbApplyUserRole]
        public ActionResult Index(string utilisateur = "",
                                    string branche = "",
                                    string typeDroit = "",
                                    string albcontext = "Generale",
                                    string rechercheAlbcontext = "Recherche",
                                    bool rechercheActive = true,
                                    bool addUserEnabled = true,
                                    bool reloadPageBack = true,
                                    bool isReadOnlyWidget = false)
        {

            model.PageTitle = "Gestion Utilisateurs";

            model.Albcontext = albcontext;
            model.RechercheAlbcontext = rechercheAlbcontext;

            model.RechercheActive = rechercheActive;
            model.AddUserEnabled = addUserEnabled;

            model.ReloadPageBack = reloadPageBack;
            model.IsReadOnly = isReadOnlyWidget;

            if (!string.IsNullOrEmpty(utilisateur) ||
                    !string.IsNullOrEmpty(branche) ||
                    !string.IsNullOrEmpty(typeDroit))
            {


                //model.Albcontext = "RechercheFloat";
                model.RechercheUtilisateur = utilisateur;
                model.RechercheBranche = branche;
                model.RechercheTypeDroit = typeDroit;
            }


            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;

            LoadInfoPage();

            LoadUtilisateurBrancheDtos(utilisateur, branche, typeDroit, albcontext);


            if (albcontext != "Generale")
            {
                model.PageTitle = "Recherche Gestion Utilisateurs";
            }
            else
            {
                model.IsReadOnly = false;
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("GestUtlisateurBody", model);
            }

            return View(model);
        }

        /// <summary>
        /// charger les information de la page(les branches)
        /// </summary>
        protected override void LoadInfoPage(string context = null)
        {
            //preparer la liste des branches disponibles
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var branches = serviceContext.BranchesGet().ToList();
                model.Branches = branches.Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Nom, Selected = false }).ToList();

                // ajouter la super branche  **
                model.Branches.Insert(0, new AlbSelectListItem { Value = "**", Text = "**" + " - " + "**", Selected = false });

                model.TypeDroits = AlbEnumInfoValue.GetListEnumInfo<ALBINGIA.Framework.Common.Constants.TypeDroit>();
            }

        }

        private void LoadUtilisateurBrancheDtos(string utilisateur = "", string branche = "", string typeDroit = "", string albcontext = "Generale")
        {
            var userCache = ALBINGIA.OP.OP_MVC.Common.CacheUserRights.AllUserRights;
            var listMenu = ALBINGIA.OP.OP_MVC.Common.CacheUsersLogin.AllUsersLogin;

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var backoffice=client.Channel;
                List<UtilisateurBrancheDto> lstMappageUtiliDroitBranche;

                if (albcontext == "Generale")
                {
                    lstMappageUtiliDroitBranche = backoffice.LoadUtilisateurBrancheDtos();
                }
                else
                {
                    lstMappageUtiliDroitBranche = backoffice.GetUtilisateurBrancheDtos(utilisateur, branche, typeDroit);
                }

                model.ModeleMappageUtiliDroitBranches = new List<ModeleUtilisateurBranche>();

                foreach (UtilisateurBrancheDto dto in lstMappageUtiliDroitBranche)
                {
                    ModeleUtilisateurBranche userModel = (ModeleUtilisateurBranche)dto;

                    if (userCache != null && userCache.Any())
                    {
                        userModel.ExistInCache = userCache.Any(el => el.Utilisateur == userModel.Utilisateur && el.Branche == userModel.Branche && el.TypeDroit == userModel.TypeDroit)
                                && listMenu.Exists(el => el.UserLogin == userModel.Utilisateur);

                    }

                    userModel.TypeDroits = GetTypeDroitsSelectListByBranche(userModel.Branche);
                    SetSelectedItem(userModel.TypeDroits, userModel.TypeDroit);

                    userModel.TypeDroitLabel = GetSelectedItemLabel(userModel.TypeDroits, userModel.TypeDroit);

                    userModel.BrancheLabel = GetSelectedItemLabel(model.Branches, userModel.Branche);

                    model.ModeleMappageUtiliDroitBranches.Add(userModel);
                }

                //ligne pour nouveau UtilisateurBranche
                model.NewUtilisateurBranche = new ModeleUtilisateurBranche();
                model.NewUtilisateurBranche.TypeDroits = AlbEnumInfoValue.GetListEnumInfo<ALBINGIA.Framework.Common.Constants.TypeDroit>();
            }
        }


        private void LoadUtilisateurBrancheDto(string utilisateur, string branche, string albContext = "")
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var backoffice=client.Channel;
                UtilisateurBrancheDto dto = backoffice.GetUtilisateurBrancheDto(utilisateur, branche);
                ModeleUtilisateurBranche userModel = (ModeleUtilisateurBranche)dto;

                //userModel.TypeDroits = AlbEnumInfoValue.GetListEnumInfo<ALBINGIA.Framework.Common.Constants.TypeDroit>();
                userModel.TypeDroits = GetTypeDroitsSelectListByBranche(userModel.Branche);

                SetSelectedItem(userModel.TypeDroits, userModel.TypeDroit);
                userModel.TypeDroitLabel = GetSelectedItemLabel(userModel.TypeDroits, userModel.TypeDroit);
                userModel.BrancheLabel = GetSelectedItemLabel(model.Branches, userModel.Branche);
                userModel.Albcontext = albContext;
                model.ModeleMappageUtiliDroitBranches = new List<ModeleUtilisateurBranche> { userModel };
                model.Albcontext = albContext;
            }
        }

        [ErrorHandler]
        public ActionResult AddUtilisateurBranche(string data)
        {
            JavaScriptSerializer serialiser = AlbJsExtendConverter<ModeleUtilisateurBranche>.GetSerializer();
            ModeleUtilisateurBranche utilisateurBranche = serialiser.ConvertToType<ModeleUtilisateurBranche>(serialiser.DeserializeObject(data));

            if (utilisateurBranche.Validate())
            {
                if (
                    UtilisateurWithSuperBrancheExist(utilisateurBranche.Utilisateur)
                    ||
                    (utilisateurBranche.Branche == ALBINGIA.Framework.Common.Constants.AlbOpConstants.SUPER_BRANCHE
                        && UtlisateurExist(utilisateurBranche.Utilisateur))
                    )
                {
                    throw new AlbFoncException(string.Format("l'utilisateur {0} est déja existant",
                        utilisateurBranche.Utilisateur), false, false, true);
                }
                else
                {
                    try
                    {
                        using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                        {
                            var backoffice=client.Channel;
                            backoffice.AjouterUtilisateurBranche(utilisateurBranche.ToUtilisateurBrancheDto());
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new AlbFoncException(ex.Message, false, false, true);
                    }

                    LoadInfoPage();
                    LoadUtilisateurBrancheDto(utilisateurBranche.Utilisateur, utilisateurBranche.Branche, utilisateurBranche.Albcontext);

                    return PartialView("UtiliDroitBranches", model);
                }
            }
            else
            {
                throw new AlbFoncException("Votre saisie est incorrecte", false, false, true);
            }
        }

        [ErrorHandler]
        public ActionResult RemoveUtilisateurBranche(string data)
        {
            JavaScriptSerializer serialiser = AlbJsExtendConverter<ModeleUtilisateurBranche>.GetSerializer();
            ModeleUtilisateurBranche utilisateurBranche = serialiser.ConvertToType<ModeleUtilisateurBranche>(serialiser.DeserializeObject(data));

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var backoffice=client.Channel;
                backoffice.SupprimerUtilisateurBranche(utilisateurBranche.ToUtilisateurBrancheDto());
            }

            return Json(utilisateurBranche);
        }

        [ErrorHandler]
        public ActionResult UpdateUtilisateurBranche(string data)
        {
            JavaScriptSerializer serialiser = AlbJsExtendConverter<ModeleUtilisateurBranche>.GetSerializer();
            ModeleUtilisateurBranche utilisateurBranche = serialiser.ConvertToType<ModeleUtilisateurBranche>(serialiser.DeserializeObject(data));

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var backoffice=client.Channel;
                UtilisateurBrancheDto oldUser = backoffice.GetUtilisateurBrancheDto(utilisateurBranche.Utilisateur,
                    utilisateurBranche.Branche);
                backoffice.ModifierUtilisateurBranche(oldUser, utilisateurBranche.ToUtilisateurBrancheDto().TypeDroit);
            }

            return Json(utilisateurBranche);
        }

        /// <summary>
        /// slectionne une option selon compareValue
        /// </summary>
        /// <param name="lookUp"></param>
        /// <param name="compareValue"></param>
        /// <param name="selectedItem"></param>
        private void SetSelectedItem(List<AlbSelectListItem> lookUp, string compareValue, AlbSelectListItem selectedItem = null)
        {
            if (!string.IsNullOrEmpty(compareValue) && lookUp != null)
            {
                selectedItem = lookUp.FirstOrDefault(elm => elm.Value == compareValue.Trim());
                if (selectedItem != null)
                    selectedItem.Selected = true;
            }
        }

        /// <summary>
        /// retourne le text d'un AlbSelectListItem
        /// </summary>
        /// <param name="lookUp"></param>
        /// <param name="compareValue"></param>
        /// <param name="selectedItem"></param>
        /// <returns></returns>
        private string GetSelectedItemLabel(List<AlbSelectListItem> lookUp, string compareValue, AlbSelectListItem selectedItem = null)
        {
            if (!string.IsNullOrEmpty(compareValue) && lookUp != null)
            {
                selectedItem = lookUp.FirstOrDefault(elm => elm.Value == compareValue.Trim());
                if (selectedItem != null)
                    return selectedItem.Text;
            }

            return string.Empty;
        }


        [ErrorHandler]
        public ActionResult GetTypeDroitsByBranche(string branche, string albContext)
        {
            ModeleUtilisateurBranche user = new ModeleUtilisateurBranche();
            user.TypeDroits = GetTypeDroitsSelectListByBranche(branche);
            user.Albcontext = albContext;

            return PartialView("TypeDroitsDropDownList", user);
        }
        /// <summary>
        /// verifier les droits d'un utilisateur
        /// </summary>
        /// <param name="branche"></param>
        /// <returns></returns>
        [ErrorHandler]
        public ActionResult VerifDroitsByUser(string id)
        {
            var result = CommonRepository.SverifDroitsByUser(id);

            if (result != null && result.Count > 0) {
                return Json(new { success = true, message = "" });
            }
            else
                return Json(new { success = false, message = "" });

        }

        /// <summary>
        /// retourne une liste des options selon la branche
        /// </summary>
        /// <param name="branche"></param>
        /// <returns></returns>
        private List<AlbSelectListItem> GetTypeDroitsSelectListByBranche(string branche)
        {
            if (branche.Equals(ALBINGIA.Framework.Common.Constants.AlbOpConstants.SUPER_BRANCHE))
            {
                return new List<AlbSelectListItem>{
                    AlbEnumInfoValue.GetSelectListItemEnumInfo<ALBINGIA.Framework.Common.Constants
                    .TypeDroit>(ALBINGIA.Framework.Common.Constants.TypeDroit.M),

                 AlbEnumInfoValue.GetSelectListItemEnumInfo<ALBINGIA.Framework.Common.Constants
                    .TypeDroit>(ALBINGIA.Framework.Common.Constants.TypeDroit.A),

                    AlbEnumInfoValue.GetSelectListItemEnumInfo<ALBINGIA.Framework.Common.Constants
                    .TypeDroit>(ALBINGIA.Framework.Common.Constants.TypeDroit.G)
                 };
            }
            else
            {
                return new List<AlbSelectListItem>{
                    AlbEnumInfoValue.GetSelectListItemEnumInfo<ALBINGIA.Framework.Common.Constants
                    .TypeDroit>(ALBINGIA.Framework.Common.Constants.TypeDroit.G),

                 AlbEnumInfoValue.GetSelectListItemEnumInfo<ALBINGIA.Framework.Common.Constants
                    .TypeDroit>(ALBINGIA.Framework.Common.Constants.TypeDroit.V)
                 };
            }
        }
        /// <summary>
        /// verifier si un utlisateur exist en base avec branche ='**'
        /// </summary>
        /// <param name="utilisateur"></param>
        /// <returns></returns>
        private bool UtilisateurWithSuperBrancheExist(string utilisateur)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var backoffice=client.Channel;
                UtilisateurBrancheDto user = backoffice.GetUtilisateurBrancheDto(utilisateur,
                    ALBINGIA.Framework.Common.Constants.AlbOpConstants.SUPER_BRANCHE);

                return user != null;
            }
        }

        private bool UtlisateurExist(string utilisateur)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var backoffice=client.Channel;
                List<UtilisateurBrancheDto> userDtos = backoffice.GetUtilisateurBranchesByCriteria(KUSRDRT_COl.Utilisateur,
                    utilisateur, WHERE_OPER.EQ);

                return userDtos != null && userDtos.Count > 0;
            }
        }

        [ErrorHandler]
        public void ResetUserRight()
        {
            RefreshCacheUserRights();
        }

        private void RefreshCacheUserRights()
        {
            //ALBINGIA.OP.OP_MVC.Common.CacheIS.InitCacheIS();
            //ALBINGIA.OP.OP_MVC.Common.CacheIS.SetIsModelsEntete();
            //ALBINGIA.OP.OP_MVC.Common.CacheIS.SetIsModelsDto();
            //ALBINGIA.OP.OP_MVC.Common.CacheIS.SetIsModels();

            ALBINGIA.OP.OP_MVC.Common.CacheUserRights.InitCacheUserRights();
            //ALBINGIA.OP.OP_MVC.Common.CacheUserRights.SetUserRightsDto();
            ALBINGIA.OP.OP_MVC.Common.CacheUserRights.SetUserRights();

        }

        [ErrorHandler]
        public void ResetUserLogin()
        {
            RefreshCacheUserLogin();
        }

        private void RefreshCacheUserLogin()
        {
            ALBINGIA.OP.OP_MVC.Common.CacheUsersLogin.InitCacheUsersLogin();
            ALBINGIA.OP.OP_MVC.Common.CacheUsersLogin.SetUsersLoginDto();
            ALBINGIA.OP.OP_MVC.Common.CacheUsersLogin.SetUserLogin();
        }

        [ErrorHandler]
        public void ResetYUTILIS()
        {
            MvcApplication._contextMenuUsers = null;
        }

    }
}
