using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using Mapster;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.MatriceFormule;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using System;
using System.Linq;
using System.Web.Mvc;
using Domain = Albingia.Kheops.OP.Domain;
using Services = Albingia.Kheops.OP.Application.Port.Driver;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class MatriceFormuleController : MatriceControllerBase<ModeleMatriceFormulePage>
    {



        #region Méthode Publique

        [AlbVerifLockedOffer("id")]
        [ErrorHandler]
        public ActionResult Index(string id)
        {
            id = InitializeParams(id);
            LoadInfoPage(id);
            return View(model);
        }

        protected override void SpecialRedirection(string cible, string job, string codeOffre, string version, string type, string codeFormule, string codeOption, string codeRisque, string tabGuid, string formGen, string paramRedirect, string modeNavig, string addParamType, string addParamValue, out string newCible, out string[] contextInfos)
        {
            base.SpecialRedirection(cible, job, codeOffre, version, type, codeFormule, codeOption, codeRisque, tabGuid, formGen, paramRedirect, modeNavig, addParamType, addParamValue, out newCible, out contextInfos);
            if (cible?.ToLower() == "conditionsgarantie")
            {
                newCible = cible;
                contextInfos = new[] { codeFormule, codeOption };
            }
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult DeleteFormule(string codeOffre, string version, string type, string codeFormule, string tabGuid, string modeNavig,
            string addParamType, string addParamValue)
        {
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                if (ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>() && AllowUpdate)
                {
                    client.Channel.DeleteFormule(codeOffre, version, type, codeFormule, "C");
                }
                return RedirectToAction("Index", "MatriceFormule", new
                {
                    id = AlbParameters.BuildStandardId(new Folder(new[] { codeOffre, version, type }), tabGuid, addParamValue, modeNavig)
                });
            }
        }

        [ErrorHandler]
        public void DeleteFormuleGen(string codeOffre, string version, string type, string codeAvn, string codeFormule, string tabGuid)
        {
            if (AllowUpdate)
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    client.Channel.DeleteFormule(codeOffre, version, type, codeFormule, "C");
                }
            }
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult DuplicateFormule(string codeOffre, string version, string type, string codeFormule, string tabGuid, string modeNavig, string addParamType, string addParamValue)
        {
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            bool isReadonly = this.model.ModeNavig.ParseCode<ModeConsultation>() != ModeConsultation.Standard || !AllowUpdate;
            if (!isReadonly)
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    var result = client.Channel.DuplicateFormule(codeOffre, version, type, codeFormule, GetUser());
                    if (result.IsEmptyOrNull() || result.ToLower() == "error")
                    {
                        throw new AlbTechException(new Exception("Problème lors de la duplication de la formule"));
                    }
                }
            }
            return RedirectToAction("Index", isReadonly ? "MatriceFormule" : "FormuleGarantie", new
            {
                id = AlbParameters.BuildStandardId(
                    new Folder(new[] { codeOffre, version, type }),
                    tabGuid,
                    addParamValue,
                    modeNavig,
                    isReadonly ? null : new[] { "modeDuplication1modeDuplication" })
            });
        }

        [ErrorHandler]
        public string GetValidRsq(string codeOffre, string version, string type, string codeRsq)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext = client.Channel;
                return serviceContext.GetValidRsq(codeOffre, version, type, codeRsq);
            }
        }

        [HttpPost]
        [HandleJsonError]
        public void DeleteOption(Affaire affaire, int numFormule, int numOption)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IFormulePort>())
            {
                client.Channel.DeleteOption(affaire.Adapt<Domain.Affaire.AffaireId>(), numFormule, numOption);
            }
        }

        [HttpPost]
        [HandleJsonError]
        public int DuplicateOption(Affaire affaire, int numFormule, int numOption)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<Services.IFormulePort>())
            {
                return client.Channel.DuplicateOption(affaire.Adapt<Domain.Affaire.AffaireId>(), numFormule, numOption);
            }
        }

        #endregion

        #region Méthode Privée

        protected override void LoadInfoPage(string id)
        {
            string[] tId = id.Split('_');
            if (tId[2] == "O")
            {
                using (var wsClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                {
                    this.model.Offre = new Offre_MetaModel();
                    this.model.Offre.LoadInfosOffre(wsClient.Channel.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig));
                    this.model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    this.model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
                    this.model.IsModifHorsAvenant = IsModifHorsAvenant;
                }
            }
            else if (tId[2] == "P")
            {
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
                        DateEffetAnnee = infosBase.DateEffetAnnee,
                        DateEffetMois = infosBase.DateEffetMois,
                        DateEffetJour = infosBase.DateEffetJour,
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

            }
            model.PageTitle = "Risques & Formules de garanties";
            if (model.Offre != null || model.Contrat != null)
            {
                model.AfficherBandeau = base.DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
            }

            #region Navigation Arbre
            SetArbreNavigation();
            if (tId[2] == "O")
            {
                model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
            }
            else
            {
                model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);
            }
            this.model.IsModifHorsAvenant = IsModifHorsAvenant;
            #endregion

            #region Bandeau
            model.Bandeau = null;
            SetBandeauNavigation(id);
            #endregion

            LoadMatriceFormule();
        }

        private void LoadMatriceFormule()
        {
            ModeleMatriceFormulePage modele = new ModeleMatriceFormulePage();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext = client.Channel;
                MatriceFormuleDto result = null;
                if (model.Offre != null)
                {
                    result = serviceContext.InitMatriceFormule(model.Offre.CodeOffre, model.Offre.Version.ToString(), model.Offre.Type, string.Empty, model.ModeNavig.ParseCode<ModeConsultation>(), GetUser(), model.ActeGestion, model.IsReadOnly || model.IsModifHorsAvenant);
                }
                else if (model.Contrat != null)
                {
                    result = serviceContext.InitMatriceFormule(model.Contrat.CodeContrat, model.Contrat.VersionContrat.ToString(), model.Contrat.Type, model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>(), GetUser(), model.ActeGestion, model.IsReadOnly || model.IsModifHorsAvenant);
                }
                if (result != null)
                {
                    modele = (ModeleMatriceFormulePage)result;
                }

                model.FormGen = modele.FormGen;
                model.Formule = modele.Formule;
                model.HasFormGen = modele.HasFormGen;
                model.AddFormule = modele.AddFormule;

                model.Risques = modele.Risques;
                model.Formules = modele.Formules;

                string avtId = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNID);
                model.IsModeAvenant = !string.IsNullOrEmpty(avtId) && avtId != "0";
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
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
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
                model.NavigationArbre = GetNavigationArbre("MatriceFormules");
            }
            else if (model.Contrat != null)
            {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("MatriceFormules");
            }
        }
        #endregion
    }
}
