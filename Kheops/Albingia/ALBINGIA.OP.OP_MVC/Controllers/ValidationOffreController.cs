using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Models;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesValidationOffre;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Partenaire;
using OP.WSAS400.DTO.Regularisation;
using OP.WSAS400.DTO.Validation;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ICommon;
using OPServiceContract.ISaisieCreationOffre;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class ValidationOffreController : ControllersBase<ModeleValidationOffrePage>
    {
        #region Méthodes Publiques


        [ErrorHandler]
        public ActionResult Index(string id)
        {
            id = InitializeParams(id);
            LoadInfoPage(id);
            return View(model);
        }

        [ErrorHandler]
        public ActionResult OpenDetail(string niveau, string modeNavig)
        {
            List<ModeleDetailNiveau> model = new List<ModeleDetailNiveau>();
            ModeleDetailNiveau dtl = new ModeleDetailNiveau { Titre = "M.", Nom = "DUBOIS", Prenom = "Jean" };
            model.Add(dtl);
            dtl = new ModeleDetailNiveau { Titre = "Mlle", Nom = "DUPONT", Prenom = "Christine" };
            model.Add(dtl);
            dtl = new ModeleDetailNiveau { Titre = "Mme", Nom = "PETIT", Prenom = "Pierre" };
            model.Add(dtl);
            return PartialView("ValidationDetailNiveau", model);
        }

        [ErrorHandler]
        public void ValidSupprDoc(string selectDoc, string unselectDoc)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var Context = client.Channel;
                Context.ValidSupprDoc(selectDoc, unselectDoc);
            }
        }

        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string tabGuid, string saveCancel, string paramRedirect, string modeNavig, string addParamType, string addParamValue)
        {
            if (paramRedirect.ContainsChars())
            {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }

            if (saveCancel == "1")
            {
                return RedirectToAction("Index", "RechercheSaisie");
            }

            return RedirectToAction(job, cible, new
            {
                id = AlbParameters.BuildStandardId(new Folder(new[] { codeOffre, version, type }), tabGuid, addParamValue, modeNavig),
                returnHome = saveCancel,
                guidTab = tabGuid
            });
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult ValiderOffreContrat(string codeOffre, string version, string type, string avenant, string acteGestion, string tabGuid, string codeObservation,
            string observation, string mode, string validable, string complet, string motif, string lotsId, string modeNavig, string addParamValue, string loadParamOffre = "loadParam")
        {
            string error;
            var folder = string.Format("{0}_{1}_{2}", codeOffre, version, type);
            var isReadOnly = GetIsReadOnly(tabGuid, folder, avenant);
            var isModifHorsAvn = GetIsModifHorsAvn(tabGuid, string.Format("{0}_{1}", folder, string.IsNullOrEmpty(avenant) ? "0" : avenant));
            var readOnly = isReadOnly && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>();

            if ((!readOnly || isModifHorsAvn) && acteGestion != AlbConstantesMetiers.TYPE_ATTESTATION)
            {
                if (!string.IsNullOrEmpty(observation))
                    observation = observation.Replace("\r\n", "<br>").Replace("\n", "<br>");
                else
                    observation = string.Empty;
                SaveObservation(codeOffre, version, type, codeObservation, observation, acteGestion, GetAddParamValue(addParamValue, AlbParameterName.REGULEID));
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                {
                    var Context = client.Channel;
                    error = Context.ValiderOffre(codeOffre, version, type, avenant, acteGestion, validable, complet, motif, mode, lotsId, GetUser(), GetAddParamValue(addParamValue, AlbParameterName.REGULEID), isModifHorsAvn);
                    if (!string.IsNullOrEmpty(error))
                        throw new AlbFoncException(error);

                }
            }
            else
            {
                var machineName = AlbNetworkInfo.GetMachineName(Request);
                var ip = Request.UserHostAddress;
#if DEBUG
                ip = AlbNetworkInfo.GetIpMachine();
#endif

                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                {
                    var Context = client.Channel;
                    var attesId = GetAddParamValue(addParamValue, AlbParameterName.ATTESTID);
                    error = Context.EditerDocParLot(codeOffre, version, type, avenant, mode, lotsId, GetUser(), readOnly, acteGestion, attesId, machineName, IsModuleGestDocOpen() ? 1 : 0);
                    if (!string.IsNullOrEmpty(error))
                        throw new AlbFoncException(error);
                }
            }
            CommonVerouillage.DeverrouilleFolder(codeOffre, version, type, avenant, tabGuid, readOnly, isReadOnly, isModifHorsAvn);
            return RedirectToAction("Index", "RechercheSaisie", new
            {
                id = AlbParameters.BuildFullId(
                    new Folder(new[] { codeOffre, version, type }),
                    loadParamOffre.IsEmptyOrNull() ? null : new[] { loadParamOffre },
                    tabGuid, null, null)
            });
        }

        [AlbAjaxRedirect]
        public RedirectToRouteResult RetourAccueil(string codeOffre, string version, string type, string codeAvn, string tabGuid, string codeObservation, string observation, string isComplet, string motif, string isValidable, string saveCancel, string paramRedirect, string acteGestion, string addParamValue, string loadParamOffre = "loadParam")
        {
            var folder = codeOffre + "_" + version + "_" + type;
            var isReadOnly = GetIsReadOnly(tabGuid, folder, codeAvn);
            if (!isReadOnly)
            {
                observation = !string.IsNullOrEmpty(observation) ? observation.Replace("\r\n", "<br>").Replace("\n", "<br>") : string.Empty;
                SaveObservation(codeOffre, version, type, codeObservation, observation, acteGestion, GetAddParamValue(addParamValue, AlbParameterName.REGULEID));
            }

            if (saveCancel == "1")
            {
                FolderController.DeverrouillerAffaire(tabGuid);
                return RedirectToAction("Index", "RechercheSaisie");
            }

            if (!string.IsNullOrWhiteSpace(paramRedirect))
            {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }

            FolderController.DeverrouillerAffaire(tabGuid);
            return RedirectToAction("Index", "RechercheSaisie", new
            {
                id = AlbParameters.BuildFullId(
                new Folder(new[] { codeOffre, version, type }),
                loadParamOffre.IsEmptyOrNull() ? null : new[] { loadParamOffre },
                tabGuid, null, null)
            });
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public ActionResult LoadDocumentsEditables(string codeOffre, string version, string type, string codeAvn, string modeEcran, string modeNavig, string acteGestion, string saveCancel, string paramRedirect, string tabGuid, string addParam)
        {
            bool isBNS = addParam.IsEmptyOrNull() ? false : GetAddParamValue(addParam, AlbParameterName.REGULMOD) == "BNS";

            if (!string.IsNullOrEmpty(paramRedirect))
            {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }

            if (saveCancel == "1")
                return RedirectToAction("Index", "RechercheSaisie");

            ModeleEditionDocuments toReturn = null;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var Context = client.Channel;
                if (modeEcran == "valider")
                {
                    var isValidable = Context.CheckValidateOffre(codeOffre, version, type, codeAvn, model.ModeNavig.ParseCode<ModeConsultation>(), acteGestion);
                    if (!isValidable && !GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn))
                        throw new AlbFoncException("La validation n'est pas permise.");
                }

                var result = Context.GetValidationEdition(codeOffre, version, type, codeAvn, model.ModeNavig.ParseCode<ModeConsultation>(), acteGestion, modeEcran, isBNS);
                if (result != null)
                {
                    toReturn = (ModeleEditionDocuments)result;
                    //toReturn.ListeDocuments.ForEach(elm => elm.ListDocInfos.ForEach(elm.
                    toReturn.ModeEcran = modeEcran;
                    toReturn.CodeAffaire = codeOffre;
                    toReturn.VersionAffaire = version;
                    toReturn.TypeAffaire = type;

                }
            }
            if (toReturn == null)
                throw new AlbFoncException("Erreur lors de la récupération des documents éditables");
            toReturn.ActeGestion = acteGestion;
            return PartialView("EditionValidationDocuments", toReturn);
        }

        [ErrorHandler]
        public ActionResult RefreshListDocEdit(string codeOffre, string version, string type, string codeAvn, string modeNavig, string acteGestion, string modeEcran)
        {
            ModeleEditionDocuments toReturn = null;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var Context = client.Channel;
                var result = Context.GetValidationEdition(codeOffre, version, type, codeAvn, model.ModeNavig.ParseCode<ModeConsultation>(), acteGestion, modeEcran, false);
                if (result != null)
                {
                    toReturn = (ModeleEditionDocuments)result;
                    toReturn.ModeEcran = modeEcran;
                    toReturn.CodeAffaire = codeOffre;
                    toReturn.VersionAffaire = version;
                    toReturn.TypeAffaire = type;

                }
            }
            if (toReturn == null)
                throw new AlbFoncException("Erreur lors de la récupération des documents éditables");

            return PartialView("/Views/DocumentGestion/LstDocuments.cshtml", toReturn.ListeDocuments);
        }

        [HandleJsonError]
        [ErrorHandler]
        public JsonResult VerifPartenaires(string codeOffre, string version, string type, string codeAvn, string modeNavig)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var Context = client.Channel;
                var result = Context.VerifCourtier(codeOffre, version, type);
                if (!string.IsNullOrEmpty(result))
                {
                    throw new AlbFoncException(result);
                }
            }
            if (CanUseLablat())
            {
                PartenairesDto partenaires = null;
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    partenaires = client.Channel.GetListPartenaires(codeOffre, version, type, codeAvn, modeNavig.ParseCode<ModeConsultation>());

                    if (partenaires != null)
                    {
                        ////T 3997 : Vérification des partenaires
                        //VerificationPartenaires(partenaires);
                    }
                }
            }
            return Json(new { success = true });
        }

        #endregion

        #region Méthodes Privées

        protected override void LoadInfoPage(string id)
        {
            string[] tId = id.Split('_');
            var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
            switch (tId[2])
            {
                case "O":
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                    {
                        var Context = client.Channel;
                        model.Offre = new Offre_MetaModel();
                        model.Offre.LoadInfosOffre(Context.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig));
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    }
                    break;
                case "P":
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                    {
                        var Context = client.Channel;
                        var infosBase = Context.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig);
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
                            Etat = infosBase.Etat,
                            Delegation = infosBase?.CabinetGestionnaire?.Delegation?.Nom,
                            Inspecteur = infosBase?.CabinetGestionnaire?.Inspecteur
                        };
                    }
                    var regulMode = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULMOD).ToUpper();
                    switch (typeAvt)
                    {
                        case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                            break;
                        case AlbConstantesMetiers.TYPE_ATTESTATION:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_ATTES;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                            if (regulMode == "PB")
                            {
                                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULPB;
                            }
                            else if (regulMode == "BNS")
                            {
                                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULBNS;
                            }
                            else if (regulMode == "BURNER")
                            {
                                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULBURNER;
                            }
                            else
                            {
                                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                            }
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                            break;
                        default:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                            break;
                    }
                    break;
            }
            model.PageTitle = typeAvt == AlbConstantesMetiers.TYPE_ATTESTATION ? "Edition" : "Validation";

            if (model.Offre != null || model.Contrat != null)
            {
                model.AfficherBandeau = DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
            }
            var folder = string.Format("{0}_{1}_{2}", tId[0], tId[1], tId[2]);
            model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), folder, model.NumAvenantPage);
            //model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), folder, tId[3]);
            model.IsModifHorsAvenant = GetIsModifHorsAvn(GetSurroundedTabGuid(model.TabGuid), string.Format("{0}_{1}", folder, model.NumAvenantPage));

            if (tId[2] == "O")
                model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
            else
                model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);

            model.Bandeau = null;
            SetBandeauNavigation(id);
            SetArbreNavigation();

            LoadDataValidation(model.NumAvenantPage);
        }

        protected override void ExtendNavigationArbreRegule(MetaModelsBase contentData)
        {
            RegularisationNavigator.StandardInitContext(model, RegularisationStep.Cotisations);
            if (model?.Context != null)
            {
                RegularisationNavigator.Initialize(contentData.NavigationArbre, model.Context);
            }
        }
        private void LoadDataValidation(string numAvn)
        {
            ModeleValidationOffrePage model = new ModeleValidationOffrePage();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var Context = client.Channel;
                var reguleId = GetAddParamValue(base.model.AddParamValue, AlbParameterName.REGULEID);
                var reguleMode = GetAddParamValue(base.Model.AddParamValue, AlbParameterName.REGULMOD);
                ValidationDto result = null;
                if (base.model.Offre != null)
                    result = Context.InitValidationOffre(base.model.Offre.CodeOffre, base.model.Offre.Version.Value.ToString(), base.model.Offre.Type, string.Empty, base.model.IsReadOnly, base.model.ModeNavig.ParseCode<ModeConsultation>(), GetUser(), base.model.ActeGestion, string.Empty, string.Empty);
                else if (base.model.Contrat != null)
                    result = Context.InitValidationOffre(base.model.Contrat.CodeContrat, base.model.Contrat.VersionContrat.ToString(), base.model.Contrat.Type, numAvn, base.model.IsReadOnly, base.model.ModeNavig.ParseCode<ModeConsultation>(), GetUser(), base.model.ActeGestion, reguleId, reguleMode);
                if (result != null)
                    model = ((ModeleValidationOffrePage)result);
                if (base.model.Contrat != null)
                {
                    var resEng = Context.InitEngagement(base.model.Contrat.CodeContrat, base.model.Contrat.VersionContrat.ToString(), base.model.Contrat.Type, numAvn, string.Empty, base.model.ModeNavig.ParseCode<ModeConsultation>(), base.model.IsReadOnly, true, GetUser(), base.model.ActeGestion, string.Empty, "valider");
                    if (resEng != null)
                    {
                        var modeleEng = ((ModeleEngagementsPage)resEng);
                        model.Traites = modeleEng.Traites;
                    }
                }

                if (model.Docs != null)
                {
                    model.Docs.ModeEcran = "valider";
                    model.Docs.CodeAffaire = base.model.Offre == null ? base.model.Contrat.CodeContrat : base.model.Offre.CodeOffre;
                    model.Docs.VersionAffaire = base.model.Offre == null ? base.model.Contrat.VersionContrat.ToString() : base.model.Offre.Version.Value.ToString();
                    model.Docs.TypeAffaire = base.model.Offre == null ? base.model.Contrat.Type : base.model.Offre.Type;
                    model.Docs.ActeGestion = base.model.ActeGestion;
                    model.Docs.RegulMode = GetAddParamValue(base.model.AddParamValue, AlbParameterName.REGULMOD).ToUpper();
                }

                base.model.OffreComplete = model.OffreComplete;
                base.model.Docs = model.Docs;
                List<AlbSelectListItem> completes = new List<AlbSelectListItem>();
                AlbSelectListItem complete = new AlbSelectListItem { Value = "Oui", Text = "Oui", Selected = false, Title = "Oui" };
                completes.Add(complete);
                complete = new AlbSelectListItem { Value = "Non", Text = "Non", Selected = false, Title = "Non" };
                completes.Add(complete);
                base.model.OffreCompletes = completes;
                base.model.Motif = model.Motif;
                List<AlbSelectListItem> motifs = result.Motifs.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                if (!string.IsNullOrEmpty(base.model.Motif))
                {
                    var sItem = motifs.FirstOrDefault(x => x.Value == base.model.Motif);
                    if (sItem != null)
                        sItem.Selected = true;
                }
                base.model.Motifs = motifs;
                base.model.ValidationRequise = model.ValidationRequise;
                base.model.Criteres = model.Criteres;

                //Chargement des info complementaires offre
                if (base.model.Offre != null)
                {
                    base.model.CodeObservation = model.CodeObservation;
                    base.model.Observation = model.Observation;
                    base.model.DelegationOffre = model.DelegationOffre;
                    base.model.SecteurOffre = model.SecteurOffre;
                    base.model.MontantReference = model.MontantReference;
                    base.model.Validable = base.model.Offre.Etat == "N" ? model.OffreComplete == "Oui" && model.IsControleOk ? "Oui" : "Non" : base.model.Offre.Etat == "A" && base.model.OffreComplete == "Oui" ? "Oui" : "Non";
                    base.model.DelegationApporteur = model.DelegationApporteur;
                    base.model.SecteurApporteur = model.SecteurApporteur;
                    base.model.DelegationGestionnaire = model.DelegationGestionnaire;
                    base.model.SecteurGestionnaire = model.SecteurGestionnaire;
                    base.model.EtatDossier = result.Etat;
                    base.model.IsControleOk = model.IsControleOk;
                    base.model.IsDocEdit = model.IsDocEdit;
                    base.model.IsDocGener = model.IsDocGener;
                    base.model.DateAccord = AlbConvert.ConvertIntToDate(Convert.ToInt32(model.DateAccordInt));

                    //model.IsReadOnly = model.IsReadOnly;
                }
                else if (base.model.Contrat != null)
                {
                    base.model.CodeObservation = model.CodeObservation;
                    base.model.Observation = model.Observation;

                    if (base.model.ActeGestion == AlbConstantesMetiers.TYPE_AVENANT_REGUL || base.model.ActeGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF)
                    {
                        base.model.Validable = (model.EtatRegule == "N" && model.IsControleOk) || model.EtatRegule == "A" ? "Oui" : "Non";
                        base.model.EtatDossier = model.EtatRegule;
                    }
                    else
                    {
                        base.model.Validable = base.model.Contrat.Etat == "N" ? model.OffreComplete == "Oui" && model.IsControleOk ? "Oui" : "Non" : base.model.Contrat.Etat == "A" && base.model.OffreComplete == "Oui" ? "Oui" : "Non";
                        base.model.EtatDossier = result.Etat;
                    }
                    base.model.Validable = base.model.IsModifHorsAvenant ? "Oui" : base.model.Validable;
                    //model.Validable = model.Contrat.Etat == "N" ? model.OffreComplete == "Oui" && model.IsControleOk ? "Oui" : "Non" : model.Contrat.Etat == "A" && model.OffreComplete == "Oui" ? "Oui" : "Non";
                    base.model.MontantReferenceCalcule = model.MontantReferenceCalcule;
                    base.model.MontantReferenceForce = model.MontantReferenceForce;
                    base.model.MontantReferenceAcquis = model.MontantReferenceAcquis;
                    base.model.DelegationApporteur = model.DelegationApporteur;
                    base.model.SecteurApporteur = model.SecteurApporteur;
                    base.model.DelegationGestionnaire = model.DelegationGestionnaire;
                    base.model.SecteurGestionnaire = model.SecteurGestionnaire;
                    base.model.Traites = model.Traites;
                    base.model.IsControleOk = model.IsControleOk;
                    base.model.IsDocEdit = model.IsDocEdit;
                    //model.IsReadOnly = model.IsReadOnly;
                    base.model.IsDocGener = model.IsDocGener;
                    base.model.DateAccord = AlbConvert.ConvertIntToDate(Convert.ToInt32(model.DateAccordInt));
                }

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
                        Etape = Navigation_MetaModel.ECRAN_INFOFIN,
                        IdOffre = model.Offre.CodeOffre,
                        Version = model.Offre.Version
                    };
                }
                else if (model.Contrat != null)
                {
                    var affaire = GetInfoBaseAffaire(model.CodePolicePage, model.VersionPolicePage, model.TypePolicePage, model.NumAvenant.ToString(), model.ModeNavig);

                    string typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    var regulMode = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULMOD).ToUpper();
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
                        case AlbConstantesMetiers.TYPE_ATTESTATION:
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_ATTES;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                            if (regulMode == "PB")
                            {
                                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULPB;
                            }
                            else if (regulMode == "BNS")
                            {
                                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULBNS;
                            }
                            else if (regulMode == "BURNER")
                            {
                                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULBURNER;
                            }
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR when affaire.IsRemiseEnVigeurSansModif:
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR_NO_MODIF;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR when affaire.IsRemiseEnVigeurAvecModif:
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                            break;
                        default:
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                            break;
                    }
                    model.Navigation = new Navigation_MetaModel
                    {
                        Etape = model.Bandeau.StyleBandeau == AlbConstantesMetiers.SCREEN_TYPE_ATTES ? 0 : Navigation_MetaModel.ECRAN_INFOFIN,
                        IdOffre = model.Contrat.CodeContrat,
                        Version = int.Parse(model.Contrat.VersionContrat.ToString())
                    };
                }
            }
        }
        private void SetArbreNavigation()
        {
            if (model.Offre != null)
            {
                //Affichage de la navigation latérale en arboresence
                model.NavigationArbre = GetNavigationArbre("Fin");
            }
            else if (model.Contrat != null)
            {
                if (model.ScreenType == AlbConstantesMetiers.SCREEN_TYPE_ATTES)
                {
                    model.NavigationArbre = GetNavigationArbreAffaireNouvelle("InfoSaisie", returnEmptyTree: true);
                }
                //else if (model.ScreenType == AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF || model.ScreenType == AlbConstantesMetiers.SCREEN_TYPE_REGUL)
                else if (model.ScreenType == AlbConstantesMetiers.SCREEN_TYPE_REGUL
                        || model.ScreenType == AlbConstantesMetiers.SCREEN_TYPE_REGULPB
                         || model.ScreenType == AlbConstantesMetiers.SCREEN_TYPE_REGULBNS
                          || model.ScreenType == AlbConstantesMetiers.SCREEN_TYPE_REGULBURNER)
                {
                    if (model.ActeGestion == AlbConstantesMetiers.TYPE_AVENANT_REGUL && model.ActeGestionRegule == AlbConstantesMetiers.TYPE_AVENANT_MODIF)
                    {
                        model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Fin");
                    }
                    else
                    {
                        model.NavigationArbre = GetNavigationArbreRegule(model, "Fin");
                    }
                }
                else
                {
                    model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Fin");
                }
            }
        }

        private void SaveObservation(string codeAffaire, string version, string type, string codeObservation, string observation, string acteGestion, string reguleId)
        {
            int iCodeObsv = 0;
            if (int.TryParse(codeObservation, out iCodeObsv))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    var Context = client.Channel;
                    Context.SaveObsvConnexite(codeAffaire, version, type, iCodeObsv, observation, acteGestion, reguleId);
                }
            }
        }
        private void SaveEtatMotif(string codeOffre, string type, string version, string etat, string motif, string acteGestion, string regulId)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var Context = client.Channel;
                Context.SaveEtatMotif(codeOffre, version, type, etat, motif, acteGestion, regulId);
            }
        }
        #endregion
    }
}
