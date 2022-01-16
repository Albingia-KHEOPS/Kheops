using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class TestController : Controller
    {
        public TestController(ObjectCache o)
        {
        }

        public ActionResult Test()
        {
            return View("TestView");
        }
        public ActionResult TestJ()
        {
            return View("Test1");
        }

        public ActionResult TestJs()
        {
            return new JavaScriptResult() { Script = "debugger; window.location.assign('/test/test');"};
        }
    }
}