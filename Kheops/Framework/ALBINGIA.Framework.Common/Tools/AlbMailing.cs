using System;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Models.Common;

namespace ALBINGIA.Framework.Common.Tools {
    public class AlbMailing {

        public static void SendMail(string message, bool asynchSendMail = true) {
            if (!AlbOpConstants.NotificationMail)
                return;
            AlbOpConstants.MainInfoContent.Body = message;
            var mail = new AlbMailing();
            mail.SendMail(AlbOpConstants.MainInfoContent, asynchSendMail);
        }
        public void SendMail(MailModel mailModel, bool asynchSendMail = true) {
            try {
                if (mailModel == null) {
                    return;
                }
                var mMailMessage = new MailMessage { From = new MailAddress(mailModel.From, mailModel.FromDisplayName ?? string.Empty) };
                //mMailMessage.To.Add(new MailAddress(mailModel.To, mailModel.ToDisplayName??string.Empty));

                AddListMailAdress(mMailMessage.To, mailModel.To);
                if (mailModel.CC != null) {
                    AddListMailAdress(mMailMessage.CC, mailModel.CC);
                }
                if (!string.IsNullOrEmpty(mailModel.CCi)) {
                    AddListMailAdress(mMailMessage.Bcc, mailModel.CCi);
                }
                mMailMessage.Subject = mailModel.Subject;
                mMailMessage.Body = mailModel.Body;
                mMailMessage.IsBodyHtml = true;
                mMailMessage.Priority = MailPriority.Normal;
                var mSmtpClient = new SmtpClient();

                Object userState = mMailMessage;
                if (!asynchSendMail) {
                    SendSynch(mMailMessage, mSmtpClient);
                } else {
                    SendAsynch(mMailMessage, mSmtpClient, userState);
                }

            } catch (Exception ex) {
                AlbLog.Log("Message:" + ex.Message + Environment.NewLine + " Stack trace" + ex.StackTrace, AlbLog.LogTraceLevel.Erreur);

            }


        }

        private void SendAsynch(MailMessage mMailMessage, SmtpClient mSmtpClient, object userState)
        {
            //Attacher l'event handler au retour Callback
            mSmtpClient.SendCompleted += SmtpClientSendCompleted;
                
            Task.Run(()=> this.SendSynch(mMailMessage, mSmtpClient)).ContinueWith( 
                (r) => {
                    if (r.Exception != null)
                    {
                        AlbLog.Log("Message:" + r.Exception.Message + Environment.NewLine + " Stack trace" + r.Exception.StackTrace, AlbLog.LogTraceLevel.Erreur);
                    }
                }, TaskContinuationOptions.OnlyOnFaulted);
        }

        private void SendSynch(MailMessage mMailMessage, SmtpClient mSmtpClient)
        {
            Task.Run(() => {
                try {
                    mSmtpClient.Send(mMailMessage);
                } catch (SmtpException smtpEx) {

                    //Todo Log Windows EventViwer
                    AlbLog.Log("Message:" + smtpEx.Message + Environment.NewLine + " Stack trace" + smtpEx.StackTrace, AlbLog.LogTraceLevel.Erreur);
                }
            });
        }

        void SmtpClientSendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e) {
            var mailMessage = e.UserState as MailMessage;
            if (e.Cancelled) {
                if (mailMessage != null)
                    AlbLog.Warn("Le mail n'est pas envoyé à cette adresse " + mailMessage.To[0].Address);
            }

            if (e.Error != null) {
                AlbLog.Warn("Message: Erreur d'envoie de mail " + e.Error.Message + Environment.NewLine + " Stack trace" + e.Error.StackTrace);
            }
            else {
                if (mailMessage != null) {
                    AlbLog.Warn("Mail envoyé avec succés à " + mailMessage.To[0].Address);
                }
            }
        }


        #region Méthodes privées

        private void AddListMailAdress(MailAddressCollection mailAddressCollection, string adress) {
            if (adress == string.Empty)
                return;
            if (!adress.Contains(";")) {
                mailAddressCollection.Add(new MailAddress(adress));
            }
            else {
                string[] ccAdr = adress.Split(';');
                foreach (var cc in ccAdr) {
                    mailAddressCollection.Add(new MailAddress(cc));
                }
            }
        }

        #endregion
    }
}
