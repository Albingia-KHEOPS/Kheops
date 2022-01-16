using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using System;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers {
    public class ScriptController : ControllersBase<ModeleBackOffice>
    {
        public ActionResult Index()
        {
            model.PageTitle = "Administration";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            DisplayBandeau();
            return View(model);
        }
        public ActionResult Regenerate()
        {
            throw new NotImplementedException();
        }
    }
}