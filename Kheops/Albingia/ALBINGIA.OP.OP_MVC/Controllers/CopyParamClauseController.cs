using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OPServiceContract.IBOParametrage;
using System.Collections.Generic;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class CopyParamClauseController : ControllersBase<ModeleCopyParamClausePage>
    {
        private List<AlbSelectListItem> Environnements
        {
            get
            {
                List<AlbSelectListItem> value = new List<AlbSelectListItem>();
                if (AlbOpConstants.ClientWorkEnvironment == AlbOpConstants.OPENV_DEV || AlbOpConstants.ClientWorkEnvironment == AlbOpConstants.OPENV_PREPROD)
                    value.Add(new AlbSelectListItem() { Value = "ZALBINKHEO", Text = "DEV" });
                if (AlbOpConstants.ClientWorkEnvironment == AlbOpConstants.OPENV_QUALIF || AlbOpConstants.ClientWorkEnvironment == AlbOpConstants.OPENV_PREPROD)
                    value.Add(new AlbSelectListItem() { Value = "ZALBINKQUA", Text = "Qualif" });
                if (AlbOpConstants.ClientWorkEnvironment == AlbOpConstants.OPENV_FORMATION || AlbOpConstants.ClientWorkEnvironment == AlbOpConstants.OPENV_PREPROD)
                    value.Add(new AlbSelectListItem() { Value = "ZALBINKFRM", Text = "Formation" });
                value.Add(new AlbSelectListItem() { Value = "YKPREDTA", Text = "PreProd" });
                if (AlbOpConstants.ClientWorkEnvironment == AlbOpConstants.OPENV_PROD || AlbOpConstants.ClientWorkEnvironment == AlbOpConstants.OPENV_PREPROD)
                    value.Add(new AlbSelectListItem() { Value = "YALBINFILE", Text = "Production" });
                return value;
            }
        }

        #region Méthodes Publiques

        public ActionResult Index()
        {
            model.PageTitle = "Livraison de paramétrage des clauses";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            LoadInfoPage();
            DisplayBandeau();
            return View(model);
        }

        public void CopyParam(string src, string dest)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IParametrageClauses>())
            {
                var serviceContext=client.Channel;
                serviceContext.CopyParamClause(src, dest);
            }
        }

        #endregion

        #region Méthodes Privées

        protected override void LoadInfoPage(string context = null)
        {
            model.Sources = Environnements;
            model.Sources.Find(e => e.Text == AlbOpConstants.OPENV_PREPROD).Selected = true;
            model.Destinations = Environnements;
            model.Destinations.Find(e => e.Text == AlbOpConstants.ClientWorkEnvironment).Selected = true;
        }

        #endregion
    }
}
