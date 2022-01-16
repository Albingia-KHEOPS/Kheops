using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Filters;
using ALBINGIA.OP.OP_MVC.Models;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesDocumentGestion;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO.DocumentGestion;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Regularisation;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ICommon;
using OPServiceContract.ISaisieCreationOffre;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class DocumentGestionController : ControllersBase<ModeleDocumentGestionPage>
    {
        #region Membres privés
        private const string BorderValuePrefix = "|###||";
        private const string BorderValueSuffix = "||###|";
        #endregion

        #region Méthodes publiques

        
        [ErrorHandler]
        [WhitespaceFilter]
        public ActionResult Index(string id) {
            id = InitializeParams(id);

            string avType = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
            string acteGestionRegule = GetAddParamValue(model.AddParamValue, AlbParameterName.ACTEGESTIONREGULE);

            bool displayModeRegule = avType == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF && acteGestionRegule == AlbConstantesMetiers.TYPE_AVENANT_REGUL;

            try
            {
                LoadInfoPage(id, displayModeRegule);
                if (displayModeRegule)
                    model.NavigationArbre.Etape = "Regule";
            }
            catch (Exception)
            {
                model.ListeDocuments = new List<DocumentGestionDoc>();
                model.IsErrorOccured = true;

            }

            return View(model);
        }

        [ErrorHandler]
        public ActionResult ShowInfoDest(string idDest, string typeDest, string typeEnvoi)
        {
            DocumentGestionInfoDest model;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.ShowInfoDest(idDest, typeDest);
                model = (DocumentGestionInfoDest)result;
                model.TypeEnvoi = typeEnvoi;
            }
            return PartialView("InfoDestinataire", model);
        }

        [ErrorHandler]
        [HttpPost]
        public JsonResult IsClickedDocumentGenerated(string code, string type, string version, string codeAvt, string modeNavig, bool isReadOnly, string addParamType, string addParamValue, string idDocClicked)
        {
            bool isValidation = GetAddParamValue(addParamValue, AlbParameterName.VALIDATION) == "1";
            string acteGestion = GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);
            string attesId = GetAddParamValue(addParamValue, AlbParameterName.ATTESTID);

            string avType = GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);
            string acteGestionRegule = GetAddParamValue(addParamValue, AlbParameterName.ACTEGESTIONREGULE);

            return new JsonResult
            {
                Data = true
            };
        }

        [ErrorHandler]
        public ActionResult RefreshListeDocument(string codeOffre, string type, string version, string codeAvt, string modeNavig, bool isReadOnly, string addParamType, string addParamValue, string idDocClicked)
        {
            bool isValidation = GetAddParamValue(addParamValue, AlbParameterName.VALIDATION) == "1";
            string acteGestion = GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);
            string attesId = GetAddParamValue(addParamValue, AlbParameterName.ATTESTID);

            string avType = GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);
            string acteGestionRegule = GetAddParamValue(addParamValue, AlbParameterName.ACTEGESTIONREGULE);

            bool isClickedDocGenerated = true;
            List<DocumentGestionDoc> docs = null;
            DateTime start = DateTime.Now;
            do
            {
                (isClickedDocGenerated, docs) = LoadInfoDocumentsForSpecificScreen(
                 LoadInfoDocuments(
                     codeOffre, version, type, codeAvt,
                     acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF ? acteGestionRegule : acteGestion,
                     AlbEnumInfoValue.GetEnumValue<ModeConsultation>(modeNavig),
                     isReadOnly, false, acteGestion, isValidation, attesId: attesId),
                 acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF && acteGestionRegule == AlbConstantesMetiers.TYPE_AVENANT_REGUL, idDocClicked);
                Thread.Sleep(2000);
            } while (!isClickedDocGenerated && (DateTime.Now - start).TotalMinutes < 2);

            return PartialView("LstDocuments", docs);
        }

        [ErrorHandler]
        public ActionResult RefreshListeDocumentLibre(string codeOffre, string version, string type, string codeAvt, string modeNavig, bool isReadOnly, string addParamType, string addParamValue, string idDocClicked)
        {
            bool isValidation = GetAddParamValue(addParamValue, AlbParameterName.VALIDATION) == "1";
            string acteGestion = GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);
            string attesId = GetAddParamValue(addParamValue, AlbParameterName.ATTESTID);

            string avType = GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);
            string acteGestionRegule = GetAddParamValue(addParamValue, AlbParameterName.ACTEGESTIONREGULE);

            bool isClickedDocGenerated = true;
            List<DocumentGestionDoc> docs = null;

            do
            {
                (isClickedDocGenerated, docs) = LoadInfoDocumentsForSpecificScreen(
                LoadInfoDocuments(
                    codeOffre, version, type, codeAvt,
                    acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF ? acteGestionRegule : acteGestion,
                    AlbEnumInfoValue.GetEnumValue<ModeConsultation>(modeNavig),
                    isReadOnly, false, acteGestion, isValidation, attesId: attesId),
                acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF && acteGestionRegule == AlbConstantesMetiers.TYPE_AVENANT_REGUL, idDocClicked);

                Thread.Sleep(2000);
            } while (!isClickedDocGenerated);

            return PartialView("LstDocumentsLibre", docs);
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string tabGuid, string formGen, string paramRedirect, string modeNavig, string addParamType, string addParamValue)
        {
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            var isReadOnly = GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, numAvn);

            #region CR809 changes
            var typeAvt = GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);
            var acteGestionRegule = GetAddParamValue(addParamValue, AlbParameterName.ACTEGESTIONREGULE);
            if (typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF && acteGestionRegule == AlbConstantesMetiers.TYPE_AVENANT_REGUL && cible == "ValidationOffre")
            {
                return RedirectToAction("Index", "CreationAvenant", new { id = codeOffre + "_" + version + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
            }
            #endregion
            //Redirection vers la page sélectionnée dans le menu
            if (!string.IsNullOrEmpty(paramRedirect))
            {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }

            if (string.IsNullOrEmpty(numAvn))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                {
                    var serviceContext = client.Channel;
                    numAvn = serviceContext.GetNumAvn(codeOffre, version, type);
                }
            }
            //numAvn = "0";
            if (cible == "RechercheSaisie")
            {
                //Déverouillage de l'offre/contrat
                var folder = string.Format("{0}_{1}_{2}_{3}", codeOffre, version, type, numAvn);
                var isModifHorsAvn = GetIsModifHorsAvn(tabGuid, folder);
                Common.CommonVerouillage.DeverrouilleFolder(codeOffre, version, type, numAvn, tabGuid, !isReadOnly && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>(), isReadOnly, isModifHorsAvn);
                return RedirectToAction("Index", "RechercheSaisie", new { id = codeOffre + "_" + version + "_" + type + "_loadParam" + tabGuid + GetFormatModeNavig(modeNavig) });
            }


            if (type == AlbConstantesMetiers.TYPE_CONTRAT)
            {
                if (!isReadOnly)
                {
                    //Sauvegarde trace arbre
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                    {
                        var serviceContext = client.Channel;
                        serviceContext.SaveTraceArbreFinAffnouv(codeOffre, version, type, GetUser());
                    }
                }
                //else
                //    return RedirectToAction(job, "Quittance", new { id = codeOffre + "_" + version + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
            }

            if (GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE) == AlbConstantesMetiers.TYPE_AVENANT_RESIL && cible == "ControleFin")
                return RedirectToAction(job, "Quittance", new { id = codeOffre + "_" + version + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });


            return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + "_" + numAvn + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
        }

        [ErrorHandler]
        public void ChangeSituationDoc(string idDoc, string isCheck)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext = client.Channel;
                serviceContext.ChangeSituationDoc(idDoc, isCheck);
            }
        }

        [ErrorHandler]
        public ActionResult RegenerateDocLibre(string codeOffre, string version, string type, string codeAvt, string modeNavig, bool isReadOnly, long[] idsDoc, string addParamValue, string acteGestion)
        {
            model.NumAvenantPage = codeAvt;
            acteGestion = addParamValue.IsEmptyOrNull() ? acteGestion : GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);
            string attesId = GetAddParamValue(addParamValue, AlbParameterName.ATTESTID);

            string acteGestionRegule = GetAddParamValue(addParamValue, AlbParameterName.ACTEGESTIONREGULE);

            var (isClickedDocGenerated, docs) = LoadInfoDocumentsForSpecificScreen(LoadInfoDocuments(codeOffre, version, type, codeAvt,
                    acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF ? acteGestionRegule : acteGestion,
                    AlbEnumInfoValue.GetEnumValue<ModeConsultation>(modeNavig),
                    isReadOnly,
                    true,
                    avType: acteGestion,
                    docsId: idsDoc,
                    attesId: attesId),
                acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF && acteGestionRegule == AlbConstantesMetiers.TYPE_AVENANT_REGUL);

            return PartialView("LstDocuments", docs
                );
        }

        [ErrorHandler]
        public ActionResult GetLigneCodeLabelDestinataire(string guid, string typeDest)
        {
            long lGuid;
            long.TryParse(guid, out lGuid);
            DestinataireLigne toReturn = new DestinataireLigne { GuidId = lGuid, CodeTypeDestinataire = typeDest };
            return PartialView("CodeLabelDestinataire", toReturn);
        }

        #endregion

        protected override void ExtendNavigationArbreRegule(MetaModelsBase contentData)
        {
            RegularisationNavigator.StandardInitContext(model, RegularisationStep.Cotisations);
            if (model?.Context != null)
            {
                RegularisationNavigator.Initialize(contentData.NavigationArbre, model.Context);
            }
        }

        #region Méthodes privées

        private void LoadInfoPage(string id, bool haveToEnforceDisplayRegulDocument = false)
        {
            string[] tId = id.Split('_');
            var typeAvt = string.Empty;
            var regulMode = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULMOD);

            if (tId[2] == "O")
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                {
                    var policeServicesClient = client.Channel;
                    model.Offre = new Offre_MetaModel();
                    model.Offre.LoadOffre(policeServicesClient.OffreGetDto(tId[0], int.Parse(tId[1]), tId[2], model.ModeNavig.ParseCode<ModeConsultation>()));
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
                }
            }
            if (tId[2] == "P")
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var serviceContext = client.Channel;
                    model.Contrat = serviceContext.GetContrat(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                    typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
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
                    model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);
                }
            }
            model.PageTitle = "Gestion des documents";
            if (model.Offre != null || model.Contrat != null)
            {
                model.AfficherBandeau = DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
            }

            if (tId[2] == AlbConstantesMetiers.TYPE_OFFRE) {
                model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
            }
            else {
                model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);
            }

            model.Bandeau = null;
            SetBandeauNavigation(id);
            SetArbreNavigation();

            bool isValidation = GetAddParamValue(model.AddParamValue, AlbParameterName.VALIDATION) == "1";
            string attesId = GetAddParamValue(model.AddParamValue, AlbParameterName.ATTESTID);
            string avnMode = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNMODE);

            if (avnMode == "CONSULT" && model.Context != null && typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGUL && regulMode == "STAND" && model.Context.IsSimplifiedRegule)
            {
                model.IsReadOnly = true;
                model.Context.IsReadOnlyMode = true;
                model.ListeDocuments = new List<DocumentGestionDoc>();
            }
            else
            {
                string regId = model.Contrat == null ? "0" : model.Contrat.ReguleId.ToString();
                bool isClickedDocGenerated = true;
                (isClickedDocGenerated, model.ListeDocuments) = LoadInfoDocumentsForSpecificScreen(
                    LoadInfoDocuments(
                        tId[0], tId[1], tId[2], model.NumAvenantPage,
                        model.ActeGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF ? model.ActeGestionRegule : model.ActeGestion,
                        AlbEnumInfoValue.GetEnumValue<ModeConsultation>(model.ModeNavig),
                        model.IsReadOnly, true, haveToEnforceDisplayRegulDocument ? AlbConstantesMetiers.TYPE_AVENANT_REGUL : typeAvt, isValidation, firstLoad: true, attesId: attesId, regulId: regId),
                    haveToEnforceDisplayRegulDocument);
            }

        }

        private (bool, List<DocumentGestionDoc>) LoadInfoDocumentsForSpecificScreen(List<DocumentGestionDoc> listeDocuments, bool haveToEnforceDisplayRegulDocument, string idDocClicked = "")
        {
            var listFiltered = new List<DocumentGestionDoc>();

            if (haveToEnforceDisplayRegulDocument)
                listeDocuments.ForEach(itemDoc => listFiltered.Add(new DocumentGestionDoc() { DocId = itemDoc.DocId, FirstGeneration = itemDoc.FirstGeneration, ListDocInfos = itemDoc.ListDocInfos.Where(it => it.TypeDoc.Trim().Equals("REGUL") || it.TypeDoc.Trim().Equals("LETTYP")).ToList() }));
            else
                listFiltered = listeDocuments;
            bool isClickedDocGenerated = true;
            if (!idDocClicked.IsEmptyOrNull())
            {
                long.TryParse(idDocClicked, out long idDoc);
                isClickedDocGenerated = listFiltered.FirstOrDefault(it => it.DocId == idDoc)?.ListDocInfos?.FirstOrDefault(it => it.IdDoc == idDoc)?.Statut == "G";
            }
            return (isClickedDocGenerated, listFiltered);
        }


        private List<DocumentGestionDoc> LoadInfoDocuments(string code, string version, string type, string codeAvt, string acteGestion, ModeConsultation modeNavig, bool isReadOnly, bool init, string avType, bool isValidation = false, bool firstLoad = false, long[] docsId = null, string attesId = "", string regulId = "0")
        {
            docsId = docsId ?? new long[0];
            var toReturn = new List<DocumentGestionDoc>();
            if (acteGestion == "AVNRM")
            {
                using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                {
                    var CommonOffreClient = chan.Channel;
                    var infosBaseAffaire = CommonOffreClient.LoadInfosBase(code, version.ToString(), type, model.NumAvenantPage, model.ModeNavig);
                    if (infosBaseAffaire.IsRemiseEnVigeurAvecModif)
                    {
                        acteGestion = "AVNMD";
                    }
                }
            }
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.InitDocumentsGestion(code, version, type, codeAvt, acteGestion, avType, GetUser(), modeNavig, isReadOnly, init, isValidation, docsId, firstLoad, attesId, regulId);
                if (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        var it = (DocumentGestionDoc)item;

                        toReturn.Add((it));
                    }
                }
            }
            return toReturn;
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
                        Etape = model.ScreenType == AlbConstantesMetiers.SCREEN_TYPE_ATTES ? 0 : Navigation_MetaModel.ECRAN_INFOFIN,
                        IdOffre = model.Offre.CodeOffre,
                        Version = model.Offre.Version
                    };
                }
                else if (model.Contrat != null)
                {
                    var affaire = GetInfoBaseAffaire(model.CodePolicePage, model.VersionPolicePage, model.TypePolicePage, model.NumAvenant.ToString(), model.ModeNavig);
                    string typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    var regulMode = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULMOD);
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
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
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
                            else
                            {
                                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
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
                else if (model.ScreenType == AlbConstantesMetiers.SCREEN_TYPE_REGUL
                    || model.ScreenType == AlbConstantesMetiers.SCREEN_TYPE_REGULPB
                    || model.ScreenType == AlbConstantesMetiers.SCREEN_TYPE_REGULBNS
                    || model.ScreenType == AlbConstantesMetiers.SCREEN_TYPE_REGULBURNER
                    || (model.ScreenType == AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF && model.ActeGestionRegule == AlbConstantesMetiers.TYPE_AVENANT_REGUL))
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
            if (model.NavigationArbre != null) {
                model.NavigationArbre.IsRegule = Model.ActeGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF || (Model.ActeGestion == AlbConstantesMetiers.TYPE_AVENANT_REGUL && model.IsReadOnly);
            }
        }

        #endregion

        #region Créer/Modifier document

        #region Membres statiques

        private static List<AlbSelectListItem> _lstDocumentsDispo;
        public static List<AlbSelectListItem> LstDocumentsDispo
        {
            get
            {
                //Si les données ont déjà été chargé en mémoire, on renvoit une nouvelle instance de la liste
                if (_lstDocumentsDispo != null)
                {
                    var toReturn = new List<AlbSelectListItem>();
                    _lstDocumentsDispo.ForEach(elm => toReturn.Add(new AlbSelectListItem
                    {
                        Value = elm.Value,
                        Text = elm.Text,
                        Title = elm.Title,
                        Selected = false
                    }));
                    return toReturn;
                }

                //Sinon on charge les données à partir de la BDD et on renvoit une nouvelle instance de la liste
                List<AlbSelectListItem> value = new List<AlbSelectListItem>();
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    var serviceContext = client.Channel;
                    var lstDocs = serviceContext.GetListeDocumentsDispo();
                    lstDocs.ForEach(elm => value.Add(new AlbSelectListItem
                    {
                        Value = elm.Code.ToString(),
                        Text = elm.Code + " - " + elm.Libelle,
                        Title = elm.Code + " - " + elm.Libelle,
                        Selected = false
                    }
                    ));
                }
                _lstDocumentsDispo = value;
                return value;
            }
        }

        private static List<AlbSelectListItem> _lstTypesDestinataire;
        public static List<AlbSelectListItem> LstTypesDestinataire
        {
            get
            {
                //Si les données ont déjà été chargé en mémoire, on renvoit une nouvelle instance de la liste
                if (_lstTypesDestinataire != null)
                {
                    var toReturn = new List<AlbSelectListItem>();
                    _lstTypesDestinataire.ForEach(elm => toReturn.Add(new AlbSelectListItem
                    {
                        Value = elm.Value,
                        Text = elm.Text,
                        Title = elm.Title,
                        Selected = false
                    }));
                    return toReturn;
                }

                //Sinon on charge les données à partir de la BDD et on renvoit une nouvelle instance de la liste
                List<AlbSelectListItem> value = new List<AlbSelectListItem>();
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    var serviceContext = client.Channel;
                    var lstTypesDest = serviceContext.GetListeTypesDestinataire();
                    lstTypesDest.ForEach(elm => value.Add(new AlbSelectListItem
                    {
                        Value = elm.Code.ToString(),
                        Text = elm.Code + " - " + elm.Libelle,
                        Title = elm.Code + " - " + elm.Libelle,
                        Selected = false
                    }
                    ));
                }
                _lstTypesDestinataire = value;
                return value;
            }
        }

        private static List<AlbSelectListItem> _lstTypesEnvoi;
        public static List<AlbSelectListItem> LstTypesEnvoi
        {
            get
            {
                //Si les données ont déjà été chargé en mémoire, on renvoit une nouvelle instance de la liste
                if (_lstTypesEnvoi != null)
                {
                    var toReturn = new List<AlbSelectListItem>();
                    _lstTypesEnvoi.ForEach(elm => toReturn.Add(new AlbSelectListItem
                    {
                        Value = elm.Value,
                        Text = elm.Text,
                        Title = elm.Title,
                        Selected = false
                    }));
                    return toReturn;
                }

                //Sinon on charge les données à partir de la BDD et on renvoit une nouvelle instance de la liste
                List<AlbSelectListItem> value = new List<AlbSelectListItem>();
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    var serviceContext = client.Channel;
                    var lstTypesEnv = serviceContext.GetListeTypesEnvoi();
                    lstTypesEnv.ForEach(elm => value.Add(new AlbSelectListItem
                    {
                        Value = elm.Code.ToString(),
                        Text = elm.Code + " - " + elm.Libelle,
                        Title = elm.Code + " - " + elm.Libelle,
                        Selected = false
                    }
                    ));
                }
                _lstTypesEnvoi = value;
                return value;
            }
        }

        private static List<AlbSelectListItem> _lstTampon;
        public static List<AlbSelectListItem> LstTampon
        {
            get
            {
                //Si les données ont déjà été chargé en mémoire, on renvoit une nouvelle instance de la liste
                if (_lstTampon != null)
                {
                    var toReturn = new List<AlbSelectListItem>();
                    _lstTampon.ForEach(elm => toReturn.Add(new AlbSelectListItem
                    {
                        Value = elm.Value,
                        Text = elm.Text,
                        Title = elm.Title,
                        Selected = false
                    }));
                    return toReturn;
                }

                //Sinon on charge les données à partir de la BDD et on renvoit une nouvelle instance de la liste
                List<AlbSelectListItem> value = new List<AlbSelectListItem>();
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    var serviceContext = client.Channel;
                    var lstTampon = serviceContext.GetListeTampons();
                    lstTampon.ForEach(elm => value.Add(new AlbSelectListItem
                    {
                        Value = elm.Code.ToString(),
                        Text = elm.Code + " - " + elm.Libelle,
                        Title = elm.Code + " - " + elm.Libelle,
                        Selected = false
                    }
                    ));
                }
                _lstTampon = value;
                return value;
            }
        }

        private static List<AlbSelectListItem> _lstLettreAcc;
        public static List<AlbSelectListItem> LstLettreAcc
        {
            get
            {
                //Si les données ont déjà été chargé en mémoire, on renvoit une nouvelle instance de la liste
                if (_lstLettreAcc != null)
                {
                    var toReturn = new List<AlbSelectListItem>();
                    _lstLettreAcc.ForEach(elm => toReturn.Add(new AlbSelectListItem
                    {
                        Value = elm.Value,
                        Text = elm.Text,
                        Title = elm.Title,
                        Selected = false
                    }));
                    return toReturn;
                }

                //Sinon on charge les données à partir de la BDD et on renvoit une nouvelle instance de la liste
                List<AlbSelectListItem> value = new List<AlbSelectListItem>();
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    var serviceContext = client.Channel;
                    var lstLettreAcc = new List<ParametreDto>();//TODO : à définir
                    lstLettreAcc.ForEach(elm => value.Add(new AlbSelectListItem
                    {
                        Value = elm.Code.ToString(),
                        Text = elm.Code + " - " + elm.Libelle,
                        Title = elm.Code + " - " + elm.Libelle,
                        Selected = false
                    }
                    ));
                }
                _lstLettreAcc = value;
                return value;
            }
        }

        private static List<AlbSelectListItem> _lstTypesIntervenant;
        private List<AlbSelectListItem> GetListeType()
        {
            var listeType = new List<AlbSelectListItem>();
            if (_lstTypesIntervenant != null)
            {

                _lstTypesIntervenant.ForEach(elm => listeType.Add(new AlbSelectListItem
                {
                    Value = elm.Value,
                    Text = elm.Text,
                    Title = elm.Title,
                    Selected = false
                }));
                return listeType;
            }

            //Sinon on charge les données à partir de la BDD et on renvoit une nouvelle instance de la liste

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var serviceContext = client.Channel;
                var lstTyp = serviceContext.GetListeTypesIntervenant();
                lstTyp.ForEach(elm => listeType.Add(new AlbSelectListItem
                {
                    Value = elm.Code,
                    Text = elm.Code + " - " + elm.Libelle,
                    Title = elm.Code + " - " + elm.Libelle,
                    Selected = false
                }
                ));
            }
            _lstTypesIntervenant = listeType;
            return listeType;
        }

        #endregion

        #region Méthodes publiques

        [ErrorHandler]
        public ActionResult GetDetailsDocument(string codeDocument, string codeOffreContrat, string versionOffreContrat)
        {
            var toReturn = new ModifierDocumentModele { ListeDocuments = LstDocumentsDispo };
            long idDoc = -1;
            if (long.TryParse(codeDocument, out idDoc))
            {
                toReturn.IdDocument = idDoc;
                toReturn.ListeDestinataire = GetListeLigneDestinatairesDetails(idDoc);
                toReturn.NewDestinataire = new List<DestinataireLigne>();
                toReturn.NewDestinataire.Add(new DestinataireLigne() { GuidId = -1, CodeTypeDestinataire = string.Empty, TypeEnvoi = string.Empty, Tampon = string.Empty, LettreAccompagnement = string.Empty, ListeTypesDestinataire = LstTypesDestinataire, ListeLettreAccompagnement = LstLettreAcc, ListeTampons = LstTampon, ListeTypesEnvoi = LstTypesEnvoi, NombreExemplaire = "1" });
                toReturn = LoadInfoDetailsComplementaires(toReturn);
                SetSelectedItem(toReturn.ListeDocuments, toReturn.Document);
                //toReturn.ModelFileDescriptions = new FileDescriptions { ListFileDescription = IOFileManager.GetAllFileDescription(AlbOpConstants.UploadParthReturnedDocument, HttpUtility.UrlDecode(string.Format("{0}-{1}", codeOffreContrat, versionOffreContrat)), MvcApplication.SPLIT_CONST_FILE) };
                toReturn.CodeOffreContrat = codeOffreContrat;
                toReturn.VersionOffreContrat = versionOffreContrat;

            }
            return PartialView("ModifierDocument", toReturn);
        }

        [ErrorHandler]
        public ActionResult GetSelectionCourrierType(string valeurCourante, string filtre, string typeDoc)
        {
            ModeleSelectionCourrierType toReturn = new ModeleSelectionCourrierType();
            toReturn.ListeCourriersType = GetLignesCourrierType(filtre, valeurCourante, typeDoc);
            toReturn.SelectedValue = toReturn.ListeCourriersType.Find(elm => elm.IsSelected) != null ? valeurCourante : string.Empty;
            toReturn.Filtre = filtre;
            return PartialView("SelectionCourrierType", toReturn);
        }

        [ErrorHandler]
        public ActionResult GetSelectionDestinataire(string typeDestinataire, string idDestinataire, string codeOffreContrat, string type, string version, Int64 codeDocument, string typeIntervenant, string codeDestinataireCourant)
        {
            ModeleSelectionDestinataire toReturn = new ModeleSelectionDestinataire();
            toReturn.ListeDestinataires = GetLignesDestinataire(codeOffreContrat, type, version, codeDocument.ToString(), typeDestinataire, codeDestinataireCourant, typeIntervenant);
            //toReturn.AutreDestinataire = toReturn.ListeDestinataires.Any() ? toReturn.ListeDestinataires.FirstOrDefault() : null;
            toReturn.GuidIdDestinataire = idDestinataire;
            toReturn.TypeIntervenant = typeIntervenant;
            return PartialView("SelectionDestinataire", toReturn);
        }

        [ErrorHandler]
        public ActionResult GetSelectionTypesIntervenant(string codeDossier, string versionDossier, string typeDossier, string idDestinataire, string codeIntervenantCourant, string nomIntervenantCourant, string typeIntervenantSelectionne)
        {
            var toReturn = new LigneTypeIntervenant();
            toReturn.ListeTypesIntervenant = GetListeType();
            toReturn.idDestinataire = idDestinataire;
            toReturn.CodeSelectedIntervenant = codeIntervenantCourant;
            toReturn.NomSelectedIntervenant = nomIntervenantCourant;
            toReturn.SelectedTypeIntervenant = typeIntervenantSelectionne;
            toReturn.IsFromAffaire = true;//From affaire par défaut
            if (!string.IsNullOrEmpty(codeIntervenantCourant))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                {
                    var serviceContext = client.Channel;
                    var result = serviceContext.GetListeIntervenantsAutocomplete(codeDossier, versionDossier, typeDossier, string.Empty, string.Empty, codeIntervenantCourant, true);
                    if (result != null && result.Any())
                        toReturn.IsFromAffaire = true;
                    else
                        toReturn.IsFromAffaire = false;
                }
            }
            if (!string.IsNullOrEmpty(typeIntervenantSelectionne) && toReturn != null)
            {
                var selectedItem = toReturn.ListeTypesIntervenant.FirstOrDefault(elm => elm.Value.Trim() == typeIntervenantSelectionne.Trim());
                if (selectedItem != null)
                    selectedItem.Selected = true;
            }

            return PartialView("SelectionTypeIntervenant", toReturn);
        }

        [ErrorHandler]
        public ActionResult GetListeDestinatairesDetails(Int64 codeDocument)
        {
            ModifierDocumentModele toReturn = new ModifierDocumentModele
            {
                ListeDestinataire = GetListeLigneDestinatairesDetails(codeDocument)
            };
            //List<DestinataireLigne> toReturn = GetListeLigneDestinatairesDetails(codeDocument);

            return PartialView("LstDetailsDestinataires", toReturn);
        }

        [ErrorHandler]
        public ActionResult SaveLigneDestinataireDetails(string codeOffre, string version, string type, string codeAvt, string modeNavig, bool isReadOnly,
            long codeDocument, long guidIdLigne, bool isPrincipalChecked, string typeDestinataire, string typeIntervenant, string codeDestinataire,
            string codeInterlocuteur, string typeEnvoi, int nbEx, string tampon, long lettreAccompagnement, int lotEmail,
            bool isGenereChecked, string lotId, string typeDoc, string courrierType, string addParamValue, string acteGestion)
        {
            bool isValidation = GetAddParamValue(addParamValue, AlbParameterName.VALIDATION) == "1";
            List<DestinataireLigne> toReturn = new List<DestinataireLigne>();
            //Vérif des données
            long iCodeDestinataire = 0;
            long iCodeInterlocuteur = 0;
            if (!long.TryParse(codeDestinataire, out iCodeDestinataire))
            {
                throw new AlbFoncException("Destinataire vide ou incorrect");
            }
            long.TryParse(codeInterlocuteur, out iCodeInterlocuteur);

            DestinataireDto toSave = new DestinataireDto
            {
                GuidId = guidIdLigne,
                IsPrincipal = isPrincipalChecked ? "O" : "N",
                TypeDestinataire = typeDestinataire,
                Code = iCodeDestinataire,
                TypeIntervenant = typeIntervenant,
                CodeInterlocuteur = iCodeInterlocuteur,
                TypeEnvoi = typeEnvoi,
                NombreEx = nbEx,
                Tampon = tampon,
                LettreAccompagnement = lettreAccompagnement,
                LotEmail = lotEmail,
                IsGenere = isGenereChecked ? "A" : "X"
            };

            #region Sauvegarde et actualisation des données
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.SaveLigneDestinataireDetails(codeOffre, version, type, GetUser(), lotId, typeDoc, courrierType, codeDocument.ToString(), toSave, acteGestion);
                if (result.Any())
                    result.ForEach(elm => toReturn.Add((DestinataireLigne)elm));
            }

            foreach (DestinataireLigne elm in toReturn)
            {
                elm.ListeTypesDestinataire = LstTypesDestinataire;
                SetSelectedItem(elm.ListeTypesDestinataire, elm.CodeTypeDestinataire);

                elm.ListeTypesEnvoi = LstTypesEnvoi;
                SetSelectedItem(elm.ListeTypesEnvoi, elm.TypeEnvoi);

                elm.ListeTampons = LstTampon;
                SetSelectedItem(elm.ListeTampons, elm.Tampon);

                elm.ListeLettreAccompagnement = LstLettreAcc;
                SetSelectedItem(elm.ListeLettreAccompagnement, elm.LettreAccompagnement);
            }
            #endregion

            string avType = GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);
            string acteGestionRegule = GetAddParamValue(addParamValue, AlbParameterName.ACTEGESTIONREGULE);

            string attesId = GetAddParamValue(addParamValue, AlbParameterName.ATTESTID);

            var (isClickedDocGenerated, docs) = LoadInfoDocumentsForSpecificScreen(
               LoadInfoDocuments(codeOffre, version, type, codeAvt,
                   acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF ? acteGestionRegule : acteGestion,
                   AlbEnumInfoValue.GetEnumValue<ModeConsultation>(modeNavig),
                   isReadOnly,
                   false,
                   avType,
                   isValidation,
                   attesId: attesId),
               avType == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF && acteGestionRegule == AlbConstantesMetiers.TYPE_AVENANT_REGUL);

            return PartialView("LstDocuments", docs);

        }

        [ErrorHandler]
        public ActionResult DeleteLigneDestinataireDetails(Int64 codeDocument, Int64 guidIdLigne)
        {
            ModifierDocumentModele toReturn = new ModifierDocumentModele
            {
                ListeDestinataire = GetListeLigneDestinatairesDetails(codeDocument)
            };

            //List<DestinataireLigne> toReturn = new List<DestinataireLigne>();
            #region Suppression et actualisation des données
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.DeleteLigneDestinataireDetails(codeDocument.ToString(), guidIdLigne.ToString());
                if (result.Any())
                    result.ForEach(elm => toReturn.ListeDestinataire.Add((DestinataireLigne)elm));
            }

            foreach (DestinataireLigne elm in toReturn.ListeDestinataire)
            {
                elm.ListeTypesDestinataire = LstTypesDestinataire;
                SetSelectedItem(elm.ListeTypesDestinataire, elm.CodeTypeDestinataire);

                elm.ListeTypesEnvoi = LstTypesEnvoi;
                SetSelectedItem(elm.ListeTypesEnvoi, elm.TypeEnvoi);

                elm.ListeTampons = LstTampon;
                SetSelectedItem(elm.ListeTampons, elm.Tampon);

                elm.ListeLettreAccompagnement = LstLettreAcc;
                SetSelectedItem(elm.ListeLettreAccompagnement, elm.LettreAccompagnement);
            }

            #endregion

            return PartialView("LstDetailsDestinataires", toReturn);
        }

        [ErrorHandler]
        public ActionResult SaveInfoComplementairesDetailsDocument(string codeOffre, string type, string version, string codeAvt, string modeNavig, bool isReadOnly, Int64 codeDocument, string document, Int64 courrierType, int nbExSupp, string addParamValue)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext=client.Channel;
                serviceContext.SaveInformationsComplementairesDetailsDocument(codeOffre, type, version, codeDocument, document, courrierType, nbExSupp, GetUser());
            }
            string acteGestion = GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);
            string attesId = GetAddParamValue(addParamValue, AlbParameterName.ATTESTID);

            string acteGestionRegule = GetAddParamValue(addParamValue, AlbParameterName.ACTEGESTIONREGULE);

            var (isClickedDocGenerated, docs) = LoadInfoDocumentsForSpecificScreen(
                LoadInfoDocuments(codeOffre, version, type, codeAvt,
                    acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF ? acteGestionRegule : acteGestion,
                    AlbEnumInfoValue.GetEnumValue<ModeConsultation>(modeNavig),
                    isReadOnly,
                    false,
                    acteGestion,
                    attesId: attesId),
                acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF && acteGestionRegule == AlbConstantesMetiers.TYPE_AVENANT_REGUL);

            return PartialView("LstDocuments", docs);
        }

        [ErrorHandler]
        public ActionResult DeleteLotDocumentsGestion(string codeOffre, string type, string version, string codeAvt, string modeNavig, bool isReadOnly, Int64 codeLot, string addParamValue)
        {
            bool isValidation = GetAddParamValue(addParamValue, AlbParameterName.VALIDATION) == "1";
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext = client.Channel;
                serviceContext.DeleteLotDocumentsGestion(codeLot);
            }
            string acteGestion = GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);
            string attesId = GetAddParamValue(addParamValue, AlbParameterName.ATTESTID);

            string acteGestionRegule = GetAddParamValue(addParamValue, AlbParameterName.ACTEGESTIONREGULE);

            var (isClickedDocGenerated, docs) = LoadInfoDocumentsForSpecificScreen(
                LoadInfoDocuments(codeOffre, version, type, codeAvt,
                    acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF ? acteGestionRegule : acteGestion,
                    AlbEnumInfoValue.GetEnumValue<ModeConsultation>(modeNavig),
                    isReadOnly,
                    false,
                    acteGestion,
                    isValidation,
                    attesId: attesId),
                acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF && acteGestionRegule == AlbConstantesMetiers.TYPE_AVENANT_REGUL);

            return PartialView("LstDocuments", docs);
        }

        #endregion

        #region Méthodes privées

        private List<LigneSelectionDestinataire> GetLignesDestinataire(string code, string type, string version, string codeDocument, string typeDestinataire, string codeDestinataireCourant, string typeIntervenant = "")
        {
            List<LigneSelectionDestinataire> toReturn = new List<LigneSelectionDestinataire>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext = client.Channel;
                List<DestinataireDto> result = null;
                switch (typeDestinataire)
                {
                    case "CT":
                    case "COURT":
                        result = serviceContext.GetListeCourtiers(code, type, version, codeDocument);
                        break;
                    case "AS":
                    case "ASS":
                        result = serviceContext.GetListeAssures(code, type, version, codeDocument);
                        break;
                    case "CI":
                        result = serviceContext.GetListeCompagnies(code, type, version, codeDocument);
                        break;
                    case "Intervenant":
                        result = serviceContext.GetListeIntervenants(code, type, version, codeDocument, typeIntervenant);
                        break;
                }
                if (result != null && result.Any())
                    result.ForEach(elm => toReturn.Add((LigneSelectionDestinataire)elm));
            }
            if (!string.IsNullOrEmpty(codeDestinataireCourant))
            {
                foreach (LigneSelectionDestinataire elm in toReturn)
                {
                    if (elm.Code == codeDestinataireCourant)
                        elm.IsSelected = true;
                }
            }

            return toReturn;
        }

        private List<LigneCourrierType> GetLignesCourrierType(string filtre, string valeurCourante, string typeDoc)
        {
            List<LigneCourrierType> toReturn = new List<LigneCourrierType>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.GetListeCourriersType(filtre, typeDoc);
                if (result.Any())
                    result.ForEach(elm => toReturn.Add((LigneCourrierType)elm));
                Int64 guidCourant = -1;
                if (Int64.TryParse(valeurCourante, out guidCourant))
                {
                    foreach (var elm in toReturn)
                    {
                        if (elm.GuidId == guidCourant)
                            elm.IsSelected = true;
                    }
                }
            }
            return toReturn;
        }

        private List<DestinataireLigne> GetListeLigneDestinatairesDetails(Int64 codeDocument)
        {
            List<DestinataireLigne> toReturn = new List<DestinataireLigne>();
            if (codeDocument > 0)
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    var serviceContext = client.Channel;
                    var result = serviceContext.GetListeDestinatairesDetails(codeDocument.ToString());
                    if (result.Any())
                        result.ForEach(elm => toReturn.Add((DestinataireLigne)elm));
                }
            }
            else
            {
                toReturn.Add(new DestinataireLigne());
            }

            foreach (DestinataireLigne elm in toReturn)
            {
                elm.ListeTypesDestinataire = LstTypesDestinataire;
                SetSelectedItem(elm.ListeTypesDestinataire, elm.CodeTypeDestinataire);

                elm.ListeTypesEnvoi = LstTypesEnvoi;
                SetSelectedItem(elm.ListeTypesEnvoi, elm.TypeEnvoi);

                elm.ListeTampons = LstTampon;
                SetSelectedItem(elm.ListeTampons, elm.Tampon);

                elm.ListeLettreAccompagnement = LstLettreAcc;
                SetSelectedItem(elm.ListeLettreAccompagnement, elm.LettreAccompagnement);
            }

            return toReturn;
        }

        private ModifierDocumentModele LoadInfoDetailsComplementaires(ModifierDocumentModele toReturn)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.GetInfoComplementairesDetailsDocumentGestion(toReturn.IdDocument.ToString());
                if (result != null)
                {
                    toReturn.Document = result.TypeDocument;
                    toReturn.ExemplairesSupplementaires = result.NombreExemplaire != 0 ? result.NombreExemplaire.ToString() : "1";
                    toReturn.LotId = result.LotId;
                    toReturn.CourrierId = result.CourrierId;
                    toReturn.CourrierCode = result.CourrierCode;
                    toReturn.CourrierLib = result.CourrierLib;
                    toReturn.DocLibre = result.DocLibre;
                    toReturn.DocGener = result.DocGener;
                }
                else
                {
                    toReturn.ExemplairesSupplementaires = "1";
                }
            }
            return toReturn;
        }

        private static void SetSelectedItem(List<AlbSelectListItem> lookUp, string compareValue, AlbSelectListItem selectedItem = null)
        {
            if (!string.IsNullOrEmpty(compareValue) && lookUp != null)
            {
                selectedItem = lookUp.FirstOrDefault(elm => elm.Value.Trim() == compareValue.Trim());
                if (selectedItem != null)
                    selectedItem.Selected = true;
            }
        }

        #endregion


        #endregion

    }
}
