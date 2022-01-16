using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Albingia.Hexavia.WebMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Bienvenue dans ASP.NET MVC !";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
