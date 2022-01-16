using System;
using ALBINGIA.Framework.Common.Tools;

namespace ALBINGIA.Framework.Common.Models.Common
{
    public class LogTrace
    {
        public AlbLog.LogTraceLevel Level { get; set; }
        public DateTime DateLog { get; set; }
        public string Utilisateur { get; set; }
        public string ContentLog { get; set; }
        public string MachineName { get; set; }

    }
}
