using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Filters;
using ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.Courtiers;
using ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.ModelePage;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesQuittances;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Partenaire;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers.AffaireNouvelle
{
    public class AnCourtierController : ControllersBase<AnCourtier>
    {
        public const string ModeStandard = "Standard";
        public const string ModePopup = "Popup";

        #region Méthodes publiques
        [AlbVerifLockedOffer("id", "P")]
        [ErrorHandler]
        [WhitespaceFilter]
        public ActionResult Index(string id)
        {
            model.PageTitle = "Commissions";
            id = InitializeParams(id);
            LoadInfoPage(id);
            model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);

            //Affichage de la navigation latérale en arboresence
            SetBandeauNavigation();
            return View(model);
        }

        [ErrorHandler]
        public void UpdateCourtier(string codeContrat, string versionContrat, string typeContrat, string tabGuid, int codeCourtier, Single? commission, string typeOperation, string addParamType, string addParamValue)
        {
            if (!commission.HasValue)
                throw new AlbFoncException("Il faut saisir une commission");
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            //Sauvegarde uniquement si l'écran n'est pas en readonly
            var folder = string.Format("{0}_{1}_{2}",codeContrat ,versionContrat , typeContrat);
            var isReadOnly = GetIsReadOnly(tabGuid, folder, numAvn);
            var isModifHorsAvn = GetIsModifHorsAvn(tabGuid , string.Format("{0}_{1}",folder,string.IsNullOrEmpty(numAvn) ? "0": numAvn));
            if (!isReadOnly || isModifHorsAvn)
            {
                //Verif & Enregistrement trace modification AVN
                string typeAvt = GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);
                if (typeAvt == AlbConstantesMetiers.TYPE_AVENANT_MODIF)
                {
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                    {
                        var serviceContext=client.Channel;
                        serviceContext.CheckAndSaveTraceAvenant(codeContrat, versionContrat, typeContrat, "KCOMM", string.Empty, string.Empty, string.Empty, string.Empty, "GEN", string.Empty, "C", string.Empty, string.Empty, "O", string.Empty, GetUser());
                    }
                }
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var serviceContext=client.Channel;
                    string retourMessage = serviceContext.UpdateCourtier(codeContrat, versionContrat, "P", "N", codeCourtier, commission.Value, typeOperation, GetUser());
                    if (!string.IsNullOrEmpty(retourMessage)) throw new AlbFoncException(retourMessage);
                }
            }
        }

        [ErrorHandler]
        public void SupprimerCourtier(string codeContrat, string versionContrat, string typeContrat, string tabGuid, int codeCourtier, string addParamType, string addParamValue)
        {
            //Supprime uniquement si l'écran n'est pas en readonly
            var numAvenant = GetAddParamValue(addParamValue, AlbParameterName.AVNID);

            var folder = string.Format("{0}_{1}_{2}", codeContrat, versionContrat, typeContrat);
            var isReadOnly = GetIsReadOnly(tabGuid, folder, numAvenant);
            var isModifHorsAvn = GetIsModifHorsAvn(tabGuid, string.Format("{0}_{1}", folder, string.IsNullOrEmpty(numAvenant) ? "0" : numAvenant));

            if (!isReadOnly || isModifHorsAvn)
            {
                //Verif & Enregistrement trace modification AVN
                string typeAvt = GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);
                if (typeAvt == AlbConstantesMetiers.TYPE_AVENANT_MODIF)
                {
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                    {
                        var serviceContext=client.Channel;
                        serviceContext.CheckAndSaveTraceAvenant(codeContrat, versionContrat, typeContrat, "KCOMM", string.Empty, string.Empty, string.Empty, string.Empty, "GEN", string.Empty, "C", string.Empty, string.Empty, "O", string.Empty, GetUser());
                    }
                }
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var serviceContext=client.Channel;
                    serviceContext.SupprimerCourtier(codeContrat, versionContrat, codeCourtier);
                }
            }
        }

        [ErrorHandler]
        public PartialViewResult UpdateListCourtiers(string codeContrat, string versionContrat, string codeAvn, string modeNavig)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                var courtiersDto = serviceContext.GetListCourtiers(codeContrat, versionContrat, "P", codeAvn, modeNavig.ParseCode<ModeConsultation>());
                var courtiers = Courtier.LoadCourtiers(courtiersDto);
                return PartialView("/Views/AnCourtier/ListeCourtiers.cshtml", courtiers);
            }
        }
        [ErrorHandler]
        public ActionResult LoadCourtier(string codeContrat, string versionContrat, int codeCourtier, string modeNavig)
        {
            CourtierDto courtierDto = null;
            Courtier courtier = null;
            if (codeCourtier != 0)
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var serviceContext=client.Channel;
                    courtierDto = serviceContext.GetCourtier(codeCourtier, codeContrat, versionContrat, "P", modeNavig.ParseCode<ModeConsultation>());
                }
            }
            if (courtierDto != null)
                courtier = (Courtier)courtierDto;
            else
                courtier = new Courtier()
                {
                    CodeCourtier = codeCourtier,
                    NomCourtier = "",
                    Commission = 0
                };
            return PartialView("/Views/AnCourtier/DivCourtier.cshtml", courtier);
        }
        [AlbAjaxRedirect]
        public RedirectToRouteResult Precedent(string codeContrat, string versionContrat, string type, string tabGuid, string modeNavig, string addParamType, string addParamValue, bool isModeConsultationEcran)
        {
            string id = AlbParameters.BuildStandardId(
                new Folder(new[] { codeContrat, versionContrat, type }),
                tabGuid,
                addParamValue + (isModeConsultationEcran ? string.Empty : "||IGNOREREADONLY|1"),
                modeNavig);
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                if (client.Channel.ExistCoAs(codeContrat, versionContrat, type, modeNavig.ParseCode<ModeConsultation>())) {
                    return RedirectToAction("Index", "AnCoAssureurs", new { id });
                }
                else {
                    string typeAvt = GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);
                    switch (typeAvt) {
                        case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                        case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                        case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                            return RedirectToAction("Index", "AvenantInfoGenerales", new { id });
                        default:
                            return RedirectToAction("Index", "AnInformationsGenerales", new { id });
                    }
                }
            }
        }
        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string codeContrat, string versionContrat, string type, string tabGuid, string modeNavig, string addParamType, string addParamValue, string readonlyDisplay, bool isModeConsultationEcran)
        {
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            var isOffreReadonly = GetIsReadOnly(tabGuid, codeContrat + "_" + versionContrat + "_" + type, numAvn);
            if (cible == "AnCourtier")
            {
                //Verif & Enregistrement trace modification AVN
                string typeAvt = GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE);
                if (typeAvt == AlbConstantesMetiers.TYPE_AVENANT_MODIF) {
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>()) {
                        client.Channel.CheckAndSaveTraceAvenant(codeContrat, versionContrat, type, "KCOMM", string.Empty, string.Empty, string.Empty, string.Empty, "GEN", string.Empty, "C", string.Empty, string.Empty, "O", string.Empty, GetUser());
                    }
                }
                addParamValue += readonlyDisplay == "true" ? "||FORCEREADONLY|1" : "||AVNREFRESHUSERUPDATE|1||IGNOREREADONLY|1";
            }
            else {
                addParamValue += isModeConsultationEcran || isOffreReadonly ? string.Empty : "||IGNOREREADONLY|1";
            }
            string id = AlbParameters.BuildStandardId(
                new Folder(new[] { codeContrat, versionContrat, type }),
                tabGuid,
                addParamValue,
                modeNavig);
            return RedirectToAction(job, cible, new { id });
        }

        [AlbAjaxRedirect]
        [HandleJsonError]
        [ErrorHandler]
        public ActionResult Suivant(string codeContrat, string versionContrat, string type, string codeAvn, string tabGuid, string risqueObjet, string txtSaveCancel,
            string modeAffichage, string tauxStdHCAT, string tauxStdCAT, string tauxContratHCAT, string tauxContratCAT, string commentaire, string codeEncaissementContrat,
            string paramRedirect, string modeNavig, bool hasRisques, string addParamType, string addParamValue, bool isModeConsultationEcran, string acteGestion, string acteGestionRegule, string reguleId)
        {
            if (!string.IsNullOrEmpty(codeAvn) && codeAvn.Trim() != "0")
                hasRisques = true;
            var folder = string.Format("{0}_{1}_{2}" ,codeContrat , versionContrat ,type);
            var isOffreReadonly = GetIsReadOnly(tabGuid, folder, codeAvn);
            var isModifHorsAvn = GetIsModifHorsAvn(tabGuid, string.Format("{0}_{1}", folder, string.IsNullOrEmpty(codeAvn) ? "0" : codeAvn));
            if ((!isOffreReadonly || isModifHorsAvn)
                    && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>())
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var serviceContext=client.Channel;
                    var courtiersDto = serviceContext.GetListCourtiers(codeContrat, versionContrat, type, codeAvn, modeNavig.ParseCode<ModeConsultation>());
                    ////T 3997 : Vérification des partenaires
                    //VerificationPartenairesCourtiers(courtiersDto);
                    float total = 0;
                    foreach (var courtier in courtiersDto)
                    {
                        total += courtier.Commission;
                    }
                    if (total != 100 && !string.IsNullOrEmpty(codeEncaissementContrat) && codeEncaissementContrat != "D") throw new AlbFoncException("La somme des % de commissions doit être égale à 100");
                    //Sauvegarde des taux de commission seulement si le mode d'encaissement contrat n'est pas direct
                    if (codeEncaissementContrat != "D")
                    {
                        if (!string.IsNullOrEmpty(commentaire)) {
                            commentaire = commentaire.Replace("\r\n", "<br>").Replace("\n", "<br>");
                        }
                        else {
                            commentaire = string.Empty;
                        }

                        _ = double.TryParse(tauxContratCAT, out var dTauxContratCAT);
                        _ = double.TryParse(tauxContratHCAT, out var dTauxContratHCAT);
                        _ = double.TryParse(tauxStdCAT, out var dTauxStdCAT);
                        _ = double.TryParse(tauxStdHCAT, out var dTauxStdHCAT);

                        CommissionCourtierDto toSend = new CommissionCourtierDto
                        {
                            Commentaires = commentaire.Trim(),
                            TauxContratCAT = dTauxContratCAT,
                            TauxContratHCAT = dTauxContratHCAT,
                            TauxStandardCAT = dTauxStdCAT,
                            TauxStandardHCAT = dTauxStdHCAT,
                            IsStandardCAT = (dTauxStdCAT == dTauxContratCAT ? "O" : "N"),
                            IsStandardHCAT = (dTauxStdHCAT == dTauxContratHCAT ? "O" : "N")
                        };
                        //Appel au webservice pour sauvegarde
                        serviceContext.UpdateCommissionsStandardCourtier(codeContrat, versionContrat, type, toSend);

                    }
                }
            }
            if (modeAffichage == ModeStandard) {
                string codeRsq;
                string codeObj;
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                    codeRsq = client.Channel.GetFirstCodeRsq(codeContrat, versionContrat, type);
                    codeObj = client.Channel.GetFirstCodeObjRsq(codeContrat, versionContrat, type, codeRsq);
                }

                if (!string.IsNullOrEmpty(paramRedirect)) {
                    var tabParamRedirect = paramRedirect.Split('/');
                    return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
                }
                string id = AlbParameters.BuildFullId(
                    new Folder(new[] { codeContrat, versionContrat, type }),
                    hasRisques ? null : new[] { codeRsq, codeObj },
                    tabGuid,
                    addParamValue + (isModeConsultationEcran && modeNavig != "H" ? "||IGNOREREADONLY|1" : String.Empty),
                    modeNavig);
                return RedirectToAction("Index", hasRisques ? "MatriceRisque" : "DetailsObjetRisque", new { id, returnHome = txtSaveCancel, guidTab = tabGuid });
            }
            else {
                return ReloadCommission(codeContrat, versionContrat, type, codeAvn, modeNavig, acteGestion, acteGestionRegule, reguleId, isOffreReadonly);
            }
        }
        #endregion
        #region Méthodes privées
        protected override void LoadInfoPage(string id)
        {

            using (var CommonOffreClient =  Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                string[] tId = id.Split('_');
                var infosBase = CommonOffreClient.Channel.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig);
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
            }

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                string[] tId = id.Split('_');
                if (tId.Length == 3)
                {
                    long version = 0;
                    long.TryParse(tId[1], out version);
                    id = tId[0];
                    ModeleInfoPageCommissionDto Toreturn = new ModeleInfoPageCommissionDto()
                   {
                       LstCourties = new List<CourtierDto>(),
                       CommissionsStd = new CommissionCourtierDto()
                   };
                    Toreturn = client.Channel.LoadInfoPageCourtier(
                        id, version.ToString(), tId[2], model.NumAvenantPage,
                        GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage), model.ModeNavig.ParseCode<ModeConsultation>(), GetUser(), model.ActeGestion);
                    var courtiersDto = Toreturn.LstCourties;
                    var courtiers = Courtier.LoadCourtiers(courtiersDto);
                    model.Courtiers = courtiers;

                    if (model.Contrat.Risques != null && model.Contrat.Risques.Count > 0 && model.Contrat.Risques[0].Objets.Count > 0)
                        model.RisqueObj = model.Contrat.Risques[0].Objets[0].Descriptif;
                    if (courtiersDto != null && courtiersDto.Count > 0)
                    {
                        model.CourtierApporteur = courtiersDto[0].CodeApporteur;
                        model.CourtierGestionnaire = courtiersDto[0].CodeGest;
                        model.CourtierPayeur = courtiersDto[0].CodePayeur;
                    }
                    // récupération des commissions de courtiers standard (cas de contrat non direct)
                    if (model.Contrat.CodeEncaissement != "D") {
                        model.CommissionsStandard = (CommissionCourtier)Toreturn.CommissionsStd;
                    }
                    else {
                        model.CommissionsStandard = new CommissionCourtier();
                    }
                    model.ModeAffichage = ModeStandard;
                    model.Contexte = string.Empty;
                }
            }
        }
        private void SetBandeauNavigation()
        {
            model.AfficherBandeau = true;
            model.AfficherNavigation = this.model.AfficherBandeau;
            model.Bandeau = base.GetInfoBandeau(this.model.AllParameters.Folder.Type);
            string typeAvt = GetAddParamValue(this.model.AddParamValue, AlbParameterName.AVNTYPE);
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
                    model.Bandeau.StyleBandeau = model.ScreenType;
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
            //Gestion des Etapes
            model.Navigation = new Navigation_MetaModel();
            model.Navigation.Etape = Navigation_MetaModel.ECRAN_INFOGENERALE;
            //Affichage de la navigation latérale en arboresence
            model.NavigationArbre = GetNavigationArbreAffaireNouvelle("CoCourtiers");
            model.NavigationArbre.IsRegule = typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGUL;
        }
        private ActionResult ReloadCommission(string codeOffre, string version, string type, string codeAvn, string modeNavig, string acteGestion, string acteGestionRegule, string reguleId, bool isreadonly)
        {
            Commission toReturn = null;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffreClient=client.Channel;
                var quittancesDto = finOffreClient.GetQuittances(codeOffre, version, type, codeAvn, modeNavig.ParseCode<ModeConsultation>(), string.Empty, string.Empty, false, false,
                    GetUser(), !string.IsNullOrEmpty(acteGestionRegule) ? acteGestionRegule : acteGestion, reguleId, isreadonly, string.Empty);
                toReturn = ModeleQuittancePage.LoadQuittanceCommission(quittancesDto);
                toReturn.ActeGestionRegule = acteGestionRegule;
            }
            if (toReturn == null)
                toReturn = new Commission();
            return PartialView("/Views/Quittance/QuittanceCommissions.cshtml", toReturn);
        }

        /// <summary>
        /// Vérification des courtier
        /// </summary>
        /// <param name="list"> Liste des courtiers </param>
        private void VerificationPartenairesCourtiers(List<CourtierDto> courtiers)
        {
            if (courtiers?.Count > 0)
            {
                var courtiersBaseIds = new List<int> { courtiers[0].CodeGest, courtiers[0].CodeApporteur, courtiers[0].CodePayeur }.Distinct();
                var partenaires = new PartenairesDto
                {
                    CourtiersAdditionnels = courtiers.Where(x=> !courtiersBaseIds.Any(y=>y==x.CodeCourtier)).Select(t => new PartenaireDto
                    {
                        Code = t.CodeCourtier.ToString(),
                        Nom = t.NomCourtier,
                    }).ToList()
                };
                VerificationPartenaires(partenaires);

            }
        }
        #endregion
    }
}
