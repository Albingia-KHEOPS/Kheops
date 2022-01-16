
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.ServiceFactory;
using ALBINGIA.OP.OP_MVC.Models.Connexites;
using Mapster;
using OP.WSAS400.DTO;
using OPServiceContract.IAffaireNouvelle;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers {
    public class FusionConnexitesController : BaseController {
        [HttpPost]
        [HandleJsonError]
        public void MergeConnexites(FusionConnexites model) {
            using (var client = ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                client.Channel.MergeConnexites(model.Adapt<FusionConnexitesDto>());
            }
        }

        [HttpPost]
        [HandleJsonError]
        public void PickTarget(FusionConnexites model) {
            using (var client = ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                client.Channel.PickTargetToConnexites(model.Adapt<FusionConnexitesDto>());
            }
        }

        [HttpPost]
        [HandleJsonError]
        public void MoveSource(FusionConnexites model) {
            using (var client = ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                client.Channel.MoveSourceInConnexites(model.Adapt<FusionConnexitesDto>());
            }
        }
    }
}
