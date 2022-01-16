using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.ModeleConditionsGarantie;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OPServiceContract.IClausesRisquesGaranties;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class RefExprCompController : ControllersBase<ModeleRefExprCompPage>
    {
        #region Méthodes publiques
        [ErrorHandler]
        public ActionResult Index()
        {
            model.PageTitle = "Référentiel des expressions complexes";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            LoadInfoPage();
            DisplayBandeau();
            return View(model);
        }

        [ErrorHandler]
        public ActionResult LoadExpComp(string typeExpr, string codeExpr)
        {
            var model = new ModeleConditionsExprComplexeDetails();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.GetInfoExpComplexe(typeExpr, codeExpr);
                if (result != null)
                {
                    model = (ModeleConditionsExprComplexeDetails)result;
                    model.Type = typeExpr;
                    model.UnitesLCINew = result.UnitesNew.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).Where(u => u.Value != "CPX").ToList();
                    model.UnitesFranchiseNew = result.UnitesNew.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).Where(u => u.Value != "CPX").ToList();
                    model.UnitesConcurrence = result.UnitesNew.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).Where(u => u.Value != "CPX").ToList();

                    model.TypesLCINew = result.TypesNew.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).ToList();
                    model.TypesFranchiseNew = result.TypesNew.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).ToList();
                    model.TypesConcurrence = result.TypesNew.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).ToList();

                    model.UniteLCINew = string.Empty;
                    model.TypeLCINew = string.Empty;
                    model.UniteConcurrence = string.Empty;
                    model.TypeConcurrence = string.Empty;
                    model.UniteFranchiseNew = string.Empty;
                    model.TypeFranchiseNew = string.Empty;

                    if (!string.IsNullOrEmpty(codeExpr) && codeExpr != "0")
                    {
                        model.IsReadOnly = true;
                    }
                }
            }

            return PartialView("DetaileExprComp", model);
        }

        [ErrorHandler]
        public string SaveDetailExpr(string idExpr, string typeExpr, string codeExpr, string libExpr, bool modifExpr, string descrExpr) {
            //Vérification du code
            if (string.IsNullOrEmpty(codeExpr) || codeExpr.Length != 3) {
                throw new AlbFoncException("Le code de l'expression doit contenir 3 caractères");
            }
            
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext=client.Channel;
                return serviceContext.SaveDetailExpr(idExpr, typeExpr, codeExpr, libExpr, modifExpr, descrExpr).ToString();
            }
        }

        [ErrorHandler]
        public ActionResult LoadListExprComp(string typeExpr)
        {
            List<AlbSelectListItem> model = new List<AlbSelectListItem>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.LoadListExprComplexe(typeExpr);
                if (result != null && result.Any())
                {
                    foreach (var expr in result)
                    {
                        model.Add(new AlbSelectListItem { Value = expr.LongId.ToString(), Text = expr.Code, Descriptif = expr.Descriptif });
                    }
                }
            }
            return typeExpr == "LCI" ? PartialView("ListLCI", model) : typeExpr == "Franchise" ? PartialView("ListFRH", model) : null;
        }

        [ErrorHandler]
        public void DeleteExprComp(string idExpr, string typeExpr)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext=client.Channel;
                serviceContext.DeleteExprComp(idExpr, typeExpr);
            }
        }

        [ErrorHandler]
        public void SaveRowExprComplexe(string idExpr, string typeExpComp, string idRowExpr,
            string valExpr, string unitExpr, string typeExpr, string concuValExpr, string concuUnitExpr, string concuTypeExpr,
            string valMinFrh, string valMaxFrh, string debFrh, string finFrh)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext=client.Channel;
                serviceContext.SaveRowExprComplexe(idExpr, typeExpComp, idRowExpr,
             valExpr, unitExpr, typeExpr, concuValExpr, concuUnitExpr, concuTypeExpr,
             valMinFrh, valMaxFrh, debFrh, finFrh);
            }
        }

        [ErrorHandler]
        public void DelRowExprComplexe(string idExpr, string typeExpComp, string idRowExpr)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext=client.Channel;
                serviceContext.DelRowExprComplexe(idExpr, typeExpComp, idRowExpr);
            }
        }

        [ErrorHandler]
        public ActionResult LoadRowsExprComplexe(string typeExpComp, string idExpr)
        {
            ModeleConditionsExprComplexeDetails model = new ModeleConditionsExprComplexeDetails();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.LoadRowsExprComplexe(typeExpComp, idExpr);
                if (result != null)
                {
                    model = (ModeleConditionsExprComplexeDetails)result;
                    model.Type = typeExpComp;
                    model.UnitesLCINew = result.UnitesNew.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).Where(u => u.Value != "CPX").ToList();
                    model.UnitesFranchiseNew = result.UnitesNew.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).Where(u => u.Value != "CPX").ToList();
                    model.UnitesConcurrence = result.UnitesNew.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).Where(u => u.Value != "CPX").ToList();

                    model.TypesLCINew = result.TypesNew.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).ToList();
                    model.TypesFranchiseNew = result.TypesNew.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).ToList();
                    model.TypesConcurrence = result.TypesNew.Select(u => new AlbSelectListItem() { Text = string.Format("{0} - {1}", u.Code, u.Libelle), Value = u.Code, Selected = false, Title = string.Format("{0} - {1}", u.Code, u.Libelle) }).ToList();

                    model.UniteLCINew = string.Empty;
                    model.TypeLCINew = string.Empty;
                    model.UniteConcurrence = string.Empty;
                    model.TypeConcurrence = string.Empty;
                    model.UniteFranchiseNew = string.Empty;
                    model.TypeFranchiseNew = string.Empty;
                }
            }

            return PartialView("RowsExprComp", model);
        }

        [ErrorHandler]
        public ActionResult SearchExprComp(string typeExpr, string strSearch)
        {
            List<AlbSelectListItem> model = new List<AlbSelectListItem>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.SearchExprComp(typeExpr, strSearch);
                if (result != null && result.Any())
                {
                    foreach (var expr in result)
                    {
                        model.Add(new AlbSelectListItem { Value = expr.LongId.ToString(), Text = expr.Code, Descriptif = expr.Descriptif });
                    }
                }
            }
            return typeExpr == "LCI" ? PartialView("ListLCI", model) : typeExpr == "Franchise" ? PartialView("ListFRH", model) : null;
        }

        #endregion

        #region Méthodes privées

        protected override void LoadInfoPage(string context = null)
        {
            List<AlbSelectListItem> lcis = new List<AlbSelectListItem>();
            List<AlbSelectListItem> frhs = new List<AlbSelectListItem>();

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.LoadListesExprComplexe();
                if (result != null)
                {
                    if (result.ListLCI != null && result.ListLCI.Any())
                    {
                        foreach (var lci in result.ListLCI)
                        {
                            lcis.Add(new AlbSelectListItem { Value = lci.LongId.ToString(), Text = lci.Code, Descriptif = lci.Descriptif });
                        }
                    }
                    if (result.ListFranchise != null && result.ListFranchise.Any())
                    {
                        foreach (var frh in result.ListFranchise)
                        {
                            frhs.Add(new AlbSelectListItem { Value = frh.LongId.ToString(), Text = frh.Code, Descriptif = frh.Descriptif });
                        }
                    }
                }
            }

            model.ListeLCI = lcis;
            model.ListeFranchise = frhs;
        }

        #endregion
    }
}
