using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Controllers.AffaireNouvelle;
using ALBINGIA.OP.OP_MVC.CustomResult;
using ALBINGIA.OP.OP_MVC.Filters;
using ALBINGIA.OP.OP_MVC.Models;
using ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.Courtiers;
using ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.ModelePage;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesQuittances;
using ALBINGIA.OP.OP_MVC.Models.Primes;
using Mapster;
using OP.Services.BLServices;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Clausier;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Cotisations;
using OP.WSAS400.DTO.NavigationArbre;
using OP.WSAS400.DTO.Regularisation;
using OPServiceContract;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class QuittanceController : ControllersBase<ModeleQuittancePage>
    {
        private const string WarningContratSansCommission = "WARNINGAttention contrat sans commission";
        private const string WarningCatnatSansCommission = "WARNINGAttention Catnat sans commission";
        private const string ErrorComment = "ERREURCOMERREURUn commentaire est nécessaire";
        private const string ErrorEcheance = "ERREURECHERREURIl faut saisir au moins une écheance";
        private const string ErrorTotalEcheance = "ERREURECHERREURLa somme des montants n'est pas égale à la prime totale";
        private const string ErrorDateEcheance = "ERREURECHERREURIncohérence dans les dates d'échéance.";
        private const string BorderValuePrefix = "|###||";
        private const string BorderValueSuffix = "||###|";

        private static List<AlbSelectListItem> _lstTypesOperation;
        public static List<AlbSelectListItem> LstTypesOperation
        {
            get
            {
                //Si les données ont déjà été chargé en mémoire, on renvoit une nouvelle instance de la liste
                if (_lstTypesOperation != null)
                {
                    var toReturn = new List<AlbSelectListItem>();
                    _lstTypesOperation.ForEach(elm => toReturn.Add(new AlbSelectListItem
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
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    var serviceContext = channelClient.Channel;
                    var lstTypOpe = serviceContext.GetListeTypesOperation();
                    lstTypOpe.ForEach(elm => value.Add(new AlbSelectListItem
                    {
                        Value = elm.Code.ToString(),
                        Text = elm.Code + " - " + elm.Libelle,
                        Title = elm.Code + " - " + elm.Libelle,
                        Selected = false
                    }
                    ));
                }
                _lstTypesOperation = value;
                return value;
            }
        }

        private static List<AlbSelectListItem> _lstSituations;
        public static List<AlbSelectListItem> LstSituations
        {
            get
            {
                //Si les données ont déjà été chargé en mémoire, on renvoit une nouvelle instance de la liste
                if (_lstSituations != null)
                {
                    var toReturn = new List<AlbSelectListItem>();
                    _lstSituations.ForEach(elm => toReturn.Add(new AlbSelectListItem
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
                value.Add(new AlbSelectListItem { Text = "Toutes", Title = "Toutes", Value = "Toutes", Selected = true });
                value.Add(new AlbSelectListItem { Text = "Non soldées", Title = "Non soldées", Value = "NonSoldees", Selected = false });
                value.Add(new AlbSelectListItem { Text = "Non annulée Non Annulation", Title = "Non annulée Non Annulation", Value = "NonAnnuleeNonAnnulation", Selected = false });
                value.Add(new AlbSelectListItem { Text = "Soldées ou Acompte", Title = "Soldées ou Acompte", Value = "SoldeesOuAcompte", Selected = false });

                _lstSituations = value;
                return value;
            }
        }

        public static List<AlbSelectListItem> LstTypesCopie
        {
            get
            {
                //Nouvelle instance à chaque récupération de la référence
                List<AlbSelectListItem> value = new List<AlbSelectListItem>();
                value.Add(new AlbSelectListItem() { Value = "C", Text = "Copie" });
                value.Add(new AlbSelectListItem() { Value = "D", Text = "Duplicata" });
                return value;
            }
        }

        [ErrorHandler]
        [WhitespaceFilter]
        public ActionResult Index(string id)
        {
            model.PageTitle = "Synthèse des cotisations";
            id = InitializeParams(id);
            LoadInfoPage(id);
            return View(model);
        }

        [HttpGet]
        [ErrorHandler]
        public JsonResult GetImpayes(int page = 0, int codeAssure = 0)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var pagingListDto = client.Channel.GetImpayes(page, codeAssure);
                var pagingList = new PagingList<Impaye>
                {
                    NbTotalLines = pagingListDto.NbTotalLines,
                    PageNumber = pagingListDto.PageNumber,
                    List = pagingListDto.List.Adapt<List<Impaye>>(),
                    Totals = pagingListDto.Totals
                };
                return JsonNetResult.NewResultToGet(pagingList);
            }
        }

        [HttpGet]
        [ErrorHandler]
        public JsonResult GetRetardsPaiement(int page = 0, int codeAssure = 0)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var pagingListDto = client.Channel.GetRetardsPaiement(page, codeAssure);
                var pagingList = new PagingList<RetardPaiement>
                {
                    NbTotalLines = pagingListDto.NbTotalLines,
                    PageNumber = pagingListDto.PageNumber,
                    List = pagingListDto.List.Adapt<List<RetardPaiement>>(),
                    Totals = pagingListDto.Totals
                };
                return JsonNetResult.NewResultToGet(pagingList);
            }
        }

        [ErrorHandler]
        public ActionResult GetCotisation(string codeOffre, string version, string type, string codeAvn, string modeNavig, string acteGestion,
            string acteGestionRegule, string reguleId, string tabGuid, string numQuittanceVisu = "")
        {
            LoadQuittances(codeOffre, version, type, codeAvn, modeNavig, acteGestion, acteGestionRegule, reguleId, tabGuid, launchPGM: false, modeAffichage: "Visu", numQuittanceVisu: numQuittanceVisu);
            model.ModeNavig = modeNavig;
            model.IsReadOnly = !string.IsNullOrEmpty(numQuittanceVisu);
            model.ActeGestionRegule = acteGestionRegule;
            return PartialView("BodyQuittances", model);
        }

        [ErrorHandler]
        [WhitespaceFilter]
        public ActionResult GetDetailsCotisation(string codeOffre, string version, string type, string codeAvn, string modeNavig, string acteGestion, string acteGestionRegule, string reguleId, string tabGuid, string modeAffichage = "", string numQuittanceVisu = "")
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffreClient = client.Channel;
                if (!string.IsNullOrEmpty(numQuittanceVisu))
                {
                    var codeAvnQuittance = finOffreClient.GetCodeAvnQuittance(codeOffre, version, numQuittanceVisu);
                    if (codeAvn != codeAvnQuittance)
                    {
                        codeAvn = codeAvnQuittance;
                        modeNavig = ModeConsultation.Historique.AsCode();
                    }
                }


                DetailsQuittance toReturn = new DetailsQuittance();
                var isreadonly = GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn);
                //Informations détails
                var quittanceDetailDto = finOffreClient.GetQuittanceDetail(codeOffre, version, codeAvn, modeNavig.ParseCode<ModeConsultation>(), !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion, modeAffichage, numQuittanceVisu);
                if (quittanceDetailDto != null)
                    toReturn.LoadInformationsDetails(quittanceDetailDto);
                //Totaux et Commission
                var quittancesDto = finOffreClient.GetQuittances(codeOffre, version, type, codeAvn, modeNavig.ParseCode<ModeConsultation>(), modeAffichage, numQuittanceVisu, false, false, GetUser(), !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion, reguleId, isreadonly, string.Empty);
                toReturn.Totaux = ModeleQuittancePage.LoadQuittanceTotaux(quittancesDto);
                toReturn.Commission = ModeleQuittancePage.LoadQuittanceCommission(quittancesDto);
                toReturn.ModeAffichage = modeAffichage;
                toReturn.NumQuittanceVisu = numQuittanceVisu;
                return PartialView("DetailsQuittance", toReturn);
            }
        }

        [ErrorHandler]
        [WhitespaceFilter]
        public ActionResult GetFraisAccessoires(string codeOffre, string version, string type, string codeAvn, int effetAnnee, string commentaire,
            string tabGuid, bool isReadonly, string modeNavig, string acteGestion, string acteGestionRegule, string reguleId, bool isModifHorsAvn)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffreClient = client.Channel;
                var result = finOffreClient.InitFraisAccessoire(codeOffre, version, type, codeAvn, effetAnnee, isReadonly, modeNavig.ParseCode<ModeConsultation>(), GetUser(), !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion, reguleId, isModifHorsAvn);
                var toReturn = (FraisAccessoires)result;
                string id = codeOffre + "_" + version + "_" + type + tabGuid;
                //toReturn.IsReadOnly = GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn);
                toReturn.IsReadOnly = isReadonly;
                toReturn.IsModifHorsAvn = isModifHorsAvn;
                toReturn.Commentaires = commentaire;//assignation du commentaire de l'écran principal
                //toReturn.TabGuid = tabGuid.Contains("tabGuid") ? tabGuid.Split(new[] { "tabGuid" }, StringSplitOptions.None)[1] : string.Empty; ;
                if (type == AlbConstantesMetiers.TYPE_CONTRAT)
                {
                    return PartialView("FraisAccessoires", toReturn);
                }
                else
                {
                    return PartialView("/Views/Cotisations/CotisationsFraisAccessoires.cshtml", toReturn);
                }

            }
        }

        [ErrorHandler]
        [WhitespaceFilter]
        public ActionResult GetFraisAccessoiresAvn(string codeOffre, string version, string type, string codeAvn, int effetAnnee, string commentaire, string tabGuid,
            bool isReadonly, string modeNavig, string acteGestion, string acteGestionRegule, string reguleId, string sourceAnnQuitt, bool isModifHorsAvn, bool isEntete = false, string numQuittanceVisu = "")
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffreClient = client.Channel;
                if (!string.IsNullOrEmpty(numQuittanceVisu))
                {
                    var codeAvnQuittance = finOffreClient.GetCodeAvnQuittance(codeOffre, version, numQuittanceVisu);
                    if (codeAvn != codeAvnQuittance)
                    {
                        codeAvn = codeAvnQuittance;
                        modeNavig = ModeConsultation.Historique.AsCode();
                    }
                }
                string folder = string.Format("{0}_{1}_{2}", codeOffre, version, type);
                var result = finOffreClient.InitFraisAccessoire(codeOffre, version, type, codeAvn, effetAnnee, isReadonly, modeNavig.ParseCode<ModeConsultation>(), GetUser(), !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion, reguleId, isModifHorsAvn);
                var toReturn = (FraisAccessoires)result;

                toReturn.IsReadOnly = GetIsReadOnly(tabGuid, folder, codeAvn);
                toReturn.IsModifHorsAvn = isModifHorsAvn;

                toReturn.Commentaires = commentaire;
                if (isEntete || sourceAnnQuitt == "true")
                {
                    toReturn.IsReadOnly = true;
                }

                toReturn.ActeGestion = acteGestion;
                toReturn.ActeGestionRegule = acteGestionRegule;
                return PartialView("FraisAccessoiresQuittance", toReturn);

            }
        }

        [ErrorHandler]
        public ActionResult UpdateFraisAccessoiresAvn(string codeOffre, string version, string type, string codeAvn, string acteGestion, string acteGestionRegule, string reguleId,
            string tabGuid, int fraisRetenus, bool taxeAttentat, string modeNavig)
        {
            var folder = string.Format("{0}_{1}_{2}", codeOffre, version, type);
            var isReadOnly = GetIsReadOnly(tabGuid, folder, codeAvn);
            var isModifHorsAvn = GetIsModifHorsAvn(tabGuid, string.Format("{0}_{1}", folder, string.IsNullOrEmpty(codeAvn) ? "0" : codeAvn));
            model.IsReadOnly = isReadOnly;
            model.IsModifHorsAvenant = isModifHorsAvn;
            if ((!model.IsReadOnly || isModifHorsAvn) && ModeConsultation.Historique != modeNavig.ParseCode<ModeConsultation>())
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    client.Channel.UpdateFraisAccessoiresAvn(codeOffre, version, type, codeAvn, !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion, reguleId, GetUser(), fraisRetenus, taxeAttentat);
                }
            }
            model.TypeAvt = acteGestion;
            model.ReguleId = reguleId;
            model.ActeGestion = acteGestion;
            model.ActeGestionRegule = acteGestionRegule;

            LoadQuittances(codeOffre, version, type, codeAvn, modeNavig, acteGestion, acteGestionRegule, reguleId, tabGuid, isFGACocheIHM: taxeAttentat ? "O" : "N");
            return PartialView("BodyQuittances", model);
        }

        private void Report_Regul_Yprtrsq_Yprtent(string codeOffre, string version, string codeAvn)
        {
            if (!model.IsForceReadOnly && !model.IsReadOnly)
            {
                using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<OPServiceContract.IRegularisation>())
                {
                    var reguleClient = chan.Channel;

                    reguleClient.ReportDataRegulFromRsqToEnt(codeOffre, version, codeAvn);
                }
            }
        }

        [ErrorHandler]
        public ActionResult UpdateFraisAccessoires(string codeOffre, string version, string type, string codeAvn, int effetAnnee, string typeFrais, int fraisRetenus,
            bool taxeAttentat, /*int fraisSpecifiques,*/ long codeCommentaires, string commentaires, string tabGuid, string modeNavig, string acteGestion, string acteGestionRegule, string reguleId)
        {
            var folder = string.Format("{0}_{1}_{2}", codeOffre, version, type);
            var isReadOnly = GetIsReadOnly(tabGuid, folder, codeAvn);
            var isModifHorsAvn = GetIsModifHorsAvn(tabGuid, String.Format("{0}_{1}", folder, string.IsNullOrEmpty(codeAvn) ? "0" : codeAvn));
            model.IsReadOnly = isReadOnly;
            model.IsModifHorsAvenant = isModifHorsAvn;
            if (!isReadOnly || isModifHorsAvn)
            {
                if (!string.IsNullOrEmpty(commentaires))
                {
                    commentaires = commentaires.Replace("\r\n", "<br>").Replace("\n", "<br>");
                }
                else
                {
                    commentaires = string.Empty;
                }
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    var finOffreClient = client.Channel;
                    var retourMessage = finOffreClient.UpdateFraisAccessoires(codeOffre, version, type, effetAnnee, typeFrais, fraisRetenus, taxeAttentat, codeCommentaires, commentaires, codeAvn, GetUser(), !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion, isModifHorsAvn);
                    if (!string.IsNullOrEmpty(retourMessage))
                        throw new AlbFoncException(retourMessage);
                }
            }

            // WI - 2330: Report de YPRTRSQ vers YPRTENT
            //Report_Regul_Yprtrsq_Yprtent(codeOffre, version, codeAvn);
            // WI - 2330 : Fin

            LoadQuittances(codeOffre, version, type, codeAvn, modeNavig, acteGestion, acteGestionRegule, reguleId, tabGuid, isFGACocheIHM: taxeAttentat ? "O" : "N");
            model.ActeGestionRegule = acteGestionRegule;
            return PartialView("BodyQuittances", model);
        }

        [ErrorHandler]
        public JsonResult GenerateClauses(string codeOffre, string version, string type, string periodeDebut, string periodeFin, string totalHorsFraisHT, string fraisHT, string fgaTaxe, string tabGuid,
            string paramRedirect, string modeNavig, string acteGestion, string addParamType, string addParamValue, string acteGestionRegule, string saveAndQuit)
        {
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            var isReadonly = GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, numAvn);
            var avnType = GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);

            if (!isReadonly
                     && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>())
            {
                var EtapeGeneration = string.Empty;
                var Perimetre = string.Empty;
                EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Cotisation);

                if (acteGestionRegule != AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF && acteGestionRegule != AlbConstantesMetiers.TYPE_AVENANT_REGUL)
                {
                    if (acteGestion == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Resiliation))
                    {
                        Perimetre = acteGestion;
                    }
                    else
                    {
                        Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Cotisation);
                    }
                }
                else
                {
                    Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Regule);
                }
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                {
                    var serviceContext = channelClient.Channel;
                    RemoveControlAssiette(codeOffre, version, type);
                    serviceContext.SetTrace(new TraceDto
                    {
                        CodeOffre = codeOffre.PadLeft(9, ' '),
                        Version = Convert.ToInt32(version),
                        Type = type,
                        EtapeGeneration = EtapeGeneration,
                        NumeroOrdreDansEtape = 64,
                        NumeroOrdreEtape = 1,
                        Perimetre = Perimetre,
                        Risque = 0,
                        Objet = 0,
                        IdInventaire = 0,
                        Formule = 0,
                        Option = 0,
                        Niveau = string.Empty,
                        CreationUser = GetUser(),
                        PassageTag = "O",
                        PassageTagClause = string.Empty
                    });
                }

                using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    var serviceontext = chan.Channel;
                    AlbConstantesMetiers.Etapes etape = GetEtape(acteGestionRegule, avnType);
                    if (avnType == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.RemiseEnVigueur))
                    {
                        var avt = BLCommon.GetInfoAvenant(codeOffre: codeOffre,
                            version: version,
                            type: type,
                            codeAvn: numAvn,
                            typeAvt: avnType,
                            modeAvt: "UPDATE",
                            user: "",
                            modeNavig: "S");

                        if (avt.AvenantRemiseEnVigueur.TypeGestion == "M")
                        {
                            etape = AlbConstantesMetiers.Etapes.Fin;
                        }
                    }
                    RetGenClauseDto retGenClause = serviceontext.GenerateClause(type, codeOffre, Convert.ToInt32(version),
                      new ParametreGenClauseDto
                      {
                          ActeGestion = "**",
                          Letape = AlbEnumInfoValue.GetEnumInfo(etape)
                      });
                    if (retGenClause != null && !string.IsNullOrEmpty(retGenClause.MsgErreur))
                    {
                        throw new AlbFoncException(retGenClause.MsgErreur);
                    }
                    if (retGenClause.ListChoixClauses.Any() && retGenClause.ListChoixClauses.Count > 1)
                    {
                        return JsonNetResult.NewResultToGet(retGenClause.ListChoixClauses);
                    }
                }
            }

            return null;
        }

        private static AlbConstantesMetiers.Etapes GetEtape(string acteGestionRegule, string avnType)
        {
            switch ((avnType, acteGestionRegule))
            {
                case var s when s.avnType == AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                    return AlbConstantesMetiers.Etapes.Resiliation;
                case var s when s.avnType == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF && s.acteGestionRegule == AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                    return AlbConstantesMetiers.Etapes.Regule;
                case var s when s.avnType == AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                    return AlbConstantesMetiers.Etapes.Regule;
                case var s when s.avnType == AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                    return AlbConstantesMetiers.Etapes.RemiseEnVigueur;
                default:
                    return AlbConstantesMetiers.Etapes.Fin;
            }
        }
        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string periodeDebut, string periodeFin, string totalHorsFraisHT, string fraisHT, string fgaTaxe, string tabGuid,
            string paramRedirect, string modeNavig, string acteGestion, string addParamType, string addParamValue, string acteGestionRegule, string saveAndQuit)
        {
            Folder folder = new Folder { CodeOffre = codeOffre, Version = int.Parse(version), Type = type };
            string id = AlbParameters.BuildStandardId(
                folder,
                tabGuid,
                addParamValue,
                modeNavig);


            var parameters = AlbParameters.Parse(id);
            var numAvn = parameters.NumeroAvenant; // GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            var isReadonly = GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, numAvn.ToString());
            var avnType = parameters.TypeAvenant; //GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);

            folder = new Folder { CodeOffre = codeOffre, Version = int.Parse(version), Type = type, NumeroAvenant = numAvn ?? 0 };

            if (cible == "AnMontantReference") //bouton Annuler
            {
                return TraiteRedirectionAnnuler(cible, job, addParamValue, acteGestionRegule, id, parameters, folder);
            }

            CheckEcheancierAndRepartition(codeOffre, version, type, modeNavig, acteGestion, numAvn, isReadonly, avnType);

            if (!string.IsNullOrEmpty(paramRedirect))
            {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }

            if (cible == "RechercheSaisie")
            {
                return RedirectToAction(job, cible);
            }

            if (cible == "ChoixClauses" && acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGUL)
            {
                cible = "ControleFin";
            }

            if (cible == "ChoixClauses")
            {
                return RedirectToAction("Index", "ChoixClauses", new
                {
                    id = AlbParameters.BuildFullId(
                    new Folder(new[] { codeOffre, version, type }),
                    new[] { "¤Quittance¤Index¤" + codeOffre + "£" + version + "£" + type },
                    tabGuid,
                    addParamValue,
                    modeNavig),
                    returnHome = saveAndQuit,
                    guidTab = tabGuid
                });
            }

            if (cible == "ControleFin")
            {
                var test = AlbParameters.BuildFullId(
                    new Folder(new[] { codeOffre, version, type }),
                    new[] { "G" },
                    tabGuid,
                    addParamValue,
                    modeNavig);
                return RedirectToAction("Index", "ControleFin", new
                {
                    id = AlbParameters.BuildFullId(
                    new Folder(new[] { codeOffre, version, type }),
                    new[] { "G" },
                    tabGuid,
                    addParamValue,
                    modeNavig),
                    returnHome = saveAndQuit,
                    guidTab = tabGuid
                });
            }

            //Redirection de l'avenant de résiliation//
            if (avnType == AlbConstantesMetiers.TYPE_AVENANT_RESIL)
            {
                // déjà testé
                //if (cible == "AnMontantReference")
                //    return RedirectToAction(job, "EngagementPeriodes", new { id });
                //else
                return RedirectToAction(job, "DocumentGestion", new { id });
            }

            // default case
            return RedirectToAction(job, cible, new { id });
        }

        private RedirectToRouteResult TraiteRedirectionAnnuler(string cible, string job, string addParamValue, string acteGestionRegule, string id, AlbParameters parameters, Folder folder)
        {
            var numAvn = parameters.NumeroAvenant;
            var avnType = parameters.TypeAvenant;
            long? reguleId = parameters.IdRegularisation;
            string modeRegul = parameters.ModeRegularisation;



            if (avnType == AlbConstantesMetiers.TYPE_AVENANT_RESIL)
            {
                return RedirectToAction(job, "EngagementPeriodes", new { id });
            }
            if (avnType == AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR)
            {
                var affaire = GetInfoBaseAffaire(folder.CodeOffre, folder.Version.ToString(), folder.Type, folder.NumeroAvenant.ToString(), parameters.ModeNavig.AsCode());
                if (affaire.IsRemiseEnVigeurSansModif)
                {
                    return RedirectToAction(job, "CreationAvenant", new { id });
                }
            }
            if (
                reguleId.HasValue &&
                !string.IsNullOrEmpty(modeRegul) &&
                acteGestionRegule == AlbConstantesMetiers.TYPE_AVENANT_REGUL &&
                (
                    avnType == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF
                    || avnType == AlbConstantesMetiers.TYPE_AVENANT_REGUL
                )
            )
            {
                // ARA - Condition de redirection du bouton précédent
                // Récupération du mode de régularisation
                if (modeRegul == "STAND" || modeRegul == "PB")
                {
                    return RedirectionAnnulRegulStandard(addParamValue, id, parameters, reguleId, modeRegul);
                }
                else
                {
                    // PB/BNS/BURNER
                    return RedirectionAnnulRegulNonStandard(addParamValue, id, reguleId, modeRegul);

                }

            }

            return RedirectToAction(job, cible, new { id });

        }

        private RedirectToRouteResult RedirectionAnnulRegulNonStandard(string addParamValue, string id, long? reguleId, string modeRegul)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                string lotId = GetAddParamValue(addParamValue, AlbParameterName.LOTID);
                var serviceContext = client.Channel;
                if (reguleId.HasValue && reguleId > 0L)
                {
                    var result = serviceContext.GetListRsqRegule(lotId, reguleId.ToString(), modeRegul);
                    if (result != null)
                    {
                        if (result.Risques != null && result.Risques.Count() > 1)
                        {
                            // Multi-risque
                            return RedirectToAction("CheckListRisques", "Regularisation", new { id });
                        }
                        else
                        {
                            return RedirectToAction("Step5_RegulContrat", "CreationRegularisation", new { id });
                        }
                    }
                }
            }
            return RedirectToAction("Index", "CreationRegularisation", new { id });
        }

        private RedirectToRouteResult RedirectionAnnulRegulStandard(string addParamValue, string id, AlbParameters parameters, long? reguleId, string modeRegul)
        {
            long? lotId = parameters.IdLot; //GetAddParamValue(addParamValue, AlbParameterName.LOTID);
            if (lotId.HasValue)
            {
                // Mode standard
                // Récupération de la liste des risques du régule
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var serviceContext = client.Channel;
                    var result = serviceContext.GetListRsqRegule(lotId.ToString(), reguleId.ToString(), modeRegul);
                    if (result != null)
                    {
                        if (result.Risques != null && result.Risques.Count() > 1)
                        {
                            // Multirisque
                            // Redirection : Liste des risques
                            return RedirectToAction("Step2_ChoixRisque", "CreationRegularisation", new { id });
                        }
                        else
                        {
                            // Mono risque / Multi garanties
                            var codeRsq = GetAddParamValue(addParamValue, AlbParameterName.RSQID);
                            var resultGar = serviceContext.GetListGarRegule(lotId.ToString(), reguleId.ToString(), codeRsq, false);
                            if (resultGar != null)
                            {
                                if (resultGar.Garanties != null && resultGar.Garanties.Count() > 1)
                                {
                                    // Liste des risques
                                    return RedirectToAction("Step3_ChoixGarantie", "CreationRegularisation", new { id });
                                }
                                else
                                {
                                    // Mono risque/ Mono garantie
                                    return RedirectToAction("Step4_ChoixPeriodeGarantie", "CreationRegularisation", new { id });
                                }
                            }
                        }
                    }
                }
            }
            return RedirectToAction("Index", "CreationRegularisation", new { id });
        }

        private static void CheckEcheancierAndRepartition(string codeOffre, string version, string type, string modeNavig, string acteGestion, int? numAvn, bool isReadonly, string avnType)
        {
            if (
                !(
                    isReadonly
                    || avnType == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF
                    || avnType == AlbConstantesMetiers.TYPE_AVENANT_REGUL
                )
            )
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    var finOffreClient = client.Channel;
                    var infoCompQuittance = finOffreClient.GetInfoComplementairesQuittance(codeOffre, version, type, numAvn.ToString(), modeNavig.ParseCode<ModeConsultation>(), acteGestion, isReadonly, GetUser(), true);
                    if (infoCompQuittance != null)
                    {
                        if ((infoCompQuittance.IsEcheanceNonTraite || infoCompQuittance.Periodicite == "E") && infoCompQuittance.TypeCalcul != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.TypesCalcul.Comptant))
                        {
                            throw new AlbFoncException("Vous devez gérer l'échéancier");
                        }
                        if (infoCompQuittance.IsEcheanceNonTraite || infoCompQuittance.Periodicite == "E")
                        {
                            //Verif cohérence échéancier
                            if (infoCompQuittance.PourcentRepartition != 100)
                            {
                                if (infoCompQuittance.PourcentRepartitionCalc != 100)
                                    throw new AlbFoncException("Veuillez modifier l'échéancier, ventilation invalide");
                            }
                        }
                    }
                }
            }
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult RedirectionChoixClauses(string codeOffre, string version, string type, string idClause, string idLot, string tabGuid, string modeNavig, string addParamValue, string saveAndQuit, string json)
        {
            var res = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ClauseOpChoixDto>>(json);
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);

            using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext = chan.Channel;
                var ret = serviceContext.ValiderChoixClause(type, codeOffre, Convert.ToInt32(version), numAvn.IsEmptyOrNull() ? 0 : Convert.ToInt32(numAvn), Convert.ToInt32(idClause), idLot, res, GetUser());
                if (ret != "1")
                {
                    throw new AlbFoncException("Erreur lors de la validation du choix de clause");
                }
            }
            return RedirectToAction("Index", "ChoixClauses", new
            {
                id = AlbParameters.BuildFullId(
                    new Folder(new[] { codeOffre, version, type }),
                    new[] { "¤Quittance¤Index¤" + codeOffre + "£" + version + "£" + type },
                    tabGuid,
                    addParamValue,
                    modeNavig),
                returnHome = saveAndQuit,
                guidTab = tabGuid
            });
        }

        [ErrorHandler]
        public ActionResult GetVentilationDetaillee(string codeOffre, string version, string type, string codeAvn, string natureContrat, string modeNavig, string acteGestion, string acteGestionRegule, string modeAffichage = "", string numQuittanceVisu = "")
        {
            VentilationDetaillee toReturn = new VentilationDetaillee();
            toReturn.Garanties = new List<VentilationDetailleeGarantie>();
            toReturn.Taxes = new List<VentilationDetailleeTaxe>();
            toReturn.NatureContrat = natureContrat;

            if (!string.IsNullOrEmpty(toReturn.NatureContrat))
            {
                if (toReturn.NatureContrat == "A" || toReturn.NatureContrat == "E")
                    toReturn.ComplementTitre = "Toutes les valeurs correspondent à la part totale";
                if (toReturn.NatureContrat == "C" || toReturn.NatureContrat == "D")
                    toReturn.ComplementTitre = "Toutes les valeurs correspondent à la part Albingia";
            }

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffreClient = client.Channel;
                if (!string.IsNullOrEmpty(numQuittanceVisu))
                {
                    var codeAvnQuittance = finOffreClient.GetCodeAvnQuittance(codeOffre, version, numQuittanceVisu);
                    if (codeAvn != codeAvnQuittance)
                    {
                        codeAvn = codeAvnQuittance;
                        modeNavig = ModeConsultation.Historique.AsCode();
                    }
                }

                var garantiesDto = finOffreClient.GetQuittanceVentilationDetailleeGaranties(codeOffre, version, type, modeNavig.ParseCode<ModeConsultation>(), modeAffichage, numQuittanceVisu, !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion);
                garantiesDto.ForEach(m => toReturn.Garanties.Add((VentilationDetailleeGarantie)m));

                var taxesDto = finOffreClient.GetQuittanceVentilationDetailleeTaxes(codeOffre, version, type, modeNavig.ParseCode<ModeConsultation>(), modeAffichage, numQuittanceVisu, !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion);
                taxesDto.ForEach(m => toReturn.Taxes.Add((VentilationDetailleeTaxe)m));
            }
            toReturn.ModeAffichage = modeAffichage;
            toReturn.NumQuittanceVisu = numQuittanceVisu;

            return PartialView("VentilationDetaillee", toReturn);
        }

        [ErrorHandler]
        [WhitespaceFilter]
        public ActionResult GetVentilationCommission(string codeOffre, string version, string type, string codeAvn, string natureContrat, string modeNavig, string acteGestion, string acteGestionRegule, string modeAffichage = "", string numQuittanceVisu = "")
        {
            VentilationCommission toReturn = new VentilationCommission();
            toReturn.Courtiers = new List<VentilationCommissionCourtier>();
            toReturn.NatureContrat = natureContrat;
            //TODO :  toReturn.Garanties

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffreClient = client.Channel;
                if (!string.IsNullOrEmpty(numQuittanceVisu))
                {
                    var codeAvnQuittance = finOffreClient.GetCodeAvnQuittance(codeOffre, version, numQuittanceVisu);
                    if (codeAvn != codeAvnQuittance)
                    {
                        codeAvn = codeAvnQuittance;
                        modeNavig = ModeConsultation.Historique.AsCode();
                    }
                }

                var courtiersDto = finOffreClient.GetQuittanceVentilationCommissionCourtiers(codeOffre, version, type, modeNavig.ParseCode<ModeConsultation>(), modeAffichage, numQuittanceVisu, !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion);
                courtiersDto.ForEach(m => toReturn.Courtiers.Add((VentilationCommissionCourtier)m));
            }
            toReturn.ModeAffichage = modeAffichage;
            toReturn.NumQuittanceVisu = numQuittanceVisu;

            return PartialView("VentilationCommission", toReturn);
        }

        [ErrorHandler]
        public ActionResult GetTabPartAlbingia(string codeOffre, string version, string type, string codeAvn, string natureContrat, string modeNavig, string acteGestion, string acteGestionRegule, string modeAffichage = "", string numQuittanceVisu = "")
        {
            PartAlbingia toReturn = null;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffreClient = client.Channel;
                if (!string.IsNullOrEmpty(numQuittanceVisu))
                {
                    var codeAvnQuittance = finOffreClient.GetCodeAvnQuittance(codeOffre, version, numQuittanceVisu);
                    if (codeAvn != codeAvnQuittance)
                    {
                        codeAvn = codeAvnQuittance;
                        modeNavig = ModeConsultation.Historique.AsCode();
                    }
                }

                var result = finOffreClient.GetQuittancePartAlbingia(codeOffre, version, type, codeAvn, modeNavig, !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion, modeAffichage, numQuittanceVisu);
                if (result != null)
                {
                    toReturn = (PartAlbingia)result;
                }
            }

            if (toReturn == null)
            {
                toReturn = new PartAlbingia();
                toReturn.ListeGaranties = new List<Garanties>();
            }

            toReturn.ModeAffichage = modeAffichage;
            toReturn.NumQuittanceVisu = numQuittanceVisu;
            toReturn.NatureContrat = natureContrat;

            return PartialView("PartAlbingia", toReturn);
        }

        [ErrorHandler]
        public ActionResult GetTabVentilationCoassureurs(string codeOffre, string version, string type, string natureContrat, string modeNavig, string acteGestion, string acteGestionRegule, string modeAffichage = "", string numQuittanceVisu = "")
        {
            VentilationCoassureur toReturn = new VentilationCoassureur();
            toReturn.ListeCoAssureurs = new List<CoAssureurGarantie>();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffreClient = client.Channel;
                var result = finOffreClient.GetQuittanceVentilationCoassureurs(codeOffre, version, type, modeAffichage, numQuittanceVisu, !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion);
                if (result != null && result.Any())
                {
                    result.ForEach(elm => toReturn.ListeCoAssureurs.Add((CoAssureurGarantie)elm));
                }
            }

            toReturn.ModeAffichage = modeAffichage;
            toReturn.NumQuittanceVisu = numQuittanceVisu;
            toReturn.NatureContrat = natureContrat;

            return PartialView("VentilationCoassureur", toReturn);
        }

        [ErrorHandler]
        public ActionResult GetVentilationCoassureurGaranties(string codeOffre, string version, string type, string codeCoAssureur, string libelleCoAssureur, double partCoAssureur, string acteGestion, string acteGestionRegule, string modeAffichage = "", string numQuittanceVisu = "")
        {
            VentilationCoassureurParGarantie toReturn = new VentilationCoassureurParGarantie { Code = codeCoAssureur, Libelle = libelleCoAssureur, Part = partCoAssureur };
            toReturn.ListeGaranties = new List<CoAssureurGarantie>();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffreClient = client.Channel;
                var result = finOffreClient.GetQuittanceVentilationCoassureursParGarantie(codeOffre, version, type, codeCoAssureur, modeAffichage, numQuittanceVisu, !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion);
                if (result != null && result.Any())
                {
                    result.ForEach(elm => toReturn.ListeGaranties.Add((CoAssureurGarantie)elm));
                }
            }

            return PartialView("VentilationCoassureurParGarantie", toReturn);
        }

        [ErrorHandler]
        public ActionResult LoadCoCourtiers(string codeOffre, string version, string type, string codeAvn, string tabGuid, string modeNavig, string acteGestion, string acteGestionRegule, string modeAffiche, bool forceReadOnly)
        {
            AnCourtier toReturn = LoadCoCourtiersByPopup(codeOffre + "_" + version + "_" + type + "_" + codeAvn + tabGuid + GetFormatModeNavig(modeNavig), GetUser(), !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion, forceReadOnly);

            toReturn.LoadedFromQuittance = true;
            toReturn.modeAffiche = modeAffiche;
            return PartialView("/Views/AnCourtier/Index.cshtml", toReturn);
        }

        [ErrorHandler]
        public string CheckCoCourtier(string codeOffre, string version, string type, string codeAvn, bool isReadonly, string modeNavig, string acteGestion, string acteGestionRegule)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = channelClient.Channel;
                CommissionCourtierDto varComm = serviceContext.GetCommissionsStandardCourtier(codeOffre, version, type, codeAvn, isReadonly, modeNavig.ParseCode<ModeConsultation>(), GetUser(), !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion);
                if (varComm != null)
                {

                    if (acteGestionRegule != AlbConstantesMetiers.TYPE_AVENANT_REGUL)
                    {
                        if (varComm.EchError == 1)
                            return ErrorEcheance;
                        if (varComm.EchError == 2)
                            return ErrorTotalEcheance;
                        if (varComm.EchError == 3)
                            return ErrorDateEcheance;
                    }
                    var tauxCatUsed = (varComm.IsStandardCAT == "O" ? varComm.TauxStandardCAT : varComm.TauxContratCAT);
                    var tauxHCatUsed = (varComm.IsStandardHCAT == "O" ? varComm.TauxStandardHCAT : varComm.TauxContratHCAT);

                    if (varComm.IsStandardCAT == "N" && varComm.TauxContratCAT == 0 && varComm.IsStandardHCAT == "N" && varComm.TauxContratHCAT == 0)
                        return WarningContratSansCommission;

                    if (varComm.IsStandardCAT == "N" && varComm.TauxContratCAT == 0 && varComm.IsStandardHCAT == "N" && varComm.TauxContratHCAT > 0)
                        return WarningCatnatSansCommission;

                    if (((varComm.IsStandardCAT == "N" && varComm.TauxStandardCAT != varComm.TauxContratCAT) || (varComm.IsStandardHCAT == "N" && varComm.TauxStandardHCAT != varComm.TauxContratHCAT)) && string.IsNullOrEmpty(varComm.Commentaires.Trim()))
                        return ErrorComment;

                }
            }
            return string.Empty;
        }
        [ErrorHandler]
        public void SupprimerEcheances(string codeOffre, string version, string type, string codeAvn, string tabGuid)
        {
            //Supprime uniquement si l'écran n'est pas en readonly
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    var finOffreClient = client.Channel;
                    finOffreClient.SupprimerEcheances(codeOffre, version, type);
                }
            }
        }
        [ErrorHandler]
        public void UpdatePeriodicite(string codeOffre, string version, string type, string codeAvn, string periodicite, string tabGuid)
        {
            //Supprime uniquement si l'écran n'est pas en readonly
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn))
            {
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var serviceContext = channelClient.Channel;
                    serviceContext.UpdatePeriodicite(codeOffre, version, type, periodicite);
                }
            }

        }

        [ErrorHandler]
        public void SaveComment(string codeOffre, string version, string type, string codeAvn, string comment, string acteGestion, string acteGestionRegule, string reguleId, string modifPeriod, string isModifDateFin, string dateDeb, string dateFin)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                modifPeriod = modifPeriod.IsEmptyOrNull() ? "" : modifPeriod;
                isModifDateFin = isModifDateFin.IsEmptyOrNull() ? "" : isModifDateFin;
                var serviceContext = channelClient.Channel;
                var updatePeriod = modifPeriod.Equals("1", StringComparison.InvariantCulture) || isModifDateFin.Equals("1", StringComparison.InvariantCulture) ? "1" : "0";
                serviceContext.SaveCommentQuittance(codeOffre, version, type, codeAvn, comment, !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion, reguleId, updatePeriod, dateDeb, dateFin);
            }
        }

        [ErrorHandler]
        public ActionResult OpenVisualisationQuittances(string codeOffre, string codeAvn, string version, bool isEntete, AlbConstantesMetiers.TypeQuittances typeQuittances, string modeNavig = "")
        {
            var model = GetVisualisationQuittancesModele(codeOffre, codeAvn, version, string.Empty, isEntete, typeQuittances, string.Empty, modeNavig: modeNavig);
            return PartialView("VisualisationQuittances", model);
        }

        [ErrorHandler]
        public ActionResult FiltrerVisualisationQuittances(string codeOffre, string codeAvn, string version, bool isEntete, DateTime? dateEmission, string typeOperation, string situation,
            DateTime? datePeriodeDebut, DateTime? datePeriodeFin, string tabGuid, string modeNavig = "", string colTri = "")
        {
            List<VisualisationQuittancesLigne> toReturn = GetListeQuittancesLignes(codeOffre, codeAvn, isEntete, version, string.Empty, tabGuid, dateEmission, typeOperation, situation, datePeriodeDebut, datePeriodeFin, modeNavig: modeNavig, colTri: colTri);
            return PartialView("VisualisationQuittancesListe", toReturn);
        }

        [ErrorHandler]
        public ActionResult GetReeditionBulletinAvis(string codeOffre, string version, string type, string codeAvn, string numQuittanceVisu)
        {
            ReeditionBulletinAvis toReturn = new ReeditionBulletinAvis { NumQuittance = numQuittanceVisu, ListeCopieDuplicata = LstTypesCopie, ValCopieDuplicata = "C" };
            return PartialView("ReeditionBulletinAvis", toReturn);
        }

        [ErrorHandler]
        public string LancerBulletinAvis(string codeOffre, string version, string type, string codeAvn, string numQuittanceVisu, string nbExemplaire, string typeCopie, bool isAvisEcheance)
        {
            string message = string.Empty;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffreClient = client.Channel;
                message = finOffreClient.LancerBulletinAvis(codeOffre, version, type, codeAvn, numQuittanceVisu, nbExemplaire, typeCopie, isAvisEcheance, GetUser());
                if (!string.IsNullOrEmpty(message) && message.ToLower().Contains("err"))
                    throw new AlbFoncException(message, trace: true, sendMail: true, onlyMessage: true);
            }
            return message;
        }

        #region Calcul Forcé

        [ErrorHandler]
        public ActionResult ReloadCotisation(string codeOffre, string version, string type, string codeAvn, string modeNavig, string acteGestion, string acteGestionRegule, string reguleId, bool loadPgm400, bool modeCalculForce, string tabGuid)
        {
            model.TypeAvt = acteGestion;
            model.ActeGestionRegule = acteGestionRegule;
            model.ReguleId = reguleId;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFormule>())
            {
                client.Channel.ComputeEachGareat(new Albingia.Kheops.OP.Domain.Affaire.AffaireId
                {
                    CodeAffaire = codeOffre,
                    IsHisto = false,
                    NumeroAliment = int.Parse(version),
                    TypeAffaire = type.ParseCode<Albingia.Kheops.OP.Domain.Affaire.AffaireType>()
                });
            }
            LoadQuittances(codeOffre, version, type, codeAvn, modeNavig, acteGestion, acteGestionRegule, reguleId, tabGuid, loadPgm400, modeCalculForce);
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = channelClient.Channel;
                model.Contrat = serviceContext.GetContrat(codeOffre, version, type, codeAvn, modeNavig.ParseCode<ModeConsultation>());
            }
            return PartialView("BodyQuittances", model);
        }

        [ErrorHandler]
        [WhitespaceFilter]
        public ActionResult LoadCalculWindow(string codeOffre, string version, string avenant, string typeVal, string typeHTTTC, string modeNavig, string acteGestion, string acteGestionRegule)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext = channelClient.Channel;
                var result = serviceContext.LoadCalculWindow(codeOffre, version, avenant, typeVal, typeHTTTC, modeNavig.ParseCode<ModeConsultation>(), !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion);
                if (result != null)
                {
                    var modele = (QuittanceCalculForce)result;


                    if (typeVal == "formule")
                    {
                        modele.ForceFormule.TypeHTTC = typeHTTTC;
                        return PartialView("CalculForceFormule", modele.ForceFormule);
                    }
                    if (typeVal == "montant")
                    {
                        modele.ForceTotal.TypeHTTC = typeHTTTC;
                        return PartialView("CalculForceTotalHT", modele.ForceTotal);
                    }
                }
                return null;
            }
        }

        [ErrorHandler]
        public string LoadMntCalcul(string codeOffre, string version, string avenant, string modeNavig, string acteGestion, string acteGestionRegule)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext = channelClient.Channel;
                var result = serviceContext.ExistMntCalcul(codeOffre, version, avenant, modeNavig.ParseCode<ModeConsultation>(), !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion);
                if (result != null)
                {
                    return result;
                }
                return string.Empty;
            }
        }



        [ErrorHandler]
        public string UpdateCalculForce(string codeOffre, string version, string type, string avenant, string typeVal, string typeHTTTC, string codeRsq, string codeFor, string montantForce,
            string acteGestion, string reguleId, string modeNavig, string tabGuid, string acteGestionRegule)
        {
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, avenant) && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>())
            {
                //Verif des paramètres
                double montantForceVal = 0;
                if (!double.TryParse(montantForce.Replace(".", ","), out montantForceVal))// || montantForceVal == 0) désactivation, voir bug 1628
                    throw new AlbFoncException("Le montant forcé saisi n'est pas correct");
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    var serviceContext = channelClient.Channel;
                    return serviceContext.UpdateCalculForce(codeOffre, type, version, avenant, typeVal, typeHTTTC, codeRsq, codeFor, montantForce.Replace(".", ","), GetUser(), !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion, reguleId);
                }
            }
            else
                return string.Empty;
        }

        [ErrorHandler]
        public ActionResult LoadGaranInfo(string codeOffre, string version, string avenant, string formId, string modeNavig, string acteGestion, string acteGestionRegule)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext = channelClient.Channel;
                var result = serviceContext.LoadGaranInfo(codeOffre, version, avenant, formId, modeNavig.ParseCode<ModeConsultation>(), !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion);
                if (result != null)
                {
                    var modele = (QuittanceForceGarantie)result;
                    modele.FormuleId = formId;
                    return PartialView("CalculForceGarantie", modele);
                }
                return null;
            }
        }

        [ErrorHandler]
        public ActionResult UpdateGaranForce(string codeOffre, string version, string avenant, string formId, string codeFor, string codeRsq, string codeGaran, string montantForce, string catnatForce, string codeTaxeForce, string modeNavig, string acteGestion, string acteGestionRegule)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext = channelClient.Channel;
                var result = serviceContext.UpdateGaranForce(codeOffre, version, avenant, formId, codeFor, codeRsq, codeGaran, montantForce, catnatForce, codeTaxeForce, modeNavig.ParseCode<ModeConsultation>(), !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion);
                if (result != null)
                {
                    var modele = (QuittanceForceGarantie)result;
                    modele.FormuleId = formId;
                    return PartialView("CalculForceGarantie", modele);
                }
                return null;
            }
        }

        [ErrorHandler]
        public string ValidFormGaranForce(string codeOffre, string version, string type, string avenant, string codeFor, string codeRsq, string acteGestion, string acteGestionRegule, string tabGuid, string modeNavig)
        {
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, avenant) && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>())
            {
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    var serviceContext = channelClient.Channel;
                    return serviceContext.ValidFormGaranForce(codeOffre, type, version, avenant, codeFor, codeRsq, GetUser(), !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion);
                }
            }
            else
                return string.Empty;
        }

        #endregion

        [ErrorHandler]
        public void GestionTraceAvt(string codeOffre, string version, string type, bool isChecked, string acteGestion, string acteGestionRegule)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext = channelClient.Channel;
                serviceContext.GestionTraceAvt(codeOffre, version, type, isChecked, GetUser(), !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion);
            }
        }

        protected override void ExtendNavigationArbreRegule(MetaModelsBase contentData)
        {
            RegularisationNavigator.StandardInitContext(model, RegularisationStep.Cotisations);
            if (model?.Context != null)
            {
                RegularisationNavigator.Initialize(contentData.NavigationArbre, model.Context);
            }
        }

        #region Méthode Privée

        private AnCourtier LoadCoCourtiersByPopup(string id, string user, string acteGestion, bool forceReadOnly)
        {
            AnCourtier toReturn = new AnCourtier();
            SetGuid(toReturn, id, out id);
            string codeOffre = id.Split('_')[0];
            string version = id.Split('_')[1];
            string type = id.Split('_')[2];
            string codeAvn = id.Split('_')[3];
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                var courtiersDto = serviceContext.GetListCourtiers(codeOffre, version, type, codeAvn, toReturn.ModeNavig.ParseCode<ModeConsultation>());
                var courtiers = Courtier.LoadCourtiers(courtiersDto);
                toReturn.Courtiers = courtiers;
                toReturn.Contrat = serviceContext.GetContrat(codeOffre, version, type, codeAvn, toReturn.ModeNavig.ParseCode<ModeConsultation>());

                if (toReturn.Contrat.Risques.Count > 0 && toReturn.Contrat.Risques[0].Objets.Count > 0)
                    toReturn.RisqueObj = toReturn.Contrat.Risques[0].Objets[0].Descriptif;
                if (courtiersDto != null && courtiersDto.Count > 0)
                {
                    toReturn.CourtierApporteur = courtiersDto[0].CodeApporteur;
                    toReturn.CourtierGestionnaire = courtiersDto[0].CodeGest;
                    toReturn.CourtierPayeur = courtiersDto[0].CodePayeur;
                }

                var folder = string.Format("{0}_{1}_{2}", toReturn.Contrat.CodeContrat, toReturn.Contrat.VersionContrat, toReturn.Contrat.Type);
                var tabGuid = string.Format("tabGuid{0}tabGuid", toReturn.TabGuid);

                toReturn.IsReadOnly = forceReadOnly || GetIsReadOnly(tabGuid, folder, codeAvn, isPopup: true);
                toReturn.IsModifHorsAvenant = GetIsModifHorsAvn(tabGuid, string.Format("{0}_{1}", folder, string.IsNullOrEmpty(codeAvn) ? "0" : codeAvn));

                toReturn.ModeAffichage = AnCourtierController.ModePopup;
                toReturn.Contexte = AnCourtierController.ModePopup;
                #region récupération des commissions de courtiers standard (cas de contrat non direct)
                if (toReturn.Contrat.CodeEncaissement != "D")
                    toReturn.CommissionsStandard = (CommissionCourtier)serviceContext.GetCommissionsStandardCourtier(codeOffre, version, type, codeAvn, toReturn.IsReadOnly, toReturn.ModeNavig.ParseCode<ModeConsultation>(), user, acteGestion);
                else
                    toReturn.CommissionsStandard = new CommissionCourtier();
                #endregion
            }
            return toReturn;
        }
        private VisualisationQuittances GetVisualisationQuittancesModele(string codeOffre, string codeAvn, string version, string etat, bool isEntete, AlbConstantesMetiers.TypeQuittances typeQuittances, string tabGuid, string modeNavig = "")
        {
            var toReturn = new VisualisationQuittances();
            toReturn.ListQuittances = GetListeQuittancesLignes(codeOffre, codeAvn, isEntete, version, etat, tabGuid, typeQuittances: typeQuittances, modeNavig: modeNavig);
            toReturn.Situations = LstSituations;
            toReturn.TypesOperation = LstTypesOperation;
            toReturn.IsOpenedFromHeader = isEntete;
            toReturn.IsHisto = modeNavig.ParseCode<ModeConsultation>() == ModeConsultation.Historique;
            toReturn.IsReadOnly = toReturn.IsHisto || isEntete;
            var folder = string.Format("{0}_{1}_{2}", codeOffre, version, AlbConstantesMetiers.TYPE_CONTRAT);
            var isModifHorsAvn = GetIsModifHorsAvn(tabGuid, string.Format("{0}_{1}", folder, string.IsNullOrEmpty(codeAvn) ? "0" : codeAvn));
            toReturn.IsModifHorsAvenant = isModifHorsAvn;
            return toReturn;
        }


        protected override void LoadInfoPage(string id)
        {
            var reguleId = string.Empty;

            var affaireId = model.AllParameters.Folder;
            affaireId.NumeroAvenant = int.TryParse(model.NumAvenantPage, out var avn) ? avn : 0;
            var codeAvn = model.NumAvenantPage;


            var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
            model.TypeAvt = typeAvt;
            model.ActeGestionRegule = GetAddParamValue(model.AddParamValue, AlbParameterName.ACTEGESTIONREGULE);
            var regulMode = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULMOD).ToUpper();

            var affaire = GetInfoBaseAffaire(model.CodePolicePage, model.VersionPolicePage, model.TypePolicePage, model.NumAvenant.ToString(), model.ModeNavig);

            if (affaireId.Type == "P")
            {

                switch (typeAvt)
                {
                    case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
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
                        reguleId = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULEID);

                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                        model.ScreenType = model.ActeGestionRegule == AlbConstantesMetiers.TYPE_AVENANT_REGUL ? AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF : AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                        reguleId = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULEID);
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR when affaire.IsRemiseEnVigeurSansModif:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR_NO_MODIF;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR when affaire.IsRemiseEnVigeurAvecModif:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                        break;
                    default:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                        reguleId = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULEID);
                        break;
                }
                model.ReguleId = reguleId;
            }



            if (typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGUL && (regulMode == "BNS" || regulMode == "BURNER"))
            {
                model.IsReadOnly = string.Compare(GetAddParamValue(model.AddParamValue, AlbParameterName.AVNMODE).Trim(), "CONSULT", true) == 0;
            }
            else
            {
                model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), affaireId.CodeOffre + "_" + affaireId.Version.ToString() + "_" + affaireId.Type, codeAvn);
            }

            LoadQuittances(affaireId.CodeOffre, affaireId.Version.ToString(), affaireId.Type, affaireId.NumeroAvenant.ToString(), model.ModeNavig, model.ActeGestion, model.ActeGestionRegule, reguleId, model.TabGuid);
            // Ajout : forcer la commission à 0 lorsque PB
            if (regulMode == "PB")
            {
                model.FormulesInfo.Commission.MontantCatNatRetenu = 0;
                model.FormulesInfo.Commission.MontantHRCatNatRetenu = 0;
                model.FormulesInfo.Commission.MontantTotalRetenu = 0;
            }

            if (affaireId.Type == "P")
            {
                if (model?.Contrat == null)
                {
                    using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                    {
                        var CommonOffreClient = chan.Channel;
                        var infosBase = CommonOffreClient.LoadaffaireNouvelleBase(affaireId.CodeOffre, affaireId.Version.ToString(), affaireId.Type, codeAvn, model.ModeNavig);
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
                            Delegation = infosBase?.CabinetGestionnaire?.Delegation?.Nom,
                            Inspecteur = infosBase?.CabinetGestionnaire?.Inspecteur
                        };
                    }
                }
            }
            if (model.Contrat != null)
            {
                model.AfficherBandeau = base.DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
                model.EffetAnnee = model.Contrat.DateEffetAnnee;
                model.PeriodiciteContrat = model.Contrat.PeriodiciteCode;
            }

            model.TypeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
            SetArbreNavigation();
            model.Bandeau = null;
            SetBandeauNavigation(id);
        }
        private void SetBandeauNavigation(string id)
        {
            if (model.AfficherBandeau)
            {
                model.Bandeau = GetInfoBandeau(model.AllParameters.Folder.Type);
                //Gestion des Etapes
                if (model.Contrat != null)
                {
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
                        Etape = model.ActeGestionRegule == AlbConstantesMetiers.TYPE_AVENANT_REGUL ? Navigation_MetaModel.ECRAN_REGULE : Navigation_MetaModel.ECRAN_COTISATIONS,
                        IdOffre = model.Contrat.CodeContrat,
                        Version = (int)model.Contrat.VersionContrat
                    };
                }
            }
        }
        private void SetArbreNavigation()
        {
            if (model.Contrat != null)
            {
                switch (model.TypeAvt)
                {
                    case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                        if (model.ActeGestionRegule == "REGUL")
                            model.NavigationArbre = GetNavigationArbreRegule(model, "Cotisation");
                        else
                            model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Cotisation");
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                        if (model.ActeGestionRegule == AlbConstantesMetiers.TYPE_AVENANT_MODIF)
                        {
                            model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Cotisation");
                            model.NavigationArbre.IsRegule = false;
                        }
                        if (model.ActeGestionRegule == AlbConstantesMetiers.TYPE_AVENANT_REGUL)
                        {
                            model.NavigationArbre = GetNavigationArbreRegule(model, "Cotisation");
                            model.NavigationArbre.IsRegule = true;
                        }
                        break;
                    default:
                        model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Cotisation");
                        break;
                }
            }
        }

        private void LoadQuittances(string codeOffre, string version, string type, string codeAvn, string modeNavig, string acteGestion, string acteGestionRegule, string idRegule, string tabGuid, bool launchPGM = true, bool modeCalculForce = false, string modeAffichage = "", string numQuittanceVisu = "", string isFGACocheIHM = "")
        {
            model.ModeAffichage = modeAffichage;
            model.ModeCalculForce = modeCalculForce;
            model.NumAvenantPage = codeAvn;
            model.NumQuittance = numQuittanceVisu;
            List<QuittanceDto> quittancesDto = null;
            ContratDto contrat = null;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                quittancesDto = client.Channel.GetQuittances(codeOffre, version, type, codeAvn, modeNavig.ParseCode<ModeConsultation>(), modeAffichage, numQuittanceVisu, launchPGM, modeCalculForce, GetUser(), !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion, idRegule, model.IsReadOnly, isFGACocheIHM);
            }
            this.model.LoadQuittances(quittancesDto);
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                contrat = client.Channel.GetContrat(codeOffre, version, type, codeAvn, model.ModeNavig.ParseCode<ModeConsultation>());
            }
            // Tester le cas de modification hors avenant
            model.IsModifHorsAvenant = GetIsModifHorsAvn(tabGuid, string.Format("{0}_{1}",
                                                                        string.Format("{0}_{1}_{2}",
                                                                        codeOffre, version, type),
                                                                        string.IsNullOrEmpty(codeAvn) ? "0" : codeAvn));
            model.ShowVisuQuittance = (model.IsReadOnly || model.IsModifHorsAvenant) && contrat.Etat == "V";

            model.Etat = contrat.Etat;
            if (model.ShowVisuQuittance)
            {
                string etat = base.model.Etat;
                var model = GetVisualisationQuittancesModele(codeOffre, codeAvn, version, etat, true, AlbConstantesMetiers.TypeQuittances.Toutes, tabGuid, modeNavig);
                base.model.ModeleVisualisationQuittances = model;
                //var folder = string.Format("{0}_{1}_{2}", codeOffre, version, type);
                // var isModifHorsAvn = GetIsModifHorsAvn(tabGuid, string.Format("{0}_{1}", folder, string.IsNullOrEmpty(codeAvn) ? "0" : codeAvn));
                // model.IsModifHorsAvenant = isModifHorsAvn;
                base.model.ModeleVisualisationQuittances.NumAvn = codeAvn;
                base.model.ModeleVisualisationQuittances.Etat = contrat.Etat;
                this.model.ModeleVisualisationQuittances.ReguleId = this.model.ReguleId;
            }

            if (quittancesDto.Any())
            {
                if (model.Contrat == null)
                {
                    model.Contrat = contrat;
                }
                DateTime? finEffetDossier = null;
                if (model.Contrat != null)
                {
                    finEffetDossier = AlbConvert.ConvertStrToDate(string.Format("{0}/{1}/{2}", model.Contrat.FinEffetJour, model.Contrat.FinEffetMois, model.Contrat.FinEffetAnnee));
                    if (!finEffetDossier.HasValue)
                    {
                        finEffetDossier = AlbConvert.GetFinPeriode(AlbConvert.ConvertStrToDate(model.PeriodeDebut), model.Contrat.DureeGarantie, model.Contrat.UniteDeTemps);
                    }
                }
            }
            InfoCompQuittanceDto infoCompQuittance = null;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                infoCompQuittance = client.Channel.GetInfoComplementairesQuittance(codeOffre, version, type, codeAvn, modeNavig.ParseCode<ModeConsultation>(), !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion, model.IsReadOnly, GetUser(), false);
            }
            if (infoCompQuittance != null)
            {
                model.CommentForce = infoCompQuittance.CommentaireForce;
                model.ExistEcheancier = infoCompQuittance.IsEcheancierExist;
                model.TypeCalcul = infoCompQuittance.TypeCalcul;
                model.IsEcheanceNonTraite = infoCompQuittance.IsEcheanceNonTraite;
                model.PeriodiciteContrat = infoCompQuittance.Periodicite;
                model.DisplayEmission = infoCompQuittance.DisplayEmission;
                model.AEmission = infoCompQuittance.AEmission;
                model.FormulesInfo.Totaux.NumAvenantPage = codeAvn;
                model.FormulesInfo.Commission.TypeAvt = model.TypeAvt;
                model.FormulesInfo.Commission.ActeGestionRegule = acteGestionRegule;
                model.ModifPeriode = !model.IsModifHorsAvenant && infoCompQuittance.ModifPeriode;
                model.IsModifDateFin = !infoCompQuittance.Periodicite.Equals("U", StringComparison.InvariantCulture)
                                                && !infoCompQuittance.Periodicite.Equals("E", StringComparison.InvariantCulture)
                                                && infoCompQuittance.AuMoinsRisqueTempExiste;
                model.FormulesInfo.Commission.TraceCC = infoCompQuittance.TraceCC;
                model.DateDebEffetContrat = infoCompQuittance.DateDebutEffetContrat;
                model.DateFinEffetContrat = infoCompQuittance.DateFinEffetContrat;
            }
        }

        private void LoadCommentForce(string codeOffre, string version, string type, string codeAvn, string modeNavig)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext = channelClient.Channel;
                var result = serviceContext.GetCommentQuittance(codeOffre, version, type, codeAvn, modeNavig.ParseCode<ModeConsultation>());
                if (result != null)
                {
                    model.CommentForce = result;
                }
            }
        }

        private void RemoveControlAssiette(string codeContrat, string versionContrat, string typeContrat)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = channelClient.Channel;
                serviceContext.RemoveControlAssiette(codeContrat, versionContrat, typeContrat);
            }
        }
        private List<VisualisationQuittancesLigne> GetListeQuittancesLignes(string codeOffre, string codeAvn, bool isEntete, string version, string etat, string tabGuid, DateTime? dateEmission = null, string typeOperation = "", string situation = "", DateTime? datePeriodeDebut = null, DateTime? datePeriodeFin = null, AlbConstantesMetiers.TypeQuittances typeQuittances = AlbConstantesMetiers.TypeQuittances.Toutes, string modeNavig = "", string colTri = "")
        {
            List<VisualisationQuittancesLigne> toReturn = new List<VisualisationQuittancesLigne>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffreClient = client.Channel;
                var folder = string.Format("{0}_{1}_{2}", codeOffre, version, AlbConstantesMetiers.TYPE_CONTRAT);
                var isModifHorsAvn = GetIsModifHorsAvn(tabGuid, string.Format("{0}_{1}", folder, string.IsNullOrEmpty(codeAvn) ? "0" : codeAvn));

                var result = finOffreClient.GetListeVisualisationQuittances(codeOffre, version, dateEmission, typeOperation, situation, datePeriodeDebut, datePeriodeFin, typeQuittances, colTri);
                if (result != null && result.Any())
                {
                    result.ForEach(elm =>
                    {
                        VisualisationQuittancesLigne ligne = (VisualisationQuittancesLigne)elm;
                        ligne.IsReadOnly = modeNavig.ParseCode<ModeConsultation>() == ModeConsultation.Historique;
                        ligne.DisplayEditionQuittance = ALBINGIA.OP.OP_MVC.Common.AlbTransverse.GetIsDisplayQuittance(HttpContext, codeAvn != "0", isEntete);
                        ligne.IsModifHorsAvenant = isModifHorsAvn;
                        ligne.Etat = etat;
                        toReturn.Add(ligne);


                    });
                }
            }
            return toReturn;
        }
        #endregion
    }
}
