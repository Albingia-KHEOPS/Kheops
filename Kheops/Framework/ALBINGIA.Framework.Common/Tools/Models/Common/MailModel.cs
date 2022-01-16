using ALBINGIA.Framework.Common.Tools;

namespace ALBINGIA.Framework.Common.Models.Common
{
    public class MailModel
    {
        public string From { get; set; }
        public string FromDisplayName { get; set; }
        public string To { get; set; }
        public string ToDisplayName { get; set; }
        public string CC { get; set; }
        public string CCi { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }


        public static MailModel InfosMail
        {
            get { return AlbGenericSingPattInstance<MailModel>.AlbSingGlobalInit<MailModel>.UniqueInstance; }
        }

    }
}
