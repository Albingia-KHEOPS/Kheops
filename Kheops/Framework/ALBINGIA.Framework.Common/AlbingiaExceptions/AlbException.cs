using System;
using System.Globalization;
using System.Web;
using ALBINGIA.Framework.Common.Tools;

namespace ALBINGIA.Framework.Common.AlbingiaExceptions
{
    public class AlbException:Exception
    {
        #region Variables membres
        private readonly bool _sendMail;
        private readonly Exception _exception;
        #endregion
        
        public bool Trace { get; set; }
        protected string AlbExpMessage { get; set; }

        public AlbException(Exception exception, bool trace = true, bool sendMail = true, string callFuncTrace = "E", bool onlyMesage = true, string erreurParameters="")
        {
            Trace = trace;
            _exception = exception;
            _sendMail = sendMail;
            if (!onlyMesage) {
                AlbExpMessage = string.Format("<b>Utilisateur</b>: {0} <br/><b>Date</b>: {1}<br/><b>Message</b> : {2}  <br/>{4}<br/><b>Stack Trace</b> : {3}",
                    HttpContext.Current.User.Identity.Name,
                    DateTime.Now.ToString(CultureInfo.CurrentCulture), exception.Message, exception.StackTrace, erreurParameters);
            }
            else {
                AlbExpMessage = erreurParameters + exception.Message;
            }

            WriteTrace(callFuncTrace);
          
        }

        private void WriteTrace(string callFuncTrace)
        {
            if (string.IsNullOrEmpty(callFuncTrace)) {
                return;
            }
            switch (callFuncTrace)
            {
                case "E":
                    TraceError(_exception);
                    break;
                case "W":
                    TraceWarning(_exception);
                    break;
            }
        }

        protected void TraceError(Exception ex)
        {
            var message = string.Format("<b>Message d'erreur de l'application outils de Production</b><br/>{0}", AlbExpMessage);
            AlbLog.Log(message, AlbLog.LogTraceLevel.Erreur);
            if (!_sendMail) return;
            message = string.Format("Utilisateur : {0}</br><b>Message d'erreur de l'application outils de Production</b><br/>{1}", HttpContext.Current.User.Identity.Name, AlbExpMessage);
            AlbMailing.SendMail(message);
        }

        protected void TraceWarning(Exception ex)
        {
            var message = string.Format("<b>Message d'avertissement de l'application Outils de Production</b><br/>{0}", AlbExpMessage);
            AlbLog.Warn(message);
            if (!_sendMail) return;
            message = string.Format("Utilisateur : {0}</br><b>Message d'avertissement de l'application Outils de Production</b><br/>{1}", HttpContext.Current.User.Identity.Name, AlbExpMessage);
            AlbMailing.SendMail(message);
        }
    }
}
