using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Extensions;
using OPServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.AlbSpecificAttribute {
    public class CheckApplicationsRisquesAttribute : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (filterContext.ActionParameters.TryGetValue("id", out var id)
                && id is string code
                && Affaire.IpbRegex.IsMatch(code.Split('_').First())) {
                var array = code.Split('_');
                var affaireId = new AffaireId {
                    CodeAffaire = array[0],
                    NumeroAliment = int.Parse(array[1]),
                    TypeAffaire = array[2].Substring(0, 1).ParseCode<AffaireType>()
                };
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFormulePort>()) {
                    if (!client.Channel.AffaireHasFormules(affaireId)) {
                        throw new AlbFoncException("Impossible d'accéder aux engagements, un ou plusieurs risques/objets n'ont aucune formule associée.");
                    }
                }

            }
            base.OnActionExecuting(filterContext);
        }
    }
}