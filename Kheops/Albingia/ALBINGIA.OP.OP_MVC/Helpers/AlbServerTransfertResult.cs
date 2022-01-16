using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ALBINGIA.OP.OP_MVC.Helpers
{
  public class AlbServerTransfertResult : RedirectToRouteResult
  {
    private readonly RouteValueDictionary _routeValueDictionary;
    
    public AlbServerTransfertResult(string actionName, string controllerName, object routeValue)
      : base(GetRouteValue(actionName, controllerName, routeValue))
    {
      _routeValueDictionary = GetRouteValue(actionName, controllerName, routeValue);
    }
    public AlbServerTransfertResult( RouteValueDictionary routeValue)
      : base(routeValue)
    {
    }
    //public AlbServerTransfertResult(object routeValues)
    //  : base(GetRouteURL(routeValues))
    //{
    //}

    private static RouteValueDictionary GetRouteValue(string actionName, string controllerName, object routeValues)
    {
      var valParams = routeValues.ToString().Split(',');
      if (!valParams.Any())
        return null;
      StringBuilder strUrlProvider = null;
      for (int i = 0; i < valParams.Length; i++)
      {
        if (strUrlProvider == null)
          strUrlProvider = new StringBuilder();
        strUrlProvider.Append("/" + valParams[i].Split('=')[1]); 
        
      }
   
      //foreach (var keyValuePair in valParams)
      //{
        
      //  strUrlProvider.Append("/" + keyValuePair.Value.ToString());

      //}
      var redirectTargetDictionary = new RouteValueDictionary
             {
               {"action", actionName+strUrlProvider},
               {"controller", controllerName}
             };

     
      //UrlHelper url = new UrlHelper(new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData()), RouteTable.Routes);
      return redirectTargetDictionary;
    }

    public override void ExecuteResult(ControllerContext context)
    {
      var httpContext = HttpContext.Current;
          
      // ASP.NET MVC 3.0
      if (context.Controller.TempData != null &&
          context.Controller.TempData.Count() > 0)
      {
        throw new ApplicationException("TempData won't work with Server.TransferRequest!");
      }
      var url = new UrlHelper(context.RequestContext);
      if (_routeValueDictionary != null)
        httpContext.Server.TransferRequest(url.RouteUrl(_routeValueDictionary), true); // change to false to pass query string parameters if you have already processed them
    }
    
  }
}