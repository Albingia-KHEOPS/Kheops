using Albingia.Kheops.Common;
using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.ServiceFactory;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.CustomResult;
using ALBINGIA.OP.OP_MVC.Models;
using Mapster;
using OPServiceContract;
using OPServiceContract.ICommon;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers {
    public class OffreController : BaseController {
        [HttpGet]
        [ErrorHandler]
        public JsonResult GetRelances(int page = 0) {
            using (var client = ServiceClientFactory.GetClient<ICommonOffre>()) {
                var pagingListDto = client.Channel.GetUserRelances(page);
                var pagingList = new PagingList<RelanceOffre> {
                    NbTotalLines = pagingListDto.NbTotalLines,
                    PageNumber = pagingListDto.PageNumber,
                    List = pagingListDto.List.Adapt<List<RelanceOffre>>()
                };
                return JsonNetResult.NewResultToGet(pagingList);
            }
        }

        [HttpPost]
        [HandleJsonError]
        public void UpdateRelances(ModelRelances modelRelances) {
            bool mustUnlock = true;
            var affaireIds = modelRelances.Relances.Select(x => x.Adapt<AffaireId>());

            using (var clientOffre = ServiceClientFactory.GetClient<ICommonOffre>())
            using (var client = ServiceClientFactory.GetClient<IAffairePort>()) {
                try {
                    if (client.Channel.TryLockAffaireList(affaireIds, "Gestion", "Offre sans-suite")?.Any() ?? false) {
                        mustUnlock = false;
                        throw new BusinessValidationException(new ValidationError("Au moins une offre est verrouillée. Veuillez ressayer ultérieurement"));
                    }
                    clientOffre.Channel.UpdateRelances(modelRelances.Relances.Select(x => x.Adapt<RelanceDto>()).ToList());
                }
                finally {
                    if (mustUnlock) {
                        try {
                            client.Channel.UnockAffaireList(affaireIds);
                        }
                        catch {
                            AlbLog.Warn($"Unable to unlock {string.Join(", ", affaireIds.Select(x => $"{x.CodeAffaire} ({x.NumeroAliment})"))}");
                        }
                    }
                }
            }
        }
    }
}