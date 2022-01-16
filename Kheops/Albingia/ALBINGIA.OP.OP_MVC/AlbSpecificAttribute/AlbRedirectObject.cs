using ALBINGIA.Framework.Common.Business;
using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.AlbSpecificAttribute
{
    public class AlbRedirectObject
    {
        public AlbRedirectObject()
        {
            Controller = string.Empty;
            Action = string.Empty;
            Querystring = new List<string>();
            GuidTab = string.Empty;
            ToHome = false;
            NewWindow = false;
            CreateSubmitter = false;
        }

        public string Controller { get; set; }

        public string Action  { get; set; }

        public string QuerystringId  { get; set; }
        public List<string> Querystring { get; set; }

        public string GuidTab { get; set; }

        public bool ToHome { get; set; }

        bool NewWindow { get; set; }

        public bool CreateSubmitter { get; set; }

        public bool IsNewWindow { get; internal set; }
        
        public AccessMode? AccessMode { get; set; }

        public string BuildQuerystring(bool includeId = true) {
            if (Querystring.Any(x => !x.IsEmptyOrNull())) {
                return (includeId && !QuerystringId.IsEmptyOrNull() ? $"{QuerystringId}" : string.Empty)
                    + "?" + string.Join("&", Querystring.Where(x => !x.IsEmptyOrNull()));
            }
            return includeId && !QuerystringId.IsEmptyOrNull() ? $"{QuerystringId}" : string.Empty;
        }
    }
}