using Albingia.MVC;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.IOFile;
using ALBINGIA.Framework.Common.Models.FileModel;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Models.Avenant.ModelesAvenant;
using ALBINGIA.OP.OP_MVC.Models.CommonNavigation;
using ALBINGIA.OP.OP_MVC.Models.MetaData;
using ALBINGIA.OP.OP_MVC.Models.ModelesDocumentsJoints;
using ALBINGIA.OP.OP_MVC.Models.ModelesDoubleSaisie;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesRecherche;
using ALBINGIA.OP.OP_MVC.Models.ModeleTransverse;
using EmitMapper;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Offres.Assures;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using OPServiceContract.ISaisieCreationOffre;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class CommonNavigationController : ControllersBase<CommonNavigationModel>
    {
        private const string acteGestion = "AFFNOUV";

        [AlbVerifLockedOffer("id", "P")]
        [ErrorHandler]
        public ActionResult Index(string id) {
            if (string.IsNullOrEmpty(id))
                return null;
            id = InitializeParams(id);
            ////Traitement de split pour sortir le contexte

            string[] tId = id.Split('¤');

            model.CodeContrat = tId[0].Split('_')[0];
            model.VersionContrat = tId[0].Split('_')[1];
            model.TypeContrat = tId[0].Split('_')[2];

            model.Contexte = tId[1];
            model.Parametres = model.CodeContrat + "_" + model.VersionContrat + "_" + model.TypeContrat;
            model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.CodeContrat + "_" + model.VersionContrat + "_" + model.TypeContrat, model.NumAvenantPage);

            return View(model);
        }

        /// <summary>
        /// Obtient la page tampon en vue partielle et charge les connexités
        /// </summary>
        /// <param name="id"></param>
        /// <param name="forceReadonly"> </param>
        /// <returns></returns>
        [AlbVerifLockedOffer("id", "P")]
        [ErrorHandler]
        public ActionResult LoadConnexites(string id, bool forceReadonly) {

            return PartialView("Index", LoadConnexites(id, forceReadonly, string.Empty));
        }


        /// <summary>
        /// Obtient la page tampon en vue partielle et charge les connexités
        /// </summary>
        /// <param name="id"></param>
        /// <param name="forceReadonly"> </param>
        /// <returns></returns>
        [AlbVerifLockedOffer("id", "P")]
        [ErrorHandler]
        public ActionResult LoadConnexitesEng(string id, bool forceReadonly) {
            return PartialView("Index", LoadConnexites(id, forceReadonly, "GESTION"));
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult CommonRedirectTo(string url, string isReadonly) {
            bool @readonly = bool.TryParse(isReadonly, out var b) ? b : false;
            if (url.StartsWith("/")) {
                url = url.Substring(1);
            }
            var route = new DefaultRoute(url.Split('/'));
            if (route.HasId) {
                if (@readonly && route.Controller == "ControleFin") {
                    route.Controller = "DocumentGestion";
                }
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                    var serviceContext = client.Channel;
                    SetFolderId(route.Params[2], out string type);
                    if (!@readonly) {
                        serviceContext.CheckFormule(route.Params[0], route.Params[1], type, string.Empty, string.Empty);
                    }
                }
                return RedirectToAction(route.Action, route.Controller, new { id = route.Id });
            }
            else {
                return RedirectToAction(route.Action, route.Controller);
            }
        }

        [AlbAjaxRedirect]
        public RedirectToRouteResult RedirectTo(string action, string control, string id) {
            return RedirectToAction(action + "/" + id, control);
        }

        [ErrorHandler]
        public ActionResult OpenDblSaisie(string id, string codeAvn) {
            ModeleDoubleSaisiePage model = new ModeleDoubleSaisiePage();
            ControllersBase<ModeleDoubleSaisiePage>.SetGuid(model, id, out id);
            var tId = id.Split('_');
            model = LoadPage(tId[0], tId[1], tId[2], codeAvn, model.TabGuid);
            return PartialView("/Views/DoubleSaisie/DblSaisieBody.cshtml", model);
        }

        [ErrorHandler]
        public ActionResult EnregistrerDemandeur(string codeOffre, string version, string type, string argDemandeur, string tabGuid, string codeAvn) {
            ModeleDoubleSaisiePage model = new ModeleDoubleSaisiePage();
            JavaScriptSerializer serializer = AlbJsExtendConverter<ModeleDoubleSaisieAdd>.GetSerializer();
            var demandeurModel = serializer.ConvertToType<ModeleDoubleSaisieAdd>(serializer.DeserializeObject(argDemandeur));

            //Get Offre From Cache
            var offreMetaModel = model.Offre = CacheModels.GetOffreFromCache(codeOffre, int.Parse(version), type);
            DateTime dateNow = DateTime.Now;

            if (demandeurModel.Action == "REM" && offreMetaModel.Etat == "V") {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IDoubleSaisie>()) {
                    var serviceContext = client.Channel;
                    version = serviceContext.GetNewVersionOffre(codeOffre, version, type, GetUser());
                    if (string.IsNullOrEmpty(version))
                        throw new AlbFoncException("Erreur de sauvegarde", true, true);
                }
            }

            var offreCabinetCourtage = new CabinetAutreDto {
                CodeOffre = codeOffre,
                Version = version,
                Type = type,
                Code = demandeurModel.Code,
                Courtier = demandeurModel.Courtier,
                Delegation = demandeurModel.Delegation,
                EnregistrementDate = dateNow,
                EnregistrementHeure = new TimeSpan(dateNow.Hour, dateNow.Minute, dateNow.Second),
                Action = demandeurModel.Action,
                Motif = demandeurModel.Action == "INI" ? "Apporteur initial" : demandeurModel.Action == "REF" ? "Refus" : demandeurModel.MotifRemp,
                Interlocuteur = demandeurModel.Interlocuteur,
                Reference = demandeurModel.Reference
            };
            if (!string.IsNullOrEmpty(demandeurModel.Souscripteur)) {
                offreCabinetCourtage.Souscripteur = demandeurModel.Souscripteur.Split('-')[1].Trim();
                offreCabinetCourtage.CodeSouscripteur = demandeurModel.Souscripteur.Split('-')[0].Trim();

            }

            if (demandeurModel.SaisieDate != null) {
                if (demandeurModel.SaisieHeure != null) {
                    offreCabinetCourtage.SaisieDate = new DateTime(demandeurModel.SaisieDate.Value.Year, demandeurModel.SaisieDate.Value.Month, demandeurModel.SaisieDate.Value.Day, demandeurModel.SaisieHeure.Value.Hours, demandeurModel.SaisieHeure.Value.Minutes, 0);
                    offreCabinetCourtage.SaisieHeure = new TimeSpan(demandeurModel.SaisieHeure.Value.Hours, demandeurModel.SaisieHeure.Value.Minutes, 0);
                }
                else {
                    offreCabinetCourtage.SaisieDate = new DateTime(demandeurModel.SaisieDate.Value.Year, demandeurModel.SaisieDate.Value.Month, demandeurModel.SaisieDate.Value.Day, 0, 0, 0);
                    offreCabinetCourtage.SaisieHeure = new TimeSpan(0, 0, 0);
                }
            }

            //Set Offre To Cache
            CacheModels.SetOffreCache(offreMetaModel, codeOffre, version, type);
            //Ecriture des informations
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IDoubleSaisie>()) {
                var serviceContext = client.Channel;
                // serviceContext.EnregistrerDoubleSaisie(CastObjectServices.WsCabAuPoltoDbl(offreCabinetCourtage), AlbSessionHelper.ConnectedUser.Split('\\')[1].Replace("ALBINGIA\\", ""));
                //serviceContext.EnregistrerDoubleSaisie(CastObjectServices.WsCabAuPoltoDbl(offreCabinetCourtage), AlbSessionHelper.ConnectedUser);
                serviceContext.EnregistrerDoubleSaisie(offreCabinetCourtage, AlbSessionHelper.ConnectedUser);

            }

            model = LoadPage(codeOffre, version, type, codeAvn, tabGuid);

            //REM = "Remplacé
            if (demandeurModel.Action == "REM") {
                return PartialView("/Views/DoubleSaisie/DblSaisieBody.cshtml", model);
            }

            return PartialView("/Views/DoubleSaisie/ListHisto.cshtml", model.CourtiersHistorique);
        }

        [ErrorHandler]
        public bool CheckTraceAssiette(string codeOffre, string version, string type) {
            var currentPath = HttpContext.Request.UrlReferrer.AbsoluteUri.ToLower();
            //Liste des écrans à partir desquels la redirection vers les cotisations n'est pas possible
            if ((currentPath.Contains(NomsInternesEcran.ChoixFormulesOptions.ToString().ToLower())) ||
                (currentPath.Contains(NomsInternesEcran.ChoixRisques.ToString().ToLower()))) {
                return false;
            }
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                var serviceContext = client.Channel;
                return serviceContext.CheckControlAssiette(codeOffre, version, type);
            }
        }

        /// <summary>
        /// Récupère les informations des boites de dialogue des courtiers/Assurés
        /// </summary>
        /// <param name="code"></param>
        /// <param name="codeInterlocuteur"></param>
        /// <param name="typeCabinet"></param>
        /// <returns></returns>
        [ErrorHandler]
        public ActionResult GetCabinetDetails(int code, int codeInterlocuteur, string typeCabinet) {
            ModeleCommonCabinetPreneur toReturn = null;
            switch (typeCabinet) {
                case "courtierGestion":
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>()) {
                        var screenClient = client.Channel;
                        var result = screenClient.ObtenirCabinetCourtageComplet(code, codeInterlocuteur);
                        toReturn = (ModeleCommonCabinetPreneur)result;
                        return PartialView("BoitesDialogueDetails/BoiteCourtierGestionnaire", toReturn);
                    }
                case "courtierPayeur":
                case "courtierApporteur":
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>()) {
                        var screenClient = client.Channel;
                        var result = screenClient.ObtenirCabinetCourtageComplet(code, codeInterlocuteur);
                        toReturn = (ModeleCommonCabinetPreneur)result;
                        if (codeInterlocuteur == 0)
                            return PartialView("BoitesDialogueDetails/BoiteCourtierApporteurPayeur", toReturn);
                        else
                            return PartialView("BoitesDialogueDetails/BoiteCourtierGestionnaire", toReturn);
                    }
                case "assure":
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>()) {
                        var screenClient = client.Channel;
                        var result = screenClient.ObtenirAssureComplet(code);
                        toReturn = (ModeleCommonCabinetPreneur)result;
                        return PartialView("BoitesDialogueDetails/BoiteAssure", toReturn);
                    }
                default:
                    break;
            }
            return null;
        }

        public FileDownloadResult SimpleDownloadFile() {
            var fullNameFile = HttpContext.Request["fullNameFile"];
            if (string.IsNullOrEmpty(fullNameFile))
                return null;
            string[] pathFileparts = fullNameFile.Split(new[] { "\\" }, StringSplitOptions.None);
            if (!pathFileparts.Any())
                return null;
            var fileName = pathFileparts[pathFileparts.Count() - 1];
            var pathFile = fullNameFile.Replace(fileName, string.Empty);
            return new FileDownloadResult(fileName, pathFile, fullNameFile);
        }

        public FileDownloadResult DownloadFile(string fileName, string fullNameFile, string pathFile) {
            return new FileDownloadResult(fileName, pathFile, fullNameFile);
        }

        [ErrorHandler]
        public ActionResult OpenVisualisationObservations(string codeOffre, string version, string type) {
            ModeleVisuObservations toReturn = null;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>()) {
                var commonOffreClient = client.Channel;
                toReturn = (ModeleVisuObservations)commonOffreClient.GetVisuObservations(codeOffre, version, type);
            }
            if (toReturn == null)
                toReturn = new ModeleVisuObservations();
            return PartialView("VisualisationObservations", toReturn);
        }

        public ActionResult UpdateObsv(string codeOffre, string type, int version, string obsvInfoGen, string obsvCotisation, string obsvEngagement, string obsvMntRef, string obsvRefGest)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var serviceContext = client.Channel;
                serviceContext.UpdateOrInsertObservation(codeOffre, type, version, obsvInfoGen, obsvCotisation, obsvEngagement, obsvMntRef, obsvRefGest);
            }
            return null;
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult OpenModif(string codeAffaire, string version, string type, string codeAvt, string addParam, string tabGuid, string modeNavig) {
            string cible = string.Empty;
            switch (type) {
                case AlbConstantesMetiers.TYPE_OFFRE:
                    cible = "ModifierOffre";
                    break;
                case AlbConstantesMetiers.TYPE_CONTRAT:
                    if (!string.IsNullOrEmpty(codeAvt) && codeAvt != "0")
                        cible = "AvenantInfoGenerales";
                    else
                        cible = "AnInformationsGenerales";
                    break;
            }

            if (!string.IsNullOrEmpty(addParam))
                return RedirectToAction("Index", cible, new { id = string.Format("{0}_{1}_{2}_{3}{4}{5}modeNavig{6}modeNavigConsultOnly{7}", codeAffaire, version, type, codeAvt, tabGuid, addParam, modeNavig, "newWindow") });
            return RedirectToAction("Index", cible, new { id = string.Format("{0}_{1}_{2}{3}modeNavig{4}modeNavigConsultOnly{5}", codeAffaire, version, type, tabGuid, modeNavig, "newWindow") });

        }

        [ErrorHandler]
        public ActionResult OpenRechercheAvanceePreneurAssurance(string codePreneur, string nomPreneur, string contexte) {
            ModeleRechercheAvancee toReturn = new ModeleRechercheAvancee { CodePreneurAssuranceRecherche = codePreneur, NomPreneurAssuranceRecherche = nomPreneur, Contexte = contexte };

            toReturn.StartLigne = 1;
            toReturn.EndLigne = PageSize;
            toReturn.LineCount = MvcApplication.PAGINATION_SIZE;
            toReturn.PageNumber = 1;
            toReturn.Order = "ASC";
            toReturn.By = 1;
            toReturn.CabinetsPreneurs = new List<ModeleCommonCabinetPreneur>();

            if (!string.IsNullOrEmpty(codePreneur) || !string.IsNullOrEmpty(nomPreneur)) {
                toReturn.CabinetsPreneurs = new List<ModeleCommonCabinetPreneur>();
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>()) {
                    var screenClient = client.Channel;
                    AssureGetResultDto preneurAssurance = screenClient.AssuresGet(new AssureGetQueryDto { Code = codePreneur, NomAssure = nomPreneur, DebutPagination = toReturn.StartLigne, FinPagination = toReturn.EndLigne, Order = toReturn.Order, By = toReturn.By }, false);

                    if (preneurAssurance.Assures.Any())
                        preneurAssurance.Assures.ForEach(assure => toReturn.CabinetsPreneurs.Add((ModeleCommonCabinetPreneur)assure));

                    toReturn.NbCount = preneurAssurance.AssuresCount;

                    if (toReturn.NbCount < toReturn.LineCount) {
                        toReturn.LineCount = toReturn.NbCount;
                    }
                }
            }
            return PartialView("/Views/RechercheSaisie/RechercheAvanceePreneurAssurance.cshtml", toReturn);
        }

        [ErrorHandler]
        public ActionResult RechercheTransversePreneursAssurance(string codePreneur, string nomPreneur, string codePostalPreneur) {
            List<ModeleCommonCabinetPreneur> toReturn = new List<ModeleCommonCabinetPreneur>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>()) {
                var screenClient = client.Channel;
                var result = screenClient.RechercheTransversePreneurAssurance(codePreneur, nomPreneur, codePostalPreneur);
                result.ForEach(elm => toReturn.Add((ModeleCommonCabinetPreneur)elm));
            }
            return PartialView("/Views/RechercheSaisie/ResultRechercheAvanceePreneurAssurance.cshtml", toReturn);
        }

        private ModeleDoubleSaisiePage LoadPage(string codeOffre, string version, string type, string codeAvn, string tabGuid = "")
        {
            string branche = string.Empty;
            string cible = string.Empty;

            ModeleDoubleSaisiePage model = new ModeleDoubleSaisiePage();
            model.PageTitle = "Création double saisie";

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var serviceContext=client.Channel;
                var newId = serviceContext.GetOffreLastVersion(codeOffre, version, type, GetUser());
                if (!string.IsNullOrEmpty(newId))
                    version = newId;
            }

            model.Offre = CacheModels.GetOffreFromCache(codeOffre, int.Parse(version), type);
            model.DivFlottante = true;

            if (model.Offre != null)
            {
                if (model.Offre.Branche != null)
                {
                    branche = model.Offre.Branche.Code;
                    if (model.Offre.Branche.Cible != null)
                        cible = model.Offre.Branche.Cible.Code;
                }

                if (model.Offre.CabinetApporteur != null)
                {
                    model.CourtierApporteur = new ModeleDoubleSaisieCourtier
                    {
                        Code = model.Offre.CabinetApporteur.Code,
                        Courtier = model.Offre.CabinetApporteur.NomCabinet,
                        Delegation = model.Offre.CabinetApporteur.Delegation.Nom,
                        SaisieDate = model.Offre.DateSaisie,
                        SaisieHeure = new TimeSpan(model.Offre.DateSaisie.Value.Hour,
                                                model.Offre.DateSaisie.Value.Minute,
                                                model.Offre.DateSaisie.Value.Second)
                    };

                    model.CourtierApporteur.Souscripteur = model.Offre.Souscripteur != null ? model.Offre.Souscripteur.Nom : string.Empty;
                    model.CourtierApporteur.SouscripteurCode = model.Offre.Souscripteur != null ? model.Offre.Souscripteur.Code : string.Empty;
                    if (model.Offre.DateEnregistrement.HasValue)
                    {
                        model.CourtierApporteur.EnregistrementDate = model.Offre.DateEnregistrement;
                        model.CourtierApporteur.EnregistrementHeure = new TimeSpan(model.Offre.DateEnregistrement.Value.Hour,
                                                                                    model.Offre.DateEnregistrement.Value.Minute,
                                                                                    model.Offre.DateEnregistrement.Value.Second);
                    }

                    model.CourtierGestionnaire = new ModeleDoubleSaisieCourtier
                    {
                        Code = model.Offre.CabinetGestionnaire.Code,
                        Courtier = model.Offre.CabinetGestionnaire.NomCabinet,
                        Delegation = model.Offre.CabinetGestionnaire.Delegation.Nom
                    };

                    model.CourtiersHistorique = new List<ModeleDoubleSaisieCourtier>();
                    if (model.Offre.DblSaisieAutresOffres != null)
                    {
                        if (model.Offre.DblSaisieAutresOffres.DblSaisieAutresOffres != null)
                        {
                            model.CourtiersHistorique = model.Offre.DblSaisieAutresOffres.DblSaisieAutresOffres.Where(
                                oa => oa.CabinetApporteur != null
                            ).Select(
                                oa => new ModeleDoubleSaisieCourtier
                                {
                                    Code = oa.CabinetApporteur.Code,
                                    Courtier = oa.CabinetApporteur.NomCabinet,
                                    Delegation = oa.CabinetApporteur.Delegation.Nom,
                                    Souscripteur = (oa.Souscripteur == null ? string.Empty : oa.Souscripteur.Nom),
                                    SouscripteurCode = (oa.Souscripteur == null ? string.Empty : oa.Souscripteur.Code),
                                    SaisieDate = oa.DateSaisie,
                                    SaisieHeure = new TimeSpan(oa.DateSaisie.Value.Hour, oa.DateSaisie.Value.Minute, oa.DateSaisie.Value.Second),
                                    EnregistrementDate = oa.DateEnregistrement,
                                    EnregistrementHeure = new TimeSpan(oa.DateEnregistrement.Value.Hour, oa.DateEnregistrement.Value.Minute, oa.DateEnregistrement.Value.Second),
                                    Motif = oa.MotifRefus
                                }
                            ).ToList();
                        }
                    }
                    else
                    {
                        if (model.Offre.CabinetAutres != null)
                        {
                            model.Offre.CabinetAutres.ForEach(
                                offCab => model.CourtiersHistorique.Add(
                                    new ModeleDoubleSaisieCourtier
                                    {
                                        Code = Convert.ToInt32(offCab.Code),
                                        Courtier = offCab.Courtier,
                                        Delegation = offCab.Delegation,
                                        Souscripteur = offCab.Souscripteur,
                                        SouscripteurCode = offCab.CodeSouscripteur,
                                        SaisieDate = offCab.SaisieDate,
                                        SaisieHeure = offCab.SaisieHeure,
                                        EnregistrementDate = offCab.EnregistrementDate,
                                        EnregistrementHeure = offCab.EnregistrementHeure,
                                        Motif = offCab.Motif,
                                        LibelleMotif = offCab.LibelleMotif
                                    }
                                )
                            );
                        }
                    }
                }
                switch (model.Offre.Type)
                {
                    case "O":
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                        break;
                    case "P":
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                        break;
                    case "A":
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                        break;
                    default:
                        break;
                }
            }

            model.CourtierDemandeur = new ModeleDoubleSaisieCourtier();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IDoubleSaisie>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.ObtenirDoubleSaisieListes(branche, cible);
                if (result != null)
                {
                    List<AlbSelectListItem> motifsRemp = result.MotifsRemp.Select(
                        m => new AlbSelectListItem { Value = m.Code, Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "", Selected = false, Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "" }
                    ).ToList();
                    model.MotifsRemp = motifsRemp;

                    List<AlbSelectListItem> notificationsApporteur = result.NotificationsApporteur.Select(
                        m => new AlbSelectListItem { Value = m.Code, Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "", Selected = false, Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "" }
                    ).ToList();
                    model.NotificationsApporteur = new List<AlbSelectListItem>();

                    List<AlbSelectListItem> notificationsDemandeur = result.NotificationsDemandeur.Select(
                        m => new AlbSelectListItem { Value = m.Code, Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "", Selected = false, Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "" }
                    ).ToList();
                    model.NotificationsDemandeur = new List<AlbSelectListItem>();
                }
            }

            //Mise en commentaire le 2015-04-01 pour permettre la saisie de double saisie
            //model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version.Value.ToString() + "_" + model.Offre.Type, codeAvn);

            return model;
        }

        [NonAction]
        public CommonNavigationModel LoadConnexites(string id, bool forceReadonly, string modeAff)
        {

            id = InitializeParams(id);

            ////Traitement de split pour sortir le contexte
            model.CodeContrat = id.Split('_')[0];
            model.VersionContrat = id.Split('_')[1];
            model.TypeContrat = id.Split('_')[2];
            model.Contexte = string.Empty;
            var folder = string.Format("{0}_{1}_{2}", model.CodeContrat, model.VersionContrat, model.TypeContrat);
            var tabGuid = string.Format("tabGuid{0}tabGuid", model.TabGuid);

            model.IsReadOnly = GetIsReadOnly(tabGuid, folder, model.NumAvenantPage);
            model.IsModifHorsAvenant = GetIsModifHorsAvn(tabGuid, string.Format("{0}_{1}", folder, string.IsNullOrEmpty(model.NumAvenantPage) ? "0" : model.NumAvenantPage));

            model.Connexite = ConnexiteController.LoadInfoConnexite(id, model.IsReadOnly, model.IsModifHorsAvenant, model.ModeNavig);
            model.Connexite.TypeAffichage = modeAff;
            if (model.Connexite != null)
                model.Connexite.IsConnexiteReadOnly = forceReadonly || (model.IsReadOnly && !model.IsModifHorsAvenant);
            if (model.Connexite != null && model.Connexite.Engagement != null)
            {
                model.Connexite.Engagement.TypeAffichage = modeAff;
                model.Connexite.Engagement.IsConnexiteReadOnly = forceReadonly || (model.IsReadOnly && !model.IsModifHorsAvenant);

            }
            if (model.Connexite != null && model.Connexite.Remplacement != null)
                model.Connexite.Remplacement.IsConnexiteReadOnly = forceReadonly || (model.IsReadOnly && !model.IsModifHorsAvenant);
            if (model.Connexite != null && model.Connexite.Information != null)
                model.Connexite.Information.IsConnexiteReadOnly = forceReadonly || (model.IsReadOnly && !model.IsModifHorsAvenant);
            if (model.Connexite != null && model.Connexite.Resiliation != null)
                model.Connexite.Resiliation.IsConnexiteReadOnly = forceReadonly || (model.IsReadOnly && !model.IsModifHorsAvenant);
            return model;
        }

        #region Documents Joints

        #region Méthodes Publiques

        [ErrorHandler]
        public ActionResult OpenDocumentsJoints(string codeOffre, string version, string type, string tabGuid, string modeNavig, string codeAvn)
        {
            bool isReadOnly = GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn);
            if (ModeConsultation.Historique == modeNavig.ParseCode<ModeConsultation>())
                isReadOnly = true;

            ModeleDocumentsJoints docJoints = GetListDocumentsJoints(codeOffre, version, type, modeNavig, isReadOnly);

            ModeleDocumentsJoints model = new ModeleDocumentsJoints
            {
                ReadOnly = isReadOnly,
                Type = type,
                CodeOffre = codeOffre,
                ListDocuments = docJoints.ListDocuments,
                IsValide = docJoints.IsValide,
                CodeAvn = codeAvn
            };

            return PartialView("DocumentsJoints", model);
        }

        [ErrorHandler]
        public ActionResult OpenEditionDocsJoints(string idDoc)
        {
            DocumentsAdd model = new DocumentsAdd
            {
                DocumentId = !string.IsNullOrEmpty(idDoc) ? Convert.ToInt64(idDoc) : 0,
                TypesDoc = new List<AlbSelectListItem>()
            };

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.OpenEditionDocsJoints(idDoc);
                if (result != null)
                {
                    model = (DocumentsAdd)result;
                    var types = result.TypesDoc.Select(m => new AlbSelectListItem() { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.TypesDoc = types;
                }
            }

            return PartialView("DocumentsJointsAdd", model);
        }

        [ErrorHandler]
        public ActionResult DeleteDocsJoints(string idDoc, string codeOffre, string version, string type, string docName, string docPath, string tabGuid, string modeNavig, string codeAvn)
        {
            bool isReadOnly = GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn);
            if (ModeConsultation.Historique == modeNavig.ParseCode<ModeConsultation>())
                isReadOnly = true;

            ModeleDocumentsJoints model = new ModeleDocumentsJoints { ListDocuments = new List<Documents>() };
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.DeleteDocsJoints(idDoc, codeOffre, version, type, modeNavig, isReadOnly);
                var chemin = docPath.Replace(docName, "");
                if (!IOFileManager.IsExistDirectory(chemin))
                    throw new AlbFoncException("Le dossier cible n'existe pas", trace: true, sendMail: true, onlyMessage: true);
                if (IOFileManager.IsExistFile(chemin, docName))
                    IOFileManager.DeleteFile(docName, chemin);
                if (result != null)
                {
                    model = (ModeleDocumentsJoints)result;
                }
                model.CodeAvn = codeAvn;
            }
            return PartialView("DocumentsJointsListe", model);
        }
        
        [ErrorMessageHandlerAttribute]
        public ActionResult SaveDocsJoints()
        {
            string codeOffre = Server.HtmlDecode(HttpContext.Request["docCodeOffre"]);
            string version = Server.HtmlDecode(HttpContext.Request["docVersIonOffre"]);
            string type = Server.HtmlDecode(HttpContext.Request["docTypeOffre"]);
            string idDoc = Server.HtmlDecode(HttpContext.Request["docIdAdd"]);
            string typeDoc = Server.HtmlDecode(HttpContext.Request["docTypeDoc"]);
            string titleDoc = Server.HtmlDecode(HttpContext.Request["docTitleDoc"]);
            string fileDoc = string.Empty;
            string refDoc = Server.HtmlDecode(HttpContext.Request["docReference"]);
            string chemin = Server.HtmlDecode(HttpContext.Request["docChemin"]);
            string tabGuid = Server.HtmlDecode(HttpContext.Request["docTabGuid"]);
            string modeNavig = Server.HtmlDecode(HttpContext.Request["docModeNavig"]);
            string acteGestion = Server.HtmlDecode(HttpContext.Request["docActeGestion"]);
            string numAvenant = Server.HtmlDecode(HttpContext.Request["docNumAvenant"]);

            string fileName = Server.HtmlDecode(HttpContext.Request["docFileName"]);
            FileDescription fileReturn = null;

            if (Request.Files.Count == 0 && idDoc == "0") return null;

            if (string.IsNullOrEmpty(typeDoc) || string.IsNullOrEmpty(titleDoc))
                throw new AlbFoncException("Toutes les informations marquées d'un astérisque est obligatoire", trace: true, sendMail: true, onlyMessage: true);


            if (typeDoc == "INTER")
            {
                fileName = string.Format("{0}{1}", AlbOpConstants.CheminIntercalaire, fileName);
                var fileParts = fileName.Split('\\');
                fileDoc = codeOffre + MvcApplication.SPLIT_CONST_FILE + version.PadLeft(4, '0') + MvcApplication.SPLIT_CONST_FILE + type + MvcApplication.SPLIT_CONST_FILE + (fileName.Contains("\\") ? fileParts[fileParts.Count() - 1] : fileName);
                fileDoc = fileDoc.Replace(" ", "-");
                try
                {
                    if (IOFileManager.IsExistFile(AlbOpConstants.CheminIntercalaire, fileName))
                    {
                        IOFileManager.CopyFile(fileName, chemin + fileDoc);
                    }
                }
                catch (Exception ex)
                {
                    throw new AlbTechException(new Exception(ex.Message), sendMail: true, trace: false);
                }
            }
            else
            {
                var enctype = Request.Files[0];
                var fileParts = enctype.FileName.Split('\\');
                if (idDoc == "0")
                {
                    fileDoc = codeOffre + MvcApplication.SPLIT_CONST_FILE + version.PadLeft(4, '0') + MvcApplication.SPLIT_CONST_FILE + type + MvcApplication.SPLIT_CONST_FILE + (enctype.FileName.Contains("\\") && fileParts.Count() > 0 ? fileParts[fileParts.Count() - 1] : enctype.FileName);
                    fileDoc = fileDoc.Replace(' ', '-');
                    if (string.IsNullOrEmpty(chemin) || !IOFileManager.IsExistDirectory(chemin))
                        throw new AlbFoncException("Le dossier cible n'existe pas", true, true);

                    //Verif existance dossier de l'offre/contrat dans le chemin, creation le cas échéant
                    int iVersion = 0;
                    int.TryParse(version, out iVersion);
                    var offerFolder = string.Format(@"{0}_{1:D4}_{2}", codeOffre, iVersion, type);
                    chemin = chemin + offerFolder;
                    if (!IOFileManager.IsExistDirectory(chemin))
                    {
                        System.IO.Directory.CreateDirectory(chemin);
                    }

                    //if (enctype != null && IOFileManager.IsExisitFile(chemin, codeOffre + MvcApplication.SPLIT_CONST_FILE + version + MvcApplication.SPLIT_CONST_FILE + type + MvcApplication.SPLIT_CONST_FILE + enctype.FileName))
                    if (enctype != null && IOFileManager.IsExistFile(chemin, fileDoc))
                    {
                        IOFileManager.DeleteFile(fileDoc, chemin);
                    }

                    fileReturn = IOFileManager.UploadFiles(enctype, chemin, codeOffre + MvcApplication.SPLIT_CONST_FILE + version + MvcApplication.SPLIT_CONST_FILE + type, MvcApplication.SPLIT_CONST_FILE, fileDoc);

                    chemin += "\\";
                }
            }

            bool isReadOnly = GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, numAvenant);
            if (ModeConsultation.Historique == modeNavig.Replace("modeNavig","").ParseCode<ModeConsultation>())
                isReadOnly = true;

            ModeleDocumentsJoints model = new ModeleDocumentsJoints { ListDocuments = new List<Documents>() };
            var pathDoc = chemin + fileDoc;
            if (typeDoc == "INTER" || ((typeDoc != "INTER" && fileReturn != null && idDoc == "0") || (typeDoc != "INTER" && idDoc != "0")))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    var serviceContext=client.Channel;
                    var result = serviceContext.SaveDocsJoints(codeOffre, version, type, idDoc, typeDoc, titleDoc, fileDoc, pathDoc, refDoc, GetUser(), modeNavig, isReadOnly, acteGestion);
                    if (result != null)
                        model = (ModeleDocumentsJoints)result;
                }
            }
            else
                throw new AlbFoncException("Le fichier n'a pas pu être enregistré", trace: false, sendMail: false, onlyMessage: true);

            model.CodeAvn = numAvenant;

            return PartialView("DocumentsJointsListe", model);
            //return PartialView("DocumentsJointsListe", model.ListDocuments);
        }

        [ErrorHandler]
        public string GetPathOfferClauseLibre(string codeOffre, string version, string type, string idClause, string emplacement, string sousemplacement, string ordre, string docFile, string newNameFile, string oldNameFile, string shortNewName)
        {
            //Modifié le 2016-01-05 : demande de l'AMOA
            //if (string.IsNullOrEmpty(idClause) || string.IsNullOrEmpty(emplacement) || string.IsNullOrEmpty(sousemplacement) || string.IsNullOrEmpty(ordre))
            if (string.IsNullOrEmpty(idClause) || string.IsNullOrEmpty(emplacement) || string.IsNullOrEmpty(ordre))
            {
                throw new AlbFoncException("L'emplacement et l'ordre sont obligatoires", trace: false, sendMail: false, onlyMessage: true);
            }
            else
            {
                var chemin = MvcApplication.PathGenClauseLibre;
                if (string.IsNullOrEmpty(chemin) || !IOFileManager.IsExistDirectory(chemin))
                    throw new AlbFoncException("Le dossier cible n'existe pas", trace: true, sendMail: true, onlyMessage: true);

                //Verif existance dossier de l'offre/contrat dans le chemin, creation le cas échéant
                int iVersion = 0;
                int.TryParse(version, out iVersion);
                var offerFolder = string.Format(@"{0}_{1}_{2}", codeOffre, iVersion.ToString("D4"), type);
                var cheminNewFolder = chemin + offerFolder;
                return cheminNewFolder + "\\";
            }
        }

        public string GetPathOfferClauseLibreMagnetic(string codeOffre, string version, string type, string idClause, string emplacement, string sousemplacement, string ordre) {
            if (string.IsNullOrEmpty(idClause) || string.IsNullOrEmpty(emplacement) || string.IsNullOrEmpty(ordre))
            {
                throw new AlbFoncException("L'emplacement et l'ordre sont obligatoires", trace: false, sendMail: false, onlyMessage: true);
            }
            else
            {
                var chemin = MvcApplication.PathGenClauseLibre;
                if (string.IsNullOrEmpty(chemin) || !IOFileManager.IsExistDirectory(chemin)) {
                    throw new AlbFoncException("Le dossier cible n'existe pas", trace: true, sendMail: true, onlyMessage: true);
                }
                //Verif existance dossier de l'offre/contrat dans le chemin, creation le cas échéant
                int iVersion = 0;
                int.TryParse(version, out iVersion);
                var offerFolder = string.Format(@"{0}_{1}_{2}", codeOffre, iVersion.ToString("D4"), type);
                var cheminNewFolder = chemin + offerFolder;

                return cheminNewFolder + "\\";
            }
        }

        [ErrorHandler]
        public ActionResult GetSaveDocsJointsForm() {
            return PartialView("SaveDocsJointsForm");
        }

        [ErrorHandler]
        public ActionResult ReloadDoc(string codeOffre, string version, string type, string tabGuid, string modeNavig, string codeAvn)
        {
            ModeleDocumentsJoints model = new ModeleDocumentsJoints { ListDocuments = new List<Documents>() };
            bool isReadOnly = GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn);
            if (ModeConsultation.Historique == modeNavig.ParseCode<ModeConsultation>())
                isReadOnly = true;

            ModeleDocumentsJoints docJoints = GetListDocumentsJoints(codeOffre, version, type, modeNavig, isReadOnly);

            model.ListDocuments = docJoints.ListDocuments;
            model.IsValide = docJoints.IsValide;
            model.CodeAvn = codeAvn;

            return PartialView("DocumentsJointsListe", model);
        }

        [ErrorHandler]
        public ActionResult ChangeOrderDoc(string codeOffre, string version, string type, string tabGuid, string modeNavig, string orderField, string orderType, string codeAvn)
        {
            ModeleDocumentsJoints model = new ModeleDocumentsJoints { ListDocuments = new List<Documents>() };

            bool isReadOnly = GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn);
            if (ModeConsultation.Historique == modeNavig.ParseCode<ModeConsultation>())
                isReadOnly = true;

            ModeleDocumentsJoints docJoints = GetListDocumentsJoints(codeOffre, version, type, modeNavig, isReadOnly, orderField, orderType);

            model.ListDocuments = docJoints.ListDocuments;
            model.IsValide = docJoints.IsValide;
            model.CodeAvn = codeAvn;

            return PartialView("DocumentsJointsListe", model);
        }

        [ErrorHandler]
        public string ReloadPathFile(string typologie)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.ReloadPathFile(typologie);
                return result;
            }
        }

        [ErrorHandler]
        public ActionResult OpenListDocsInter(string branche)
        {
            var tSrc = ALBINGIA.Framework.Common.Constants.AlbOpConstants.CheminIntercalaire.Split(new[] { "\\" }, StringSplitOptions.None);
            var src = tSrc[tSrc.Length - 2];
            var chmBranche = string.IsNullOrEmpty(branche) ? "" : "\\" + branche;
            var lstDoc = DirectoryManager.GetFileFromFolder(ALBINGIA.Framework.Common.Constants.AlbOpConstants.CheminIntercalaire + chmBranche, true);

            lstDoc?.ForEach(l =>
            {
                var fullPath = l.FullName.Replace(ALBINGIA.Framework.Common.Constants.AlbOpConstants.CheminIntercalaire, string.Empty);
                l.FilePath = string.Format("\\{0}", fullPath.Replace(l.Name, string.Empty));
            });
            return PartialView("DocumentsJointsIntercalaire", lstDoc);
        }

        #endregion

        private ModeleDocumentsJoints GetListDocumentsJoints(string codeOffre, string version, string type, string modeNavig, bool isReadOnly, string orderField = "", string orderType = "")
        {
            ModeleDocumentsJoints model = new ModeleDocumentsJoints();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.GetListDocumentsJoints(codeOffre, version, type, modeNavig, isReadOnly, orderField, orderType);
                if (result != null)
                {
                    model = (ModeleDocumentsJoints)result;
                }
            }
            return model;
        }

        #endregion
    }
}
