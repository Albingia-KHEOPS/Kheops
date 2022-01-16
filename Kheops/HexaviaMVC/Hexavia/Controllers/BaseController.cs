using Hexavia.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Hexavia.Controllers
{
    public class BaseController : Controller
    {
        protected bool ApplyZoomMax(List<LatLong> list)
        {
            if (list == null)
            {
                return false;
            }

            //On applique le zoom max si une adresse a des coordonnées
            //en dehors de la france metropolitaine
            var latMinFrance = 41.333538;
            var latMaxFrance = 51.08854;
            var lonMinFrance = -5.150906;
            var lonMaxFrance = 8.233242;

            return list.Exists(l => l.Lat > latMaxFrance || l.Lat < latMinFrance || l.Lon > lonMaxFrance || l.Lon < lonMinFrance);
        }
        /// <summary>
        /// LargeJson
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected JsonResult LargeJson(object data)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = "application/json",
                ContentEncoding = System.Text.Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }
    }
}