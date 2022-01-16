using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class LogTraceController : ControllersBase<ModeleLogTracePage> 
    {
        #region méthodes publiques
        [AlbApplyUserRole]
        public ActionResult Index()
        {
            model.PageTitle = "Trace des Logs";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            LoadInfoPage();
            DisplayBandeau();
            return View(model);
        }
        public ActionResult RechercheLogTraces(string codeType, string motCle, DateTime? dateDebut, DateTime? dateFin)
        {
            AlbLog.LogTraceLevel level = AlbLog.LogTraceLevel.Tous;
            Enum.TryParse(codeType, out level);
            var listTraces = AlbLog.GetLogTrace(dateDebut, dateFin, level, Server.UrlDecode(motCle));
            return PartialView("ListeLogTraces", listTraces);
        }
        public ActionResult Tri(string modeTri, string codeType, string motCle, DateTime? dateDebut, DateTime? dateFin)
        {
            AlbLog.LogTraceLevel level;
            Enum.TryParse(codeType, out level);
            var listTraces = AlbLog.GetLogTrace(dateDebut, dateFin, level, Server.UrlDecode(motCle));
            if (listTraces.Any())
            {
                if (modeTri == "desc")
                    listTraces = listTraces.OrderByDescending(t => t.DateLog).ToList();
                else if (modeTri == "asc") listTraces = listTraces.OrderBy(t => t.DateLog).ToList();
            }
            return PartialView("ListeLogTraces", listTraces);
        }

        #endregion
        #region méthodes privées
        protected override void LoadInfoPage(string context = null)
        {
            model.CodesType = GetListTypes();
            model.DateDebutFiltre = DateTime.Now;
            model.DateFinFiltre = DateTime.Now;
            GetListLogTraces();
        }

        private List<AlbSelectListItem> GetListTypes()
        {
            var toReturn = new List<AlbSelectListItem>();
            var itemNames = Enum.GetNames(typeof(AlbLog.LogTraceLevel));
            foreach (string itemName in itemNames)
            {
                AlbLog.LogTraceLevel level;
                Enum.TryParse(itemName, out level);
                var val = (int)level;
                var item = new AlbSelectListItem { Text = itemName, Value = val.ToString(), Title = itemName };
                toReturn.Add(item);
            }
            return toReturn;
        }
        private void GetListLogTraces()
        {
            var listTraces = AlbLog.GetLogTrace(DateTime.Now, DateTime.Now, AlbLog.LogTraceLevel.Tous, string.Empty);
            if (listTraces.Any())
                listTraces = listTraces.OrderByDescending(t => t.DateLog).ToList();
            model.LogTraces = listTraces;
        }
        #endregion
    }
}
