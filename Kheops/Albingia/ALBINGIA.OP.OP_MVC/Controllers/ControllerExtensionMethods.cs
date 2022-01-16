using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public static class ControllerExtensions
  {
    public static string GetGuid(this Controller controller)
    {
      return controller.RouteData.Values["guid"].ToString();
    }

    public static void SetGuidSession
    (this Controller controller, string name, object value)
    {
      controller.Session[controller.GetGuid() + "_" + name] = value;
    }

    public static object GetGuidSession(this Controller controller, string name)
    {
      return controller.Session[controller.GetGuid() + "_" + name];
    }

    public static void GetTabGuid(this Controller controller)
    {
      var guid=controller.RouteData.Values.FirstOrDefault(el => el.Key == "guid");
    }
  }
}