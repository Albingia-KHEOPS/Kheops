using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamClause;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamFamilles;
using OPServiceContract.IAdministration;
using OPServiceContract.IBOParametrage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class ParamClauseController : ControllersBase<ModeleParamClausePage>
    {
        #region Méthodes Publiques
        
        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index()
        {
            model.PageTitle = "Paramétrages des clauses";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            LoadInfoPage();
            DisplayBandeau();
            return View(model);
        }

        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job)
        {
            return RedirectToAction(job, cible);
        }

        [ErrorHandler]
        public ActionResult RedirectionParam(string codeService)
        {
            ModeleParamClausePage model = new ModeleParamClausePage();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IParametrageClauses>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.InitParamClauses();
                if (result != null)
                {
                    model = ((ModeleParamClausePage)result);

                    List<AlbSelectListItem> services = result.Services.Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Libelle, Selected = false, Title = m.Libelle }).ToList();
                    if (!string.IsNullOrEmpty(codeService))
                    {
                        var sItem = services.FirstOrDefault(x => x.Value == codeService);
                        if (sItem != null)
                            sItem.Selected = true;
                    }
                    model.Services = services;
                }
            }

            return PartialView("ParamClauseActeGestion", model);
        }

        [ErrorHandler]
        public ActionResult LoadListActeGestion(string codeService)
        {
            List<ParamListParam> model = new List<ParamListParam>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IParametrageClauses>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.LoadListActeGestion(codeService).ToList();
                if (result != null)
                {
                    foreach (var acteGestion in result)
                    {
                        model.Add(((ParamListParam)acteGestion));
                    }
                }
            }
            return PartialView("ParamClauseListActeGestion", model);
        }

        [ErrorHandler]
        public ActionResult OpenParam(string etape, string codeService, string codeActGes, string codeEtape, string codeContexte)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IParametrageClauses>())
            {
                var serviceContext=client.Channel;
                switch (etape)
                {
                    case "ActeGestion":
                        ParamActeGestionEtape modelEtape = new ParamActeGestionEtape();
                        var resultEtape = serviceContext.LoadListEtapes(codeService, codeActGes);
                        if (resultEtape != null)
                        {
                            modelEtape = ((ParamActeGestionEtape)resultEtape);
                        }
                        return PartialView("ParamClauseListEtapes", modelEtape);
                    case "Etape":
                        ParamEtapeContexte modelContexte = new ParamEtapeContexte();
                        var resultContexte = serviceContext.LoadListContextes(codeService, codeActGes, codeEtape);
                        if (resultContexte != null)
                        {
                            modelContexte = ((ParamEtapeContexte)resultContexte);
                        }
                        return PartialView("ParamClauseListContextes", modelContexte);
                    case "Contexte":
                        ParamContexteEGDI modelEGDI = new ParamContexteEGDI();
                        var resultEGDI = serviceContext.LoadListEGDI(codeService, codeActGes, codeEtape, codeContexte);
                        if (resultEGDI != null)
                        {
                            modelEGDI = ((ParamContexteEGDI)resultEGDI);
                        }
                        return PartialView("ParamClauseListEGDI", modelEGDI);
                    default:
                        return null;
                }
            }
        }

        [ErrorHandler]
        public void DeleteParam(string etape, string codeParam)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IParametrageClauses>())
            {
                var serviceContext=client.Channel;
                serviceContext.DeleteParam(etape, codeParam);
            }
        }

        [ErrorHandler]
        public ActionResult AjoutParam(string etape, string codeService, string codeActGes, string codeEtape, string codeContexte, string codeEGDI)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IParametrageClauses>())
            {
                var serviceContext=client.Channel;
                switch (etape)
                {
                    case "ActeGestion":
                        ParamAjoutActeGestion modelActeGestion = new ParamAjoutActeGestion();
                        var resultActeGestion = serviceContext.LoadAjoutActeGestion(codeService);
                        if (resultActeGestion != null)
                        {
                            modelActeGestion = ((ParamAjoutActeGestion)resultActeGestion);
                            List<AlbSelectListItem> actesGestion = resultActeGestion.ActesGestion.Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Libelle, Selected = false, Title = m.Libelle }).ToList();
                            modelActeGestion.ActesGestion = actesGestion;
                        }
                        return PartialView("ParamClauseAjoutActeGestion", modelActeGestion);
                    case "Etape":
                        ParamAjoutEtape modelEtape = new ParamAjoutEtape();
                        var resultEtape = serviceContext.LoadAjoutEtape(codeService, codeActGes);
                        if (resultEtape != null)
                        {
                            modelEtape = ((ParamAjoutEtape)resultEtape);
                            List<AlbSelectListItem> etapes = resultEtape.Etapes.Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Libelle, Selected = false, Title = m.Libelle }).ToList();
                            modelEtape.Etapes = etapes;
                        }
                        return PartialView("ParamClauseAjoutEtape", modelEtape);
                    case "Contexte":
                        ParamAjoutContexte modelContext = new ParamAjoutContexte();
                        var resultContext = serviceContext.LoadAjoutContexte(codeService, codeActGes, codeEtape);
                        if (resultContext != null)
                        {
                            modelContext = ((ParamAjoutContexte)resultContext);
                            List<AlbSelectListItem> contextes = resultContext.Contextes.Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Libelle, Selected = false, Title = m.Libelle }).ToList();
                            modelContext.Contextes = contextes;
                            List<AlbSelectListItem> specifites = resultContext.Specificites.Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Libelle, Selected = false, Title = m.Libelle }).ToList();
                            modelContext.Specificites = specifites;
                            List<AlbSelectListItem> scripts = resultContext.ScriptsControle.Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Libelle, Selected = false, Title = m.Libelle }).ToList();
                            modelContext.ScriptsControle = scripts;
                            modelContext.Rubriques = resultContext.ModelesClause1.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(CultureInfo.InvariantCulture), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                            modelContext.ModeleDDLSequence = new Models.ModelesClauses.ModeleDDLSequence();
                            modelContext.ModeleDDLSousRubrique = new Models.ModelesClauses.ModeleDDLSousRubrique();
                            modelContext.Emplacements = resultContext.Emplacements.Select(m => new AlbSelectListItem() { Value = m.Code, Text = m.Code + " - " + m.Libelle, Selected = false, Title = m.Libelle }).ToList();
                            modelContext.SousEmplacements = resultContext.SousEmplacements.Select(m => new AlbSelectListItem() { Value = m.Code, Text = m.Code + " - " + m.Libelle, Selected = false, Title = m.Libelle }).ToList();
                            modelContext.NumOrdonnancements = new List<AlbSelectListItem>();
                        }
                        return PartialView("ParamClauseAjoutContexte", modelContext);
                    case "EGDI":
                        ParamAjoutEGDI modelEGDI = new ParamAjoutEGDI();
                        var resultEGDI = serviceContext.LoadAjoutEGDI(codeService, codeActGes, codeEtape, codeContexte, codeEGDI);
                        if (resultEGDI != null)
                        {
                            modelEGDI = ((ParamAjoutEGDI)resultEGDI);
                        }
                        return PartialView("ParamClauseAjoutEGDI", modelEGDI);
                    default:
                        return null;
                }
            }
        }

        [ErrorHandler]
        public void AddActeGestion(string codeService, string codeActeGestion)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IParametrageClauses>())
            {
                var serviceContext=client.Channel;
                serviceContext.AddActeGestion(codeService, codeActeGestion);
            }
        }

        [ErrorHandler]
        public void AddEtape(string codeService, string codeActeGestion, string codeEtape, int numOrdre)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IParametrageClauses>())
            {
                var serviceContext=client.Channel;
                serviceContext.AddEtape(codeService, codeActeGestion, codeEtape, numOrdre);
            }
        }

        [ErrorHandler]
        public void AddContexte(string idContexte, string codeService, string codeActeGestion, string codeEtape, string codeContexte, bool emplacementModif, bool ajoutClausier, bool ajoutLibre, string scriptControle,
            string rubrique, string sousRubrique, string sequence, string emplacement, string sousEmplacement, string numOrdonnancement, string specificite)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IParametrageClauses>())
            {
                var serviceContext=client.Channel;
                serviceContext.AddContexte(idContexte, codeService, codeActeGestion, codeEtape, codeContexte, emplacementModif, ajoutClausier, ajoutLibre, scriptControle, rubrique, sousRubrique, sequence, emplacement, sousEmplacement, numOrdonnancement, specificite);
            }
        }

        [ErrorHandler]
        public void AddEGDI(string codeService, string codeActeGestion, string codeEtape, string codeContexte, string type, string codeEGDI, int numOrdre, string libelleEGDI, string commentaire, string code)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IParametrageClauses>())
            {
                var serviceContext=client.Channel;
                serviceContext.AddEGDI(codeService, codeActeGestion, codeEtape, codeContexte, type, codeEGDI, numOrdre, libelleEGDI, commentaire, code);
            }
        }

        [ErrorHandler]
        public ActionResult OpenRattachClause(string codeService, string codeActGes, string codeEtape, string codeContexte, string codeEGDI)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IParametrageClauses>())
            {
                var serviceContext=client.Channel;
                ParamRattachClause model = new ParamRattachClause();
                var result = serviceContext.OpenRattachClause(codeService, codeActGes, codeEtape, codeContexte, codeEGDI);
                if (result != null)
                {
                    model = ((ParamRattachClause)result);
                }

                model.Affichages = new List<AlbSelectListItem>();
                AlbSelectListItem affichage = new AlbSelectListItem { Value = "", Text = "Toutes", Selected = false, Title = "Toutes" };
                model.Affichages.Insert(0, affichage);
                affichage = new AlbSelectListItem { Value = "R", Text = "Rattachées", Selected = false, Title = "Rattachées" };
                model.Affichages.Insert(1, affichage);
                affichage = new AlbSelectListItem { Value = "N", Text = "Non rattachées", Selected = false, Title = "Non rattachées" };
                model.Affichages.Insert(2, affichage);

                return PartialView("ParamClauseRattachement", model);
            }
        }

        [ErrorHandler]
        public ActionResult ReloadClauses(string codeEGDI, string restrict)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IParametrageClauses>())
            {
                var serviceContext=client.Channel;
                ParamRattachClause model = new ParamRattachClause();
                var result = serviceContext.ReloadClauses(codeEGDI, restrict);
                if (result != null)
                {
                    model = ((ParamRattachClause)result);
                }

                return PartialView("ParamClauseListClauses", model);
            }
        }

        [ErrorHandler]
        public ActionResult AttachClause(string codeService, string codeActGes, string codeEtape, string codeContexte, string codeEGDI, string type, string codeClause, string codeRattach, bool modifMode)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IParametrageClauses>())
            {
                var serviceContext=client.Channel;
                ParamRattachSaisie model = new ParamRattachSaisie();
                var result = serviceContext.AttachClause(codeService, codeActGes, codeEtape, codeContexte, codeEGDI, type, codeClause);
                if (result != null)
                {
                    model = ((ParamRattachSaisie)result);
                }

                model.ModifMode = modifMode;
                model.CodeRattachement = Convert.ToInt32(codeRattach);
                return PartialView("ParamClauseSaisieRattachement", model);
            }
        }

        [ErrorHandler]
        public int SaveRattachClause(int codeRattach, string codeClause, string codeEGDI, int numOrdre, string nom1, string nom2, string nom3, string nature, string impressAnnexe, string codeAnnexe, string attribut, string version)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IParametrageClauses>())
            {
                var serviceContext=client.Channel;
                int toReturn = 0;
                string user = GetUser();
                toReturn = serviceContext.SaveAttachClause(codeRattach, codeClause, codeEGDI, numOrdre, nom1, nom2, nom3, nature, impressAnnexe, codeAnnexe, HttpUtility.UrlDecode(attribut), version, user);
                return toReturn;
            }
        }

        [ErrorHandler]
        public void DeleteAttachClause(string codeEGDI, string codeClause)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IParametrageClauses>())
            {
                var serviceContext=client.Channel;
                serviceContext.DeleteAttachClause(codeEGDI, codeClause);
            }
        }

        [ErrorHandler]
        public string CheckConceptFamille(string codeConcept, string codeFamille)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.CheckConceptFamille(codeConcept, codeFamille);
                if (!string.IsNullOrEmpty(result))
                    return result;
            }
            return string.Empty;
        }

        [ErrorHandler]
        public ActionResult OpenConceptFamille(string codeConcept, string codeFamille)
        {
            var famille = new Famille();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var valeurs = new List<Valeur>();
                var result = serviceContext.GetFamille(codeConcept, codeFamille, string.Empty, string.Empty);
                if (result != null)
                {
                    famille = (Famille)result;
                    famille.CodeConcept = codeConcept;
                    //famille.LibelleConcept = libelleConcept;
                    famille.Longueurs = LoadDDLLongeurs();
                    var valeursResult = serviceContext.GetValeursByFamille(codeConcept, codeFamille, string.Empty, string.Empty);
                    if (valeursResult != null && valeursResult.Count > 0)
                        valeursResult.ForEach(m => valeurs.Add((Valeur)m));
                    foreach (var v in valeurs)
                    {
                        v.AdditionalParam = "**";
                        v.RestrictionParam = famille.Restriction;
                    }
                    famille.Valeurs = valeurs;
                    famille.nbvaleur = valeurs.Count;
                }
            }
            return PartialView("../ParamFamilles/EditValeurs", famille);
        }

        public List<AlbSelectListItem> LoadDDLLongeurs()
        {
            var longueurs = new List<AlbSelectListItem>();
            longueurs.Insert(0, new AlbSelectListItem { Value = "1", Text = "1", Selected = true });
            longueurs.Insert(1, new AlbSelectListItem { Value = "2", Text = "2", Selected = false });
            longueurs.Insert(2, new AlbSelectListItem { Value = "3", Text = "3", Selected = false });
            longueurs.Insert(3, new AlbSelectListItem { Value = "4", Text = "4", Selected = false });
            longueurs.Insert(4, new AlbSelectListItem { Value = "5", Text = "5", Selected = false });
            longueurs.Insert(5, new AlbSelectListItem { Value = "6", Text = "6", Selected = false });
            longueurs.Insert(6, new AlbSelectListItem { Value = "7", Text = "7", Selected = false });
            longueurs.Insert(7, new AlbSelectListItem { Value = "8", Text = "8", Selected = false });
            longueurs.Insert(8, new AlbSelectListItem { Value = "9", Text = "9", Selected = false });
            longueurs.Insert(9, new AlbSelectListItem { Value = "10", Text = "10", Selected = false });
            longueurs.Insert(10, new AlbSelectListItem { Value = "11", Text = "11", Selected = false });
            longueurs.Insert(11, new AlbSelectListItem { Value = "12", Text = "12", Selected = false });
            longueurs.Insert(12, new AlbSelectListItem { Value = "13", Text = "13", Selected = false });
            longueurs.Insert(13, new AlbSelectListItem { Value = "14", Text = "14", Selected = false });
            longueurs.Insert(14, new AlbSelectListItem { Value = "15", Text = "15", Selected = false });
            return longueurs;
        }

        [ErrorHandler]
        public ActionResult EditContexte(string codeService, string codeActeGestion, string codeEtape, string codeContexte)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IParametrageClauses>())
            {
                var serviceContext=client.Channel;
                ParamAjoutContexte model = new ParamAjoutContexte();
                var result = serviceContext.LoadContexte(codeService, codeActeGestion, codeEtape, codeContexte);
                if (result != null)
                {
                    model = ((ParamAjoutContexte)result);
                    List<AlbSelectListItem> scripts = result.ScriptsControle.Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Libelle, Selected = false, Title = m.Libelle }).ToList();
                    model.ScriptsControle = scripts;
                    List<AlbSelectListItem> specifites = result.Specificites.Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Libelle, Selected = false, Title = m.Libelle }).ToList();
                    model.Specificites = specifites;
                    model.Rubrique = result.ModeleClause1;
                    model.Rubriques = result.ModelesClause1.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(CultureInfo.InvariantCulture), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    model.ModeleDDLSequence = new Models.ModelesClauses.ModeleDDLSequence
                    {
                        Sequence = result.ModeleClause3,
                        Sequences = result.ModelesClause3.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(CultureInfo.InvariantCulture), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList()
                    };
                    model.ModeleDDLSousRubrique = new Models.ModelesClauses.ModeleDDLSousRubrique
                    {
                        SousRubrique = result.ModeleClause2,
                        SousRubriques = result.ModelesClause2.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(CultureInfo.InvariantCulture), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList(),
                        ModeleDDLSequence = model.ModeleDDLSequence
                    };
                    model.Emplacements = result.Emplacements.Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Libelle, Selected = false, Title = m.Libelle }).ToList();
                    model.SousEmplacements = result.SousEmplacements.Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Libelle, Selected = false, Title = m.Libelle }).ToList();
                    model.NumOrdonnancements = new List<AlbSelectListItem>();
                }

                return PartialView("ParamClauseAjoutContexte", model);
            }
        }

        #endregion

        #region Méthodes Privées

        protected override void LoadInfoPage(string context = null)
        {
            ModeleParamClausePage model = new ModeleParamClausePage();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IParametrageClauses>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.InitParamClauses();
                if (result != null)
                {
                    model = ((ModeleParamClausePage)result);
                    LoadContentData(model);

                    List<AlbSelectListItem> services = result.Services.Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Libelle, Selected = false, Title = m.Libelle }).ToList();
                    if (!string.IsNullOrEmpty(base.model.Service))
                    {
                        var sItem = services.FirstOrDefault(x => x.Value == base.model.Service);
                        if (sItem != null)
                            sItem.Selected = true;
                    }
                    base.model.Services = services;
                }
            }
        }

        private void LoadContentData(ModeleParamClausePage model)
        {
            base.model.Etape = model.Etape;
            base.model.Service = model.Service;
        }

        #endregion
    }
}
