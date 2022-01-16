using System;
using System.Text;
using System.Web.Mvc;
using ALBINGIA.Framework.Common.Models.Common;


namespace ALBINGIA.Framework.Common.AlbingiaAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AlbRefreshPageAttribute:ActionFilterAttribute
    {
        #region varibles membres

        private readonly string _controllerName = string.Empty;
        private readonly string _actionName = string.Empty;
        private readonly string _numOffreParam = string.Empty;
        #endregion

        #region Constructeur

        public AlbRefreshPageAttribute(string controllerName, string actionName, string numOffreParam)
        {
            _controllerName = controllerName;
            _actionName = actionName;
            _numOffreParam = numOffreParam;
        }
        #endregion

        #region Méthodes publiques
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            object valeurNumeroOffre;
            var queryParam=string.Empty;
            if (_numOffreParam!="NoParam" && filterContext.ActionParameters.Count > 0 )
            {
                string[] tabParam = _numOffreParam.Split('|');
                if (tabParam.Length>0)
                {
                    foreach (var elem in tabParam)
                    {
                        filterContext.ActionParameters.TryGetValue(elem, out valeurNumeroOffre);
                        queryParam += "/"+(string) valeurNumeroOffre;
                    }
                }
            }
            if (CacheTools.AlbSessionHelper.CurrentScreen == null)
                CacheTools.AlbSessionHelper.CurrentScreen = new NavigationTrace { CurrentController = _controllerName,CurrentAction = _actionName + "/"+queryParam, ReloadScreen = true };
            else
            {
               
                if(CacheTools.AlbSessionHelper.CurrentScreen.CurrentController==_controllerName && CacheTools.AlbSessionHelper.CurrentScreen.CurrentAction==_actionName)
                {
                    if (CacheTools.AlbSessionHelper.CurrentScreen.CurrentParam == queryParam)
                    {
                        CacheTools.AlbSessionHelper.CurrentScreen.ReloadScreen = false;
                        filterContext.Result = null;
                    }
                    else
                    {
                        CacheTools.AlbSessionHelper.CurrentScreen.ReloadScreen = true;
                        CacheTools.AlbSessionHelper.CurrentScreen.CurrentParam = queryParam;

                    }
                }
                else
                {
                    CacheTools.AlbSessionHelper.CurrentScreen.CurrentController = _controllerName;
                    CacheTools.AlbSessionHelper.CurrentScreen.CurrentAction = _actionName;
                    CacheTools.AlbSessionHelper.CurrentScreen.ReloadScreen = true;
                    CacheTools.AlbSessionHelper.CurrentScreen.CurrentParam = queryParam;
                }
            }

           
             if (CacheTools.AlbSessionHelper.CurrentScreen.ReloadScreen)
             {
                 var responseUrl = new StringBuilder();
                 responseUrl.Append("/" + CacheTools.AlbSessionHelper.CurrentScreen.CurrentController + "/" + CacheTools.AlbSessionHelper.CurrentScreen.CurrentAction + "/" + CacheTools.AlbSessionHelper.CurrentScreen.CurrentParam);
                 filterContext.Result = new RedirectResult(responseUrl.ToString());
                 CacheTools.AlbSessionHelper.CurrentScreen.ReloadScreen = false;
                 
                
             }
             else
                base.OnActionExecuting(filterContext);
        }
        #endregion
    }
}
