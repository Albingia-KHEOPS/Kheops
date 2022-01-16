using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Clausier;
using OP.WSAS400.DTO.ControleFin;
using OP.WSAS400.DTO.Regularisation;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class ControleFinController : ControllersBase<ModeleControleFinPage>
    {
        #region Méthode Publique
        [ErrorHandler]
        
        public ActionResult Index(string id)
        {
            id = InitializeParams(id);
            LoadInfoPage(id);
            return View(model);
        }

        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string tabGuid, string saveCancel, string modeNavig, string addParamType, string addParamValue)
        {
            if (cible?.ToUpper() == "RECHERCHESAISIE") {
                FolderController.DeverrouillerAffaire(tabGuid);
                return RedirectToAction("Index", cible, new { id = codeOffre + "_" + version + "_" + type + "_loadParam" + tabGuid + GetFormatModeNavig(modeNavig) });
            }
            if (saveCancel == "1") {
                return RedirectToAction("Index", "RechercheSaisie", new { returnHome = saveCancel });
            }

            return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
        }

        [AlbAjaxRedirect]
        public RedirectToRouteResult RedirectLink(string param)
        {
            string[] tParam = param.Split('/');
            if (tParam.Length == 3)
            {
                return RedirectToAction(tParam[2], tParam[1]);
            }
            if (tParam.Length == 4)
            {
                return RedirectToAction(tParam[2], tParam[1], new { id = tParam[3] });
            }
            return null;
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

        #region Méthode privées
        protected override void LoadInfoPage(string id)
        {
            string[] tId = id.Split('_');
            string clausesGen = tId.Length > 3 ? tId[3] : string.Empty;
            var typeAvt = "";
            switch (tId[2])
            {
                case "O":
                    {
                        using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                        {
                            model.Offre = new Offre_MetaModel();
                            model.Offre.LoadInfosOffre(chan.Channel.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig));
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                            model.PageTitle = "Contrôle de cohérence de l'Offre";
                        }
                    }
                    break;
                case "P":
                    using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                    {
                        var infosBase = chan.Channel.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig);
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
                    typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    var regulMode = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULMOD);

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
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                            break;
                        default:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                            break;
                    }
                    model.PageTitle = "Contrôle de cohérence du Contrat";
                    break;
            }
            if (model.Offre != null || model.Contrat != null)
            {
                model.AfficherBandeau = base.DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
            }

            var tabGuid = string.Format("tabGuid{0}tabGuid", model.TabGuid);
            var folder = string.Format("{0}_{1}_{2}", tId[0], tId[1], tId[2]);

            model.IsReadOnly = GetIsReadOnly(tabGuid, folder, model.NumAvenantPage);
            model.IsModifHorsAvenant = GetIsModifHorsAvn(tabGuid, string.Format("{0}_{1}", folder, string.IsNullOrEmpty(model.NumAvenantPage) ? "0" : model.NumAvenantPage));

            if (tId[2] == AlbConstantesMetiers.TYPE_OFFRE) {
                model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
            }
            else {
                model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);
            }
            model.Bandeau = null;
            SetBandeauNavigation(id);
            SetArbreNavigation();

            if (string.IsNullOrEmpty(clausesGen) && !model.IsModifHorsAvenant && ! model.IsReadOnly
                && typeAvt != AlbConstantesMetiers.TYPE_AVENANT_REGUL
                && typeAvt != AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR
                )
            {
                using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    var etape = typeAvt == AlbConstantesMetiers.TYPE_AVENANT_RESIL ? AlbConstantesMetiers.Etapes.Resiliation : AlbConstantesMetiers.Etapes.Fin;
                    var serviceontext = chan.Channel;
                    RetGenClauseDto retGenClause = serviceontext.GenerateClause(tId[2], tId[0], Convert.ToInt32(tId[1]),
                      new ParametreGenClauseDto
                      {
                          ActeGestion = "**",
                          Letape = AlbEnumInfoValue.GetEnumInfo(etape)
                      });

                    if (retGenClause != null && !string.IsNullOrEmpty(retGenClause.MsgErreur))
                    {
                        throw new AlbFoncException(retGenClause.MsgErreur);
                    }
                }
            }

            LoadDataControleFin();
        }

        private void LoadDataControleFin()
        {
            ModeleControleFinPage modele = new ModeleControleFinPage();

            var numAvn = string.IsNullOrEmpty(model.NumAvenantPage) ? "0" : model.NumAvenantPage;
            var regulId = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULEID);
            var regulMode = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULMOD);

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var serviceContext = client.Channel;
                ControleFinDto result = null;
                if (model.Offre != null)
                {
                    var isModifHorsAvn = GetIsModifHorsAvn(model.TabGuid, string.Format("{0}_{1}_{2}_{3}", model.Offre.CodeOffre, model.Offre.Version.ToString(), model.Offre.Type, numAvn));
                    serviceContext.Alimentation(model.Offre.CodeOffre, model.Offre.Version.ToString(), model.Offre.Type, GetUser(), model.ModeNavig.ParseCode<ModeConsultation>(), isModifHorsAvn, !numAvn.Equals("0"), regulId, Convert.ToInt32(model.Offre.PreneurAssurance.Code));
                    result = serviceContext.InitControleFin(model.Offre.CodeOffre, model.Offre.Version.ToString(), model.Offre.Type, string.Empty, model.ModeNavig.ParseCode<ModeConsultation>());
                }
                else if (model.Contrat != null)
                {
                    var isModifHorsAvn = GetIsModifHorsAvn(model.TabGuid, string.Format("{0}_{1}_{2}_{3}", model.Contrat.CodeContrat, model.Contrat.VersionContrat.ToString(), model.Contrat.Type, numAvn));
                    if (regulMode != "BNS" && regulMode != "BURNER") {
                        serviceContext.Alimentation(model.Contrat.CodeContrat, model.Contrat.VersionContrat.ToString(), model.Contrat.Type, GetUser(), AlbEnumInfoValue.GetEnumValue<ModeConsultation>(model.ModeNavig), isModifHorsAvn, !numAvn.Equals("0"), regulId, model.Contrat.CodePreneurAssurance);
                    }
                    else {
                        serviceContext.UpdateEtatRegul(model.Contrat.CodeContrat, model.Contrat.VersionContrat.ToString(), model.Contrat.Type, regulId);
                    }
                    result = serviceContext.InitControleFin(model.Contrat.CodeContrat, model.Contrat.VersionContrat.ToString(), model.Contrat.Type, model.NumAvenantPage, AlbEnumInfoValue.GetEnumValue<ModeConsultation>(model.ModeNavig));
                }

                if (result != null)
                    modele = ((ModeleControleFinPage)result);
                if (modele.ModeleControleFinControles.Any())
                {
                    foreach (var cf in modele.ModeleControleFinControles)
                    {
                        if (!string.IsNullOrEmpty(cf.LienReference))
                            cf.LienReference += GetSurroundedTabGuid(model.TabGuid) + BuildAddParamString(model.AddParamType, model.AddParamValue) + GetFormatModeNavig(model.ModeNavig);
                    }
                }

                model.ModeleControleFinControles = modele.ModeleControleFinControles;
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
                    string typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    var regulMode = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULMOD);
                    var affaire = GetInfoBaseAffaire(model.CodePolicePage, model.VersionPolicePage, model.TypePolicePage, model.NumAvenant.ToString(), model.ModeNavig);
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
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR when affaire.IsRemiseEnVigeurAvecModif:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR when affaire.IsRemiseEnVigeurSansModif:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR_NO_MODIF;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        default:
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                            break;
                    }
                    model.Navigation = new Navigation_MetaModel
                    {
                        Etape = Navigation_MetaModel.ECRAN_INFOFIN,
                        IdOffre = model.Contrat.CodeContrat,
                        Version = int.Parse(model.Contrat.VersionContrat.ToString())
                    };
                }
            }
        }

        private void SetArbreNavigation()
        {

            var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
            if (model.Offre != null)
            {
                //Affichage de la navigation latérale en arboresence
                model.NavigationArbre = GetNavigationArbre("Fin");
            }
            else if (model.Contrat != null)
            {
                //if (typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGUL)
                if (model.ScreenType == AlbConstantesMetiers.SCREEN_TYPE_REGUL
                                    || model.ScreenType == AlbConstantesMetiers.SCREEN_TYPE_REGULPB
                                    || model.ScreenType == AlbConstantesMetiers.SCREEN_TYPE_REGULBNS
                                    || model.ScreenType == AlbConstantesMetiers.SCREEN_TYPE_REGULBURNER)
                {
                    if(model.ActeGestionRegule == AlbConstantesMetiers.TYPE_AVENANT_MODIF)
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
                model.NavigationArbre.ScreenType = model.ScreenType;
                model.NavigationArbre.IsRegule = typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF || (typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGUL && model.IsReadOnly);
            }
        }
        #endregion
    }
}
