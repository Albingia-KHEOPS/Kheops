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
using OP.WSAS400.DTO.MatriceFormule;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class ClauseSimplifieeController : ControllersBase<ModeleClauseSimplifieePage>
    {
        #region Variables Membres
        private const string contexteOrigine = "Tous";
        //private const string toutesSaufObligatoires = "TSO";
        //private const string obligatoires = "O";
        //private const string toutes = "T";
        //private const string proposes = "P";
        //private const string suggerees = "S";
        //private const string ajoutees = "A";
        //private const string libres = "L";
        #endregion

        #region Méthodes Publiques
        
        [ErrorHandler]
        public ActionResult Index(string id)
        {
            id = InitializeParams(id);
            model.IsOffreSimplifiee = true;
            LoadInfoPage(id);
            return View(model);
        }

        [ErrorHandler]
        public ActionResult OpenClausesTab(string codeOffre, string version, string type, string codeAvn, string tabGuid, string etape, string codeRisque, string codeFormule, string codeOption, bool fullScreen, string modeNavig)
        {
            ModeleClauseSimplifieePage model = new ModeleClauseSimplifieePage();
            var lstClause = GetClausesTriees("Titre", codeOffre, version, type, codeAvn, etape, "", tabGuid, codeRisque, codeFormule, codeOption, string.Empty, modeNavig);
            model.Clauses = lstClause;
            model.Etape = etape;
            model.Etapes = GetListeEtapes();
            model.Contextes = GetContextes(lstClause);
            model.ModeleContexteCible = new ModeleContexteCible { ContextesCibles = GetContextesCible(etape, codeOffre, version, type, codeAvn, modeNavig) };
            model.Filtres = GetFiltres();
            model.FullScreen = fullScreen;
            return PartialView("InfoClauses", model);
        }

        [ErrorHandler]
        public ActionResult OpenClausier(string libelleContexte, string codeContexte, string libelleEtapeAjout, string codeEtapeAjout, int nbrRisques, int nbrObjetsRisque1, string codeRisqueObjet, string risqueObjet)
        {
            var model = SetListClausier();
            model.CodeContexte = codeContexte;
            model.LibelleContexte = libelleContexte;
            model.CodeEtapeAjout = codeEtapeAjout;
            model.LibelleEtapeAjout = libelleEtapeAjout;
            model.NbrRisques = nbrRisques;
            model.NbrObjetsRisque1 = nbrObjetsRisque1;
            model.CodeRisqueObjet = codeRisqueObjet;
            model.RisqueObjet = risqueObjet;
            return PartialView("Clausier", model);
        }

        [ErrorHandler]
        public ActionResult Recherche(string libelle, string motcle1, string motcle2, string motcle3, string sequence, string rubrique, string sousrubrique, string selectionPossible, string modaliteAffichage, int date)
        {

            var resClausier = new List<ModeleClausier>();
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var screenClient=channelClient.Channel;
                var result = screenClient.SearchClause(libelle, motcle1, motcle2, motcle3, sequence, rubrique, sousrubrique, modaliteAffichage, date);

                if (result != null)
                {
                    result.ToList().ForEach(m => resClausier.Add((ModeleClausier)m));
                }
            }
            var modeleListeClauses = new ModeleListeClauses
            {
                Clauses = resClausier,
                SelectionPossible = selectionPossible,
                ModaliteAffichage = modaliteAffichage,
                Date = date
            };
            if (modeleListeClauses.Clauses != null)
            {
                modeleListeClauses.Clauses.ForEach(c => c.SelectionPossible = selectionPossible);
            }
            return PartialView("../Clausier/ListeClauses", modeleListeClauses);
        }

        [ErrorHandler]
        public ActionResult GetSousRubriques(string codeRubrique)
        {
            var model = new ModeleDDLSousRubrique();
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var screenClient=channelClient.Channel;
                var result = screenClient.GetListSousRubriques(codeRubrique);
                if (result != null)
                {
                    model.SousRubrique = string.Empty;
                    model.SousRubriques = result.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                }
                return PartialView("../Clausier/ListSousRubriques", model);
            }
        }

        [ErrorHandler]
        public ActionResult GetSequences(string codeRubrique, string codeSousRubrique)
        {
            var model = new ModeleDDLSequence();
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var screenClient=channelClient.Channel;
                var result = screenClient.GetListSequences(codeRubrique, codeSousRubrique);
                if (result != null)
                {
                    model.Sequence = string.Empty;
                    model.Sequences = result.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                }
                return PartialView("../Clausier/ListSequences", model);
            }
        }

        [ErrorHandler]
        public ActionResult EnregistrerClause(string type, string codeOffre, string version, string codeAvn, string tabGuid, string natureClause, string codeClause, string rubrique, string sousRubrique, string sequence,
            string versionClause, string actionEnchaine, string contexte, string etape, string codeRsq, string codeFor, string codeOption, string codeObj, bool fullScreen, string modeNavig)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext=channelClient.Channel;
                string retourMsg = serviceContext.SaveClause(codeOffre, version, type, etape, etape, codeRsq, codeObj, codeFor, codeOption, contexte, codeClause, rubrique, sousRubrique, sequence, versionClause, codeAvn);
                if (!string.IsNullOrEmpty(retourMsg)) throw new AlbFoncException(retourMsg);
                ModeleClauseSimplifieePage model = GetModeleClause(type, codeOffre, version, codeAvn, tabGuid, etape, fullScreen, modeNavig);
                return PartialView("InfoClauses", model);
            }
        }

        private ModeleClauseSimplifieePage GetModeleClause(string type, string codeOffre, string version, string codeAvn, string tabGuid, string etape, bool fullScreen, string modeNavig)
        {
            ModeleClauseSimplifieePage model = new ModeleClauseSimplifieePage();
            var lstClause = GetClausesTriees("Titre", codeOffre, version, type, codeAvn, etape, "", tabGuid, string.Empty, string.Empty, string.Empty, string.Empty, modeNavig);
            model.Clauses = lstClause;
            model.Etape = etape;
            model.Etapes = GetListeEtapes();
            model.Contextes = GetContextes(lstClause);
            //model.ContextesCibles = GetContextesCible(etape, codeOffre, version, type, modeNavig);
            model.ModeleContexteCible = new ModeleContexteCible { ContextesCibles = GetContextesCible(etape, codeOffre, version, type, codeAvn, modeNavig) };
            model.Filtres = GetFiltres();
            model.FullScreen = fullScreen;
            return model;
        }

        [ErrorHandler]
        public ActionResult Supprime(string id, string codeOffre, string version, string type, string codeAvn, string etape, string tabGuid, string codeRisque, string codeFormule, string codeOption, bool fullScreen, string modeNavig)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext=channelClient.Channel;
                serviceContext.SupprimeClauseUnique(id);
                ModeleClauseSimplifieePage model = GetModeleClause(type, codeOffre, version, codeAvn, tabGuid, etape, fullScreen, modeNavig);
                return PartialView("InfoClauses", model);
            }
        }

        [ErrorHandler]
        public ActionResult Filtrer(string codeOffre, string version, string type, string codeAvn, string etape, string tabGuid, string codeRisque, string codeFormule, string codeOption, string filtre, bool fullScreen, string modeNavig)
        {
            ModeleClauseSimplifieePage model = new ModeleClauseSimplifieePage();
            model.Etapes = GetListeEtapes();
            model.Etape = etape;
            var lstClause = GetClausesTriees("Titre", codeOffre, version, type, codeAvn, etape, "", tabGuid, codeRisque, codeFormule, codeOption, filtre, modeNavig);
            model.Clauses = lstClause;
            model.Contextes = GetContextes(lstClause);
            //model.ContextesCibles = GetContextesCible(etape,codeOffre, version, type, modeNavig);
            model.ModeleContexteCible = new ModeleContexteCible { ContextesCibles = GetContextesCible(etape, codeOffre, version, type, codeAvn, modeNavig) };
            model.Filtres = GetFiltres();
            model.FullScreen = fullScreen;
            return PartialView("InfoClauses", model);
        }

        [ErrorHandler]
        public void UpdateCheckBox(string clauseId, bool isChecked)
        {
            // string etatTitre = isChecked ? "P" : "S";
            string situation = isChecked ? "V" : string.Empty;
            if (string.IsNullOrEmpty(clauseId))
                throw new AlbTechException(new Exception("Erreur lors de la mise à jour de la clause"));
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var client=channelClient.Channel;
                client.UpdateEtatTitre(clauseId, situation);
            }
        }

        #region Clause Libre
        [ErrorHandler]
        public ActionResult AfficherEcranClauseLibre(string codeOffre, string version, string type, string codeRisque, string provenance, string codeEtapeAjout, string contexte,
            int nbrRisques, string codeRisqueObjet, string risqueObjet)
        {
            //var toReturn = new ModeleClauseLibre { Contexte = contexte, IsRsqSelected = false };
            var toReturn = new ModeleClauseLibreSimp
            {
                CodeEtapeAjout = codeEtapeAjout,
                NbrRisques = nbrRisques,
                CodeRisqueObjet = codeRisqueObjet,
                RisqueObjet = risqueObjet
            };

            return PartialView("ClauseLibre", toReturn);

        }

        [ErrorHandler]
        public ActionResult EnregistrerClauseLibre(string codeOffreContrat, string versionOffreContrat, string typeOffreContrat, string codeAvn, string tabGuid, string provenance, string contexte, string etape, string libelle, string texte, string codeRisque, string codeFormule, string codeOption, string codeObj, string modeNavig, bool fullScreen)
        {
            //Sauvegarde uniquement si l'écran n'est pas en readonly
            if (!GetIsReadOnly(tabGuid, codeOffreContrat + "_" + versionOffreContrat + "_" + typeOffreContrat, codeAvn))
            {
                var texteClauseLibre = Server.UrlDecode(texte);
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    var client=channelClient.Channel;
                    var retourMsg = client.EnregistreClauseLibre(codeOffreContrat, versionOffreContrat, typeOffreContrat, contexte, etape, codeRisque, codeFormule, codeOption, codeObj, libelle, texteClauseLibre);
                    if (!string.IsNullOrEmpty(retourMsg)) throw new AlbFoncException(retourMsg);
                }
            }
            ModeleClauseSimplifieePage model = GetModeleClause(typeOffreContrat, codeOffreContrat, versionOffreContrat, codeAvn, tabGuid, etape, fullScreen, modeNavig);
            return PartialView("InfoClauses", model);
        }
        [ErrorHandler]
        public ActionResult UpdateTextClauseLibre(string clauseId, string codeOffreContrat, string versionOffreContrat, string typeOffreContrat, string codeAvn, string tabGuid, string provenance, string etape, string titre, string texte, string codeRisque, string codeFormule, string codeOption, string codeObj, string modeNavig, bool fullScreen)
        {
            //Sauvegarde uniquement si l'écran n'est pas en readonly
            if (!GetIsReadOnly(tabGuid, codeOffreContrat + "_" + versionOffreContrat + "_" + typeOffreContrat, codeAvn))
            {
                string texteClauseLibre = Server.UrlDecode(texte);
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    var client=channelClient.Channel;
                    client.UpdateTextClauseLibreOffreSimp(clauseId, titre, texteClauseLibre, codeRisque, codeObj, codeFormule, codeOption);
                }
            }
            ModeleClauseSimplifieePage model = GetModeleClause(typeOffreContrat, codeOffreContrat, versionOffreContrat, codeAvn, tabGuid, etape, fullScreen, modeNavig);
            return PartialView("InfoClauses", model);
        }

        #endregion
        
        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string branche, string message, string tabGuid, string modeNavig, string addParamType, string addParamValue)
        {
            return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + "_" + branche + "_" + message + GetSurroundedTabGuid(tabGuid) + BuildAddParamString(addParamType, addParamValue) + modeNavig });
        }

        [ErrorHandler]
        public ActionResult FullScreen(string codeOffre, string version, string type, string codeAvn, string etape, string tabGuid, string codeRisque, string codeFormule, string codeOption, string modeNavig, bool fullScreen = false)
        {
            ModeleClauseSimplifieePage model = new ModeleClauseSimplifieePage();
            var lstClause = GetClausesTriees("Titre", codeOffre, version, type, codeAvn, etape, "", tabGuid, codeRisque, codeFormule, codeOption, string.Empty, modeNavig);
            model.Clauses = lstClause;
            model.Etape = etape;
            model.Etapes = GetListeEtapes();
            model.Contextes = GetContextes(lstClause);
            model.ModeleContexteCible = new ModeleContexteCible { ContextesCibles = GetContextesCible(etape, codeOffre, version, type, codeAvn, modeNavig) };
            model.Filtres = GetFiltres();

            model.FullScreen = fullScreen;
            return PartialView("InfoClauses", model);
        }
        
        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult CreateSimpleFolder(string codeOffre, string version, string type, string tabGuid, string branche, string cible, string addParamType, string addParamValue)
        {
            return RedirectToAction("Index", "MatriceRisque", new { id = codeOffre + "_" + version + "_" + type + GetSurroundedTabGuid(tabGuid) + BuildAddParamString(addParamType, addParamValue) });
        }
        [ErrorHandler]
        public ActionResult GetDDLContextesCible(string etape, string codeOffre, string version, string type, string codeAvn, string modeNavig)
        {
            var modeleContexteCible = new ModeleContexteCible { ContextesCibles = GetContextesCible(etape, codeOffre, version, type, codeAvn, modeNavig) };
            return PartialView("ListContextesCibles", modeleContexteCible);
        }

        [ErrorHandler]
        public ActionResult TrierClauses(string modeAffichage, string colTri, string codeOffre, string version, string type, string codeAvn, string etape, string provenance, string tabGuid, string codeRisque, string codeFormule, string codeOption, string filtre, string modeNavig, string imgTri)
        {
            return PartialView("ListeClauses", GetClausesTriees(colTri, codeOffre, version, type, codeAvn, etape, provenance, tabGuid, codeRisque, codeFormule, codeOption, filtre, modeNavig, imgTri));
        }
        #endregion

        #region Méthodes Privées

        protected override void LoadInfoPage(string id)
        {
            string[] tId = id.Split('_');
            switch (tId[2])
            {
                case "O":
                    using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                    {
                        var policeServicesClient=channelClient.Channel;
                        model.Offre = new Offre_MetaModel();
                        model.Offre.LoadOffre(policeServicesClient.OffreGetDto(tId[0], int.Parse(tId[1]), tId[2], model.ModeNavig.ParseCode<ModeConsultation>()));
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    }
                    break;
                case "P":
                    using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                    {
                        var serviceContext=channelClient.Channel;
                        model.Contrat = serviceContext.GetContrat(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                    }
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                    break;
            }
            model.PageTitle = "Clauses simplifiées";
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

            //var lstClause = GetClausesTriees("Titre", tId[0], tId[1], tId[2], AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.InfoGenerale), "", model.TabGuid, null, null, null, string.Empty, model.ModeNavig);
            var lstClause = GetClausesTriees("Titre", tId[0], tId[1], tId[2], model.NumAvenantPage, string.Empty, "", model.TabGuid, null, null, null, string.Empty, model.ModeNavig);
            model.Clauses = lstClause;
            //model.Etape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.InfoGenerale);
            model.Etapes = GetListeEtapes();
            model.Contextes = GetContextes(lstClause);
            // model.ContextesCibles = GetContextesCible(string.Empty, tId[0], tId[1], tId[2], model.ModeNavig);
            model.ModeleContexteCible = new ModeleContexteCible { ContextesCibles = GetContextesCible(string.Empty, tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig) };
            model.Filtres = GetFiltres();
            model.SelectionPossible = AlbConstantesMetiers.SELECTION_POSSIBLE_OUI;
            model.ModaliteAffichage = AlbConstantesMetiers.MODALITE_AFFICHAGE_DERNIERE_VERSION;
            var convertDateToInt = AlbConvert.ConvertDateToInt(DateTime.Now);
            if (convertDateToInt != null)
                model.Date = convertDateToInt.Value;
            //Récuperer Formules et Risques
            GetRisquesFormules();
        }

        private void SetArbreNavigation()
        {
            if (model.Offre != null)
            {
                //Affichage de la navigation latérale en arboresence
                model.NavigationArbre = GetNavigationArbre("AttentatGareat");
            }
            else if (model.Contrat != null)
            {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("AttentatGareat");
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
                        Etape = Navigation_MetaModel.ECRAN_INFOGENERALE,
                        IdOffre = model.Offre.CodeOffre,
                        Version = model.Offre.Version
                    };
                }
                else if (model.Contrat != null)
                {
                    model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                    model.Navigation = new Navigation_MetaModel
                    {
                        Etape = Navigation_MetaModel.ECRAN_INFOGENERALE,
                        IdOffre = model.Contrat.CodeContrat,
                        Version = int.Parse(model.Contrat.VersionContrat.ToString())
                    };
                }
            }
        }

        private List<ModeleClause> GetClausesTriees(string colTri, string codeOffre, string version, string type, string codeAvn, string etape, string provenance, string tabGuid, string codeRisque, string codeFormule, string codeOption, string filtre, string modeNavig, string imgTri = null)
        {
            var lstClause = GetClauses(type, codeOffre, version, codeAvn, etape, tabGuid, codeRisque, codeFormule, codeOption, filtre, modeNavig, colTri,imgTri);
            //if (lstClause.Any())
            //{
            //    //Mise à jour de l'origine
            //    lstClause.FindAll(elm => elm.Origine != "Utilisateur").ForEach(elm => elm.Origine = "Systeme");
            //    lstClause.FindAll(elm => elm.Origine == "Utilisateur" && elm.IsClauseLibre).ForEach(elm => elm.Origine = "Libre");
            //    lstClause.FindAll(elm => elm.Origine == "Utilisateur" && !elm.IsClauseLibre).ForEach(elm => elm.Origine = "Ajoutée");


            //    if (!string.IsNullOrEmpty(colTri))
            //    {
            //        switch (colTri)
            //        {
            //            case "Risque":
            //                lstClause = lstClause.OrderBy(elm => elm.CodeRisque)
            //                                     .ThenBy(elm => elm.CodeObjet)
            //                                     .ThenBy(elm => elm.CodeFormule)
            //                                     .ThenBy(elm => elm.Origine)
            //                                     .ThenBy(elm => elm.Rubrique)
            //                                     .ThenBy(elm => elm.SousRubrique)
            //                                     .ThenBy(elm => elm.Sequence)
            //                                     .ThenBy(elm => elm.Titre).ToList();
            //                break;
            //            case "Titre":
            //                if (imgTri == "tri_asc")
            //                {
            //                    lstClause = lstClause.OrderByDescending(elm => elm.Origine)
            //                                        .ThenByDescending(elm => elm.Rubrique)
            //                                        .ThenByDescending(elm => elm.SousRubrique)
            //                                        .ThenByDescending(elm => elm.Sequence)
            //                                        .ThenByDescending(elm => elm.Titre).ToList();
            //                }
            //                else
            //                {
            //                    lstClause = lstClause.OrderBy(elm => elm.Origine)
            //                                        .ThenBy(elm => elm.Rubrique)
            //                                        .ThenBy(elm => elm.SousRubrique)
            //                                        .ThenBy(elm => elm.Sequence)
            //                                        .ThenBy(elm => elm.Titre).ToList();
            //                }
            //                break;
            //            case "Contexte":
            //                lstClause = lstClause.OrderBy(elm => elm.Contexte)
            //                                     .ThenBy(elm => elm.Origine)
            //                                     .ThenBy(elm => elm.Rubrique)
            //                                     .ThenBy(elm => elm.SousRubrique)
            //                                     .ThenBy(elm => elm.Sequence)
            //                                     .ThenBy(elm => elm.Titre).ToList();
            //                break;
            //        }
            //    }
            //}
            return lstClause;
        }

        /// <summary>
        /// recupère tous les clauses, et charge la liste pour le filtre.
        /// </summary>
        /// <param name="typeOp"></param>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="filtreEtape"></param>
        /// <param name="filtreContext"></param>
        private List<ModeleClause> GetClauses(string typeOp, string codeOffre, string version, string codeAvn, string filtreEtape, string tabGuid, string codeRisque, string codeFormule, string codeOption, string filtre,string colTri,string imgTri, string modeNavig, string filtreContext = contexteOrigine)
        {

            int pageNumber = 1;
            int LineCount = MvcApplication.PAGINATION_SIZE; ;
            int StartLine = ((pageNumber - 1) * PageSize) + 1;
            int EndLine = pageNumber * PageSize;
            List<ModeleClause> toReturn = new List<ModeleClause>();
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var client=channelClient.Channel;
               // List<ClauseDto> wsClause = client.ClausesGet(typeOp, codeOffre, version, codeAvn, filtreEtape, filtreContext, string.Empty, string.Empty, string.Empty, string.Empty, codeRisque, codeFormule, codeOption, filtre,colTri, imgTri, modeNavig.ParseCode<ModeConsultation>(),StartLine,EndLine).ToList();
                //toReturn = ObjectMapperManager.DefaultInstance.GetMapper<List<ClauseDto>, List<ModeleClause>>().Map(wsClause);
                //bool isReadOnly = GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + typeOp, codeAvn);
                //toReturn.ForEach(m => m.IsReadOnlyMode = isReadOnly);
            }
            return toReturn;
        }

        private List<AlbSelectListItem> GetListeEtapes()
        {
            List<AlbSelectListItem> toReturn = new List<AlbSelectListItem>();
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext=channelClient.Channel;
                var result = serviceContext.GetListEtapes(OrigineAppel.OffreSimp);
                if (result.Any())
                {

                    toReturn = result.Where(m => m.Code != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.DeclencheurIncond) && m.Code != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Document) && m.Code != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Inventaire) && m.Code != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Objet) && m.Code != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Saisie))
                        .Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Libelle, Selected = false, Title = m.Libelle }).ToList();
                }
            }
            return toReturn;
        }

        /// <summary>
        /// récupère les contextes des clauses de l'écran
        /// </summary>
        private List<AlbSelectListItem> GetContextes(List<ModeleClause> tableauClause)
        {
            // Liste des contexte (filtre)
            char[] split = { ' ', '-', ' ' };
            List<AlbSelectListItem> toReturn = new List<AlbSelectListItem>();
            toReturn = tableauClause.Select(x => x.Contexte + " - " + x.ContexteLabel).Distinct().Select(x => new AlbSelectListItem { Text = x, Value = x.Split(split)[0], Selected = false, Title = x }).ToList();
            toReturn.Insert(0, new AlbSelectListItem { Text = "Tous", Value = "Tous", Selected = false, Title = "Tous" });
            return toReturn;
        }
        private List<AlbSelectListItem> GetContextesCible(string codeEtape, string codeOffre, string version, string type, string codeAvn, string modeNavig)
        {
            //Obtient la liste de l'ensemble des contextes
            var toReturn = new List<AlbSelectListItem>();
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext=channelClient.Channel;
                var listContextes = serviceContext.GetListContextes(codeEtape, codeOffre, version, type, codeAvn, "PRODU", "QECTX", modeNavig.ParseCode<ModeConsultation>());
                if (listContextes != null)
                    toReturn = listContextes.Select(p => new AlbSelectListItem
                    {
                        Text = string.Format("{0} - {1}", p.Code, p.Libelle),
                        Value = p.Code,
                        Selected = false,
                        Title = !string.IsNullOrEmpty(p.Code) ? string.Format("{0} - {1}", p.Code, p.Libelle) : ""
                    }).ToList();
            }
            return toReturn;
        }
        private static List<AlbSelectListItem> GetFiltres()
        {

            var filtres = new List<AlbSelectListItem>();
            filtres.Insert(0, new AlbSelectListItem { Value = AlbConstantesMetiers.ToutesSaufObligatoires, Text = "Toutes sauf obligatoires", Selected = true, Title = "Toutes sauf obligatoires" });
            filtres.Insert(1, new AlbSelectListItem { Value = AlbConstantesMetiers.Suggerees, Text = "Suggérées", Selected = false, Title = "Suggérées" });
            filtres.Insert(2, new AlbSelectListItem { Value = AlbConstantesMetiers.Ajoutees, Text = "Ajoutées", Selected = false, Title = "Ajoutées" });
            filtres.Insert(3, new AlbSelectListItem { Value = AlbConstantesMetiers.Obligatoires, Text = "Obligatoires", Selected = false, Title = "Obligatoires" });
            return filtres;
        }


        private Clausier SetListClausier()
        {
            Clausier model = new Clausier();
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var screenClient=channelClient.Channel;
                var result = screenClient.InitClausier();
                // Récuperer les mots clés et les rubriques/sousrubriques                
                model.MotCles1 = result.MotsCles1.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(CultureInfo.InvariantCulture), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                model.MotCles2 = result.MotsCles2.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(CultureInfo.InvariantCulture), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                model.MotCles3 = result.MotsCles3.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(CultureInfo.InvariantCulture), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                model.Rubriques = result.Rubriques.Select(m => new AlbSelectListItem() { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                model.ModeleDDLSousRubrique = new ModeleDDLSousRubrique();
                model.ModeleDDLSequence = new ModeleDDLSequence();
            }

            return model;
        }
        private void GetRisquesFormules()
        {
            if (model.Offre == null) return;
            model.Risques = new List<ModeleClauseSimpRisque>();

            foreach (var risque in model.Offre.Risques)
            {
                var modeleRisque = new ModeleClauseSimpRisque()
                {
                    Code = risque.Code.ToString(),
                    Designation = risque.Descriptif
                };
                var objets = new List<ModeleObjet>();
                objets.AddRange(risque.Objets.Select(obj => new ModeleObjet
                {
                    Code = obj.Code.ToString(CultureInfo.InvariantCulture),
                    Designation = obj.Descriptif
                }));
                var objetsRisque = new ModeleObjetsRisque
                {
                    CodeRisque = risque.Code.ToString(),
                    DescriptifRisque = risque.Descriptif,
                    Objets = objets
                };
                modeleRisque.Objets = objetsRisque;
                model.Risques.Add(modeleRisque);
            }

            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext=channelClient.Channel;
                ModeleMatriceFormulePage modele = new ModeleMatriceFormulePage();
                MatriceFormuleDto result = serviceContext.InitMatriceFormule(model.Offre.CodeOffre, model.Offre.Version.ToString(), model.Offre.Type, model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>(), GetUser(), model.ActeGestion, model.IsReadOnly);
                if (result != null)
                    modele = ((ModeleMatriceFormulePage)result);
                model.Formules = modele.Formules;
            }

            model.NbrRisques = model.Risques.Count;
            model.NbrObjetsRisque1 = model.NbrRisques > 0 ? model.Risques[0].Objets.Objets.Count : 0;
            if (model.NbrRisques == 1)
            {
                if (model.NbrObjetsRisque1 == 1)
                {
                    model.CodeRisqueObjet = model.Risques[0].Code + "_" + model.Risques[0].Objets.Objets[0].Code;
                    model.AppliqueA = model.Risques[0].Objets.Objets[0].Code + "-" + model.Risques[0].Objets.Objets[0].Designation;
                }
                else if (model.NbrObjetsRisque1 > 1)
                {
                    model.CodeRisqueObjet = model.Risques[0].Code;
                    model.AppliqueA = model.Risques[0].Code + "-" + model.Risques[0].Designation;
                }
            }

        }

        #endregion
    }
}
