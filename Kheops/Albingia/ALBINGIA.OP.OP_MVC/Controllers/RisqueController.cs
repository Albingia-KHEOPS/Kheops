using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using OPServiceContract;
using OPServiceContract.IAffaireNouvelle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Controllers {
    public class RisqueController<T> : ControllersBase<T> where T : MetaModelsBase {

        protected override bool GetIsReadOnly(string guid, string currentFolder, string numAvenant = "0", bool isPopup = false, string modeAvenant = "") {
            bool @readonly = base.GetIsReadOnly(guid, currentFolder, numAvenant, isPopup, modeAvenant);
            if ((int.TryParse(numAvenant, out var x) ? x : 0) > 0 && this.model?.Contrat != null) {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAvenant>()) {
                    if (client.Channel.IsReadonlyRemiseEnVigueur(this.model.AllParameters.Folder)) {
                        if (!@readonly) {
                            @readonly = true;
                        }
                        else {
                            this.model.IsModeConsultationEcran = true;
                        }
                    }
                }
            }
            return @readonly;
        }
    }
}
