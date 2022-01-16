using Albingia.Kheops.Mvc.Models;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Mvc.Common;
using ALBINGIA.Framework.Common.Business;
using ALBINGIA.OP.OP_MVC.Controllers;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using System;
using System.Linq;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages {
    public class ModeleEngagementBase : MetaModelsBase {
        protected AccessMode currentAccessMode;
        public override bool IsReadOnly {
            get {
                var affaireId = new AffaireId {
                    CodeAffaire = CodePolicePage,
                    NumeroAliment = int.TryParse(VersionPolicePage, out int i) ? i : default
                };
                bool isReadonly = BaseController.GetState(Guid.Parse(TabGuid), affaireId) == ControllerState.Readonly;
                return isReadonly || CurrentAccessMode == Framework.Common.Business.AccessMode.CONSULT;
            }
            set { }
        }

        virtual public AccessMode? CurrentAccessMode {
            get {
                return IsEditEngagementOnly ? this.currentAccessMode : default(AccessMode?);
            }
            set {
                if (IsEditEngagementOnly) {
                    this.currentAccessMode = value.GetValueOrDefault();
                }
            }
        }

        public bool IsEditEngagementOnly {
            get {
                Guid guid = Guid.TryParse(TabGuid, out var g) ? g : default;
                var acces = MvcApplication.ListeAccesAffaires.FirstOrDefault(x =>
                    x.TabGuid == guid
                    && x.Code?.Trim() == CodePolicePage
                    && x.Version.ToString() == VersionPolicePage);

                return acces.VerrouillageEffectue && acces.ModeAcces == AccesOrigine.Engagements;
            }
        }
    }
}