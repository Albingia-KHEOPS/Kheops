using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Common.Tools {
    public class WCFHelper {
        static readonly Regex keyRegex = new Regex(@"^[\w\-\.]+$", RegexOptions.Singleline | RegexOptions.Compiled);
        public static string GetFromHeader(string key) {
            if (!keyRegex.IsMatch(key)) {
                return null;
            }
            try {
                int? x = OperationContext.Current?.IncomingMessageHeaders.FindHeader(key, "http://albingia.fr/kheops/2018");
                //return x.HasValue ? OperationContext.Current.IncomingMessageHeaders.GetHeader<string>(x.Value) : null;
                return x.HasValue ? OperationContext.Current.IncomingMessageHeaders.GetHeader<string>(x.Value) : string.Empty;
            }
            catch (ObjectDisposedException) { }
            return null;
        }
    }
}
