using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.ServiceFactory;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.CustomResult;
using ALBINGIA.OP.OP_MVC.Models;
using Mapster;
using OPServiceContract;
using OPServiceContract.ICommon;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers {
    public class SinistreController : BaseController {
        [HttpGet]
        [ErrorHandler]
        public JsonResult GetSinistres(int page = 0, int codeAssure = 0) {
            using (var client = ServiceClientFactory.GetClient<ICommonOffre>()) {
                var pagingListDto = client.Channel.GetSinistres(page, codeAssure);
                var pagingList = new PagingList<Sinistre> {
                    NbTotalLines = pagingListDto.NbTotalLines,
                    PageNumber = pagingListDto.PageNumber,
                    List = pagingListDto.List.Adapt<List<Sinistre>>(),
                    Totals = pagingListDto.Totals
                };
                return JsonNetResult.NewResultToGet(pagingList);
            }
        }

        [HttpGet]
        [ErrorHandler]
        public decimal GetTotalChargementSinistresPreneur(int codePreneur) {
            using (var client = ServiceClientFactory.GetClient<ISinistres>()) {
                return client.Channel.TotalByPreneurCalculProvisionsPrevisionsChargement(codePreneur);
            }
        }
    }
}