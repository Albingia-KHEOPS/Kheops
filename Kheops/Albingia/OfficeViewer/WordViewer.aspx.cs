using ALBINGIA.Framework.Common.Constants;
using System;
using System.Data;
using System.Web.UI;

namespace OfficeViewer
{
    public partial class WordViewer : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var pathFile = Request["of"];
            pathFile = System.Web.HttpContext.Current.Server.UrlDecode(pathFile);
            if (string.IsNullOrEmpty(pathFile))
            {
                //TODO LOG + Erreur
                fileNamePath.Value = string.Empty;
                return;
            }
            jsVersion.Value = AlbOpConstants.JsCsVersion;
            var parmFile = pathFile.Split(new[] { "-__-" }, StringSplitOptions.None);
            // mettre en forme le chemin
            fileNamePath.Value = "http://" + Request.Url.Host + Request.ApplicationPath + "/Documents" + parmFile[0].Replace("_--_", "/");
            ruleFile.Value = parmFile[1];
           
            prefixPathDocuments.Value = AlbOpConstants.PrefixPathDocuments;
            physicNameDoc.Value = AlbOpConstants.PrefixPathDocuments + parmFile[0].Replace("_--_", "\\");
        }
    }
}