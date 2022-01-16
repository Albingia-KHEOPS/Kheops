using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.IOFile;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OPServiceContract.IAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class LogPerfController : ControllersBase<ModeleLogPerfPage>
    {
        [AlbApplyUserRole]
        public ActionResult LogPerf(DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            model.PageTitle = "Performances";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            LoadInfoPagePerf(dateDebut, dateFin);
            DisplayBandeau();
            return View(model);
        }

        public ActionResult RechercheLogPerf(DateTime? dateDebut, DateTime? dateFin)
        {
            model.PageTitle = "Performances";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            LoadInfoPagePerf(dateDebut, dateFin);
            DisplayBandeau();
            return PartialView("ListeLogPerf", model.LogPerfs);
        }

        public ExportToCSVResult<LogPerfExport> ExportFile(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
            var paramList = id.Split('_');
            if (paramList.Count() != 2)
                return null;
            var dateDebut = paramList[0];
            var dateFin = paramList[1];


            var lstPerfs = new List<LogPerfExport>();
            var fileName = "LogPerfs_" + GetUser() + '_' + DateTime.Now.ToString();

            LoadInfoPagePerf(AlbConvert.ConvertStrToDate(dateDebut), AlbConvert.ConvertStrToDate(dateFin));

            if (model.LogPerfs != null)
            {
                foreach (var item in model.LogPerfs)
                {
                    lstPerfs.Add(new LogPerfExport
                    {
                        DateLog = item.DateLog.ToString(),
                        User = item.User,
                        Action = item.Action,
                        Screen = item.Screen,
                        ElapsedTime = item.ElapsedTime
                    });
                }
            }

            const string columns = "DateLog;Utilisateur;Ecran;Action;Temps";
            var ret = new ExportToCSVResult<LogPerfExport>(lstPerfs, fileName, columns);
            return ret;
        }

        private void LoadInfoPagePerf(DateTime? dateDebut, DateTime? dateFin)
        {
            if (dateDebut == null || dateFin == null)
            {
                model.DateDebutFiltre = DateTime.Now;
                model.DateFinFiltre = DateTime.Now;
            }
            else
            {
                model.DateDebutFiltre = dateDebut;
                model.DateFinFiltre = dateFin;
            }
            GetListLogPerfs(model.DateDebutFiltre, model.DateFinFiltre);
        }

        private void GetListLogPerfs(DateTime? dateDebut, DateTime? dateFin)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var screenClient=client.Channel;
                var listTraces = screenClient.GetLogPerfs(dateDebut, dateFin);

                foreach (var item in listTraces)
                {
                    var paramList = item.DateLog.Split('/');
                    item.DateLog = paramList[1] + '/' + paramList[0] + '/' + paramList[2];
                }

                model.LogPerfs = ModeleLogPerfPage.LoadListPerf(listTraces);
            }
        }
    }
}
