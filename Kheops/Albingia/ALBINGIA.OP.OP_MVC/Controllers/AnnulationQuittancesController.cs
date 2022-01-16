using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesQuittances;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class AnnulationQuittancesController : ControllersBase<ModeleAnnulationQuittancesPage>
    {
        #region Méthodes Publiques

        [AlbVerifLockedOffer("id", "P")]
        [ErrorHandler]
        public ActionResult Index(string id)
        {
            model.Reprise = id.Contains("reprise") ? id.Split(new[] { "reprise" }, StringSplitOptions.None)[1] == "1" : false;
            id = id.Contains("reprise") ? id.Split(new[] { "reprise" }, StringSplitOptions.None)[0] : id;
            model.PageTitle = "Quittances en-cours";
            id = InitializeParams(id);

            LoadInfoPage(id);
            if (model.ListQuittances.Count == 0)
            {
                if (model.Reprise)
                {
                    return RedirectToAction("Index", "AvenantInfoGenerales", new { id = id + GetSurroundedTabGuid(model.TabGuid) + BuildAddParamString(model.AddParamType, model.AddParamValue) + GetFormatModeNavig(model.ModeNavig) });
                }

                string[] tId = id.Split('_');

                if (!model.IsReadOnly && GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE) == AlbConstantesMetiers.TYPE_AVENANT_RESIL)
                    return RedirectToAction("Index", "EngagementPeriodes", new { id = tId[0] + "_" + tId[1] + "_" + tId[2] + "_" + GetSurroundedTabGuid(model.TabGuid) + BuildAddParamString(model.AddParamType, model.AddParamValue) + GetFormatModeNavig(model.ModeNavig), returnHome = string.Empty });

                return RedirectToAction("Index", "ChoixClauses", new { id = tId[0] + "_" + tId[1] + "_" + tId[2] + "_¤AvenantInfoGenerales¤Index¤" + tId[0] + "£" + tId[1] + "£" + tId[2] + GetSurroundedTabGuid(model.TabGuid) + BuildAddParamString(model.AddParamType, model.AddParamValue) + GetFormatModeNavig(model.ModeNavig), returnHome = string.Empty });
            }
            return View(model);
        }

        [ErrorHandler]
        public ActionResult FiltrerAnnulationQuittances(string codeOffre, string version, int avenant, bool isEntete, DateTime dateEffetAvenant, DateTime? dateEmission, string typeOperation, string situation, DateTime? datePeriodeDebut, DateTime? datePeriodeFin, string acteGestion, string tabGuid, string modeNavig, string colTri)
        {
            var isreadonly = GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + AlbConstantesMetiers.TYPE_CONTRAT, avenant.ToString());
            var result = GetListeQuittancesLignes(false, codeOffre, version, isEntete, avenant, dateEffetAvenant, acteGestion, isreadonly, modeNavig.ParseCode<ModeConsultation>(), tabGuid, dateEmission, typeOperation, situation, datePeriodeDebut, datePeriodeFin, colTri: colTri);
            List<VisualisationQuittancesLigne> toReturn = result.ListQuittances;
            return PartialView("/Views/Quittance/VisualisationQuittancesListe.cshtml", toReturn);
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult AnnulationQuittancesEnregistrer(string codeOffre, string version, string type, string listeAnnulQuittances, string tabGuid, string isCheckedEch,
            string paramRedirect, string modeNavig, string txtSaveCancel, string addParamType, string addParamValue)
        {
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            if (!GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, numAvn)
              && ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>())
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
                {
                    var finOffreClient = client.Channel;
                    finOffreClient.EnregistrerQuittancesAnnulees(codeOffre, version, listeAnnulQuittances);
                }
            }
            if (!string.IsNullOrEmpty(paramRedirect))
            {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }

            if (GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE) == AlbConstantesMetiers.TYPE_AVENANT_RESIL)
            {
                if (isCheckedEch == "False")
                {
                    return RedirectToAction("Index", "EngagementPeriodes", new { id = codeOffre + "_" + version + "_" + type + "_" + GetSurroundedTabGuid(tabGuid) + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig), returnHome = txtSaveCancel, guidTab = tabGuid });
                }
                else
                {
                    return RedirectToAction("Index", "ControleFin", new { id = codeOffre + "_" + version + "_" + type + GetSurroundedTabGuid(tabGuid) + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
                }
            }

            if (GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE) == AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR)
            {
                return RedirectToAction("Index", "AvenantInfoGenerales", new { id = codeOffre + "_" + version + "_" + type + GetSurroundedTabGuid(tabGuid) + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
            }

            return RedirectToAction("Index", "ChoixClauses", new { id = codeOffre + "_" + version + "_" + type + "_¤AvenantInfoGenerales¤Index¤" + codeOffre + "£" + version + "£" + type + GetSurroundedTabGuid(tabGuid) + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig), returnHome = txtSaveCancel, guidTab = tabGuid });
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult RedirectionAnnuler(string codeOffre, string version, string type, string tabGuid, string modeNavig, string addParamType, string addParamValue)
        {
            var controller =
                (GetAddParamValue(addParamValue, AlbParameterName.AVNTYPE) == AlbConstantesMetiers.TYPE_AVENANT_RESIL)
                ? "CreationAvenant"
                : "AvenantInfoGenerales";

            return RedirectToAction("Index", controller, new { id = codeOffre + "_" + version + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });

        }

        #endregion

        #region Méthode Privée
        protected override void LoadInfoPage(string id)
        {
            string situation = string.Empty;
            string[] tId = id.Split('_');
            if (tId[2] == "P")
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var serviceContext = client.Channel;
                    model.Contrat = serviceContext.GetContrat(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                }
                string typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                switch (typeAvt)
                {
                    case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                        model.NumAvenantExterne = model.Contrat.NumAvenant.ToString();
                        if (model.Reprise)
                        {
                            model.Situation = "NonAnnuleeNonAnnulation";
                        }
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                        model.NumAvenantExterne = model.Contrat.NumAvenant.ToString();
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                        model.NumAvenantExterne = model.Contrat.NumAvenant.ToString();
                        break;
                }
            }
            if (model.Contrat != null)
            {
                model.AfficherBandeau = base.DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
            }

            #region Navigation Arbre
            SetArbreNavigation();
            #endregion
            #region Bandeau
            model.Bandeau = null;
            SetBandeauNavigation(id);
            #endregion

            int numAvn = 0;
            Int32.TryParse(model.NumAvenantPage, out numAvn);
            model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), tId[0] + "_" + tId[1] + "_" + tId[2], numAvn.ToString());

            var resultModel = GetListeQuittancesLignes(true, tId[0], tId[1], false, numAvn, model.Contrat.DateEffetAvenant.Value, model.ActeGestion, model.IsReadOnly, model.ModeNavig.ParseCode<ModeConsultation>(), model.TabGuid, typeQuittances: AlbConstantesMetiers.TypeQuittances.Toutes, situation: model.Situation); ;

            model.ListQuittances = resultModel.ListQuittances;
            if (model.Reprise)
            {
                model.PeriodeDebut = resultModel.PeriodeDebut;
            }

            model.Situations = QuittanceController.LstSituations;
            model.TypesOperation = QuittanceController.LstTypesOperation;
        }
        private void SetBandeauNavigation(string id)
        {
            if (model.AfficherBandeau)
            {
                model.Bandeau = GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
                //Gestion des Etapes
                if (model.Contrat != null)
                {
                    string typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    switch (typeAvt)
                    {
                        case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;

                        case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                            break;

                        case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                            break;

                        case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                            break;

                        default:
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                            break;
                    }
                    model.Navigation = new Navigation_MetaModel
                    {
                        Etape = Navigation_MetaModel.ECRAN_COTISATIONS,
                        IdOffre = model.Contrat.CodeContrat,
                        Version = int.Parse(model.Contrat.VersionContrat.ToString())
                    };
                    //model.NavigationArbre = GetNavigationArbreAffaireNouvelle(ContentData, "CoCourtiers");
                    //model.NavigationArbre.IsRegule = typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGUL;
                }
            }
        }
        private void SetArbreNavigation()
        {
            if (model.Contrat != null)
            {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Cotisation");
            }
        }
        private VisualisationQuittances GetListeQuittancesLignes(bool init, string codeOffre, string version, bool isEntete, int avenant, DateTime dateEffetAvenant, string acteGestion, bool isreadonly, ModeConsultation modeNavig, string tabGuid, DateTime? dateEmission = null, string typeOperation = "", string situation = "", DateTime? datePeriodeDebut = null, DateTime? datePeriodeFin = null, AlbConstantesMetiers.TypeQuittances typeQuittances = AlbConstantesMetiers.TypeQuittances.Toutes, string colTri = "")
        //private List<VisualisationQuittancesLigne> GetListeQuittancesLignes(bool init, string codeOffre, string version, int avenant, DateTime dateEffetAvenant, string acteGestion, bool isreadonly, ModeConsultation modeNavig, DateTime? dateEmission = null, string typeOperation = "", string situation = "", DateTime? datePeriodeDebut = null, DateTime? datePeriodeFin = null, AlbConstantesMetiers.TypeQuittances typeQuittances = AlbConstantesMetiers.TypeQuittances.Toutes)
        {
            VisualisationQuittances toReturn = new VisualisationQuittances();


            //List<VisualisationQuittancesLigne> toReturn = new List<VisualisationQuittancesLigne>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>())
            {
                var finOffreClient = client.Channel;
                //var result = finOffreClient.GetListeQuittancesAnnulation(init, codeOffre, version, avenant, dateEffetAvenant, dateEmission, typeOperation, situation, datePeriodeDebut, datePeriodeFin, typeQuittances, GetUser(), acteGestion, isreadonly, modeNavig);
                //if (result != null && result.Any())
                //{
                //    result.ForEach(elm => toReturn.Add((VisualisationQuittancesLigne)elm));
                //    toReturn.ForEach(elm =>
                //    {
                //        elm.IsModeAnnulation = true;
                //        elm.CurrentCodeAvt = avenant;
                //    });
                //}

                var result = finOffreClient.GetListeQuittancesAnnulation(init, codeOffre, version, avenant, dateEffetAvenant, dateEmission, typeOperation, situation, datePeriodeDebut, datePeriodeFin, typeQuittances, GetUser(), acteGestion, isreadonly, modeNavig, colTri);
                if (result != null)
                {
                    var listQuittances = new List<VisualisationQuittancesLigne>();
                    toReturn = (VisualisationQuittances)result;
                    if (result.ListQuittances != null && result.ListQuittances.Any())
                    {
                        result.ListQuittances.ForEach(elm => listQuittances.Add((VisualisationQuittancesLigne)elm));
                    }
                    var folder = string.Format("{0}_{1}_{2}", codeOffre, version, AlbConstantesMetiers.TYPE_CONTRAT);
                    var isModifHorsAvn = GetIsModifHorsAvn(tabGuid, string.Format("{0}_{1}", folder, avenant));
                    listQuittances.ForEach(elm =>
                    {
                        elm.IsModeAnnulation = true &&
                        (acteGestion != AlbConstantesMetiers.TRAITEMENT_AVNMD && acteGestion != AlbConstantesMetiers.TRAITEMENT_AVNRG && acteGestion != AlbConstantesMetiers.TRAITEMENT_AVNRM
                        || elm.Avenant == avenant);
                        elm.CurrentCodeAvt = avenant;
                        elm.DisplayEditionQuittance = Common.AlbTransverse.GetIsDisplayQuittance(HttpContext, model.NumAvenantPage != "0");
                        model.IsModifHorsAvenant = isModifHorsAvn;
                    });

                    toReturn.ListQuittances = listQuittances;

                }
            }
            return toReturn;
        }
        #endregion
    }
}
