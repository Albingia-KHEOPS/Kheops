using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleEngagements;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using EmitMapper;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Engagement;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ICommon;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers {
    public class EngagementPeriodesController : ControllersBase<ModeleEngagementPeriodesPage> {
        [CheckApplicationsRisques]
        [ErrorHandler]
        public ActionResult Index(string id) {
            model.PageTitle = "Périodes d'engagement";

            id = InitializeParams(id);
            LoadInfoPage(id);

            if (!string.IsNullOrEmpty(model.AccessMode)) {
                model.IsModeConsultationEcran = false;
                return View("EngagementsPeriodeBody", model);
            }

            return View(model);
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult Redirection(string cible, string job, string codeContrat, string versionContrat, string type, string cdPeriode, string tabGuid, string modeNavig, string addParamType, string addParamValue, string modeConsult = "", string accessMode = "") {
            var modeAccess = (!string.IsNullOrEmpty(accessMode) ? "accessMode" + accessMode + "accessMode" : string.Empty);
            var codePeriode = String.IsNullOrEmpty(cdPeriode) ? "" : "_" + cdPeriode;
            if (modeConsult == "ConsultOnly") {
                SetAddParamValue(ref addParamValue, AlbParameterName.FORCEREADONLY, "1");
            }

            var id = new { id = codeContrat + "_" + versionContrat + "_" + type + codePeriode + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) + modeAccess + modeConsult };
            //Si annulation (cible = MatriceRisque)
            if (cible == "MatriceRisque") {
                var typeAvt = GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);
                switch (typeAvt) {
                    //Si avenant de résiliation
                    case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                        var codeAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
                        if (!GetIsReadOnly(tabGuid, codeContrat + "_" + versionContrat + "_" + type, codeAvn)
                                && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>())
                            return RedirectToAction("Index", "CreationAvenant", id);
                        else
                            return RedirectToAction(job, cible, id);
                    //Si avenant de modification
                    case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                        return RedirectToAction(job, cible, id);
                    //Si avenant de régule avec modification
                    case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                        return RedirectToAction(job, cible, id);
                    case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                        return RedirectToAction(job, cible, id);

                    //Si on vient de la recherche
                    default:
                        return RedirectToAction(job, "RechercheSaisie");
                }
            }
            if (cible == "RechercheSaisie") {
                return RedirectToAction(job, cible);
            }
            return RedirectToAction(job, cible, id);
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult RedirectionTerminer(string codeContrat, string versionContrat, string type, string codeAvn, string cdPeriode, string tabGuid,
            string paramRedirect, string modeNavig, string addParamType, string addParamValue, string controller = "") {
            var typeAvt = GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);
            bool existEngCnx = HasEngCnx();
            bool reachPageAttentat = HasCatnatBase();
            if (!GetIsReadOnly(tabGuid, codeContrat + "_" + versionContrat + "_" + type, codeAvn)
                && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>()) {
                if (CheckPeriodesDiscontinues(codeContrat, versionContrat, type, codeAvn)) {
                    if (model.EngagementPeriodes.Count == 0)
                        throw new AlbFoncException("Il doit exister au moins une période d'engagement", trace: false, sendMail: false, onlyMessage: true);

                    if (SaveCacheInDB(codeContrat, versionContrat, type, codeAvn, modeNavig, false)) {
                        if (!string.IsNullOrEmpty(controller)) {
                            return Redirection(controller, "Index", codeContrat, versionContrat, type, cdPeriode, tabGuid, modeNavig, addParamType, addParamValue);
                        }

                        using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                            client.Channel.SaveArbreEngagementPeriode(codeContrat, versionContrat, type, GetUser());

                            if (!string.IsNullOrEmpty(paramRedirect)) {
                                var tabParamRedirect = paramRedirect.Split('/');
                                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
                            }
                            if (typeAvt == AlbConstantesMetiers.TYPE_AVENANT_RESIL)
                                return RedirectToAction("Index", "Quittance", new { id = codeContrat + "_" + versionContrat + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });

                            //existEngCnx=true si le contrat contient une connexité de type engagement
                            if (!existEngCnx) {
                                if (typeAvt == AlbConstantesMetiers.TYPE_AVENANT_MODIF
                                    || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF
                                    || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR)
                                    return RedirectToAction("Index", reachPageAttentat ? "AttentatGareat" : "AnMontantReference", new { id = codeContrat + "_" + versionContrat + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
                                else
                                    return RedirectToAction("Index", "RechercheSaisie");
                            }
                            return RedirectToAction("Index", "EngagementsConnexite", new { id = codeContrat + "_" + versionContrat + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
                        }
                    }
                }
            }
            else {
                if (!string.IsNullOrEmpty(paramRedirect)) {
                    var tabParamRedirect = paramRedirect.Split('/');
                    return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
                }
                if (!string.IsNullOrEmpty(controller)) {
                    return Redirection(controller, "Index", codeContrat, versionContrat, type, cdPeriode, tabGuid, modeNavig, addParamType, addParamValue);
                }
                if (existEngCnx) {
                    return RedirectToAction("Index", "EngagementsConnexite", new { id = codeContrat + "_" + versionContrat + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
                }
                else if (typeAvt == AlbConstantesMetiers.TYPE_AVENANT_MODIF
                    || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF
                    || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_RESIL
                    || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR) {
                    return RedirectToAction("Index", reachPageAttentat ? "AttentatGareat" : "AnMontantReference", new { id = codeContrat + "_" + versionContrat + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
                }
                return null;
            }
            return null;
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public ActionResult SaveLineEngagementPeriode(string codeOffre, string version, string type, string codeAvn, int code, string dateDeb, string dateFin,
            Int64 engTotal, Int64 engAlbingia, Int64 partAlbingia, string actifStatut, string utiliseStatut, string typeOperation, string tabGuid,
            string modeNavig, string addParamType, string addParamValue, bool isReadOnly, string dateDebContrat, string dateFinContrat, string accessMode = "") {
            string sep = MvcApplication.SPLIT_CONST_HTML;

            int iDateDeb = 0;
            int iDateFin = 0;
            int.TryParse(dateDeb, out iDateDeb);
            int.TryParse(dateFin, out iDateFin);

            DateTime? dDateDeb = AlbConvert.ConvertIntToDate(iDateDeb);
            DateTime? dDateFin = AlbConvert.ConvertIntToDate(iDateFin);


            if (!dDateDeb.HasValue)
                iDateDeb = 0;
            else
                iDateDeb = int.Parse(dateDeb);
            if (!dDateFin.HasValue) {
                iDateFin = 0;
            }
            else {
                iDateFin = int.Parse(dateFin);
            }

            EngagementPeriodeDto nouvellePeriode = new EngagementPeriodeDto {
                Code = code,
                DateDebut = iDateDeb,
                DateFin = iDateFin,
                EngagementTotal = engTotal,
                EngagementAlbingia = engAlbingia,
                Part = partAlbingia,
                Actif = actifStatut,
                Utilise = utiliseStatut,
                Action = typeOperation
            };
            var retour = code.ToString();

            if (!IsReadonly && !IsModifHorsAvenant) {
                if (nouvellePeriode.Action == "Update" || nouvellePeriode.Action == "Consult" || nouvellePeriode.Action == "Disable") {
                    //récupération de la clé et vérification de son existance dans le cache
                    string key = GetKeyFormules($"{codeOffre}{sep}{version}{sep}{type}{sep}{codeAvn}{sep}{code}");
                    dynamic tempPeriodeDto = null;
                    if (AlbSessionHelper.EngagementPeriodesUtilisateurs.TryGetValue(key, out tempPeriodeDto)) {
                        AlbSessionHelper.EngagementPeriodesUtilisateurs.Remove(key);
                        AlbSessionHelper.EngagementPeriodesUtilisateurs.Add(key, nouvellePeriode);
                    }
                }
                else if (nouvellePeriode.Action == "Insert") {
                    //récupère le code max dans le cache
                    string keyModel = $"{codeOffre}{sep}{version}{sep}{type}{sep}{codeAvn}";
                    string sCodeMax = AlbSessionHelper.EngagementPeriodesUtilisateurs.Where(elm => elm.Key.Contains(keyModel)).ToDictionary(p => p.Key, p => p.Value).Max(elm => elm.Key);
                    int iCodeMax = 0;
                    if (!string.IsNullOrEmpty(sCodeMax)) {
                        iCodeMax = int.Parse(sCodeMax.Split(new[] { MvcApplication.SPLIT_CONST_HTML }, StringSplitOptions.None)[sCodeMax.Split(new[] { MvcApplication.SPLIT_CONST_HTML }, StringSplitOptions.None).Length - 1]);
                    }
                    iCodeMax += 1;
                    string key = GetKeyFormules($"{codeOffre}{sep}{version}{sep}{type}{sep}{codeAvn}{sep}{iCodeMax}");

                    //Actualisation des valeurs dû au mode insertion
                    nouvellePeriode.Actif = "A";
                    nouvellePeriode.Utilise = "N";
                    nouvellePeriode.Code = iCodeMax;

                    AlbSessionHelper.EngagementPeriodesUtilisateurs.Add(key, nouvellePeriode);
                }
                else if (nouvellePeriode.Action == "Delete") {
                    //récupération de la clé et vérification de son existance dans le cache
                    string key = GetKeyFormules(
                                                 $"{codeOffre}{sep}{version}{sep}{type}{sep}{codeAvn}{sep}{code}"
                                                 );
                    dynamic tempPeriodeDto = null;
                    if (AlbSessionHelper.EngagementPeriodesUtilisateurs.TryGetValue(key, out tempPeriodeDto)) {
                        //si la ligne dans le cache est insert, cela veut dire que la ligne n'existe pas encore dans la bdd, on retire juste la ligne du cache
                        if (tempPeriodeDto.Action == "Insert")
                            AlbSessionHelper.EngagementPeriodesUtilisateurs.Remove(key);
                        else {
                            //effacage de l'objet dans le cache
                            AlbSessionHelper.EngagementPeriodesUtilisateurs.Remove(key);
                            //Ajout de la nouvelle période mise  à jour
                            AlbSessionHelper.EngagementPeriodesUtilisateurs.Add(key, nouvellePeriode);
                        }
                    }
                }
                //Enregistrement en BDD
                retour = SavePeriodeInDB(codeOffre, version, type, codeAvn, modeNavig, nouvellePeriode, false);
            }

            if (nouvellePeriode.Action == "Update" && utiliseStatut != "U" || nouvellePeriode.Action == "Consult" || nouvellePeriode.Action == "Insert") {
                var modeAccess = (!string.IsNullOrEmpty(accessMode) ? "accessMode" + accessMode + "accessMode" : string.Empty);
                if (utiliseStatut == "O" || IsReadonly) {
                    nouvellePeriode.Action = "Consult";
                }

                return RedirectToAction("Index", "Engagements", new {
                    id = codeOffre + "_" + version + "_" + type + "cdPeriode" + retour + "cdPeriode" + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) + modeAccess,
                    actionEngagement = nouvellePeriode.Action == "Insert" ? "CREATE" : nouvellePeriode.Action
                });
            }

            //Reinitialisation du cache
            string keyFormules = GetKeyFormules($"{codeOffre}{sep}{version}{sep}{type}{sep}{codeAvn}");
            InitCachePeriodesEngagement(keyFormules);
            LoadEngagementPeriodes(codeOffre, version, type, codeAvn, modeNavig);

            // réalimentation des dates du contrat

            DateTime? dtDebContrat = AlbConvert.ConvertIntToDate(Convert.ToInt32(dateDebContrat));
            DateTime? dtFinContrat = AlbConvert.ConvertIntToDate(Convert.ToInt32(dateFinContrat));

            model.Contrat = new ContratDto {
                DateEffetAnnee = dtDebContrat.HasValue ? Convert.ToInt16(dtDebContrat.Value.Year) : Convert.ToInt16(0),
                DateEffetMois = dtDebContrat.HasValue ? Convert.ToInt16(dtDebContrat.Value.Month) : Convert.ToInt16(0),
                DateEffetJour = dtDebContrat.HasValue ? Convert.ToInt16(dtDebContrat.Value.Day) : Convert.ToInt16(0),

                FinEffetAnnee = dtFinContrat.HasValue ? Convert.ToInt16(dtFinContrat.Value.Year) : Convert.ToInt16(0),
                FinEffetMois = dtFinContrat.HasValue ? Convert.ToInt16(dtFinContrat.Value.Month) : Convert.ToInt16(0),
                FinEffetJour = dtFinContrat.HasValue ? Convert.ToInt16(dtFinContrat.Value.Day) : Convert.ToInt16(0),
            };

            // fin réalimentation des dates du contrat

            return PartialView("ListeEngagementPeriodes", model);
        }

        #region Méthodes Privées

        protected override void LoadInfoPage(string id) {
            string branche = string.Empty;
            string cible = string.Empty;

            string[] tId = id.Split('_');
            if (tId[2] == "P") {
                using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>()) {
                    var CommonOffreClient = chan.Channel;
                    var infosBase = CommonOffreClient.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig);
                    Model.Contrat = new ContratDto() {
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
                        Delegation = infosBase?.CabinetGestionnaire?.Delegation?.Nom,
                        Inspecteur = infosBase?.CabinetGestionnaire?.Inspecteur,
                        DateEffetAnnee = infosBase.DateEffetAnnee,
                        DateEffetMois = infosBase.DateEffetMois,
                        DateEffetJour = infosBase.DateEffetJour,
                        FinEffetAnnee = infosBase.FinEffetAnnee,
                        FinEffetMois = infosBase.FinEffetMois,
                        FinEffetJour = infosBase.FinEffetJour
                    };
                }
                var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                switch (typeAvt) {
                    case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                        break;
                    default:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                        break;
                }
            }
            else {
                throw new AlbFoncException("Cet écran n'est accessible que pour les contrats", trace: false, sendMail: false, onlyMessage: true);
            }

            if (model.Contrat != null) {
                model.AfficherBandeau = base.DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
                SetList();
                branche = model.Contrat.Branche;
                cible = model.Contrat.Cible;
            }

            SetArbreNavigation();
            model.Bandeau = null;
            SetBandeauNavigation(id);

            string sep = MvcApplication.SPLIT_CONST_HTML;
            string keyFormules = GetKeyFormules($"{tId[0]}{sep}{tId[1]}{sep}{tId[2]}{sep}{model.NumAvenantPage}");

            InitCachePeriodesEngagement(keyFormules);
            LoadEngagementPeriodes(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig);

            GetDateControle(branche, cible);

            Model.IsModifHorsAvenant = IsModifHorsAvenant;
        }
        private void GetDateControle(string branche, string cible) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                var finOffreClient = client.Channel;
                //récupération de la date de controle
                string sDateControle = finOffreClient.GetDateControle(branche, cible);
                int iDateControle;
                DateTime? dDateControle = null;
                if (int.TryParse(sDateControle, out iDateControle)) {
                    dDateControle = AlbConvert.ConvertIntToDate(iDateControle);
                }

                if (dDateControle != null) {
                    model.DateControle = iDateControle;
                }
            }
        }
        private void LoadEngagementPeriodes(string codeOffre, string version, string type, string codeAvn, string modeNavig) {
            string sep = MvcApplication.SPLIT_CONST_HTML;

            string keyFormules = GetKeyFormules($"{codeOffre}{sep}{version}{sep}{type}{sep}{codeAvn}");
            //Filtrage des éléments qui ont été effacés par l'utilisateur (mais pas encore appliqué en bdd)
            var lstPeriodesFiltered = GetPeriodesEngagementFromCache(keyFormules, modeNavig).Where(elm => elm.Action != "Delete").OrderBy(elm => elm.DateDebut).ToList();
            model.EngagementPeriodes = ObjectMapperManager.DefaultInstance.GetMapper<List<EngagementPeriodeDto>, List<ModeleEngagementPeriode>>().Map(lstPeriodesFiltered);

            if (model.EngagementPeriodes != null && model.EngagementPeriodes.Count > 0) {
                model.FinDernierePeriode = model.EngagementPeriodes.Max(elm => elm.DateFin);
                model.DebutDernierePeriode = model.EngagementPeriodes.FirstOrDefault(elm => elm.DateFin == model.FinDernierePeriode).DateDebut;
                model.CodePremierePeriode = int.Parse(model.EngagementPeriodes.First().Code.ToString());
                model.CodeDernierePeriode = int.Parse(model.EngagementPeriodes.Last().Code.ToString());
            }
        }
        private void MiseAZeroDesBornes() {
            var lstPeriode = AlbSessionHelper.EngagementPeriodesUtilisateurs.OrderBy(elm => elm.Value.DateDebut).ToList();
            if (lstPeriode.Count > 0) {
                if (lstPeriode.Exists(elm => elm.Value.Actif == "A")) {
                    long iCodeFirst = lstPeriode.First(elm => elm.Value.Actif == "A").Value.Code;
                    long iCodeLast = lstPeriode.Last(elm => elm.Value.Actif == "A").Value.Code;

                    AlbSessionHelper.EngagementPeriodesUtilisateurs.First(elm => elm.Value.Code == iCodeFirst).Value.DateDebut = 0;
                    if (AlbSessionHelper.EngagementPeriodesUtilisateurs.First(elm => elm.Value.Code == iCodeFirst).Value.Action != "Insert")
                        AlbSessionHelper.EngagementPeriodesUtilisateurs.First(elm => elm.Value.Code == iCodeFirst).Value.Action = "Update";

                    AlbSessionHelper.EngagementPeriodesUtilisateurs.First(elm => elm.Value.Code == iCodeLast).Value.DateFin = 0;
                    if (AlbSessionHelper.EngagementPeriodesUtilisateurs.First(elm => elm.Value.Code == iCodeLast).Value.Action != "Insert")
                        AlbSessionHelper.EngagementPeriodesUtilisateurs.First(elm => elm.Value.Code == iCodeLast).Value.Action = "Update";
                }
            }
        }

        #region vérification des données avant insertion
        private bool CheckData(string codeOffre, string version, string type, int code, int dateDeb, int dateFin) {
            bool toReturn = true;
            //Vu avec FDU, désactivation des contrôles ligne par ligne
            //////Récupération du contrat
            //using (var serviceContext = new AffaireNouvelleClient())
            //{
            //    model.Contrat = serviceContext.GetContrat(codeOffre, version, type, model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
            //}
            ////Chargement des périodes d'engagement
            //LoadEngagementPeriodes(codeOffre, version, type);
            //if (code != model.CodePremierePeriode && dateDeb != 0)
            //    toReturn = CheckStartDate(dateDeb);
            //if (toReturn)
            //    toReturn = CheckEndDate(code, dateDeb, dateFin);
            //if (toReturn)
            //    toReturn = CheckEmpietement(code, dateDeb, dateFin);
            return toReturn;
        }

        private bool CheckStartDate(int dateDeb) {
            bool toReturn = true;
            //Vérification si une date est présente
            if (dateDeb < 20000000)
                return false;

            //Vérification du format de la date
            DateTime? dDate = AlbConvert.ConvertIntToDate(dateDeb);
            if (dDate == null)
                return false;

            //Comparaison à la date d'effet du contrat
            string sDateEffetContrat = model.Contrat.DateEffetAnnee.ToString() + (model.Contrat.DateEffetMois < 10 ? "0" + model.Contrat.DateEffetMois.ToString() : model.Contrat.DateEffetMois.ToString()) + (model.Contrat.DateEffetJour < 10 ? "0" + model.Contrat.DateEffetJour.ToString() : model.Contrat.DateEffetJour.ToString());
            int iDateEffetContrat = 0;
            if (int.TryParse(sDateEffetContrat, out iDateEffetContrat)) {
                if (dateDeb < iDateEffetContrat)
                    toReturn = false;
            }
            else
                toReturn = false;

            return toReturn;
        }

        private bool CheckEndDate(int code, int dateDeb, int dateFin) {
            bool toReturn = true;

            // Verification de l'obligation de la date de fin ou non
            bool isMandatory = true;

            if (code != model.CodePremierePeriode) {
                if (dateDeb >= model.DebutDernierePeriode)
                    isMandatory = false;
            }

            if (code == model.CodeDernierePeriode) {
                isMandatory = false;
            }

            // Vérification de la validité du format
            DateTime? dDate = AlbConvert.ConvertIntToDate(dateFin);
            if ((dDate == null) && (isMandatory == false))
                return true;
            else if ((dDate == null) && (isMandatory == true))
                return false;

            // Comparaison à la date d'effet du contrat si elle est renseignée
            string sDateFinEffetContrat = model.Contrat.FinEffetAnnee.ToString() + (model.Contrat.FinEffetMois < 10 ? "0" + model.Contrat.FinEffetMois.ToString() : model.Contrat.FinEffetMois.ToString()) + (model.Contrat.FinEffetJour < 10 ? "0" + model.Contrat.FinEffetJour.ToString() : model.Contrat.FinEffetJour.ToString());
            if (sDateFinEffetContrat != "00000") {
                int iDateFinEffetContrat = 0;
                if (int.TryParse(sDateFinEffetContrat, out iDateFinEffetContrat)) {
                    if (dateFin > iDateFinEffetContrat)
                        toReturn = false;
                }
                else
                    toReturn = false;
            }
            return toReturn;
        }

        private bool CheckEmpietement(int code, int dateDeb, int dateFin) {
            bool toReturn = true;
            if (model.EngagementPeriodes != null && model.EngagementPeriodes.Count > 0) {
                var periodesActives = model.EngagementPeriodes.FindAll(elm => elm.Actif == "A");
                foreach (ModeleEngagementPeriode periode in periodesActives) {
                    if ((periode.Code != code) && (periode.Code != model.CodePremierePeriode) && (periode.Code != model.CodeDernierePeriode) && (periode.DateDebut != 0) && (periode.DateFin != 0)) {
                        if (dateDeb == periode.DateDebut || dateDeb == periode.DateFin || dateFin == periode.DateDebut || dateFin == periode.DateFin)
                            return false;

                        if (dateDeb < periode.DateDebut && dateFin > periode.DateFin)
                            return false;

                        if (dateDeb > periode.DateDebut && dateDeb < periode.DateFin)
                            return false;

                        if (dateDeb > periode.DateDebut && dateFin < periode.DateFin)
                            return false;
                    }
                }
            }
            return toReturn;
        }

        #endregion
        #region vérification des données avant redirection

        private bool CheckPeriodesDiscontinues(string codeContrat, string versionContrat, string type, string codeAvn) {
            bool toReturn = true;
            //Récupération du contrat
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                var serviceContext = client.Channel;
                model.Contrat = serviceContext.GetContrat(codeContrat, versionContrat, type, model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
            }
            if (model.Contrat == null)
                toReturn = false;
            if (toReturn) {
                MiseAZeroDesBornes();
                LoadEngagementPeriodes(codeContrat, versionContrat, type, codeAvn, model.ModeNavig);
                GetDateControle(model.Contrat.Branche, model.Contrat.Cible);
                string sDateCreationContrat = model.Contrat.DateCreationAnnee.ToString() + (model.Contrat.DateCreationMois < 10 ? "0" + model.Contrat.DateCreationMois.ToString() : model.Contrat.DateCreationMois.ToString()) + (model.Contrat.DateCreationJour < 10 ? "0" + model.Contrat.DateCreationJour.ToString() : model.Contrat.DateCreationJour.ToString());
                int iDateCreationContrat = int.Parse(sDateCreationContrat);
                if (model.EngagementPeriodes != null && model.EngagementPeriodes.Count > 0) {
                    int datePrecedente = 1;
                    var periodesActives = model.EngagementPeriodes.FindAll(elm => elm.Actif == "A");
                    foreach (ModeleEngagementPeriode periode in periodesActives) {
                        if (datePrecedente != 1) {
                            if ((AlbConvert.ConvertIntToDate(periode.DateDebut).HasValue && AlbConvert.ConvertIntToDate(datePrecedente).HasValue) && AlbConvert.ConvertIntToDate(periode.DateDebut) != AlbConvert.ConvertIntToDate(datePrecedente).Value.AddDays(1)) {
                                if (iDateCreationContrat >= model.DateControle) {
                                    throw new AlbFoncException("Il ne peut y avoir de périodes discontinues", trace: false, sendMail: false, onlyMessage: true);
                                }
                            }
                            if (datePrecedente == 0)
                                throw new AlbFoncException("Il ne peut y avoir de périodes discontinues", trace: false, sendMail: false, onlyMessage: true);
                        }
                        datePrecedente = periode.DateFin;
                    }
                }

            }

            return toReturn;
        }

        #endregion

        /// <summary>
        /// Applique le contenu du cache en bdd (après vérification préalable de l'integrité des données)
        /// </summary>
        /// <returns></returns>
        private bool SaveCacheInDB(string codeOffre, string version, string type, string codeAvn, string modeNavig, bool updateTableReass) {
            bool toReturn = true;

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                var finOffreClient = client.Channel;
                string keyFormules = GetKeyFormules(
                                                   string.Format("{0}{4}{1}{4}{2}{4}{3}", codeOffre, version, type, codeAvn, MvcApplication.SPLIT_CONST_HTML)
                                                   );
                foreach (EngagementPeriodeDto periode in GetPeriodesEngagementFromCache(keyFormules, modeNavig)) {
                    string ret = string.Empty;
                    if (toReturn) {
                        ret = SavePeriodeInDB(codeOffre, version, type, codeAvn, modeNavig, periode, updateTableReass);
                        //gestion des erreurs
                        switch (ret) {
                            case "LoadAS400Engagement":
                                toReturn = false;
                                throw new AlbTechException(new Exception("Erreur lors de l'exécution du programme 400 KA010CL (engagement)"));
                            case "0":
                                toReturn = false;
                                throw new AlbTechException(new Exception("Aucune ligne mise à jour"));
                            default: break;
                        }
                    }
                }
            }

            if (toReturn) {
                //Vidage du cache
                AlbSessionHelper.EngagementPeriodesUtilisateurs = null;
            }
            return toReturn;
        }

        private void SetList() {
            //modification document KP-SPECS-Engagements-V2.docx
            //var listeModesActif = new List<AlbSelectListItem>();
            //AlbSelectListItem modeActif = null;
            //modeActif = new AlbSelectListItem { Value = "I", Text = "I", Selected = false, Title = "Inactif" };
            //listeModesActif.Add(modeActif);
            //modeActif = new AlbSelectListItem { Value = "A", Text = "A", Selected = false, Title = "Actif" };
            //listeModesActif.Add(modeActif);
            //model.ListeModesActif = listeModesActif;

            var listeModesActif = new List<AlbSelectListItem>();
            AlbSelectListItem modeActif = null;
            modeActif = new AlbSelectListItem { Value = "A", Text = "Actif", Selected = true, Title = "Actif" };
            listeModesActif.Add(modeActif);
            modeActif = new AlbSelectListItem { Value = "", Text = "Tous", Selected = false, Title = "Tous" };
            listeModesActif.Add(modeActif);
            model.ListeModesActif = listeModesActif;

            var listeModesUtilise = new List<AlbSelectListItem>();
            AlbSelectListItem modeUtilise = null;
            modeUtilise = new AlbSelectListItem { Value = "O", Text = "O", Selected = false, Title = "Oui" };
            listeModesUtilise.Add(modeActif);
            modeUtilise = new AlbSelectListItem { Value = "N", Text = "N", Selected = false, Title = "Non" };
            listeModesUtilise.Add(modeUtilise);
            model.ListeModesUtilise = listeModesUtilise;
        }

        private void SetBandeauNavigation(string id) {
            if (model.AfficherBandeau) {
                model.Bandeau = GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
                //Gestion des Etapes
                if (model.Contrat != null) {
                    string typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    switch (typeAvt) {
                        case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
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
                    model.Navigation = new Navigation_MetaModel {
                        Etape = Navigation_MetaModel.ECRAN_ENGAGEMENTS,
                        IdOffre = model.Contrat.CodeContrat,
                        Version = int.Parse(model.Contrat.VersionContrat.ToString())
                    };
                }
            }
        }
        private void SetArbreNavigation() {
            if (model.Contrat != null) {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Engagement");
            }
        }

        private string SavePeriodeInDB(string codeOffre, string version, string type, string codeAvn, string modeNavig, EngagementPeriodeDto periode, bool updateTableReass) {
            string ret = string.Empty;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                var finOffreClient = client.Channel;
                switch (periode.Action) {
                    case "Insert":
                        ret = finOffreClient.SaveEngagementPeriode(codeOffre, version, type, codeAvn, GetUser(), periode, "I", modeNavig.ParseCode<ModeConsultation>(), updateTableReass);
                        break;
                    case "Update":
                    case "Consult":
                    case "Disable":
                        ret = finOffreClient.SaveEngagementPeriode(codeOffre, version, type, codeAvn, GetUser(), periode, "U", modeNavig.ParseCode<ModeConsultation>(), updateTableReass);
                        break;
                    case "Delete":
                        ret = finOffreClient.SaveEngagementPeriode(codeOffre, version, type, codeAvn, GetUser(), periode, "D", modeNavig.ParseCode<ModeConsultation>(), updateTableReass);
                        break;
                    default:
                        ret = periode.Code.ToString();
                        break;
                }
            }
            return ret;
        }


        #endregion

        #region Gestion du cache

        private void InitCachePeriodesEngagement(string key) {
            var tempCollec = new List<string>();
            foreach (KeyValuePair<string, dynamic> item in AlbSessionHelper.EngagementPeriodesUtilisateurs) {
                if (item.Key.Contains(key)) {
                    tempCollec.Add(item.Key);
                }
            }
            foreach (string elm in tempCollec) {
                AlbSessionHelper.EngagementPeriodesUtilisateurs.Remove(elm);
            }
        }

        private List<EngagementPeriodeDto> GetPeriodesEngagementFromCache(string key, string modeNavig) {
            List<EngagementPeriodeDto> toReturn = new List<EngagementPeriodeDto>();

            foreach (KeyValuePair<string, dynamic> item in AlbSessionHelper.EngagementPeriodesUtilisateurs) {
                if (item.Key.Contains(key)) {
                    toReturn.Add(item.Value);
                }
            }
            if (toReturn.Count == 0) {
                toReturn = GetPeriodesEngagementFromDb(key, GetUser(), modeNavig);

                //Initialisation de la variable de session
                foreach (EngagementPeriodeDto elm in toReturn) {
                    string keyElm = string.Format("{0}{2}{1}", key, elm.Code, MvcApplication.SPLIT_CONST_HTML);
                    AlbSessionHelper.EngagementPeriodesUtilisateurs.Add(keyElm, elm);
                }
            }
            return toReturn;
        }

        private static List<EngagementPeriodeDto> GetPeriodesEngagementFromDb(string keyConditions, string user, string modeNavig) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                var finOffreClient = client.Channel;
                string[] tKeyCondition = keyConditions.Split(new[] { MvcApplication.SPLIT_CONST_HTML }, StringSplitOptions.None);
                return finOffreClient.GetEngagementPeriodes(tKeyCondition[1], tKeyCondition[2], tKeyCondition[3], tKeyCondition[4], user, modeNavig.ParseCode<ModeConsultation>());
            }
        }

        private string GetKeyFormules(string suffixeKey) {
            return GetUser() + MvcApplication.SPLIT_CONST_HTML + suffixeKey;
        }

        #endregion


        #region Méthodes Accès Recherche

        [ErrorHandler]
        public string CloseEngPer(string codeContrat, string version, string type, string codeAvn, string modeNavig) {
            if (CheckPeriodesDiscontinues(codeContrat, version, type, codeAvn)) {
                if (model.EngagementPeriodes.Count == 0) {
                    throw new AlbFoncException("Il doit exister au moins une période d'engagement", trace: false, sendMail: false, onlyMessage: true);
                }
                if (SaveCacheInDB(codeContrat, version, type, codeAvn, modeNavig, true)) {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        #endregion
    }
}
