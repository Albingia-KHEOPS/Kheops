using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.OP.OP_MVC.Models.ModeleInformationDatabase;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using EmitMapper;
using OP.WSAS400.DTO.Common;
using OPServiceContract.IAdministration;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class InformationsDataBaseController : ControllersBase<ModeleInformationsDataBasePage>
    {
        public ActionResult Index()
        {
            model.PageTitle = "Informations table";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            model.Colonnes = new List<ModeleColumnInfo>();
            DisplayBandeau();

            return View(model);
        }

        [HttpPost]
        public ActionResult RecherchInformationsTable(string env, string tableName)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var chan = client.Channel;

                var dtos = chan.GetTableDescription(env, tableName);

                var mapper = ObjectMapperManager.DefaultInstance.GetMapper<ColumnInfoDto, ModeleColumnInfo>();
                var modele = dtos.Select(i => mapper.Map(i));
                return PartialView("ListeColonnes", modele.ToList());
            }
        }
    }
}