using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.DynamicGuiIS.Common;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using Mapster;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Clausier;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using OPServiceContract.IS;
using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers {
    public class InformationsSpecifiquesGarantieController : InformationsSpecifiquesControllerBase<ModeleInformationsSpecifiquesGarantiePage> {
        [ErrorHandler]
        public ActionResult Index(string id)
        {
            id = InitializeParams(id);
            LoadInfoPage(id);
            return View(model);
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult RedirectionVersCondition(string branche, string section, string cible, string additionalParams
          , string dataToSave, string splitChars, string strParameters,
          string argCodeOffre, string argVersion, string argType, string codeRisque, string argCodeFormule, string argCodeOption, string argLibelleFormule, string argLettreLibelleFormule, string argRedirectIS, string tabGuid, string paramRedirect, string modeNavig,
            string addParamType, string addParamValue, bool isForceReadOnly)
        {
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            if (modeNavig.ParseCode<ModeConsultation>() != ModeConsultation.Historique && AllowUpdate)
            {
                if (!DbIOParam.SaveISToDB(branche, section, cible, additionalParams, dataToSave, splitChars, strParameters))
                {
                    throw new AlbFoncException("Erreur de sauvegarde des informations spécifiques");
                }

                int version, codefor, codeopt, codersq = 0;
                if (int.TryParse(argVersion, out version) && int.TryParse(argCodeFormule, out codefor) && int.TryParse(argCodeOption, out codeopt) && int.TryParse(codeRisque, out codersq))
                {
                    using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                    {
                        var serviceontext = chan.Channel;
                        RetGenClauseDto retGenClause = serviceontext.GenerateClause(argType, argCodeOffre,
                          version,
                          new ParametreGenClauseDto
                          {
                              ActeGestion = "**",
                              Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Option),
                              NuFormule = codefor,
                              NuOption = codeopt,
                              NuRisque = codersq
                          });
                        if (retGenClause != null && !string.IsNullOrEmpty(retGenClause.MsgErreur))
                        {
                            throw new AlbFoncException(retGenClause.MsgErreur);
                        }
                    }
                }
            }

            if (paramRedirect.ContainsChars())
            {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }

            return RedirectToAction("Index", "ConditionsGarantie", new
            {
                id = AlbParameters.BuildFullId(
                new Folder(new[] { argCodeOffre, argVersion, argType }),
                new[] { argCodeFormule, argCodeOption, codeRisque },
                tabGuid,
                addParamValue + (isForceReadOnly ? "||FORCEREADONLY|1" : string.Empty),
                modeNavig),
                returnHome = "0"
            });
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult RedirectionCondition(string argCodeOffre, string argVersion, string argType, string codeRisque, string argCodeFormule, string argCodeOption, string tabGuid, string paramRedirect, string modeNavig, string addParamValue, bool isForceReadOnly)
        {
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            if (modeNavig.ParseCode<ModeConsultation>() != ModeConsultation.Historique && AllowUpdate)
            {
                int version, codefor, codeopt, codersq = 0;
                if (int.TryParse(argVersion, out version) && int.TryParse(argCodeFormule, out codefor) && int.TryParse(argCodeOption, out codeopt) && int.TryParse(codeRisque, out codersq))
                {
                    using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                    {
                        var serviceontext = chan.Channel;
                        RetGenClauseDto retGenClause = serviceontext.GenerateClause(argType, argCodeOffre,
                          version,
                          new ParametreGenClauseDto
                          {
                              ActeGestion = "**",
                              Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Option),
                              NuFormule = codefor,
                              NuOption = codeopt,
                              NuRisque = codersq
                          });
                        if (retGenClause != null && !string.IsNullOrEmpty(retGenClause.MsgErreur))
                        {
                            throw new AlbFoncException(retGenClause.MsgErreur);
                        }
                    }
                }
            }

            if (paramRedirect.ContainsChars())
            {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }

            return RedirectToAction("Index", "ConditionsGarantie", new
            {
                id = AlbParameters.BuildFullId(
                new Folder(new[] { argCodeOffre, argVersion, argType }),
                new[] { argCodeFormule, argCodeOption, codeRisque },
                tabGuid,
                addParamValue + (isForceReadOnly ? "||FORCEREADONLY|1" : string.Empty),
                modeNavig),
                returnHome = "0"
            });
        }

        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string codeFormule, string codeOption, string libelle, string lettreLib, string tabGuid, string modeNavig, string addParamType, string addParamValue)
        {
            if (cible == "RechercheSaisie")
            {
                return RedirectToAction(job, cible);
            }
            string[] contextInfos = null;
            if (cible == "InformationsSpecifiquesRecupGaranties")
            {
                contextInfos = new[] { codeFormule, codeOption, libelle, lettreLib };
            }
            else
            {
                contextInfos = new[] { codeFormule, codeOption };
            }

            if (cible == "CreationFormuleGarantie" && job == "Index")
            {
                cible = "FormuleGarantie";
            }

            return RedirectToAction(job, cible, new
            {
                id = AlbParameters.BuildFullId(
                new Folder(new[] { codeOffre, version, type }),
                contextInfos,
                tabGuid,
                addParamValue,
                modeNavig)
            });
        }

        protected override bool GetInfoSpeReadonly(Folder folder)
        {
            int opt = int.TryParse(this.model.CodeOption, out int o) ? o : default;
            int frm = int.TryParse(this.model.CodeFormule, out int f) ? f : default;
            if (opt == default || frm == default)
            {
                return true;
            }
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFormulePort>()) {
                return client.Channel.IsAvnDisabled(folder.Adapt<AffaireId>(), opt, frm);
            }
        }

        #region Méthode Privée

        protected override void LoadInfoPage(string id)
        {
            string codeBranche = string.Empty;
            string[] tId = id.Split('_');
            this.model.CodeFormule = tId[3];
            this.model.CodeOption = tId[4];
            this.model.CodeRisque = tId[5];
            switch (tId[2])
            {
                case "O":
                    using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                    {
                        var CommonOffreClient = chan.Channel;
                        model.Offre = new Offre_MetaModel();
                        model.Offre.LoadInfosOffre(CommonOffreClient.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig));
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                        model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
                    }

                    break;
                case "P":
                    using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                    {
                        var CommonOffreClient = chan.Channel;
                        var infosBase = CommonOffreClient.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig);
                        this.model.Contrat = new ContratDto()
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
                            IndiceReference = infosBase.IndiceReference,
                            Delegation = infosBase?.CabinetGestionnaire?.Delegation?.Nom,
                            Inspecteur = infosBase?.CabinetGestionnaire?.Inspecteur
                        };
                        this.model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);
                        this.model.IsModifHorsAvenant = IsModifHorsAvenant;
                        var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                        switch (typeAvt)
                        {
                            case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                                break;
                            case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
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
                    }

                    break;
            }
            model.PageTitle = "Informations Spécifiques Garantie";
            if (this.model.Offre != null || this.model.Contrat != null)
            {
                if (tId[2] == "O")
                {
                    codeBranche = model.Offre.Branche.Code;
                }
                if (tId[2] == "P")
                {
                    codeBranche = model.Contrat.Branche;
                }
                model.AfficherBandeau = base.DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
            }

            #region Bandeau
            model.Bandeau = null;
            SetBandeauNavigation(id);
            #endregion

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.GetLibFormule(Convert.ToInt32(model.CodeFormule), tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                if (result != null && !string.IsNullOrEmpty(result))
                {
                    model.LettreLibelleFormule = result.Split(new[] { "-" }, StringSplitOptions.None)[0];
                    model.LibelleFormule = result.Split(new[] { "-" }, StringSplitOptions.None)[1];
                }
            }

            #region Navigation Arbre
            SetArbreNavigation();
            #endregion

            var elemPar = id.Split('_');
            var sbParams = new StringBuilder();
            sbParams.Append(elemPar[2] + MvcApplication.SPLIT_CONST_HTML + elemPar[0].PadLeft(9) + MvcApplication.SPLIT_CONST_HTML);
            sbParams.Append(elemPar[1] + MvcApplication.SPLIT_CONST_HTML);
            sbParams.Append(elemPar[3] + MvcApplication.SPLIT_CONST_HTML);
            sbParams.Append(elemPar[4]);
            model.SpecificParameters = sbParams.ToString();
            model.IsDataRecup = CheckDataRecup(codeBranche, AlbConstantesMetiers.INFORMATIONS_RECUP_GARANTIES, model.SpecificParameters);
            model.Parameters = string.Format("&branche={0}&section={1}&", codeBranche, AlbConstantesMetiers.INFORMATIONS_GARANTIES);
        }

        private void SetArbreNavigation()
        {
            if (model.Offre != null)
            {
                //Affichage de la navigation latérale en arboresence
                model.NavigationArbre = GetNavigationArbre("Formule", codeFormule: Convert.ToInt32(model.CodeFormule), numOption: this.model.CodeOption.ParseInt().Value);
            }
            else if (model.Contrat != null)
            {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Formule", codeFormule: Convert.ToInt32(model.CodeFormule));
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
                    string typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
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
                }
            }
        }

        private bool CheckDataRecup(string branche, string section, string strParameters)
        {
            string idModeleRecup = DbIOParam.PrepareIsIdModele(branche, section);
            string splitChars = MvcApplication.SPLIT_CONST_HTML;

            var modele = CacheIS.AllISEnteteModelesDto.Find(elm => elm.NomModele.ToLower() == idModeleRecup.ToLower());
            if (modele == null || string.IsNullOrEmpty(modele.SqlExist)) return false;

            var parameters = DbIOParam.GetParams(strParameters, splitChars);
            var hParam = DbIOParam.PrepareParameter(parameters).ToArray();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IInfoSpecif>())
            {
                var wsInfoData = client.Channel;
                return wsInfoData.IsDataISExist(modele.SqlExist, hParam);
            }
        }

        #endregion

    }
}
