using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Diagnostics.CodeAnalysis;
using Albingia.Hexavia.CoreDomain;
using System.Configuration;

namespace Albingia.Hexavia.Web
{
    [SuppressMessage("Gendarme.Rules.Performance", "AvoidUncalledPrivateCodeRule")]
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown
            
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            //Exception objErr = Server.GetLastError().GetBaseException();
            // At this point we have information about the error
            HttpContext ctx = HttpContext.Current;

            // set the exception to the Context 
            Exception exception = ctx.Server.GetLastError();
            Session["MessageErreur"] = exception.InnerException!=null?exception.InnerException.Message:exception.Message;
            Server.Transfer("~/Erreur.aspx");
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

    }
}
