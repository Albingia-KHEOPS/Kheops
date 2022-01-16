using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace Pandore.Helper
{
    public class EmailSender
    {
        // Send mail with attachement file
        public static void Send(List<string> to, string subject, string body, bool isHtmlBody = false, string pathAttachmentFile = null)
        {

            MailMessage message = new MailMessage
            {
                Body = body,
                Subject = subject,
                IsBodyHtml = isHtmlBody,
            };
            foreach (var toMail in to)
            {
                message.To.Add(toMail);
            }
            if (!string.IsNullOrEmpty(pathAttachmentFile))
            {
                Attachment data = new Attachment(pathAttachmentFile, MediaTypeNames.Application.Octet);
                ContentDisposition disposition = data.ContentDisposition;
                disposition.CreationDate = System.IO.File.GetCreationTime(pathAttachmentFile);
                disposition.ModificationDate = System.IO.File.GetLastWriteTime(pathAttachmentFile);
                disposition.ReadDate = System.IO.File.GetLastAccessTime(pathAttachmentFile);
                message.Attachments.Add(data);
            }
            try
            {
                //Send the message.
                SmtpClient client = new SmtpClient();
                client.Send(message);
            }
            catch (Exception ex)
            {

            }

        }
    }
}