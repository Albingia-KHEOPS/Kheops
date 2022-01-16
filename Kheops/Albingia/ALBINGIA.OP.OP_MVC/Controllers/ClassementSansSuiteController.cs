using Albingia.Kheops.OP.Application.Port.Driver;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesQuittances;
using OP.WSAS400.DTO.AffaireNouvelle;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ICommon;
using OPServiceContract.ISaisieCreationOffre;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    [ErrorHandler]
    public class ClassementSansSuiteController : ControllersBase<ModeleClassementSansSuitePage>
    {
        // GET: ClassementSansSuite
        public ActionResult Index(string id)
        {
            model.PageTitle = "Classement sans suite";
            id = InitializeParams(id);
            LoadInfoPage(id);
            return View(model);
        }

        public ActionResult ClasserSansSuite(string codeAffaire, int version, string type, string listeAnnulQuitt)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var policeServicesClient = client.Channel;
                policeServicesClient.ClasserContratsSansSuite(codeAffaire, version, type, listeAnnulQuitt, GetUser());
            }
            return null;
        }

        [AlbAjaxRedirect]
        [ErrorHandler]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string tabGuid,
            string addParamType, string addParamValue, string modeNavig)
        {
            if (cible == "RechercheSaisie")
            {
                return RedirectToAction("Index", "RechercheSaisie", new { id = codeOffre + "_" + version + "_" + type + "_loadParam" + tabGuid });
            }
            else
            {
                var isReadOnly = GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type)
                    || ModeConsultation.Historique == modeNavig.ParseCode<ModeConsultation>();

                if (isReadOnly)
                {
                    return RedirectToAction(job, cible, new { id = string.Format("{0}_{1}_{2}_{3}{4}{5}", codeOffre, version, type, tabGuid, "addParam" + addParamType + "|||" + addParamValue + "addParam", "modeNavig" + modeNavig + "modeNavig") });
                }

                var myId = string.Format("{0}_{1}_{2}_{3}", codeOffre, version, type, tabGuid);

                return RedirectToAction(job, cible, new { id = myId });
            }
        }

        #region Méthodes privées
        private VisualisationQuittances GetVisualisationQuittancesModele(string codeOffre, string codeAvn, string version, string etat, bool isEntete, AlbConstantesMetiers.TypeQuittances typeQuittances, string tabGuid, string modeNavig = "")
        {
            var toReturn = new VisualisationQuittances();
            toReturn.ListQuittances = GetListeQuittancesLignes(codeOffre, codeAvn, isEntete, version, etat, tabGuid, typeQuittances: typeQuittances, modeNavig: modeNavig);
            //toReturn.Situations = LstSituations;
            //toReturn.TypesOperation = LstTypesOperation;
            toReturn.IsOpenedFromHeader = isEntete;
            toReturn.IsHisto = modeNavig.ParseCode<ModeConsultation>() == ModeConsultation.Historique;
            toReturn.IsReadOnly = toReturn.IsHisto || isEntete;
            var folder = string.Format("{0}_{1}_{2}", codeOffre, version, AlbConstantesMetiers.TYPE_CONTRAT);
            var isModifHorsAvn = GetIsModifHorsAvn(tabGuid, string.Format("{0}_{1}", folder, string.IsNullOrEmpty(codeAvn) ? "0" : codeAvn));
            toReturn.IsModifHorsAvenant = isModifHorsAvn;
            return toReturn;
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

        protected override void LoadInfoPage(string id)
        {
            string[] tId = id.Split('_');
            //model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type); //false;


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
                    Etat = infosBase.Etat,
                    Situation = infosBase.Situation,
                    NatureContrat = infosBase.Nature,
                    LibelleNatureContrat = infosBase.LibNature,
                    Observations = infosBase.Observation,
                    NumAvenant = infosBase.NumAvenant,
                    Delegation = infosBase?.CabinetGestionnaire?.Delegation?.Nom,
                    Inspecteur = infosBase.CabinetGestionnaire?.Inspecteur
                };

                model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type); //false;

            }


            SetContentData(model.Contrat);

            model.Navigation = new Navigation_MetaModel
            {
                Etape = Navigation_MetaModel.ECRAN_INFOGENERALE
            };

            model.NavigationArbre = GetNavigationArbreAffaireNouvelle("InfoSaisie", returnEmptyTree: true);

            if (!string.IsNullOrEmpty(GetAddParamValue(model.AddParamValue, AlbParameterName.AVNID)))
            {
                model.Bandeau = null;
                model.AfficherBandeau = base.DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
                model.AfficherArbre = true;
                if (model.AfficherBandeau)
                    model.Bandeau = base.GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
                model.Bandeau.StyleBandeau = model.ScreenType;
            }

            model.Primes = GetVisualisationQuittancesModele(model.Contrat.CodeContrat, model.Contrat.NumAvenant.ToString(), model.Contrat.VersionContrat.ToString(), model.Contrat.Etat, false, AlbConstantesMetiers.TypeQuittances.Toutes, "");
            //model.LayoutModeAvt = model.ModeAvt;
        }


        private void SetContentData(ContratDto contrat)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.GetContrat(contrat.CodeContrat, contrat.VersionContrat.ToString(), contrat.Type, model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                if (result != null)
                {
                    if (result != null)
                    {
                        model.Contrat.DateEffetAnnee = result.DateEffetAnnee;
                        model.Contrat.DateEffetMois = result.DateEffetMois;
                        model.Contrat.DateEffetJour = result.DateEffetJour;
                        model.Contrat.DateEffetHeure = result.DateEffetHeure;
                        model.Contrat.FinEffetAnnee = result.FinEffetAnnee;
                        model.Contrat.FinEffetMois = result.FinEffetMois;
                        model.Contrat.FinEffetJour = result.FinEffetJour;
                        model.Contrat.FinEffetHeure = result.FinEffetHeure;
                        model.Contrat.NomCourtierGest = result.NomCourtierGest;
                        model.Contrat.SouscripteurCode = result.SouscripteurCode;
                        model.Contrat.SouscripteurNom = result.SouscripteurNom;
                        model.Contrat.GestionnaireCode = result.GestionnaireCode;
                        model.Contrat.GestionnaireNom = result.GestionnaireNom;
                        model.Contrat.TypeRetour = result.TypeRetour;
                        model.Contrat.LibRetour = result.LibRetour;
                        model.Contrat.PeriodiciteCode = result.PeriodiciteCode;
                        model.Contrat.PeriodiciteNom = result.PeriodiciteNom;
                        model.Contrat.ProchaineEchAnnee = result.ProchaineEchAnnee;
                        model.Contrat.ProchaineEchMois = result.ProchaineEchMois;
                        model.Contrat.ProchaineEchJour = result.ProchaineEchJour;
                        model.Contrat.DateEffetAvenant = AlbConvert.ConvertIntToDate(result.DateEffetAnnee * 10000 + result.DateEffetMois * 100 + result.DateEffetJour);
                        model.Contrat.ReguleId = result.ReguleId;

                        model.Contrat.TypePolice = !string.IsNullOrEmpty(result.TypePolice) ? result.TypePolice : "S";

                    }
                }
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
                        Etape = Navigation_MetaModel.ECRAN_ENGAGEMENTS,
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
                        Etape = Navigation_MetaModel.ECRAN_ENGAGEMENTS,
                        IdOffre = model.Contrat.CodeContrat,
                        Version = int.Parse(model.Contrat.VersionContrat.ToString())
                    };
                }
            }
        }

        private void SetArbreNavigation()
        {
            if (model.Offre != null)
                model.NavigationArbre = GetNavigationArbre(string.Empty, returnEmptyTree: true);
            else if (model.Contrat != null)
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle(string.Empty, returnEmptyTree: true);
        }

        //private void RefreshLock(string controller, string tabGuid)
        //{
        //    if (controller?.ToUpperInvariant() == "MODIFIEROFFRE")
        //    {
        //        var g = Guid.TryParse(tabGuid.Replace(PageParamContext.TabGuidKey, string.Empty).ToLower(), out var x) ? x : Guid.Empty;
        //        var acces = MvcApplication.ListeAccesAffaires.First(a => a.ModeAcces == AccesOrigine.PrisePosition && a.TabGuid == g);
        //        acces.ModeAcces = AccesOrigine.Modifier;
        //        MvcApplication.ListeAccesAffaires.Remove(acces);
        //        MvcApplication.ListeAccesAffaires.Add(acces);
        //    }
        //    else
        //    {
        //        FolderController.DeverrouillerAffaire(tabGuid);
        //    }
        //}

        private static List<AlbSelectListItem> _lstMotifs;
        public static List<AlbSelectListItem> LstMotifs
        {
            get
            {
                //Si les données ont déjà été chargé en mémoire, on renvoit une nouvelle instance de la liste
                if (_lstMotifs != null)
                {
                    var toReturn = new List<AlbSelectListItem>();
                    _lstMotifs.ForEach(elm => toReturn.Add(new AlbSelectListItem
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
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IConfirmationSaisie>())
                {
                    var serviceContext = client.Channel;
                    var lstMotifs = serviceContext.GetListeMotifs();
                    lstMotifs.ForEach(elm => value.Add(new AlbSelectListItem
                    {
                        Value = elm.Code.ToString(),
                        Text = elm.Code + " - " + elm.Libelle,
                        Title = elm.Code + " - " + elm.Libelle,
                        Selected = false
                    }
                    ));
                }
                _lstMotifs = value;
                return value;
            }
        }

        #endregion
    }


}