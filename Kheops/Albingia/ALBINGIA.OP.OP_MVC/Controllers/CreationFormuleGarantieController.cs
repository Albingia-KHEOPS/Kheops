using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.DynamicGuiIS.Ajax;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModeleModeles;
using ALBINGIA.OP.OP_MVC.Models.ModelesBlocs;
using ALBINGIA.OP.OP_MVC.Models.ModelesFormuleGarantie;
using ALBINGIA.OP.OP_MVC.Models.ModelesGarantieModeles;
using ALBINGIA.OP.OP_MVC.Models.ModelesObjet;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesRisque;
using ALBINGIA.OP.OP_MVC.Models.ModelesVolets;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Clausier;
using OP.WSAS400.DTO.FormuleGarantie;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class CreationFormuleGarantieController : ControllersBase<ModeleFormuleGarantiePage>
    {
        #region Méthode Publique

        /// <summary>
        /// Indexes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        
        [ErrorHandler]
        public ActionResult Index(string id)
        {
            //var dDeb = ALBINGIA.Framework.Common.Tools.AlbLog.InitTracePerf("recherchesaisie");

            //Traitement du mode duplication
            model.ModeDuplicate = id.Contains("modeDuplication") && id.Split(new[] { "modeDuplication" }, StringSplitOptions.None)[1] == "1";
            id = id.Contains("modeDuplication") ? id.Split(new[] { "modeDuplication" }, StringSplitOptions.None)[0] : id;

            id = InitializeParams(id);

            var tId = id.Split('_');

            var keyFormules = GetKeyFormules(
                string.Format("{0}{6}{1}{6}{2}{6}{3}{6}{4}{6}{5}", tId[0], tId[1], tId[2], tId[3], tId[4], model.ModeNavig, MvcApplication.SPLIT_CONST_HTML)
            );

            DeleteFormuleFromCache(keyFormules);

            #region Commentaires
            //ECM : Rechargement de la formule en AVN s'il faut la mettre en readonly (à voir)
            //if (tId[2] == "P")
            //{
            //    using (var serviceContext = new RisquesGarantiesClient())
            //    {
            //        bool isTraceAvnExist = serviceContext.IsTraceAvnExist(tId[0], tId[1], tId[2], tId[3], tId[4]);
            //        if (isTraceAvnExist)//Une trace avenant existe-elle en base ?
            //            model.IsAvenantModificationLocale = true;
            //        if (model.IsAvnRefreshUserUpdate)//l'utilisateur vient-il de cocher la case?
            //            model.IsAvenantModificationLocale = true;
            //        if (tId[3] == "0")//cas de creation de formule
            //            model.IsAvenantModificationLocale = true;

            //        var typeAvt = GetAddParamValue(model.AddParamValue, AlbNameVarAddParam.AVNTYPE);
            //        bool isModeAvenant = typeAvt == AlbConstantesMetiers.TYPE_AVENANT_MODIF || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF;

            //        if (isModeAvenant && !model.IsAvenantModificationLocale && !model.IsForceReadOnly && !model.IsReadOnly)
            //        {
            //            return ReloadFormuleGarantie(tId[0], tId[1], tId[2], tId[3], tId[4]);
            //        }
            //    }
            //}
            #endregion

            LoadInfoPage(id);

            if (model.Contrat != null && model.IsModeAvenant && !model.IsAvenantModificationLocale && !model.IsForceReadOnly && !model.IsReadOnly)
                return RedirectToAction("Index", "CreationFormuleGarantie", new { id = model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type + "_" + model.CodeFormule + "_" + model.CodeOption + GetSurroundedTabGuid(model.TabGuid) + BuildAddParamString(model.AddParamType, model.AddParamValue + "||FORCEREADONLY|1") + GetFormatModeNavig(model.ModeNavig) });

            //ALBINGIA.Framework.Common.Tools.AlbLog.SetTracePerf(dDeb);
            return View(model);
        }

        /// <summary>
        /// Loads the formules garanties.
        /// </summary>
        /// <param name="codeOffre">The code offre.</param>
        /// <param name="version">The version.</param>
        /// <param name="type">The type.</param>
        /// <param name="codeAvn"></param>
        /// <param name="codeFormule">The code formule.</param>
        /// <param name="codeOption">The code option.</param>
        /// <param name="branche"> </param>
        /// <param name="codeCible">The code categorie.</param>
        /// <param name="formGen"></param>
        /// <param name="codeAlpha"> </param>
        /// <param name="libFormule"> </param>
        /// <param name="appliqueA"></param>
        /// <param name="offreReadOnly"></param>
        /// <param name="modeNavig"></param>
        /// <returns></returns>
        [ErrorHandler]
        public ActionResult LoadFormulesGaranties(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string formGen, string codeAlpha, string branche, string codeCible, string libFormule, int appliqueA, bool offreReadOnly, string modeNavig)
        {
            var keyFormules = GetKeyFormules(
                string.Format("{0}{6}{1}{6}{2}{6}{3}{6}{4}{6}{5}", codeOffre, version, type, codeFormule, codeOption, modeNavig, MvcApplication.SPLIT_CONST_HTML)
            );

            int iCodeFormule;
            int iCodeOption;
            int.TryParse(codeFormule, out iCodeFormule);
            int.TryParse(codeOption, out iCodeOption);
            var toReturn = GetFormuleFromCache(keyFormules, codeAvn, formGen, codeCible, codeAlpha, branche, libFormule, offreReadOnly, modeNavig, appliqueA: appliqueA);
            if (toReturn != null)
                toReturn.IsReadOnly = offreReadOnly;
            return PartialView("ListeFormuleGarantie", toReturn);
        }

        [ErrorHandler]
        public ActionResult RetrieveFormulesGaranties(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption,
            string formGen, string codeCible, bool fullScreen, string modeNavig, bool isReadOnly)
        {
            var keyFormules = GetKeyFormules(
                string.Format("{0}{6}{1}{6}{2}{6}{3}{6}{4}{6}{5}", codeOffre, version, type, codeFormule, codeOption, modeNavig, MvcApplication.SPLIT_CONST_HTML)
            );
            var toReturn = GetFormuleFromCache(keyFormules, codeAvn, formGen, codeCible, string.Empty, string.Empty, string.Empty, true, modeNavig, true) ??
                           new ModeleFormuleGarantie();

            toReturn.FullScreen = fullScreen;
            toReturn.IsReadOnly = isReadOnly;
            return PartialView("ListeFormuleGarantie", toReturn);
        }

        /// <summary>
        /// Saves the formule garantie.
        /// </summary>
        /// <param name="codeOffre">The code offre.</param>
        /// <param name="version">The version.</param>
        /// <param name="type">The type.</param>
        /// <param name="codeFormule">The code formule.</param>
        /// <param name="codeOption">The code option.</param>
        /// <param name="formGen"></param>
        /// <param name="codeAlpha">The code alpha.</param>
        /// <param name="branche">The branche.</param>
        /// <param name="codeCible">Code cible</param>
        /// <param name="libFormule">The lib formule.</param>
        /// <param name="codeObjetRisque">The code objet risque.</param>
        /// <param name="tabGuid"></param>
        /// <param name="modeNavig"></param>
        /// <param name="codeAvn"></param>
        /// <returns></returns>
        [ErrorHandler]
        public string SaveFormuleGarantie(string codeOffre, string version, string type, string codeFormule, string codeOption, string formGen, string codeAlpha, string branche, string codeCible, string libFormule, string codeObjetRisque, string tabGuid, string modeNavig, string codeAvn)
        {
            if (tabGuid == null) throw new ArgumentNullException("tabGuid");
            var toReturn = string.Empty;

            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn))
            {
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    var serviceContext = channelClient.Channel;
                    toReturn = serviceContext.EnregistrerFormuleGarantie(codeOffre, version, type, codeAvn, codeFormule, codeOption, formGen, codeAlpha,
                            branche, codeCible, libFormule, codeObjetRisque, GetUser());
                }

                var keyFormules = GetKeyFormules(
                     string.Format("{0}{6}{1}{6}{2}{6}{3}{6}{4}{6}{5}", codeOffre, version, type, codeFormule, codeOption, modeNavig, MvcApplication.SPLIT_CONST_HTML)
                );
                DeleteFormuleFromCache(keyFormules);
            }

            return toReturn;
        }

        /// <summary>
        /// Updates the option.
        /// </summary>
        /// <param name="guidOption">The GUID option.</param>
        /// <param name="isChecked">The is checked.</param>
        /// <returns></returns>
        [ErrorHandler]
        public string UpdateOption(string codeOffre, string version, string type, string codeGarantie, string codeAvn, string codeFormule, string codeOption, string formGen, string guidOption, string codeCible, string codeObjetRisque, string isChecked, string albNiv, string modeNavig, string dateModifAvt)
        {
            return SaveLineGarantie(codeOffre, version, type, string.Empty, codeAvn, codeFormule, codeOption, formGen, codeCible, codeObjetRisque, isChecked, albNiv, string.Empty, string.Empty, modeNavig, dateModifAvt);
        }

        /// <summary>
        /// Updates the garantie.
        /// </summary>
        /// <param name="guidGarantie">The GUID garantie.</param>
        /// <param name="isChecked">The is checked.</param>
        /// <param name="caractere">The caractere.</param>
        /// <param name="nature">The nature.</param>
        /// <returns></returns>
        [ErrorHandler]
        public string UpdateGarantie(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string formGen, string guidGarantie,
            string isChecked, string caractere, string nature, string codeCible, string codeObjetRisque, string albNiv, string paramNat, string codeGarantie, string codeInventaire, string actegestion, string modeNavig, string dateModifAvt)
        {
            if (!string.IsNullOrEmpty(codeGarantie) && !string.IsNullOrEmpty(codeInventaire) && codeInventaire != "0")
            {
                DeleteInventaire(codeOffre, version, type, codeFormule, guidGarantie, codeInventaire);
                UpdateCacheGarantie(codeOffre, version, type, codeAvn, codeFormule, codeOption, albNiv, modeNavig, 0);
            }
            return SaveLineGarantie(codeOffre, version, type, guidGarantie, codeAvn, codeFormule, codeOption, formGen, codeCible, codeObjetRisque, isChecked, albNiv, actegestion, paramNat, modeNavig, dateModifAvt);
        }

        /// <summary>
        /// Formules the garantie enregistrer.
        /// </summary>
        /// <param name="codeOffre">The code offre.</param>
        /// <param name="version">The version.</param>
        /// <param name="type">The type.</param>
        /// <param name="codeFormule">The code formule.</param>
        /// <param name="codeOption">The code option.</param>
        /// <param name="libelle">The libelle.</param>
        /// <param name="codeObjetRisque">The code objet risque.</param>
        /// <returns></returns>
        [ErrorHandler]
        public string FormuleGarantieEnregistrer(string codeOffre, string version, string type, string codeAvenant, string codeFormule, string codeOption,
            string formGen, string libelle, string codeObjetRisque, string codeCible, string tabGuid, string saveCancel, string modeNavig, DateTime? dateAvenant)
        {
            var dateAvtStr = dateAvenant != null ? AlbConvert.ConvertDateToInt(dateAvenant).ToString() : string.Empty;

            var strReturn = string.Empty;

            var keyFormules = GetKeyFormules(
                 string.Format("{0}{6}{1}{6}{2}{6}{3}{6}{4}{6}{5}", codeOffre, version, type, codeFormule, codeOption, modeNavig, MvcApplication.SPLIT_CONST_HTML)
            );
            //Sauvegarde uniquement si l'écran n'est pas en readonly
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvenant)
                && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>())
            {
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    var serviceContext = channelClient.Channel;
                    if (string.IsNullOrEmpty(formGen))
                        formGen = "0";
                    var formulesGaranties = GetFormuleFromCache(keyFormules, codeAvenant, formGen, codeCible, string.Empty, string.Empty, string.Empty, true, modeNavig, true);
                    if (formulesGaranties == null)
                        throw new AlbFoncException(string.Format("{0}-ERRORCACHE", User));
                    var formulesGarantiesSave = GetFormuleSaveFromCache(keyFormules);
                    var errorMsg = serviceContext.FormulesGarantiesSaveSet(codeOffre, version, type, codeAvenant, dateAvtStr, codeFormule, codeOption, formGen, libelle, codeObjetRisque, ModeleFormuleGarantieSave.LoadDto(formulesGarantiesSave), GetUser());

                    if (!string.IsNullOrEmpty(errorMsg.Trim()) && !errorMsg.Trim().Contains(";ERRORMSG;##"))
                        strReturn = errorMsg.Trim();
                }
            }
            if (string.IsNullOrEmpty(strReturn))
                DeleteFormuleFromCache(keyFormules);

            return strReturn;
        }

        [ErrorHandler]
        [HttpGet]
        public JsonResult GetDatesDebByGaranties(string idGaranties, string codeAvn)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var client = channelClient.Channel;
                var ids = idGaranties.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries).Select(i => Convert.ToInt32(i)).ToArray();

                var periodes = client.GetDateDebByGaranties(ids, Convert.ToInt32(codeAvn));

                return Json(periodes, JsonRequestBehavior.AllowGet);
            }
        }

        [ErrorHandler]
        public ActionResult LoadDetailsGarantie(string codeOffre, string version, string type, string codeFormule, string codeOption, string codeGarantie, string codeObjetRisque, string modeNavig, string codeAvn, DateTime? dateEffAvnModifLocale, bool isReadonly = false)
        {
            var modele = new ModeleDetailsGarantie();

            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext = channelClient.Channel;
                var keyFormules = GetKeyFormules(string.Format("{0}{6}{1}{6}{2}{6}{3}{6}{4}{6}{5}", codeOffre, version, type, codeFormule, codeOption, modeNavig, MvcApplication.SPLIT_CONST_HTML));
                var formulesGarantiesSave = GetFormuleSaveFromCache(keyFormules);
                var volets = ModeleFormuleGarantieSave.LoadDto(formulesGarantiesSave);

                var result = serviceContext.ObtenirInfosDetailsFormuleGarantie(codeOffre, version, type, codeAvn, codeFormule, codeOption, codeGarantie, codeObjetRisque, volets, dateEffAvnModifLocale, GetUser(), isReadonly, modeNavig.ParseCode<ModeConsultation>());

                if (result != null)
                {
                    modele = ((ModeleDetailsGarantie)result);
                    modele.CodeGarantie = codeGarantie;
                }

                modele.isReadOnly = isReadonly;
            }

            return PartialView("DetailsGarantie", modele);
        }

        [ErrorHandler]
        public string SaveDetailsGarantie(string argDetails, string codeOffre, string version, string type, string codeAvenant, string codeFormule, string codeOption, string albNiv, int codeInven, string codeObjetRisque, string modeNavig)
        {
            string toReturn;


            var serialiser = AlbJsExtendConverter<ModeleDetailsGarantie>.GetSerializer();
            var detailsGarantie = serialiser.ConvertToType<ModeleDetailsGarantie>(serialiser.DeserializeObject(argDetails));

            //Calcul de la date de fin par rapport à la durée
            if (!string.IsNullOrEmpty(detailsGarantie.Duree) && Convert.ToInt32(detailsGarantie.Duree) > 0)
            {
                detailsGarantie.DateFin = AlbConvert.GetFinPeriode(detailsGarantie.DateDeb ?? detailsGarantie.DateDebStd, Convert.ToInt32(detailsGarantie.Duree), detailsGarantie.DureeUnite);
                detailsGarantie.HeureFin = AlbConvert.ConvertIntToTime(235900);
            }

            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext = channelClient.Channel;
                var keyFormules = GetKeyFormules(
                string.Format("{0}{6}{1}{6}{2}{6}{3}{6}{4}{6}{5}", codeOffre, version, type, codeFormule, codeOption, modeNavig, MvcApplication.SPLIT_CONST_HTML));
                var formulesGarantiesSave = GetFormuleSaveFromCache(keyFormules);
                toReturn = serviceContext.SauverDetailsFormuleGarantie(codeOffre, version, type, codeFormule, codeOption, codeAvenant, codeObjetRisque, GetUser(), ModeleDetailsGarantie.LoadDto(detailsGarantie), ModeleFormuleGarantieSave.LoadDto(formulesGarantiesSave));
                if (!string.IsNullOrEmpty(toReturn) && toReturn != "O" && toReturn != "N")
                {
                    throw new AlbFoncException(toReturn);
                }
            }

            UpdateCacheGarantie(codeOffre, version, type, codeAvenant, codeFormule, codeOption, albNiv, modeNavig, codeInven, toReturn, string.Empty);

            return toReturn;
        }

        [ErrorHandler]
        public void SaveAppliqueA(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string cible, string formGen, string objetRisqueCode, string tabGuid, string modeNavig)
        {
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn))
            {
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    var serviceContext = channelClient.Channel;
                    serviceContext.SaveAppliqueA(codeOffre, version, type, codeFormule, codeOption, cible, formGen, objetRisqueCode, GetUser());
                    var keyFormules = GetKeyFormules(
                     string.Format("{0}{6}{1}{6}{2}{6}{3}{6}{4}{6}{5}", codeOffre, version, type, codeFormule, codeOption, modeNavig, MvcApplication.SPLIT_CONST_HTML)
                );
                    DeleteFormuleFromCache(keyFormules);
                }
            }
        }

        [ErrorHandler]
        public void DeleteFormuleGarantie(string codeOffre, string version, string type, string codeFormule, string codeOption, string modeNavig)
        {
            var keyFormules = GetKeyFormules(
                 string.Format("{0}{6}{1}{6}{2}{6}{3}{6}{4}{6}{5}", codeOffre, version, type, codeFormule, codeOption, modeNavig, MvcApplication.SPLIT_CONST_HTML)
            );
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext = channelClient.Channel;
                serviceContext.DeleteFormuleGarantie(codeOffre, version, type, codeFormule, "R");
            }
            DeleteFormuleFromCache(keyFormules);
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string codeFormule, string codeOption, string codeRisque, string codeObjet, string branche,
            string tabGuid, string modeNavig, string libelle, string lettreLib, string saveCancel, string paramRedirect, string addParamType, string addParamValue,
            string readonlyDisplay, bool isModeConsultationEcran, bool isForceReadOnly, bool sessionLost, bool isModeConsult)
        {
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            if (Convert.ToBoolean(readonlyDisplay)) {
                CheckFormule(codeOffre, version, type, codeFormule, codeOption);
                return RedirectToAction(job, cible, new
                {
                    id = codeOffre + "_" + version + "_" + type + "_" + codeFormule + "_" + codeOption + tabGuid +
                        BuildAddParamString(addParamType, addParamValue +
                        (isModeConsult ? string.Empty :
                        readonlyDisplay == "true" ? "||FORCEREADONLY|1" :
                        (sessionLost ? "||IGNOREREADONLY|1" : "||AVNREFRESHUSERUPDATE|1||IGNOREREADONLY|1"))) + GetFormatModeNavig(modeNavig) + (isModeConsult ? "ConsultOnly" : string.Empty)
                });
            }
            else if (!isModeConsultationEcran)
            {
                var folderId = $"{tabGuid.Replace("tabGuid","")}_{AlbSessionHelper.ConnectedUser}_{codeOffre}_{version}_{type}_{numAvn}";
                if (AlbSessionHelper.CurrentFolders.TryGetValue(new FolderKey(codeOffre, version.ParseInt().Value, type, numAvn.ParseInt().Value, AlbSessionHelper.ConnectedUser, tabGuid.Replace("tabGuid", string.Empty)), out var info)) {
                    info.ReadOnlyFolder = isForceReadOnly;
                }
            }
            var shouldRefreshClauses = !(GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, numAvn) || GetIsModifHorsAvn(tabGuid, codeOffre + "_" + version + "_" + type));
            if (!string.IsNullOrEmpty(paramRedirect))
            {
                if (shouldRefreshClauses) {
                    GenererClauses(codeOffre, version, type, codeFormule, codeOption, codeRisque);
                }

                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }

            //var ContentIS = "";
            //string splitC = "#**#";
            //var strparam = type + splitC + codeOffre + splitC + version + splitC + codeFormule + splitC + codeOption;

            //using (var serviceContext = new DbInteraction()) {
            //    ContentIS = serviceContext.LoadDbData(modeNavig, "0", codeRisque, codeFormule, codeOption, "Garantie", codeOffre, version, type, branche, "Garanties", "", "", splitC, strparam);
            //}

            if (cible?.ToUpper() == "INFORMATIONSSPECIFIQUESGARANTIE") {
                bool hasIS = InformationsSpecifiquesGarantieController.HasIS(
                    new Albingia.Kheops.OP.Domain.Affaire.AffaireId {
                        CodeAffaire = codeOffre,
                        NumeroAliment = int.Parse(version),
                        TypeAffaire = type.ParseCode<Albingia.Kheops.OP.Domain.Affaire.AffaireType>(),
                        NumeroAvenant = int.TryParse(numAvn, out int num) && num >= 0 ? num : default(int?)
                    },
                    int.Parse(codeRisque),
                    int.TryParse(codeObjet, out num) && num >= 0 ? num : default,
                    int.Parse(codeOption),
                    int.Parse(codeFormule));

                if (hasIS) {
                    return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + "_" + codeFormule + "_" + codeOption + "_" + codeRisque + "_" + tabGuid + BuildAddParamString(addParamType, addParamValue + (isForceReadOnly ? "||FORCEREADONLY|1" : string.Empty)) + GetFormatModeNavig(modeNavig), returnHome = saveCancel });
                }
                else {
                    if (shouldRefreshClauses) {
                        GenererClauses(codeOffre, version, type, codeFormule, codeOption, codeRisque);
                    }

                    return RedirectToAction(job, "ConditionsGarantie",
                        new {
                            id = codeOffre + "_" + version + "_" + type + "_" + codeFormule
                                + "_" + codeOption + "_" + codeRisque + tabGuid + BuildAddParamString(addParamType, addParamValue + (isForceReadOnly ? "||FORCEREADONLY|1" : string.Empty)) + GetFormatModeNavig(modeNavig),
                            returnHome = saveCancel
                        });
                }
            }

            if (shouldRefreshClauses)
                GenererClauses(codeOffre, version, type, codeFormule, codeOption, codeRisque);

            if (cible == "MatriceFormule")
                CheckFormule(codeOffre, version, type, codeFormule, codeOption);

            if (cible == "CreationFormuleGarantie")
                return RedirectToAction(job, cible, new
                {
                    id = codeOffre + "_" + version + "_" + type + "_" + codeFormule + "_" + codeOption + tabGuid +
                        BuildAddParamString(addParamType, addParamValue +
                        (isModeConsult ? string.Empty :
                        readonlyDisplay == "true" ? "||FORCEREADONLY|1" :
                        (sessionLost ? "||IGNOREREADONLY|1" : "||AVNREFRESHUSERUPDATE|1||IGNOREREADONLY|1"))) + GetFormatModeNavig(modeNavig) + (isModeConsult ? "ConsultOnly" : string.Empty)
                });

            return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue + ((isModeConsultationEcran || shouldRefreshClauses) && !isForceReadOnly ? string.Empty : "||IGNOREREADONLY|1")) + GetFormatModeNavig(modeNavig) });
        }

        private void GenererClauses(string codeOffre, string version, string type, string codeFormule, string codeOption, string codeRisque)
        {
            int argVersion;
            int argCodeFor;
            int argCodeOpt;
            int argCodeRsq;

            if (int.TryParse(version, out argVersion) && int.TryParse(codeFormule, out argCodeFor) && int.TryParse(codeOption, out argCodeOpt) && int.TryParse(codeRisque, out argCodeRsq))
            {
                using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    var serviceontext = chan.Channel;
                    RetGenClauseDto retGenClause = serviceontext.GenerateClause(type, codeOffre,
                      argVersion,
                      new ParametreGenClauseDto
                      {
                          ActeGestion = "**",
                          Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Option),
                          NuFormule = argCodeFor,
                          NuOption = argCodeOpt,
                          NuRisque = argCodeRsq
                      });
                    if (retGenClause != null && !string.IsNullOrEmpty(retGenClause.MsgErreur))
                    {
                        throw new AlbFoncException(retGenClause.MsgErreur);
                    }
                }
            }
        }

        [ErrorHandler]
        public ActionResult LoadPorteeGarantie(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string codeGarantie, string modeNavig, string alimAssiette, string codeBranche, string codeCible, string modifAvn, string codeObjetRisque, bool isReadonly = false)
        {
            var model = new ModelePorteeGarantie();
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext = channelClient.Channel;
                var result = serviceContext.ObtenirInfosPorteeFormuleGarantie(codeOffre, version, type, codeAvn, codeFormule, codeOption, codeGarantie, modeNavig.ParseCode<ModeConsultation>(), alimAssiette, codeBranche, codeCible, modifAvn, codeObjetRisque);
                if (result != null)
                    model = ((ModelePorteeGarantie)result);


                model.isReadOnly = isReadonly;
                model.AlimAssiette = alimAssiette;

                if (alimAssiette == "B" || alimAssiette == "C")
                {
                    model.Action = "A";
                }
            }

            return PartialView("PorteeGarantie", model);
        }

        [ErrorHandler]
        public void SavePorteeGarantie(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string idGarantie, string codeGarantie, string nature, string codeRsq, string codesObj, int codeInven, string albNiv, string modeNavig, string objPortee, string alimAssiette, bool reportCal, string codeObjetRisque)
        {
            var serializer = AlbJsExtendConverter<ModeleObjet>.GetSerializer();
            var objetsPortee = serializer.ConvertToType<List<ModeleObjet>>(serializer.DeserializeObject(objPortee));

            var rsq = new ModeleRisque { Code = codeRsq, Objets = objetsPortee };

            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext = channelClient.Channel;
                var keyFormules = GetKeyFormules(
                string.Format("{0}{6}{1}{6}{2}{6}{3}{6}{4}{6}{5}", codeOffre, version, type, codeFormule, codeOption, modeNavig, MvcApplication.SPLIT_CONST_HTML));
                var formulesGarantiesSave = GetFormuleSaveFromCache(keyFormules);

                serviceContext.SavePorteeGarantie(codeOffre, version, type, codeAvn, codeFormule, codeOption, idGarantie, codeGarantie, nature, codeRsq, codesObj, codeObjetRisque, ModeleFormuleGarantieSave.LoadDto(formulesGarantiesSave), GetUser(), ModeleRisque.LoadDto(rsq), alimAssiette, reportCal);
            }

            UpdateCacheGarantie(codeOffre, version, type, codeAvn, codeFormule, codeOption, albNiv, modeNavig, codeInven, string.Empty, nature);
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult Annuler(string codeOffre, string version, string type, string codeFormule, string codeOption, string addedInventaires, string tabGuid, string modeNavig,
            string addParamType, string addParamValue, bool isModeConsultationEcran, bool isForceReadOnly, bool isModeDuplication)
        {
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            var isOffreReadonly = GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, numAvn);

            if (!isOffreReadonly)
            {
                if (isModeDuplication)
                {
                    DeleteFormuleGarantie(codeOffre, version, type, codeFormule, codeOption, modeNavig);
                }

                CheckFormule(codeOffre, version, type, codeFormule, codeOption);

                var codesGaranties = string.Empty;
                var codesInventaires = string.Empty;
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    var screenClient = channelClient.Channel;
                    if (!string.IsNullOrEmpty(addedInventaires))
                    {
                        var garanties = addedInventaires.Split(';');
                        foreach (var gar in garanties)
                        {
                            codesGaranties = string.IsNullOrEmpty(codesGaranties) ? gar.Split('_')[0] : codesGaranties + ";" + gar.Split('_')[0];
                            codesInventaires = string.IsNullOrEmpty(codesInventaires) ? gar.Split('_')[1] : codesInventaires + ";" + gar.Split('_')[1];
                        }
                        codesInventaires += ";**fin";
                        codesGaranties += ";**fin";
                        var toReturn = screenClient.SupprimerGarantieListInventaires(codeOffre, version, type, codeFormule, codesGaranties, codesInventaires);
                        if (!string.IsNullOrEmpty(toReturn))
                            throw new AlbFoncException(toReturn);
                    }
                }
                var keyFormules = GetKeyFormules(
                    string.Format("{0}{6}{1}{6}{2}{6}{3}{6}{4}{6}{5}", codeOffre, version.ToString(CultureInfo.InvariantCulture), type, codeFormule,
                        codeOption, modeNavig, MvcApplication.SPLIT_CONST_HTML));
                DeleteFormuleFromCache(keyFormules);
            }
            return RedirectToAction("Index", "MatriceFormule", new { id = codeOffre + "_" + version + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue + ((isModeConsultationEcran || isOffreReadonly) && !isForceReadOnly ? string.Empty : "||IGNOREREADONLY|1")) + GetFormatModeNavig(modeNavig) });
        }

        [ErrorHandler]
        public ActionResult OpenInfoDetail(string codeOffre, string version, string type, string codeGarantie, string codeFormule, string codeOption)
        {
            var toReturn = new ModeleInfoDetailsGarantie();
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext = channelClient.Channel;
                var result = serviceContext.GetInfoDetailsGarantie(codeOffre, version, type, codeGarantie, codeFormule, codeOption);
                if (result != null)
                {
                    toReturn = (ModeleInfoDetailsGarantie)result;
                }
            }

            return PartialView("InfoDetailsGarantie", toReturn);
        }

        [ErrorHandler]
        public string GetLibCible(string codeCible)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext = channelClient.Channel;
                return serviceContext.GetLibCible(codeCible);
            }
        }

        #region Div flottante Inventaire
        [ErrorHandler]
        public void EnregistrerInventaire(string codeOffre, string version, string type, string codeAvn, string codeRisque, string codeObjet, string codeInven, string codeOption, string codeFormule,
            string descriptif, string description, string tabGuid, string ecranProvenance, string albNiv,
            string typeAlim, string valReport, string unitReport, string typeReport, string taxeReport, bool activeReport, string idGaran, string modeNavig)
        {
            //Sauvegarde uniquement si l'écran n'est pas en readonly
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn))
            {
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    var serviceContext = channelClient.Channel;
                    serviceContext.SaveInventaire(codeOffre, version, type, ecranProvenance, codeRisque, codeObjet, codeInven, descriptif, HttpUtility.UrlDecode(description), valReport, unitReport, typeReport, taxeReport, activeReport, typeAlim, idGaran, codeFormule, codeOption);
                }
                UpdateCacheGarantie(codeOffre, version, type, codeAvn, codeFormule, codeOption, albNiv, modeNavig, Convert.ToInt32(codeInven));
            }
        }
        [ErrorHandler]
        public void SupprimerInventaire(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeGarantie, string codeInventaire, string codeOption, string albNiv, string formGen, string tabGuid, string saveCancel, string modeNavig)
        {
            DeleteInventaire(codeOffre, version, type, codeFormule, codeGarantie, codeInventaire);
            UpdateCacheGarantie(codeOffre, version, type, codeAvn, codeFormule, codeOption, albNiv, modeNavig, 0);
        }


        [ErrorHandler]
        public void FermerDivFInventaire(string codeOffre, string version, string type, string codeAvn, string codeOption, string codeFormule, string albNiv, string tabGuid, string modeNavig)
        {
            //Sauvegarde uniquement si l'écran n'est pas en readonly
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn))
            {
                UpdateCacheGarantie(codeOffre, version, type, codeAvn, codeFormule, codeOption, albNiv, modeNavig, 0);
            }
        }

        #endregion

        #endregion

        #region Méthode Privée

        protected override void LoadInfoPage(string id)
        {
            var tId = id.Split('_');
            switch (tId[2])
            {
                case "O":
                    //using (var policeServicesClient = new PoliceServicesClient())
                    //{
                    //    model.Offre = new Offre_MetaModel();
                    //    model.Offre.LoadOffre(policeServicesClient.OffreGetDto(tId[0], int.Parse(tId[1]), tId[2], model.ModeNavig.ParseCode<ModeConsultation>()));
                    using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                    {
                        var CommonOffreClient = chan.Channel;
                        model.Offre = new Offre_MetaModel();
                        model.Offre.LoadInfosOffre(CommonOffreClient.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig));
                        model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
                    }
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    break;
                case "P":
                    //using (var serviceContext = new AffaireNouvelleClient())
                    //{
                    //    model.Contrat = serviceContext.GetContrat(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());

                    using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                    {
                        var CommonOffreClient = chan.Channel;
                        var infosBase = CommonOffreClient.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig);
                        model.Contrat = new ContratDto()
                        {
                            CodeContrat = infosBase.CodeOffre,
                            VersionContrat = Convert.ToInt64(infosBase.Version),
                            Type = infosBase.Type,
                            Branche = infosBase.Branche.Code,
                            BrancheLib = infosBase.Branche.Nom,
                            Cible = infosBase.Branche.Cible.Code,
                            CibleLib = infosBase.Branche.Cible.Nom,
                            CourtierGestionnaire = infosBase.CabinetGestionnaire.Code,
                            Descriptif = infosBase.Descriptif,
                            CodeInterlocuteur = infosBase.CabinetGestionnaire.Code,
                            NomInterlocuteur = infosBase.CabinetGestionnaire.Inspecteur,
                            CodePreneurAssurance = Convert.ToInt32(infosBase.PreneurAssurance.Code),
                            NomPreneurAssurance = infosBase.PreneurAssurance.NomAssure,
                            PeriodiciteCode = infosBase.Periodicite,
                            DateEffetAvenant = AlbConvert.ConvertIntToDate(infosBase.DateAvnAnnee * 10000 + infosBase.DateAvnMois * 100 + infosBase.DateAvnJour),
                            Delegation = infosBase?.CabinetGestionnaire?.Delegation?.Nom,
                            Inspecteur = infosBase?.CabinetGestionnaire?.Inspecteur
                        };
                    }

                    using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                    {
                        var policeServicesClient = channelClient.Channel;
                        model.Contrat.Risques = policeServicesClient.ObtenirRisques(model.ModeNavig.ParseCode<ModeConsultation>(), tId[0], Convert.ToInt32(tId[1]), tId[2], model.NumAvenantPage);
                    }

                    var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    switch (typeAvt)
                    {
                        case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                            model.IsModeAvenant = true;
                            model.DateEffetAvenant = model.Contrat.DateEffetAvenant;
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                            model.IsModeAvenant = true;
                            model.DateEffetAvenant = model.Contrat.DateEffetAvenant;
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                            model.IsModeAvenant = true;
                            model.DateEffetAvenant = model.Contrat.DateEffetAvenant;
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                            model.IsModeAvenant = true;
                            model.DateEffetAvenant = model.Contrat.DateEffetAvenant;
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                            break;
                        default:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                            break;
                    }
                    model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);
                    break;
            }
            model.PageTitle = "Formule de garanties";
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext = channelClient.Channel;
                model.Bandeau = null;

                if (model.Offre == null && model.Contrat == null) return;

                model.AfficherBandeau = DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;

                #region Bandeau
                model.Bandeau = null;
                SetBandeauNavigation(id);
                #endregion

                LoadInfoFormule(serviceContext, id);

                if (model.Offre != null)
                {
                    if (model.Offre.Branche != null && !string.IsNullOrEmpty(model.Offre.Branche.Code))
                    {
                        model.Branche = model.Offre.Branche.Code;
                        model.BrancheLib = model.Offre.Branche.Nom;
                    }
                }
                if (model.Contrat != null)
                {

                    if (model.IsTraceAvnExist)//Une trace avenant existe-elle en base ?
                        model.IsAvenantModificationLocale = true;
                    if (model.IsAvnRefreshUserUpdate)//l'utilisateur vient-il de cocher la case?
                        model.IsAvenantModificationLocale = true;
                    if (model.CodeFormule == "0")//cas de creation de formule
                        model.IsAvenantModificationLocale = true;
                    if (!string.IsNullOrEmpty(model.Contrat.Branche))
                    {
                        model.Branche = model.Contrat.Branche;
                        model.BrancheLib = model.Contrat.BrancheLib;
                    }

                    var dateModifRsq = default(DateTime?);

                    if (!string.IsNullOrEmpty(model.ObjetRisqueCode))
                    {
                        var code = Convert.ToInt32(model.ObjetRisqueCode.Split(';')[0]);
                        var risque = model.Contrat.Risques.FirstOrDefault(m => m.Code == code);

                        dateModifRsq = risque.DateEffetAvenantModificationLocale;
                        if (!dateModifRsq.HasValue)
                        {
                            dateModifRsq = !risque.EntreeGarantie.HasValue || model.DateEffetAvenant > risque.EntreeGarantie
                                ? model.DateEffetAvenant
                                : risque.EntreeGarantie;
                        }
                    }

                    if (model.IsAvenantModificationLocale && !model.DateEffetAvenantModificationLocale.HasValue)
                    {
                        model.DateEffetAvenantModificationLocale = dateModifRsq;
                    }
                    model.DateModificationRsq = dateModifRsq.HasValue ? dateModifRsq.Value.ToString("dd/MM/yyyy") : string.Empty;
                    model.DateFinEffet = model.Contrat.FinEffetAnnee > 0 ? model.Contrat.FinEffetJour.ToString().PadLeft(2, '0') + "/" + model.Contrat.FinEffetMois.ToString().PadLeft(2, '0') + "/" + model.Contrat.FinEffetAnnee : string.Empty;
                    //model.HeureFinEffet = model.cont
                }

                #region Navigation Arbre
                SetArbreNavigation();
                if (tId[2] == "O")
                    model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
                else
                    model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);
                #endregion

            }
        }

        private string LoadInfoFormule(IRisquesGaranties serviceContext, string id)
        {
            var codeOffre = id.Split('_')[0];
            var version = id.Split('_')[1];
            var type = id.Split('_')[2];

            var libelle = string.Empty;

            model.CodeFormule = "0";
            model.CodeOption = "0";

            var lettreLibelle = "A";
            if (id.Split('_').Length > 3)
            {
                model.CodeFormule = id.Split('_')[3];
                model.CodeOption = id.Split('_')[4];
                model.FormGen = id.Split('_').Length > 5 ? id.Split('_')[5] : "0";
            }

            //var dateDeb = AlbLog.InitTracePerf("recherchesaisie");
            var result = serviceContext.InitFormuleGarantie(codeOffre, version, type, GetAddParamValue(model.AddParamValue, AlbParameterName.AVNID),
                model.CodeFormule, model.CodeOption, model.FormGen, model.ModeNavig.ParseCode<ModeConsultation>(), GetIsReadOnly(model.TabGuid, codeOffre + "_" + version + "_" + type), GetUser());
            //AlbLog.SetTracePerf(dateDeb);
            lettreLibelle = !string.IsNullOrEmpty(result.LettreLib) || model.FormGen != "0" ? result.LettreLib : lettreLibelle;
            libelle = result.Libelle;

            if ((string.IsNullOrEmpty(model.CodeFormule) || model.CodeFormule == "0") && result.Code != 0)
            {
                model.CodeFormule = result.Code.ToString();
                model.CodeOption = "1";
            }

            model.IsAlertePeriode = result.IsSorti;

            model.DateEffetAvenantModificationLocale = result.DateEffetAvenantModificationLocale;
            model.IsTraceAvnExist = result.IsTraceAvnExist;

            if (result.CodeCible > 0)
            {
                model.CodeCible = result.CodeCible;
                model.Cible = result.Cible;
                model.CibleLib = result.DescCible;
            }

            if ((result.Risques == null || !result.Risques.Any()) && result.Formule == null)
                return "ERROR";

            SetLibAppliqueA(result);

            model.LettreLib = lettreLibelle;
            model.Libelle = libelle;

            //Charge les infos "S'applique à"
            model.ObjetsRisquesAll = new ModeleFormuleGarantieLstObjRsq
            {
                TableName = "ListRsqObj"
            };
            if (result.Risques != null && result.Risques.Any())
            {
                var rsq = new List<ModeleRisque>();
                result.Risques.ForEach(m => rsq.Add((ModeleRisque)m));
                model.ObjetsRisquesAll.Risques = rsq;
                model.ObjetsRisquesAll.IsReadonly = model.IsReadOnly;
            }
            var keyFormules = GetKeyFormules(
                 string.Format("{0}{6}{1}{6}{2}{6}{3}{6}{4}{6}{5}", codeOffre, version, type, model.CodeFormule, model.CodeOption, model.ModeNavig, MvcApplication.SPLIT_CONST_HTML)
            );

            //Supression de toutes les formules du cache
            DeleteFormuleFromCache(keyFormules);

            if (result.Formule != null)
            {
                var formuleGarantieDto = result.Formule.FormuleGarantie;
                var formuleGarantieSaveDto = result.Formule.FormuleGarantieSave;
                var formuleGarantieHistoDto = result.Formule.FormuleGarantieHisto;

                if (formuleGarantieHistoDto != null && formuleGarantieHistoDto.Volets != null && formuleGarantieHistoDto.Volets.Any())
                {
                    var modelFormuleGarantieHisto = new ModeleFormuleGarantie();
                    formuleGarantieHistoDto.Volets.ToList().ForEach(m =>
                    {
                        var volet = (ModeleVolet)m;

                        modelFormuleGarantieHisto.Volets.Add(volet);
                    });
                    SetFormuleHistoFromCache(keyFormules, modelFormuleGarantieHisto);
                }

                if (formuleGarantieSaveDto.Volets != null && formuleGarantieSaveDto.Volets.Any())
                {
                    var modeleFormuleGarantieSave = new ModeleFormuleGarantieSave();
                    formuleGarantieSaveDto.Volets.ToList().ForEach(m =>
                    {
                        var volet = (VoletSave)m;

                        modeleFormuleGarantieSave.Volets.Add(volet);
                    });
                    SetFormuleSaveFromCache(keyFormules, modeleFormuleGarantieSave);
                }

                if (formuleGarantieDto.Volets?.Any() == true)
                {
                    var paramNatMods = new List<AlbSelectListItem>();
                    paramNatMods.Add(new AlbSelectListItem { Text = "C - Comprise", Value = "C", Selected = false, Title = "Comprise" });
                    paramNatMods.Add(new AlbSelectListItem { Text = "E - Exclue", Value = "E", Selected = false, Title = "Exclue" });

                    var modeleFormuleGarantie = new ModeleFormuleGarantie();
                    formuleGarantieDto.Volets.ToList().ForEach(m =>
                    {
                        var volet = (ModeleVolet)m;
                        volet.ParamNatMods = modeleFormuleGarantie.ParamNatMods;
                        modeleFormuleGarantie.Volets.Add(volet);
                    });
                    modeleFormuleGarantie.CodeOption = formuleGarantieDto.CodeOption;

                    modeleFormuleGarantie.ParamNatMods = paramNatMods;
                    modeleFormuleGarantie.IsReadOnly = model.IsReadOnly;

                    SetFormuleFromCache(keyFormules, modeleFormuleGarantie);
                    model.Formule = modeleFormuleGarantie;

                    var settings = new Newtonsoft.Json.JsonSerializerSettings()
                    {
                        DateFormatString = "dd/MM/yyyy hh:mm:ss"
                    };
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(model.Formule, Newtonsoft.Json.Formatting.Indented, settings);
                }
            }

            return string.Empty;
        }

        private ActionResult ReloadFormuleGarantie(string codeAffaire, string version, string type, string codeFormule, string codeOption)
        {
            return RedirectToAction("Index", "CreationFormuleGarantie", new { id = codeAffaire + "_" + version + "_" + type + "_" + codeFormule + "_" + codeOption + GetSurroundedTabGuid(model.TabGuid) + BuildAddParamString(model.AddParamType, model.AddParamValue + "||FORCEREADONLY|1") + GetFormatModeNavig(model.ModeNavig) });
        }

        private void SetLibAppliqueA(FormuleDto formuleDto)
        {
            var selectedRisque = string.Empty;
            var selectedObjet = string.Empty;

            if (!string.IsNullOrEmpty(formuleDto.ObjetRisqueCode))
            {
                var countSelectedObjet = -1;
                if (formuleDto.ObjetRisqueCode.Split(';').Length > 1)
                {
                    countSelectedObjet = formuleDto.ObjetRisqueCode.Split(';')[1].Split('_').Length;
                }
                var rsqDto = formuleDto.Risques.FirstOrDefault(m => m.Code.ToString() == formuleDto.ObjetRisqueCode.Split(';')[0]);

                if (rsqDto != null)
                {
                    if (formuleDto.Risques.Count() == 1)
                    {
                        if (rsqDto.Objets.Count() == 1 || rsqDto.Objets.Count() == countSelectedObjet || countSelectedObjet == -1)
                        {
                            foreach (var obj in rsqDto.Objets)
                            {
                                selectedObjet += "_" + obj.Code;
                            }
                            if (!string.IsNullOrEmpty(selectedObjet))
                            {
                                model.ObjetRisqueCode = rsqDto.Code + ";" + selectedObjet.Substring(1);
                                model.ObjetRisque = "à l'ensemble du risque";
                            }
                        }
                        else
                        {
                            foreach (var obj in rsqDto.Objets)
                            {
                                if (("_" + formuleDto.ObjetRisqueCode.Split(';')[1] + "_").Replace("_" + obj.Code + "_", "") != "_" + formuleDto.ObjetRisqueCode.Split(';')[1] + "_")
                                {
                                    selectedObjet += "_" + obj.Code;
                                }
                            }
                            if (!string.IsNullOrEmpty(selectedObjet))
                            {
                                model.ObjetRisqueCode = rsqDto.Code + ";" + selectedObjet.Substring(1);
                                model.ObjetRisque = "à l'objet(s) du risque";
                                //model.ObjetRisque = "à " + countSelectedObjet + " objet(s) du risque";
                            }
                        }
                    }
                    else
                    {
                        var rsq = formuleDto.Risques.FirstOrDefault(m => m.Code.ToString() == formuleDto.ObjetRisqueCode.Split(';')[0]);

                        if (rsq.Objets.Count() == 1 || rsq.Objets.Count() == countSelectedObjet || countSelectedObjet == -1)
                        {
                            foreach (var obj in rsq.Objets)
                            {
                                selectedObjet += "_" + obj.Code;
                            }
                            if (!string.IsNullOrEmpty(selectedObjet))
                            {
                                model.ObjetRisqueCode = rsq.Code + ";" + selectedObjet.Substring(1);
                                model.ObjetRisque = "au risque " + rsq.Code + " '" + rsq.Designation + "'";
                            }
                        }
                        else
                        {
                            foreach (var obj in rsq.Objets)
                            {
                                if (("_" + formuleDto.ObjetRisqueCode.Split(';')[1] + "_").Replace("_" + obj.Code + "_", "") != "_" + formuleDto.ObjetRisqueCode.Split(';')[1] + "_")
                                {
                                    selectedObjet += "_" + obj.Code;
                                }
                            }
                            if (!string.IsNullOrEmpty(selectedObjet))
                            {
                                model.ObjetRisqueCode = rsq.Code + ";" + selectedObjet.Substring(1);
                                model.ObjetRisque = "à l'objet(s) du risque " + rsq.Code + " '" + rsq.Designation + "'";
                                //model.ObjetRisque = "à " + countSelectedObjet + " objet(s) du risque " + rsq.Code + " '" + rsq.Designation + "'";
                            }
                        }
                    }
                }
                else if (model.Contrat != null)
                {
                    model.ObjetRisqueCode = formuleDto.ObjetRisqueCode;
                    var rsqContrat = model.Contrat.Risques.FirstOrDefault(m => m.Code.ToString() == formuleDto.ObjetRisqueCode.Split(';')[0]);

                    if (rsqContrat != null)
                    {
                        if (model.Contrat.Risques.Count() == 1)
                        {
                            if (rsqContrat.Objets.Count() == 1 || rsqContrat.Objets.Count() == countSelectedObjet || countSelectedObjet == -1)
                            {
                                foreach (var obj in rsqContrat.Objets)
                                {
                                    selectedObjet += "_" + obj.Code;
                                }
                                if (!string.IsNullOrEmpty(selectedObjet))
                                {
                                    model.ObjetRisqueCode = rsqContrat.Code + ";" + selectedObjet.Substring(1);
                                    model.ObjetRisque = "à l'ensemble du risque";
                                }
                            }
                            else
                            {
                                foreach (var obj in rsqContrat.Objets)
                                {
                                    if (("_" + formuleDto.ObjetRisqueCode.Split(';')[1] + "_").Replace("_" + obj.Code + "_", "") != "_" + formuleDto.ObjetRisqueCode.Split(';')[1] + "_")
                                    {
                                        selectedObjet += "_" + obj.Code;
                                    }
                                }
                                if (!string.IsNullOrEmpty(selectedObjet))
                                {
                                    model.ObjetRisqueCode = rsqContrat.Code + ";" + selectedObjet.Substring(1);
                                    model.ObjetRisque = "à l'objet(s) du risque";
                                    //model.ObjetRisque = "à " + countSelectedObjet + " objet(s) du risque";
                                }
                            }
                        }
                        else
                        {
                            var rsq = model.Contrat.Risques.FirstOrDefault(m => m.Code.ToString() == formuleDto.ObjetRisqueCode.Split(';')[0]);

                            if (rsq.Objets.Count() == 1 || rsq.Objets.Count() == countSelectedObjet || countSelectedObjet == -1)
                            {
                                foreach (var obj in rsq.Objets)
                                {
                                    selectedObjet += "_" + obj.Code;
                                }
                                if (!string.IsNullOrEmpty(selectedObjet))
                                {
                                    model.ObjetRisqueCode = rsq.Code + ";" + selectedObjet.Substring(1);
                                    model.ObjetRisque = "au risque " + rsq.Code + " '" + rsq.Designation + "'";
                                }
                            }
                            else
                            {
                                foreach (var obj in rsq.Objets)
                                {
                                    if (("_" + formuleDto.ObjetRisqueCode.Split(';')[1] + "_").Replace("_" + obj.Code + "_", "") != "_" + formuleDto.ObjetRisqueCode.Split(';')[1] + "_")
                                    {
                                        selectedObjet += "_" + obj.Code;
                                    }
                                }
                                if (!string.IsNullOrEmpty(selectedObjet))
                                {
                                    model.ObjetRisqueCode = rsq.Code + ";" + selectedObjet.Substring(1);
                                    model.ObjetRisque = "à l'objet(s) du risque " + rsq.Code + " '" + rsq.Designation + "'";
                                    //model.ObjetRisque = "à " + countSelectedObjet + " objet(s) du risque " + rsq.Code + " '" + rsq.Designation + "'";
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (!formuleDto.OffreAppliqueA)
                {
                    if (formuleDto.Risques != null && formuleDto.Risques.Count > 0)
                    {
                        foreach (var obj in formuleDto.Risques[0].Objets)
                        {
                            selectedObjet += "_" + obj.Code;
                        }
                        model.ObjetRisqueCode = formuleDto.Risques[0].Code + ";" + selectedObjet.Substring(1);
                        if (formuleDto.Risques.Count() == 1)
                        {
                            model.ObjetRisque = "à l'ensemble du risque";
                        }
                        else
                        {
                            model.ObjetRisque = "au risque " + formuleDto.Risques[0].Code + " '" + formuleDto.Risques[0].Designation + "'";
                        }
                    }
                    else
                    {
                        model.ObjetRisque = "à l'ensemble du contrat";
                    }
                }
            }
        }

        private void CheckFormule(string codeOffre, string version, string type, string codeFormule, string codeOption)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext = channelClient.Channel;
                serviceContext.CheckFormule(codeOffre, version, type, codeFormule, codeOption);
            }
        }

        private void SetBandeauNavigation(string id)
        {
            if (model.AfficherBandeau)
            {
                model.Bandeau = GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
                //Gestion des Etapes
                if (model.Offre != null)
                {
                    model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    model.Navigation = new Navigation_MetaModel
                    {
                        Etape = Navigation_MetaModel.ECRAN_RISQUEETGARANTIE,
                        IdOffre = model.Offre.CodeOffre,
                        Version = model.Offre.Version
                    };
                }
                else if (model.Contrat != null)
                {
                    var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    switch (typeAvt)
                    {
                        case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                            model.Bandeau.StyleBandeau = model.ScreenType;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        default:
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                            break;
                    }
                    model.Navigation = new Navigation_MetaModel
                    {
                        Etape = Navigation_MetaModel.ECRAN_RISQUEETGARANTIE,
                        IdOffre = model.Contrat.CodeContrat,
                        Version = int.Parse(model.Contrat.VersionContrat.ToString())
                    };
                    //model.NavigationArbre = GetNavigationArbreRegule(ContentData, "Regule");
                    //model.NavigationArbre.IsRegule = typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGUL;
                }
            }
        }

        private void SetArbreNavigation()
        {
            if (model.Offre != null)
            {
                //Affichage de la navigation latérale en arboresence
                model.NavigationArbre = GetNavigationArbre("Formule", codeFormule: Convert.ToInt32(model.CodeFormule));
            }
            else if (model.Contrat != null)
            {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Formule", codeFormule: Convert.ToInt32(model.CodeFormule));
            }
        }

        private string GetKeyFormules(string suffixeKey)
        {
            return GetUser() + MvcApplication.SPLIT_CONST_HTML + suffixeKey;
        }

        private ModeleFormuleGarantie GetFormuleFromCache(string key, string codeAvn, string formGen, string codeCible, string codeAlpha, string branche, string libFormule, bool isReadOnly, string modeNavig, bool onlyFromCache = false, int appliqueA = 0)
        {
            dynamic formulesGar;
            if (onlyFromCache)
            {
                AlbSessionHelper.FormulesGarantiesUtilisateurs.TryGetValue(key, out formulesGar);
                return formulesGar;
            }
            return AlbSessionHelper.FormulesGarantiesUtilisateurs.TryGetValue(key, out formulesGar) ? formulesGar : GetFormulesGarantiesFromDb(key, codeAvn, formGen, codeCible, codeAlpha, branche, libFormule, appliqueA, isReadOnly, modeNavig);
        }
        private ModeleFormuleGarantieSave GetFormuleSaveFromCache(string key)
        {
            dynamic formulesGar;
            AlbSessionHelper.FormulesGarantiesSaveUtilisateurs.TryGetValue(key, out formulesGar);
            return formulesGar;
        }

        private void SetFormuleFromCache(string key, ModeleFormuleGarantie newFormulesGar)
        {
            dynamic formulesGar;
            if (AlbSessionHelper.FormulesGarantiesUtilisateurs.TryGetValue(key, out formulesGar))
            {
                AlbSessionHelper.FormulesGarantiesUtilisateurs.Remove(key);
            }
            AlbSessionHelper.FormulesGarantiesUtilisateurs.Add(key, newFormulesGar);
        }

        private void SetFormuleSaveFromCache(string key, ModeleFormuleGarantieSave newFormulesGar)
        {

            dynamic formulesGar;
            if (AlbSessionHelper.FormulesGarantiesSaveUtilisateurs.TryGetValue(key, out formulesGar))
            {
                AlbSessionHelper.FormulesGarantiesSaveUtilisateurs.Remove(key);
            }
            AlbSessionHelper.FormulesGarantiesSaveUtilisateurs.Add(key, newFormulesGar);
        }

        private void SetFormuleHistoFromCache(string key, ModeleFormuleGarantie newFormulesGar)
        {
            dynamic formulesGar;
            if (AlbSessionHelper.FormulesGarantiesHistoUtilisateurs.TryGetValue(key, out formulesGar))
            {
                AlbSessionHelper.FormulesGarantiesHistoUtilisateurs.Remove(key);
            }
            AlbSessionHelper.FormulesGarantiesHistoUtilisateurs.Add(key, newFormulesGar);
        }

        private void DeleteFormuleFromCache(string key)
        {
            dynamic formulesGar;
            if (AlbSessionHelper.FormulesGarantiesUtilisateurs.TryGetValue(key, out formulesGar))
            {
                AlbSessionHelper.FormulesGarantiesUtilisateurs.Remove(key);
            }
            if (AlbSessionHelper.FormulesGarantiesSaveUtilisateurs.TryGetValue(key, out formulesGar))
            {
                AlbSessionHelper.FormulesGarantiesSaveUtilisateurs.Remove(key);
            }
            if (AlbSessionHelper.FormulesGarantiesHistoUtilisateurs.TryGetValue(key, out formulesGar))
            {
                AlbSessionHelper.FormulesGarantiesHistoUtilisateurs.Remove(key);
            }
        }

        private ModeleFormuleGarantie GetFormulesGarantiesFromDb(string keyFormules, string codeAvn, string formGen, string codeCible, string codeAlpha, string branche, string libFormule, int appliqueA, bool isReadOnly, string modeNavig)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext = channelClient.Channel;
                var tKeyFormule = keyFormules.Split(new[] { MvcApplication.SPLIT_CONST_HTML }, StringSplitOptions.None);
                if (string.IsNullOrEmpty(formGen))
                    formGen = "0";

                var formGarDto = serviceContext.FormulesGarantiesGet(tKeyFormule[1], tKeyFormule[2], tKeyFormule[3], codeAvn, tKeyFormule[4], tKeyFormule[5], formGen, codeCible, codeAlpha, branche, libFormule, AlbSessionHelper.ConnectedUser, appliqueA, isReadOnly, modeNavig.ParseCode<ModeConsultation>());

                var formuleGarantieDto = formGarDto.FormuleGarantie;
                var formuleGarantieSaveDto = formGarDto.FormuleGarantieSave;

                if (formuleGarantieSaveDto.Volets != null && formuleGarantieSaveDto.Volets.Any())
                {
                    var modeleFormuleGarantieSave = new ModeleFormuleGarantieSave();
                    formuleGarantieSaveDto.Volets.ToList().ForEach(m =>
                    {
                        var volet = (VoletSave)m;

                        modeleFormuleGarantieSave.Volets.Add(volet);
                    });
                    SetFormuleSaveFromCache(keyFormules, modeleFormuleGarantieSave);
                }

                if (formuleGarantieDto.Volets != null && formuleGarantieDto.Volets.Any())
                {
                    var modeleFormuleGarantie = new ModeleFormuleGarantie();
                    formuleGarantieDto.Volets.ToList().ForEach(m =>
                    {
                        var volet = (ModeleVolet)m;

                        modeleFormuleGarantie.Volets.Add(volet);
                    });
                    modeleFormuleGarantie.CodeOption = formuleGarantieDto.CodeOption;

                    var paramNatMods = new List<AlbSelectListItem>();
                    paramNatMods.Add(new AlbSelectListItem { Text = "C - Comprise", Value = "C", Selected = false, Title = "Comprise" });
                    paramNatMods.Add(new AlbSelectListItem { Text = "E - Exclue", Value = "E", Selected = false, Title = "Exclue" });
                    modeleFormuleGarantie.ParamNatMods = paramNatMods;


                    SetFormuleFromCache(keyFormules, modeleFormuleGarantie);
                    return modeleFormuleGarantie;
                }
                return null;

            }
        }

        private string SaveLineGarantie(string codeOffre, string version, string type, string codeGarantie, string codeAvn, string codeForm, string codeOpt, string formGen, string codeCategorie, string codeObjetRisque,
            string isCheck, string albNiv, string actegestion, string paramNat, string modeNavig, string dateModifAvt)
        {
            var keyFormules = GetKeyFormules(
                string.Format("{0}{6}{1}{6}{2}{6}{3}{6}{4}{6}{5}", codeOffre, version, type, codeForm, codeOpt, modeNavig, MvcApplication.SPLIT_CONST_HTML)
            );
            var codeFormule = 0;
            int.TryParse(codeForm, out codeFormule);
            if (codeFormule == 0)
                throw new Exception("Code formule erroné");
            var codeOption = 1;
            int.TryParse(codeOpt, out codeOption);
            var isChecked = false;
            bool.TryParse(isCheck, out isChecked);
            if (string.IsNullOrEmpty(albNiv))
                return string.Empty;
            var niveau = albNiv.Split('_');
            if (niveau.Length == 0)
                return string.Empty;


            #region FormuleGarantieSave

            var modeleFormuleGarantieSave = GetFormuleSaveFromCache(keyFormules);
            if (modeleFormuleGarantieSave != null)
            {
                switch (niveau[0].ToLower())
                {
                    case "volet":
                        var volet = GetVoletSave(niveau[1], modeleFormuleGarantieSave);
                        if (volet != null)
                        {
                            volet.isChecked = isChecked;
                            volet.MAJ = true;
                        }
                        break;
                    case "bloc":
                        var voletB = GetVoletSave(niveau[1], modeleFormuleGarantieSave);
                        var bloc = GetBlocSave(niveau[2], voletB);

                        if (bloc != null)
                        {
                            bloc.isChecked = isChecked;
                            bloc.MAJ = true;
                        }
                        break;
                    case "niv1":
                        var voletN1 = GetVoletSave(niveau[1], modeleFormuleGarantieSave);
                        var blocN1 = GetBlocSave(niveau[2], voletN1);
                        var modN1 = GetModeleSave(niveau[3], blocN1);
                        if (modN1 != null)
                        {
                            var niv1 = GetNiv1Save(niveau[4], modN1);
                            if (niv1 != null)
                            {
                                niv1.MAJ = true;
                                niv1.NatureParam = GetParamNatGar(niv1.Caractere, niv1.Nature, isChecked, paramNat);
                            }
                        }
                        break;
                    case "niv2":
                        var voletN21 = GetVoletSave(niveau[1], modeleFormuleGarantieSave);
                        var blocN21 = GetBlocSave(niveau[2], voletN21);
                        var modN21 = GetModeleSave(niveau[3], blocN21);
                        if (modN21 != null)
                        {
                            var niv21 = GetNiv1Save(niveau[4], modN21);
                            if (niv21 != null)
                            {
                                var niv2 = GetNiv2Save(niveau[5], niv21);
                                if (niv2 != null)
                                {
                                    niv2.MAJ = true;
                                    niv2.NatureParam = GetParamNatGar(niv2.Caractere, niv2.Nature, isChecked, paramNat);
                                }
                            }
                        }
                        break;
                    case "niv3":
                        var voletN31 = GetVoletSave(niveau[1], modeleFormuleGarantieSave);
                        var blocN31 = GetBlocSave(niveau[2], voletN31);
                        var modN31 = GetModeleSave(niveau[3], blocN31);
                        if (modN31 != null)
                        {
                            var niv31 = GetNiv1Save(niveau[4], modN31);
                            if (niv31 != null)
                            {
                                var niv32 = GetNiv2Save(niveau[5], niv31);
                                if (niv32 != null)
                                {
                                    var niv3 = GetNiv3Save(niveau[6], niv32);
                                    if (niv3 != null)
                                    {
                                        niv3.MAJ = true;
                                        niv3.NatureParam = GetParamNatGar(niv3.Caractere, niv3.Nature, isChecked, paramNat);
                                    }
                                }
                            }
                        }
                        break;

                    case "niv4":
                        var voletN4 = GetVoletSave(niveau[1], modeleFormuleGarantieSave);
                        var blocN4 = GetBlocSave(niveau[2], voletN4);
                        var modN4 = GetModeleSave(niveau[3], blocN4);
                        if (modN4 != null)
                        {
                            var niv41 = GetNiv1Save(niveau[4], modN4);
                            if (niv41 != null)
                            {
                                var niv42 = GetNiv2Save(niveau[5], niv41);
                                if (niv42 != null)
                                {
                                    var niv43 = GetNiv3Save(niveau[6], niv42);
                                    if (niv43 != null)
                                    {
                                        var niv4 = GetNiv4Save(niveau[7], niv43);
                                        if (niv4 != null)
                                        {
                                            niv4.MAJ = true;
                                            niv4.NatureParam = GetParamNatGar(niv4.Caractere, niv4.Nature, isChecked, paramNat);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            #endregion

            if (!string.IsNullOrEmpty(codeAvn) && codeAvn != "0")
            {
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    var serviceContext = channelClient.Channel;
                    var formulesGarantiesSave = GetFormuleSaveFromCache(keyFormules);

                    var dateModif = AlbConvert.ConvertStrToDate(dateModifAvt);
                    if (!isChecked)
                        dateModif = dateModif.Value.AddDays(-1);

                    var niveauLib = niveau[0];
                    var guidV = niveau[1];
                    var guidB = niveauLib == "volet" ? "0" : niveau[2];
                    var guidG = niveauLib == "niv1" ? niveau[4] : niveauLib == "niv2" ? niveau[5] : niveauLib == "niv3" ? niveau[6] : niveauLib == "niv4" ? niveau[7] : "0";
                    serviceContext.UpdateDateForcee(codeOffre, version, type, codeGarantie, codeFormule.ToString(), codeOption.ToString(), codeAvn, codeObjetRisque, niveauLib, guidV, guidB, guidG, !string.IsNullOrEmpty(dateModifAvt) ? AlbConvert.ConvertDateToInt(dateModif) : 0, isChecked, GetUser(), ModeleFormuleGarantieSave.LoadDto(formulesGarantiesSave));
                }
            }
            /*SAB Parfait achèvement*/
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext = channelClient.Channel;
                if (!string.IsNullOrEmpty(codeGarantie) && codeGarantie != "0" && isCheck == "true" && actegestion != AlbConstantesMetiers.TYPE_AVENANT_REGUL && actegestion != AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF)
                {
                    var formulesGarantiesSave = GetFormuleSaveFromCache(keyFormules);

                    serviceContext.InitParfaitAchevement(codeOffre, version, type, codeGarantie, codeFormule.ToString(), codeOption.ToString(), codeAvn, codeObjetRisque, GetUser(), ModeleFormuleGarantieSave.LoadDto(formulesGarantiesSave));
                }
            }
            #region FormuleGarantie
            var modeleFormuleGarantie = GetFormuleFromCache(keyFormules, codeAvn, formGen, codeCategorie, string.Empty, string.Empty, string.Empty, true, modeNavig, true);
            if (modeleFormuleGarantie != null)
            {
                switch (niveau[0].ToLower())
                {
                    case "volet":
                        var volet = GetVolet(niveau[1], modeleFormuleGarantie);
                        if (volet != null)
                        {
                            volet.isChecked = isChecked;
                            volet.MAJ = true;
                        }
                        break;
                    case "bloc":
                        var voletB = GetVolet(niveau[1], modeleFormuleGarantie);
                        var bloc = GetBloc(niveau[2], voletB);

                        if (bloc != null)
                        {
                            bloc.isChecked = isChecked;
                            bloc.MAJ = true;
                        }
                        break;
                    case "niv1":
                        var voletN1 = GetVolet(niveau[1], modeleFormuleGarantie);
                        var blocN1 = GetBloc(niveau[2], voletN1);
                        var modN1 = GetModele(niveau[3], blocN1);
                        if (modN1 != null)
                        {
                            var niv1 = GetNiv1(niveau[4], modN1);
                            if (niv1 != null)
                            {
                                niv1.MAJ = true;
                                niv1.NatureParam = GetParamNatGar(niv1.Caractere, niv1.Nature, isChecked, paramNat);
                                niv1.ParamNatModVal = paramNat;
                                return niv1.NatureParam;
                            }
                        }
                        break;
                    case "niv2":
                        var voletN21 = GetVolet(niveau[1], modeleFormuleGarantie);
                        var blocN21 = GetBloc(niveau[2], voletN21);
                        var modN21 = GetModele(niveau[3], blocN21);
                        if (modN21 != null)
                        {
                            var niv21 = GetNiv1(niveau[4], modN21);
                            if (niv21 != null)
                            {
                                var niv2 = GetNiv2(niveau[5], niv21);
                                if (niv2 != null)
                                {
                                    niv2.MAJ = true;
                                    niv2.NatureParam = GetParamNatGar(niv2.Caractere, niv2.Nature, isChecked, paramNat);
                                    niv2.ParamNatModVal = paramNat;
                                    return niv2.NatureParam;
                                }
                            }
                        }
                        break;
                    case "niv3":
                        var voletN31 = GetVolet(niveau[1], modeleFormuleGarantie);
                        var blocN31 = GetBloc(niveau[2], voletN31);
                        var modN31 = GetModele(niveau[3], blocN31);
                        if (modN31 != null)
                        {
                            var niv31 = GetNiv1(niveau[4], modN31);
                            if (niv31 != null)
                            {
                                var niv32 = GetNiv2(niveau[5], niv31);
                                if (niv32 != null)
                                {
                                    var niv3 = GetNiv3(niveau[6], niv32);
                                    if (niv3 != null)
                                    {
                                        niv3.MAJ = true;
                                        niv3.NatureParam = GetParamNatGar(niv3.Caractere, niv3.Nature, isChecked, paramNat);
                                        niv3.ParamNatModVal = paramNat;
                                        return niv3.NatureParam;
                                    }
                                }
                            }
                        }
                        break;

                    case "niv4":
                        var voletN4 = GetVolet(niveau[1], modeleFormuleGarantie);
                        var blocN4 = GetBloc(niveau[2], voletN4);
                        var modN4 = GetModele(niveau[3], blocN4);
                        if (modN4 != null)
                        {
                            var niv41 = GetNiv1(niveau[4], modN4);
                            if (niv41 != null)
                            {
                                var niv42 = GetNiv2(niveau[5], niv41);
                                if (niv42 != null)
                                {
                                    var niv43 = GetNiv3(niveau[6], niv42);
                                    if (niv43 != null)
                                    {
                                        var niv4 = GetNiv4(niveau[7], niv43);
                                        if (niv4 != null)
                                        {
                                            niv4.MAJ = true;
                                            niv4.NatureParam = GetParamNatGar(niv4.Caractere, niv4.Nature, isChecked, paramNat);
                                            niv4.ParamNatModVal = paramNat;
                                            return niv4.NatureParam;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            else
                return "ERRORCACHE";
            #endregion
            return string.Empty;
        }

        private static VoletSave GetVoletSave(string guid, ModeleFormuleGarantieSave modeleFormuleGarantieSave)
        {
            return modeleFormuleGarantieSave.Volets.FirstOrDefault(el => el.GuidId == Convert.ToInt32(guid));
        }
        private static BlocSave GetBlocSave(string guid, VoletSave volet)
        {
            return volet.Blocs.FirstOrDefault(el => el.GuidId == Convert.ToInt32(guid));
        }
        private static ModeleSave GetModeleSave(string guid, BlocSave bloc)
        {
            return bloc.Modeles.FirstOrDefault(el => el.GuidId == guid);
        }
        private static ModeleNiv1Save GetNiv1Save(string guid, ModeleSave modele)
        {
            return modele.Modeles.FirstOrDefault(el => el.GuidGarantie == guid);
        }
        private static ModeleNiv2Save GetNiv2Save(string guid, ModeleNiv1Save niv1)
        {
            return niv1.Modeles.FirstOrDefault(el => el.GuidGarantie == guid);
        }
        private static ModeleNiv3Save GetNiv3Save(string guid, ModeleNiv2Save niv2)
        {
            return niv2.Modeles.FirstOrDefault(el => el.GuidGarantie == guid);
        }
        private static ModeleNiv4Save GetNiv4Save(string guid, ModeleNiv3Save niv3)
        {
            return niv3 == null ? null : niv3.Modeles.FirstOrDefault(el => el.GuidGarantie == guid);
        }

        private static ModeleVolet GetVolet(string guid, ModeleFormuleGarantie modeleFormuleGarantie)
        {
            return modeleFormuleGarantie.Volets.FirstOrDefault(el => el.GuidId == guid);
        }
        private static ModeleBloc GetBloc(string guid, ModeleVolet volet)
        {
            return volet.Blocs.FirstOrDefault(el => el.GuidId == guid);
        }
        private static ModeleModele GetModele(string guid, ModeleBloc bloc)
        {
            return bloc.Modeles.FirstOrDefault(el => el.GuidId == guid);
        }
        private static ModeleGarantieNiveau1 GetNiv1(string guid, ModeleModele modele)
        {
            return modele.Modeles.FirstOrDefault(el => el.GuidGarantie == guid);
        }
        private static ModeleGarantieNiveau2 GetNiv2(string guid, ModeleGarantieNiveau1 niv1)
        {
            return niv1.Modeles.FirstOrDefault(el => el.GuidGarantie == guid);
        }
        private static ModeleGarantieNiveau3 GetNiv3(string guid, ModeleGarantieNiveau2 niv2)
        {
            return niv2.Modeles.FirstOrDefault(el => el.GuidGarantie == guid);
        }
        private static ModeleGarantieNiveau4 GetNiv4(string guid, ModeleGarantieNiveau3 niv3)
        {
            return niv3 == null ? null : niv3.Modeles.FirstOrDefault(el => el.GuidGarantie == guid);
        }

        private string GetParamNatGar(string carac, string nature, bool isChecked, string paramNat)
        {
            if (!string.IsNullOrEmpty(paramNat))
                return paramNat;

            var paramNatGar = MvcApplication.AlbLstParamNatGar.FirstOrDefault(el => el.Caractere == carac && el.Nature == nature);
            if (paramNatGar != null)
            {
                return isChecked ? paramNatGar.NatureParamChecked : paramNatGar.NatureParamNoChecked;
            }
            return string.Empty;
        }

        private string UpdateCacheGarantie(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string albNiv, string modeNavig, int codeInventaire, string detailModif = "", string porteeModif = "")
        {
            var niveau = albNiv.Split('_');
            if (niveau.Length == 0)
                return string.Empty;

            var keyFormules = GetKeyFormules(
                string.Format("{0}{6}{1}{6}{2}{6}{3}{6}{4}{6}{5}", codeOffre, version, type, codeFormule, codeOption, modeNavig, MvcApplication.SPLIT_CONST_HTML)
            );

            var modeleFormuleGarantie = GetFormuleFromCache(keyFormules, codeAvn, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, true, modeNavig, true);
            if (modeleFormuleGarantie != null)
            {
                switch (niveau[0].ToLower())
                {
                    case "niv1":
                        var voletN1 = GetVolet(niveau[1], modeleFormuleGarantie);
                        var blocN1 = GetBloc(niveau[2], voletN1);
                        var modN1 = GetModele(niveau[3], blocN1);
                        if (modN1 != null)
                        {
                            var niv1 = GetNiv1(niveau[4], modN1);
                            if (niv1 != null)
                            {
                                niv1.MAJ = true;
                                if (!string.IsNullOrEmpty(detailModif))
                                    niv1.FlagModif = detailModif;
                                else
                                    niv1.Action = porteeModif;
                            }
                        }
                        break;
                    case "niv2":
                        var voletN21 = GetVolet(niveau[1], modeleFormuleGarantie);
                        var blocN21 = GetBloc(niveau[2], voletN21);
                        var modN21 = GetModele(niveau[3], blocN21);
                        if (modN21 != null)
                        {
                            var niv21 = GetNiv1(niveau[4], modN21);
                            if (niv21 != null)
                            {
                                var niv2 = GetNiv2(niveau[5], niv21);
                                if (niv2 != null)
                                {
                                    niv2.CodeInven = codeInventaire;
                                    niv2.MAJ = true;
                                    if (!string.IsNullOrEmpty(detailModif))
                                        niv2.FlagModif = detailModif;
                                }
                            }
                        }
                        break;
                    case "niv3":
                        var voletN31 = GetVolet(niveau[1], modeleFormuleGarantie);
                        var blocN31 = GetBloc(niveau[2], voletN31);
                        var modN31 = GetModele(niveau[3], blocN31);
                        if (modN31 != null)
                        {
                            var niv31 = GetNiv1(niveau[4], modN31);
                            if (niv31 != null)
                            {
                                var niv32 = GetNiv2(niveau[5], niv31);
                                if (niv32 != null)
                                {
                                    var niv3 = GetNiv3(niveau[6], niv32);
                                    if (niv3 != null)
                                    {
                                        niv3.MAJ = true;
                                        if (!string.IsNullOrEmpty(detailModif))
                                            niv3.FlagModif = detailModif;
                                    }
                                }
                            }
                        }
                        break;

                    case "niv4":
                        var voletN4 = GetVolet(niveau[1], modeleFormuleGarantie);
                        var blocN4 = GetBloc(niveau[2], voletN4);
                        var modN4 = GetModele(niveau[3], blocN4);
                        if (modN4 != null)
                        {
                            var niv41 = GetNiv1(niveau[4], modN4);
                            if (niv41 != null)
                            {
                                var niv42 = GetNiv2(niveau[5], niv41);
                                if (niv42 != null)
                                {
                                    var niv43 = GetNiv3(niveau[6], niv42);
                                    if (niv43 != null)
                                    {
                                        var niv4 = GetNiv4(niveau[7], niv43);
                                        if (niv4 != null)
                                        {
                                            niv4.MAJ = true;
                                            if (!string.IsNullOrEmpty(detailModif))
                                                niv4.FlagModif = detailModif;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            return string.Empty;
        }
        private static void DeleteInventaire(string codeOffre, string version, string type, string codeFormule, string codeGarantie, string codeInventaire)
        {
            string toReturn;

            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var screenClient = channelClient.Channel;
                toReturn = screenClient.SupprimerGarantieInventaire(codeOffre, version, type, codeFormule, codeGarantie, codeInventaire);
            }
            if (!string.IsNullOrEmpty(toReturn))
                throw new AlbFoncException(toReturn);
        }
        #endregion
    }
}
