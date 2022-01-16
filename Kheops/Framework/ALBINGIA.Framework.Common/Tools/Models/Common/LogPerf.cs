using System;
using ALBINGIA.Framework.Common.Tools;

namespace ALBINGIA.Framework.Common.Models.Common
{
    public class LogPerf
    {
        public DateTime? DateLog { get; set; }
        public string User { get; set; }
        public string Screen { get; set; }
        public string Action { get; set; }
        public string ElapsedTime { get; set; }
    }
}
