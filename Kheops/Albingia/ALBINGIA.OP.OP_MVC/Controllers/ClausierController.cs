using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesClauses;
using ALBINGIA.OP.OP_MVC.Models.ModelesObjet;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO.AffaireNouvelle;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class ClausierController : ControllersBase<ModeleClausierPage>
    {
        #region méthodes publiques
        /// <summary>
        /// Indexes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        
        [ErrorHandler]
        public ActionResult Index(string id)
        {
            id = InitializeParams(id);
            LoadInfoPage(id);
            return View(model);
        }

        public class RechercheParams {
             public string Libelle { get; set; }
             public string Motcle1 { get; set; }
             public string Motcle2 { get; set; }
             public string Motcle3 { get; set; }
             public string Sequence { get; set; }
             public string Rubrique { get; set; }
             public string Sousrubrique { get; set; }
             public string SelectionPossible { get; set; }
             public string ModaliteAffichage { get; set; }
             public int Date{ get; set; }

        }

        /// <summary>
        /// Recherches the specified titre.
        /// </summary>
        /// <param name="libelle"></param>
        /// <param name="motcle1"></param>
        /// <param name="motcle2"></param>
        /// <param name="motcle3"></param>
        /// <param name="sequence"></param>
        /// <param name="rubrique"></param>
        /// <param name="sousrubrique"></param>
        /// <param name="selectionPossible"></param>
        /// <param name="modaliteAffichage"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [ErrorHandler]
        public ActionResult Recherche(RechercheParams recherche)
        {

            var resClausier = new List<ModeleClausier>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var screenClient=client.Channel;
                var result = screenClient.SearchClause(
                                 recherche.Libelle,
                                 recherche.Motcle1,
                                 recherche.Motcle2,
                                 recherche.Motcle3,
                                 recherche.Sequence,
                                 recherche.Rubrique,
                                 recherche.Sousrubrique,
                                 recherche.ModaliteAffichage,
                                 recherche.Date
                                );

                if (result != null)
                {
                    result.ToList().ForEach(m => resClausier.Add((ModeleClausier)m));
                }
            }
            var modeleListeClauses = new ModeleListeClauses
            {
                Clauses = resClausier,
                SelectionPossible = recherche.SelectionPossible,
                ModaliteAffichage = recherche.ModaliteAffichage,
                Date = recherche.Date
            };
            if (modeleListeClauses.Clauses != null)
            {
                modeleListeClauses.Clauses.ForEach(c => c.SelectionPossible = recherche.SelectionPossible);
            }
            return PartialView("ListeClauses", modeleListeClauses);
        }
        /// <summary>
        /// Historique
        /// </summary>
        /// <param name="rubrique"></param>
        /// <param name="sousrubrique"></param>
        /// <param name="sequence"></param>
        /// <param name="code"></param>
        /// <param name="libelle"></param>
        /// <param name="date"></param>
        /// <param name="origineAffichage"></param>
        /// <returns></returns>
        [ErrorHandler]
        public ActionResult Historique(string rubrique, string sousrubrique, string sequence, string code, string libelle, int date, string origineAffichage)
        {
            var modeleClausier = new ModeleClausier();
            var resHisto = new List<ClausierHistorique>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var screenClient=client.Channel;
                var result = screenClient.GetHistoClause(rubrique, sousrubrique, sequence).ToList();
                if (result.Any())
                {
                    result.ToList().ForEach(m => resHisto.Add(ClausierHistorique.GetClausierHistorique(m, date)));
                }
                modeleClausier.Historique = resHisto;
                foreach (var c in resHisto)
                {
                    if (!c.Valide) continue;
                    modeleClausier.ClauseValideExist = true;
                    modeleClausier.CodeClauseValide = c.Code;
                    modeleClausier.VersionClauseValide = c.Version;
                    break;
                }
                modeleClausier.Code = code;
                modeleClausier.Libelle = libelle;
                modeleClausier.Date = date;
                modeleClausier.OrigineAffichage = origineAffichage;
            }
            return PartialView(string.IsNullOrEmpty(modeleClausier.OrigineAffichage) ? "ClausierHistorique" : "ClausierHistoriqueInvalide", modeleClausier);
        }
        /// <summary>
        /// Validers this instance.
        /// </summary>
        /// <returns></returns>
        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult Valider(string type, string numeroOffre, string version, string tabGuid, string natureClause, string codeClause, string rubrique, string sousRubrique, string sequence, string versionClause,
            string actionEnchaine, string contexte, string provenance, string saveCancel, string paramRedirect, string codeRsq, string codeFor, string codeOption, string etape, string codeObj, string modeNavig, string addParamType, string addParamValue)
        {
            var numAvn = GetAddParamValue(addParamValue, AlbParameterName.AVNID);
            //Sauvegarde uniquement si l'écran n'est pas en readonly
            if (!GetIsReadOnly(tabGuid, numeroOffre + "_" + version + "_" + type, numAvn))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    var screenClient=client.Channel;
                    //screenClient.EnregistreClauseUnique(type, numeroOffre, version, natureClause, codeClause, versionClause, actionEnchaine, contexte, "O", etape);
                    string retourMsg = screenClient.SaveClause(numeroOffre, version, type, etape, etape, codeRsq, codeObj, codeFor, codeOption, contexte, codeClause, rubrique, sousRubrique, sequence, versionClause, numAvn);
                    if (!string.IsNullOrEmpty(retourMsg)) throw new AlbFoncException(retourMsg);
                }
            }
            //Redirection vers la page sélectionnée dans le menu
            if (!string.IsNullOrEmpty(paramRedirect))
            {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] + GetFormatModeNavig(modeNavig) });
            }
            return RedirectToAction("Index", "ChoixClauses", new { id = numeroOffre + "_" + version + "_" + type + "_" + provenance.Replace("/", "¤").Replace("_", "£") + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig), returnHome = saveCancel, guidTab = tabGuid });
        }
        [ErrorHandler]
        public ActionResult LoadVisualisationClause(string codeClause)
        {
            var modeleClausier = new ModeleClausier { Code = codeClause };
            return PartialView("VisualisationClause", modeleClausier);
        }
        [ErrorHandler]
        public ActionResult GetSousRubriques(string codeRubrique, int paramScreen = 0)
        {
            var model = new ModeleDDLSousRubrique();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var screenClient=client.Channel;
                var result = screenClient.GetListSousRubriques(codeRubrique);
                if (result != null)
                {
                    model.SousRubrique = string.Empty;
                    model.SousRubriques = result.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                }
                if (paramScreen > 0)
                    return PartialView("../ParamClause/ParamClauseSousRubrique", model);
                return PartialView("ListSousRubriques", model);
            }
        }
        [ErrorHandler]
        public ActionResult GetSequences(string codeRubrique, string codeSousRubrique, int paramScreen = 0)
        {
            var model = new ModeleDDLSequence();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var screenClient=client.Channel;
                var result = screenClient.GetListSequences(codeRubrique, codeSousRubrique);
                if (result != null)
                {
                    model.Sequence = string.Empty;
                    model.Sequences = result.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                }
                if (paramScreen > 0)
                    return PartialView("../ParamClause/ParamClauseSequence", model);
                return PartialView("ListSequences", model);
            }
        }
        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string tabGuid, string provenance, string modeNavig, string addParamType, string addParamValue)
        {
            return cible == "RechercheSaisie" ?
                RedirectToAction(job, cible)
                : RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + "_" + provenance + GetSurroundedTabGuid(tabGuid) + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
        }

        #endregion

        #region méthodes privées
        protected override void LoadInfoPage(string id)
        {
            string[] tId = id.Split('_');
            switch (tId[2])
            {
                case "O":
                    using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                    {
                     var CommonOffreClient=chan.Channel;
                        model.Offre = new Offre_MetaModel();
                        model.Offre.LoadInfosOffre(CommonOffreClient.LoadInfosBase(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig));
                        // Chargement des risques 
                        if(model.Offre != null) {
                            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                            {
                                var policeServicesClient = channelClient.Channel;
                                model.Offre.Risques = policeServicesClient.ObtenirRisques(model.ModeNavig.ParseCode<ModeConsultation>(), tId[0], Convert.ToInt32(tId[1]), tId[2], model.NumAvenantPage);
                            }
                        }
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    }
                    break;
                case "P":
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
                        // Chargement des risques 
                        if (model.Contrat != null)
                        {
                            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                            {
                                var policeServicesClient = channelClient.Channel;
                                model.Contrat.Risques = policeServicesClient.ObtenirRisques(model.ModeNavig.ParseCode<ModeConsultation>(), tId[0], Convert.ToInt32(tId[1]), tId[2], model.NumAvenantPage);
                            }
                        }
                    }
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
                        case AlbConstantesMetiers.TYPE_ATTESTATION:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_ATTES;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                            break;
                        default:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                            break;
                    }
                    break;
            }
            model.PageTitle = "Clausier";
            if (model.Offre != null || model.Contrat != null)
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
            if (model.Offre != null)
            {
                model.SelectionPossible = AlbConstantesMetiers.SELECTION_POSSIBLE_OUI;
                model.ModaliteAffichage = AlbConstantesMetiers.MODALITE_AFFICHAGE_DERNIERE_VERSION;
                var convertDateToInt = AlbConvert.ConvertDateToInt(DateTime.Now);
                if (convertDateToInt != null)
                    model.Date = convertDateToInt.Value;
                // ----------------------------------------- Données de la page
                model.CodeOffre = model.Offre.CodeOffre;
                model.Version = model.Offre.Version.ToString();
                model.Type = model.Offre.Type;
                model.Etape = tId[3];
                model.Branche = model.Offre.Branche.Code;
                // Récuperer les type d'inventaire
                model.TypeInventaire = "Concerts";
                SetList();
            }
            else if (model.Contrat != null)
            {
                model.SelectionPossible = AlbConstantesMetiers.SELECTION_POSSIBLE_OUI;
                model.ModaliteAffichage = AlbConstantesMetiers.MODALITE_AFFICHAGE_VERSION_ACTIVE;
                var convertDateToInt = AlbConvert.ConvertDateToInt(new DateTime(model.Contrat.DateEffetAnnee, model.Contrat.DateEffetMois, model.Contrat.DateEffetJour));
                if (convertDateToInt != null)
                    model.Date = convertDateToInt.Value;
                // ----------------------------------------- Données de la page
                model.CodeOffre = model.Contrat.CodeContrat;
                model.Version = model.Contrat.VersionContrat.ToString(CultureInfo.InvariantCulture);
                model.Type = model.Contrat.Type;
                model.Etape = tId[3];
                model.Branche = model.Contrat.Branche;
                // Récuperer les type d'inventaire
                model.TypeInventaire = "Concerts";
                SetList();
            }
            //Récuperer Code Formule et Code Risque
            GetFormuleRisqueOption();
        }
        private void SetBandeauNavigation(string id)
        {
            if (!model.AfficherBandeau) return;
            model.Bandeau = GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
            //Gestion des Etapes
            if (model.Offre != null)
            {
                model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                model.Navigation = new Navigation_MetaModel
                {
                    Etape = Navigation_MetaModel.ECRAN_INFOGENERALE,
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
                    case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                        model.Bandeau.StyleBandeau = model.ScreenType;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                        model.Bandeau.StyleBandeau = model.ScreenType;
                        break;
                    case AlbConstantesMetiers.TYPE_ATTESTATION:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_ATTES;
                        model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_ATTES;
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
                    Etape = model.ScreenType != AlbConstantesMetiers.SCREEN_TYPE_ATTES ? Navigation_MetaModel.ECRAN_INFOGENERALE : 0,
                    IdOffre = model.Contrat.CodeContrat,
                    Version = int.Parse(model.Contrat.VersionContrat.ToString(CultureInfo.InvariantCulture))
                };
                //model.NavigationArbre = GetNavigationArbreRegule(ContentData, "Regule");
                //model.NavigationArbre.IsRegule = typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGUL;
            }
        }
        private void SetArbreNavigation()
        {
            if (model.Offre != null)
            {
                //Affichage de la navigation latérale en arboresence
                model.NavigationArbre = GetNavigationArbre(string.Empty);
            }
            else if (model.Contrat != null)
            {
                if (model.ScreenType == AlbConstantesMetiers.SCREEN_TYPE_ATTES)
                {
                    model.NavigationArbre = GetNavigationArbreAffaireNouvelle("InfoSaisie", returnEmptyTree: true);
                }
                else
                {
                    model.NavigationArbre = GetNavigationArbreAffaireNouvelle(string.Empty);
                }
            }
            if (model.NavigationArbre != null)
                model.NavigationArbre.ListeClauses = true;
        }
        private void SetList()
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var screenClient=client.Channel;
                var result = screenClient.InitClausier();
                // Récuperer les mots clés et les rubriques/sousrubriques                
                model.MotCles1 = result.MotsCles1.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(CultureInfo.InvariantCulture), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                model.MotCles2 = result.MotsCles2.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(CultureInfo.InvariantCulture), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                model.MotCles3 = result.MotsCles3.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(CultureInfo.InvariantCulture), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                model.Rubriques = result.Rubriques.Select(m => new AlbSelectListItem() { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                model.ModeleDDLSousRubrique = new ModeleDDLSousRubrique();
                model.ModeleDDLSequence = new ModeleDDLSequence();
                var modes = new List<AlbSelectListItem>();
                var mode = new AlbSelectListItem { Value = "D", Text = "Dernière version", Selected = false, Title = "D-Dernière version" };
                modes.Add(mode);
                mode = new AlbSelectListItem { Value = "A", Text = "Active", Selected = false, Title = "A-Active" };
                modes.Add(mode);
                model.Modes = modes;
            }
        }
        private void GetFormuleRisqueOption()
        {
            var prov = model.Provenance.Replace('¤', '/').Replace('£', '_');
            var tab = prov.Split('_');
            if (tab.Length <= 3) return;

            if (model.Etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Risque))
            {
                var codeRisque = int.Parse(tab[3]);
                var descriptif = string.Empty;
                if (model.Offre != null && model.Offre.Risques != null)
                    descriptif = model.Offre.Risques.Find(r => r.Code == codeRisque).Descriptif;
                else if (model.Contrat != null && model.Contrat.Risques != null)
                    descriptif = model.Contrat.Risques.Find(r => r.Code == codeRisque).Descriptif;
                model.Risque = string.IsNullOrEmpty(descriptif) ? codeRisque.ToString(CultureInfo.InvariantCulture) : codeRisque + "-" + descriptif;
                model.CodeRsq = codeRisque.ToString(CultureInfo.InvariantCulture);
                //Récuperer les objets du risque
                var objets = new List<ModeleObjet>();
                if (model.Offre != null && model.Offre.Risques != null)
                    objets.AddRange(model.Offre.Risques.Find(r => r.Code == codeRisque).Objets.Select(obj => new ModeleObjet
                    {
                        Code = obj.Code.ToString(CultureInfo.InvariantCulture),
                        Designation = obj.Descriptif
                    }));
                else if (model.Contrat != null && model.Contrat.Risques!= null)
                    objets.AddRange(model.Contrat.Risques.Find(r => r.Code == codeRisque).Objets.Select(obj => new ModeleObjet
                    {
                        Code = obj.Code.ToString(CultureInfo.InvariantCulture),
                        Designation = obj.Descriptif
                    }));
                model.ObjetsRisqueAll = new ModeleObjetsRisque
                {
                    Objets = objets
                };
                if (model.ObjetsRisqueAll.Objets != null)
                    model.NbrObjets = model.ObjetsRisqueAll.Objets.Count;
                if (model.NbrObjets == 1)
                {
                    model.IsRsqSelected = false;
                    model.ObjetRisqueCode = objets != null ? objets[0].Code : string.Empty;
                }
                else model.IsRsqSelected = true;
            }
            else if (model.Etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Garantie) || model.Etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Option))
            {
                var codeFormule = int.Parse(tab[3]);
                string descriptifFormule;
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    var screenClient=client.Channel;
                    descriptifFormule = screenClient.GetLibFormule(codeFormule, model.CodeOffre, model.Version, model.Type, model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                }
                model.Formule = string.IsNullOrEmpty(descriptifFormule) ? codeFormule.ToString(CultureInfo.InvariantCulture) : codeFormule + "-" + descriptifFormule;
                model.CodeFormule = codeFormule.ToString(CultureInfo.InvariantCulture);
                model.CodeOption = tab[4];

            }
        }
        #endregion

    }
}
