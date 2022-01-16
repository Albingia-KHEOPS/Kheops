using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesCotisations;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO.Clausier;
using OP.WSAS400.DTO.Cotisations;
using OP.WSAS400.DTO.NavigationArbre;
using OPServiceContract;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class CotisationsController : ControllersBase<ModeleCotisationsPage>
    {
        #region Méthode Publique
        
        [ErrorHandler]
        public ActionResult Index(string id)
        {
            id = InitializeParams(id);
            LoadInfoPage(id);
            return View(model);
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult UpdateRedirect(string codeOffre, string version, string type, string argCotisations, string argModelCommentForce,
            string field, string value, string oldvalue, string tabGuid, string saveCancel, string paramRedirect, string modeNavig, string acteGestion, string codeAvn)
        {
            UpdateCotisation(codeOffre, version, type, argCotisations, argModelCommentForce, field, value, oldvalue, tabGuid, modeNavig, acteGestion, codeAvn);

            if (string.IsNullOrEmpty(paramRedirect))
            {
                return null;
            }
            var tabParamRedirect = paramRedirect.Split('/');
            return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
        }
        [ErrorHandler]
        [AlbAjaxRedirect]
        public ActionResult UpdateInpage(string codeOffre, string version, string type, string argCotisations, string argModelCommentForce,
            string field, string value, string oldvalue, string tabGuid, string saveCancel, string modeNavig, string acteGestion, string codeAvn)
        {
            UpdateCotisation(codeOffre, version, type, argCotisations, argModelCommentForce, field, value, oldvalue, tabGuid, modeNavig, acteGestion, codeAvn);
            if (saveCancel == "1")
            {
                return RedirectToRoute("/RechercheSaisie/Index");
            }
            return PartialView("CotisationsBody", model);
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public void GenerateClauses(string type, string codeOffre, string version)
        {
            using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceontext = chan.Channel;
                RetGenClauseDto retGenClause = serviceontext.GenerateClause(type, codeOffre, Convert.ToInt32(version),
                  new ParametreGenClauseDto
                  {
                      ActeGestion = "**",
                      Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Fin)
                  });
                if (retGenClause != null && !string.IsNullOrEmpty(retGenClause.MsgErreur))
                {
                    throw new AlbFoncException(retGenClause.MsgErreur);
                }
            }
        }

        private void UpdateCotisation(string codeOffre, string version, string type, string argCotisations, string argModelCommentForce, string field,
                                      string value, string oldvalue, string tabGuid, string modeNavig, string acteGestion, string codeAvn)
        {
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn)
                && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>())
            {
                JavaScriptSerializer serialiser = AlbJsExtendConverter<ModeleCotisationUpdate>.GetSerializer();
                var cotisations =
                  serialiser.ConvertToType<List<ModeleCotisationUpdate>>(serialiser.DeserializeObject(argCotisations));

                if (!string.IsNullOrEmpty(argModelCommentForce))
                {
                    argModelCommentForce = argModelCommentForce.Replace("\r\n", "<br>").Replace("\n", "<br>");
                }

                var infoGarantiesDto = new CotisationInfoGarantieDto {
                      Garanties = new List<CotisationGarantieDto>(),
                      CoefCom = cotisations[0].CoefCom,
                      TotalHorsFraisHT =
                        !string.IsNullOrEmpty(cotisations[0].TotalHorsFraisHT)
                          ? Convert.ToDecimal(cotisations[0].TotalHorsFraisHT)
                          : 0,
                      TotalHT = !string.IsNullOrEmpty(cotisations[0].TotalHT) ? Convert.ToDecimal(cotisations[0].TotalHT) : 0,
                      TotalTTC = cotisations[0].TotalTTC
                };

                infoGarantiesDto.Commission = new CotisationCommissionDto
                  {
                      TauxForce = cotisations[0].TauxForce,
                      MontantForce =
                        !string.IsNullOrEmpty(cotisations[0].MontantForce) ? Convert.ToDecimal(cotisations[0].MontantForce) : 0
                  };

                List<CotisationGarantieDto> lstCotisGarantiesDto = new List<CotisationGarantieDto>();
                cotisations.ForEach(
                  elem =>
                  { lstCotisGarantiesDto.Add(new CotisationGarantieDto { CodeGarantie = elem.CodeGarantie, Tarif = elem.Tarif }); }
                  );

                infoGarantiesDto.Garanties = lstCotisGarantiesDto;

                CotisationsDto cotisationsDto = new CotisationsDto();
                cotisationsDto.GarantiesInfo = infoGarantiesDto;
                
                cotisationsDto.CommentForce = argModelCommentForce;

                UpdateCotisations(codeOffre, version, type, cotisationsDto, field, value, oldvalue, modeNavig, acteGestion);

                string id = string.Format("{0}_{1}_{2}", codeOffre, version, type);
                model.PageTitle = "Synthèse des cotisations";
                LoadInfoPage(id, true);
            }
        }

        [ErrorHandler]
        public ActionResult ShowTarif(string codeGarantie)
        {
            ModeleInfoTarif modele = new ModeleInfoTarif();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                var result = client.Channel.LoadCotisationsTarif(codeGarantie);
                if (result != null) {
                    modele = (ModeleInfoTarif)result;
                }
            }

            return PartialView("CotisationsTarif", modele);
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string argModelCommentForce, string tabGuid, string paramRedirect, string modeNavig, string addParamType, string addParamValue)
        {
            //Ajout de la cible RechercheSaisie pour l'enregistrement et la suppression de la ligne KPCTRLA
            if (cible == "FinOffre" || cible == "RechercheSaisie" || cible == "ChoixClauses")
            {
                var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
                if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, numAvn)
                    && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>())
                {
                    //Verif des paramètres
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                    {
                        var serviceContextFinOffre=client.Channel;
                        CotisationsDto result = null;
                        result = serviceContextFinOffre.InitCotisations(codeOffre, version, type, numAvn, modeNavig.ParseCode<ModeConsultation>(), false, true, GetUser(), string.Empty, false);
                        if (result != null)
                        {
                            if (result.GarantiesInfo != null && result.GarantiesInfo.Commission != null)
                            {
                                _ = double.TryParse(result.GarantiesInfo.Commission.TauxForce, out var dTauxForce);
                                _ = double.TryParse(result.GarantiesInfo.Commission.TauxStd, out var dTauxStd);
                                if (dTauxForce > 0 && (string.IsNullOrEmpty(argModelCommentForce) || string.IsNullOrEmpty(argModelCommentForce.Trim())))
                                {
                                    if (dTauxForce != dTauxStd)
                                    {
                                        throw new AlbFoncException("Un commentaire est obligatoire");
                                    }
                                }
                            }
                        }
                    }

                    UpdateCommentaireOnly(codeOffre, version, type, argModelCommentForce);
                    RemoveControlAssiette(codeOffre, version, type);
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                    {
                        var serviceContext=client.Channel;
                        serviceContext.SetTrace(new TraceDto
                        {
                            CodeOffre = codeOffre.PadLeft(9, ' '),
                            Version = Convert.ToInt32(version),
                            Type = type,
                            EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Cotisation),
                            NumeroOrdreDansEtape = 64,
                            NumeroOrdreEtape = 1,
                            Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Cotisation),
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
                }
                //if (GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type))
                //    cible = "FinOffre";
            }

            if (!string.IsNullOrEmpty(paramRedirect))
            {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }

            if (cible == "ChoixClauses")
                //return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + tabGuid + GetFormatModeNavig(modeNavig), returnHome = 1, guidTab = tabGuid });
                return RedirectToAction("Index", "ChoixClauses", new
                {
                    id = AlbParameters.BuildFullId(
                        new Folder(new[] { codeOffre, version, type }),
                        new[] { "¤Cotisations¤Index¤" + codeOffre + "£" + version + "£" + type },
                        tabGuid,
                        addParamValue,
                        modeNavig),
                        returnHome = "",
                         guidTab = tabGuid
                }); 

            if (cible == "RechercheSaisie")
                return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + tabGuid + GetFormatModeNavig(modeNavig), returnHome = 1, guidTab = tabGuid });

            return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
        }

        [ErrorHandler]
        public ActionResult FiltrerGaranties(string codeOffre, string version, string type, string codeAvn, string modeNavig, bool onlyGarPorteuse, string typePart)
        {
            ModeleCotisationsPage modele = new ModeleCotisationsPage();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.GetCotisationsGaranties(codeOffre, version, type, codeAvn, modeNavig.ParseCode<ModeConsultation>(), onlyGarPorteuse, typePart);
                if (result != null)
                    modele = ((ModeleCotisationsPage)result);
            }
            return PartialView("CotisationsListGarantie", modele.GarantiesInfo);
        }

        [ErrorHandler]
        public ActionResult UpdateFraisAccessoires(string codeOffre, string version, string type, string codeAvn, int effetAnnee, string typeFrais, int fraisRetenus,
            bool taxeAttentat, long codeCommentaires, string commentaires, bool onlyGarPorteuse, string tabGuid, string modeNavig, string acteGestion)
        {
            model.IsReadOnly = GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, codeAvn);
            model.IsModifHorsAvenant = GetIsModifHorsAvn(tabGuid, codeOffre + "_" + version + "_" + type + "_" + (string.IsNullOrEmpty(codeAvn) ? "0" : codeAvn));
            if (!model.IsReadOnly
                && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>())
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
                    var retourMessage = finOffreClient.UpdateFraisAccessoires(codeOffre, version, type, effetAnnee, typeFrais, fraisRetenus, taxeAttentat, codeCommentaires, commentaires, codeAvn, GetUser(), acteGestion, model.IsModifHorsAvenant);
                    if (!string.IsNullOrEmpty(retourMessage)) throw new AlbFoncException(retourMessage);
                }
            }

            // WI - 2330: Report de YPRTRSQ vers YPRTENT
            //Report_Regul_Yprtrsq_Yprtent(codeOffre, version, codeAvn);
            // WI - 2330 : Fin

            LoadDataCotisations(codeOffre, version, type, codeAvn, modeNavig, false, acteGestion, onlyGarPorteuse);
            return PartialView("CotisationsBody", model);
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
        public ActionResult ReloadBandeauInferieur(string codeOffre, string version, string type, string codeAvn, string modeNavig,
                                                    string acteGestion, bool onlyGarPorteuse, string typePart, bool isReadonly)
        {
            ModeleCotisationsPage modele = new ModeleCotisationsPage();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.GetCotisationsGaranties(codeOffre, version, type, codeAvn, modeNavig.ParseCode<ModeConsultation>(), onlyGarPorteuse, typePart);
                if (result != null)
                    modele = ((ModeleCotisationsPage)result);
            }
            modele.GarantiesInfo.IsReadonly = isReadonly;
            modele.GarantiesInfo.Commission.isReadonly = isReadonly;
            return PartialView("CotisationsBandeau", modele.GarantiesInfo);
        }
        #endregion

        #region Méthode Privée

        private void LoadInfoPage(string id, bool reload = false)
        {
            string[] tId = id.Split('_');
            switch (tId[2])
            {
                case AlbConstantesMetiers.TYPE_OFFRE:
                    LoadDataCotisations(tId[0], tId[1], tId[2], string.Empty, model.ModeNavig, reload, model.ActeGestion, true);
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>()) {
                        model.Offre = new Offre_MetaModel();
                        model.Offre.LoadInfosOffre(client.Channel.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig));
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                        model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
                    }
                    break;
                default:
                    throw new AlbFoncException("Cet écran n'est accessible que pour les offres", trace: true, sendMail: true, onlyMessage: true);
            }
            model.PageTitle = "Synthèse des cotisations";
            if (model.Offre != null)
            {
                model.AfficherBandeau = base.DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
            }

            SetArbreNavigation();
            this.model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
            this.model.Bandeau = null;
            SetBandeauNavigation(id);
        }

        private void LoadDataCotisations(string codeOffre, string version, string type, string codeAvn, string modeNavig, bool reload, string acteGestion, bool isChecked)
        {
            ModeleCotisationsPage modele = new ModeleCotisationsPage();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                CotisationsDto result = null;
                model.IsReadOnly = GetIsReadOnly(model.TabGuid, codeOffre + "_" + version + "_" + type, codeAvn);
                result = client.Channel.InitCotisations(codeOffre, version, type, codeAvn, modeNavig.ParseCode<ModeConsultation>(), isChecked, reload, GetUser(), acteGestion, model.IsReadOnly);
                if (result != null) {
                    modele = (ModeleCotisationsPage)result;
                }

                model.isChecked = isChecked;
                model.GarantiesInfo = modele.GarantiesInfo;
                model.GarantiesInfo.GarantiesFS = modele.GarantiesInfo.Garanties;
                model.CommentForce = modele.CommentForce;
                model.GarantiesInfo.IsReadonly = model.IsReadOnly;
                model.GarantiesInfo.Commission.TraceCC = modele.TraceCC;
            }
        }

        private void UpdateCotisations(string codeOffre, string version, string type, CotisationsDto cotisationsDto, string field, string value, string oldvalue, string modeNavig, string acteGestion)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                client.Channel.UpdateCotisations(codeOffre, version, type, cotisationsDto, field, value, oldvalue, GetUser(), modeNavig.ParseCode<ModeConsultation>(), "0", acteGestion);
            }
            var id = new Albingia.Kheops.OP.Domain.Affaire.AffaireId {
                CodeAffaire = codeOffre,
                NumeroAliment = int.Parse(version),
                TypeAffaire = Albingia.Kheops.OP.Domain.Affaire.AffaireType.Offre
            };
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFormule>()) {
                var valeurs = client.Channel.ComputeEachGareat(new Albingia.Kheops.OP.Domain.Affaire.AffaireId {
                    CodeAffaire = codeOffre,
                    NumeroAliment = int.Parse(version),
                    TypeAffaire = Albingia.Kheops.OP.Domain.Affaire.AffaireType.Offre
                });

                if (valeurs?.Any() ?? false) {
                    // full reload if gareat is changed
                    field = "INIT";
                    oldvalue = "0";
                    value = "0";
                }
            }
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>()) {
                client.Channel.UpdateCotisationsAS400(id, field, decimal.TryParse(oldvalue, out var d) ? d : default(decimal?), decimal.TryParse(value, out d) ? d : default(decimal?));
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
                        Etape = Navigation_MetaModel.ECRAN_COTISATIONS,
                        IdOffre = model.Offre.CodeOffre,
                        Version = model.Offre.Version
                    };
                }
                else if (model.Contrat != null)
                {
                    model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                    model.Navigation = new Navigation_MetaModel
                    {
                        Etape = Navigation_MetaModel.ECRAN_COTISATIONS,
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
                model.NavigationArbre = GetNavigationArbre("Cotisation");
            }
            else if (model.Contrat != null)
            {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Cotisation");
            }
        }
        private void UpdateCommentaireOnly(string codeOffre, string version, string type, string argModelCommentForce)
        {
            string commentaireDecode = Server.UrlDecode(argModelCommentForce);

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffreClient=client.Channel;
                finOffreClient.SaveCommentaireCotisation(codeOffre, version, type, commentaireDecode);
            }
        }

        private void RemoveControlAssiette(string codeContrat, string versionContrat, string typeContrat)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                serviceContext.RemoveControlAssiette(codeContrat, versionContrat, typeContrat);
            }
        }

        #endregion
    }
}
