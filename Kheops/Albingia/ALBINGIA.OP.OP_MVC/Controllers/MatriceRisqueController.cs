using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.MatriceRisque;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class MatriceRisqueController : MatriceControllerBase<ModeleMatriceRisquePage> {
        #region Méthode Publique
        
        [ErrorHandler]
        public ActionResult Index(string id)
        {
            id = InitializeParams(id);
            LoadInfoPage(id);
            return View(model);
        }

        [ErrorHandler]
        public void DeleteFormuleGen(string codeOffre, string version, string type, string codeAvn, string codeFormule, string tabGuid)
        {
            if (AllowUpdate) {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                    client.Channel.DeleteFormule(codeOffre, version, type, codeFormule, "C");
                }
            }
        }


        #endregion

        #region Méthodes Privées

        protected override void LoadInfoPage(string id)
        {
            string[] tId = id.Split('_');
            if (tId[2] == AlbConstantesMetiers.TYPE_OFFRE)
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>()) {
                    this.model.Offre = new Offre_MetaModel();
                    this.model.Offre.LoadInfosOffre(client.Channel.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig));
                    this.model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    this.model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
                    this.model.IsModifHorsAvenant = IsModifHorsAvenant;
                    this.model.StrDateDebutEffetGenerale = model.Offre.DateEffetGarantie.HasValue ? model.Offre.DateEffetGarantie.Value.ToString("dd/MM/yyyy") : string.Empty;
                    this.model.StrDateFinEffetGenerale = model.Offre.DateFinEffetGarantie.HasValue ? model.Offre.DateFinEffetGarantie.Value.ToString("dd/MM/yyyy") : string.Empty;
                }
            }
            else if (tId[2] == AlbConstantesMetiers.TYPE_CONTRAT)
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
                        DateEffetAnnee = infosBase.DateEffetAnnee,
                        DateEffetMois = infosBase.DateEffetMois,
                        DateEffetJour = infosBase.DateEffetJour,
                        Delegation = infosBase?.CabinetGestionnaire?.Delegation?.Nom,
                        Inspecteur = infosBase?.CabinetGestionnaire?.Inspecteur
                    };

                    this.model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);
                    this.model.IsModifHorsAvenant = IsModifHorsAvenant;

                    if (model.Contrat.DateEffetAnnee != 0 && model.Contrat.DateEffetMois != 0 && model.Contrat.DateEffetJour != 0)
                    {
                        model.StrDateDebutEffetGenerale = new DateTime(model.Contrat.DateEffetAnnee, model.Contrat.DateEffetMois, model.Contrat.DateEffetJour).ToString("dd/MM/yyyy");
                    }

                    if (model.Contrat.FinEffetAnnee != 0 && model.Contrat.FinEffetMois != 0 && model.Contrat.FinEffetJour != 0)
                    {
                        model.StrDateFinEffetGenerale = new DateTime(model.Contrat.FinEffetAnnee, model.Contrat.FinEffetMois, model.Contrat.FinEffetJour).ToString("dd/MM/yyyy");
                    }
                    var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    switch (typeAvt)
                    {
                        case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
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
            if (tId[2] == AlbConstantesMetiers.TYPE_OFFRE) {
                model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
            }
            else {
                model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);
            }
            this.model.IsModifHorsAvenant = IsModifHorsAvenant;
            #endregion

            #region Bandeau
            model.Bandeau = null;
            SetBandeauNavigation(id);
            #endregion

            LoadMatriceRisque();
        }

        private void LoadMatriceRisque()
        {
            ModeleMatriceRisquePage modele = new ModeleMatriceRisquePage();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                MatriceRisqueDto result = null;
                if (this.model.Offre != null) {
                    result = client.Channel.InitMatriceRisque(model.Offre.CodeOffre, model.Offre.Version.ToString(), model.Offre.Type, "0", model.ModeNavig.ParseCode<ModeConsultation>(), GetUser(), model.ActeGestion, model.IsReadOnly || model.IsModifHorsAvenant);
                }
                else if (this.model.Contrat != null) {
                    result = client.Channel.InitMatriceRisque(model.Contrat.CodeContrat, model.Contrat.VersionContrat.ToString(), model.Contrat.Type, model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>(), GetUser(), model.ActeGestion, model.IsReadOnly || model.IsModifHorsAvenant);
                }

                if (result != null) {
                    modele = (ModeleMatriceRisquePage)result;
                }
                this.model.FormGen = modele.FormGen;
                this.model.Formule = modele.Formule;
                this.model.HasFormGen = modele.HasFormGen;
                this.model.Risques = modele.Risques;
                this.model.AddFormule = modele.AddFormule;
                this.model.CopyRisque = modele.CopyRisque;
                this.model.CodeFormule = modele.formuleOption.Split('_')[0];
                this.model.CodeOption = modele.formuleOption.Split('_')[1];
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
                        Version = int.Parse(model.Contrat.VersionContrat.ToString(CultureInfo.InvariantCulture))
                    };
                }
            }
        }
        private void SetArbreNavigation()
        {
            if (model.Offre != null)
            {
                //Affichage de la navigation latérale en arboresence
                model.NavigationArbre = GetNavigationArbre("MatriceRisques");
            }
            else if (model.Contrat != null)
            {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("MatriceRisques");
            }
        }
        #endregion
    }
}
