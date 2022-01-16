using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.IOFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace OfficeViewer
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            //Version JS CSS
            AlbOpConstants.JsCsVersion = FileContentManager.GetConfigValue("JsCsVersion");
             //Path Documents
            AlbOpConstants.PrefixPathDocuments = FileContentManager.GetConfigValue("PrefixPathDocuments");
           

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}