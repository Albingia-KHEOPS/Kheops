using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using OPServiceContract;
using OPServiceContract.IAffaireNouvelle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class RemiseEnVigueurController<T> : ControllersBase<T> where T : MetaModelsBase
    {
        protected override bool GetIsReadOnly(string guid, string currentFolder, string numAvenant = "0", bool isPopup = false, string modeAvenant = "")
        {
            if (base.GetIsReadOnly(guid, currentFolder, numAvenant, isPopup, modeAvenant))
            {
                return true;
            }
            if ((int.TryParse(numAvenant, out var x) ? x : 0) > 0 && this.model?.Contrat != null)
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAvenant>())
                {
                    return client.Channel.IsReadonlyRemiseEnVigueur(new Framework.Common.Folder(Model.CodePolicePage, int.Parse(Model.VersionPolicePage), Model.TypePolicePage[0]));
                }
            }

            return false;
        }

        protected bool GetIsReadOnlyControllerBase(string guid, string currentFolder, string numAvenant = "0", bool isPopup = false, string modeAvenant = "") {
            return base.GetIsReadOnly(guid, currentFolder, numAvenant, isPopup, modeAvenant);
        }
    }
}
