using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.DynamicGuiIS.Common;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using Mapster;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Offres.Risque;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using OPServiceContract.IS;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class InformationsSpecifiquesObjetsController : InformationsSpecifiquesControllerBase<ModeleInformationsSpecifiquesObjetsPage>
    {
        [ErrorHandler]
        public ActionResult Index(string id) {
            id = InitializeParams(id);
            LoadInfoPage(id);
            var elemPar = id.Split('_');
            var sbParams = new StringBuilder();
            if (model.Offre != null) {
                sbParams.Append(model.Offre.Type + MvcApplication.SPLIT_CONST_HTML + elemPar[0].PadLeft(9) + MvcApplication.SPLIT_CONST_HTML);
            }
            else if (model.Contrat != null) {
                sbParams.Append(model.Contrat.Type + MvcApplication.SPLIT_CONST_HTML + elemPar[0].PadLeft(9) + MvcApplication.SPLIT_CONST_HTML);
            }
            sbParams.Append(elemPar[1] + MvcApplication.SPLIT_CONST_HTML);
            sbParams.Append(elemPar[3] + MvcApplication.SPLIT_CONST_HTML);
            sbParams.Append(elemPar[4]);
            model.SpecificParameters = sbParams.ToString();
            if (model.Offre != null)
            {
                model.Parameters = string.Format("&branche={0}&section={1}&", model.Offre.Branche.Code, AlbConstantesMetiers.INFORMATIONS_OBJETS);
                model.IsDataRecup = CheckDataRecup(model.Offre.Branche.Code, AlbConstantesMetiers.INFORMATIONS_RECUP_OBJETS, model.SpecificParameters);
            }
            else if (model.Contrat != null)
            {
                model.Parameters = string.Format("&branche={0}&section={1}&", model.Contrat.Branche, AlbConstantesMetiers.INFORMATIONS_OBJETS);
                model.IsDataRecup = CheckDataRecup(model.Contrat.Branche, AlbConstantesMetiers.INFORMATIONS_RECUP_OBJETS, model.SpecificParameters);

            }
            model.PageTitle = "Informations Spécifiques Objet";
            return View(model);
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult Redirection(string branche, string section, string additionalParams
          , string dataToSave, string splitChars, string strParameters,
          string cible, string job, string codeOffre, string version, string type, string codeRisque, string codeObjet, string libelleRisque, string libelleObjet, string tabGuid, string paramRedirect, bool withSave, string modeNavig,
            string addParamType, string addParamValue, bool isForceReadOnly)
        {
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            if (modeNavig.ParseCode<ModeConsultation>() != ModeConsultation.Historique && AllowUpdate)
            {
                var questMedical = string.Empty;
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    // Récupération de la valeur du questionnaire médical
                    questMedical = client.Channel.GetQuestionMedical(codeOffre, version, type, codeRisque, codeObjet, questMedical, false, GetUser());

                    if (withSave)
                    {
                        if (!DbIOParam.SaveISToDB(branche, section, cible, additionalParams, dataToSave, splitChars, strParameters))
                            throw new AlbFoncException("Erreur de sauvegarde des informations spécifiques");
                    }

                    //Ecriture des traces dans KPCTRLA si valeur questionnaire médical différente
                    questMedical = client.Channel.GetQuestionMedical(codeOffre, version, type, codeRisque, codeObjet, questMedical, true, GetUser());
                }
            }


            if (paramRedirect.ContainsChars()) {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }
            string[] contextInfos = null;
            switch (cible?.ToLower()) {
                case "informationsspecifiquesrecupobjets":
                    job = "Index";
                    contextInfos = new[] { codeRisque, codeObjet, libelleRisque, libelleObjet };
                    break;
                case "recherchesaisie":
                    return RedirectToAction(job, cible);
                case "detailsobjetrisque":
                    contextInfos = new[] { codeRisque, codeObjet };
                    addParamValue += isForceReadOnly ? ("||" + AlbParameterName.IGNOREREADONLY + "|1") : string.Empty;
                    break;
                default:
                    job = "Index";
                    cible = "DetailsRisque";
                    contextInfos = new[] { codeRisque };
                    addParamValue += isForceReadOnly ? ("||" + AlbParameterName.IGNOREREADONLY + "|1") : string.Empty;
                    break;
            }

            return RedirectToAction(job, cible, new {
                id = AlbParameters.BuildFullId(
                new Folder(new[] { codeOffre, version, type }),
                contextInfos,
                tabGuid,
                addParamValue,
                modeNavig)
            });
        }

        protected override bool GetInfoSpeReadonly(Folder folder) {
            int rsq = int.TryParse(this.model.CodeRisque, out int r) ? r : default;
            if (rsq == default) {
                return true;
            }
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquePort>()) {
                return client.Channel.IsAvnDisabled(folder.Adapt<AffaireId>(), rsq);
            }
        }

        #region Méthode Privée

        protected override void LoadInfoPage(string id)
        {

            string[] tId = id.Split('_');

            if (tId[2] == "O")
            {
                using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                {
                 var CommonOffreClient=chan.Channel;
                    model.Offre = new Offre_MetaModel();
                    model.Offre.LoadInfosOffre(CommonOffreClient.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig));
                }
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                {
                    var policeServicesClient=client.Channel;
                    model.Offre.Risques = policeServicesClient.ObtenirRisques(model.ModeNavig.ParseCode<ModeConsultation>(), tId[0], Convert.ToInt32(tId[1]), tId[2], model.NumAvenantPage);
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                }
            }
            else if (tId[2] == "P")
            {
                using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                {
                 var CommonOffreClient=chan.Channel;
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
                        IndiceReference = infosBase.IndiceReference,
                        Delegation = infosBase?.CabinetGestionnaire?.Delegation?.Nom,
                        Inspecteur = infosBase?.CabinetGestionnaire?.Inspecteur
                    };
                    string typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
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

                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                {
                    var policeServicesClient=client.Channel;
                    model.Contrat.Risques = policeServicesClient.ObtenirRisques(model.ModeNavig.ParseCode<ModeConsultation>(), tId[0], Convert.ToInt32(tId[1]), tId[2], model.NumAvenantPage);
                }

            }

            if (model.Offre != null || model.Contrat != null)
            {
                model.AfficherBandeau = base.DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
            }

            if (!string.IsNullOrEmpty(tId[3].ToString()))
            {
                model.Code = tId[3];
                model.CodeObjet = tId[4];
                model.CodeRisque = tId[3];
                
                RisqueDto currentRsq = null;
                if (model.Offre != null)
                    currentRsq = model.Offre.Risques.FirstOrDefault(x => x.Code == Convert.ToInt32(tId[3].ToString()));
                else if (model.Contrat != null)
                    currentRsq =model.Contrat.Risques.FirstOrDefault(x => x.Code == Convert.ToInt32(tId[3].ToString()));

                var currentObj = currentRsq.Objets.FirstOrDefault(o => o.Code == Convert.ToInt32(tId[4].ToString()));
                model.LibelleRisque = currentRsq.Descriptif;
                model.LibelleObjet = currentObj.Descriptif;
            }

            this.model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), tId[0] + "_" + tId[1] + "_" + tId[2], model.NumAvenantPage);
            this.model.IsModifHorsAvenant = IsModifHorsAvenant;

            #region Navigation Arbre
            SetArbreNavigation();
            #endregion

            #region Bandeau
            model.Bandeau = null;
            SetBandeauNavigation(id);
            #endregion

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
        private void SetArbreNavigation()
        {
            if (model.Offre != null)
            {
                //Affichage de la navigation latérale en arboresence
                model.NavigationArbre = GetNavigationArbre("Risque", codeRisque: Convert.ToInt32(model.CodeRisque));
            }
            else if (model.Contrat != null)
            {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Risque", codeRisque: Convert.ToInt32(model.CodeRisque));
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
                var wsInfoData=client.Channel;
                return wsInfoData.IsDataISExist(modele.SqlExist, hParam);
            }
        }
        #endregion
    }
}
