using System;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.AlbSpecificAttribute
{
    /// <summary>
    /// Cet attribut peut 'applique à uneclasse ou une méthode.
    /// Cette attribut sert a indiquer que la classe ou la méthode peut traiter une redirection cas d'un 
    /// appel Ajax
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AlbApplyUserRole : ActionFilterAttribute
    {

        /// <summary>
        /// Traite la redirection Ajax aprés que l'action soit executé
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {


            base.OnActionExecuting(filterContext);
         
               
        }
    }


}
