using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;

namespace ALBINGIA.OP.OP_MVC.Helpers
{
    public static class AlbAddCssJsScripts
    {
        /// <summary>
        /// Retourne les balises scripts à intégrer dans la vue (les fichiers doivenet être dansle même répèrtoire)
        /// </summary>
        /// <param name="urlHelper">UrlHeler à utiliser</param>
        /// <param name="jsFiles">Liste des fichier Js</param>
        /// <param name="path">Chemain des fichiers JS ex:"~/Content/"</param>
        /// <returns></returns>
        public static MvcHtmlString AlbAddJsToViews(UrlHelper urlHelper, string[] jsFiles, string path = "~/Scripts/AlbingiaJS/", bool autoSuffixMin = true) {
            var resScripts = new StringBuilder();
            const string JsExt = ".js";
            const string MinJs = "min" + JsExt;
            if (!path?.StartsWith("~/") ?? true) { path = "~/Scripts/"; }
            Array.ForEach(jsFiles, jsFile => {
                if (jsFile.ContainsChars() && jsFile.EndsWith(JsExt)) {
                    var jsEl = path.ToLower().Contains("albingiajs") || jsFile.ToLower().Contains("albingiajs") || jsFile.ToLower().Contains("date-fr-fr")
                      ? jsFile
                      : autoSuffixMin ? (!jsFile.EndsWith(MinJs) ? (jsFile.Substring(0, jsFile.LastIndexOf(JsExt)) + MinJs) : jsFile) : jsFile;

                    resScripts.Append(string.Format(
                        "<script src=\"{0}{1}\" type=\"text/javascript\"></script>",
                        urlHelper.Content(path + (path.EndsWith("/") ? string.Empty : "/") + jsEl),
                        JsVersionSuffix()));
                }
            });

            return MvcHtmlString.Create(resScripts.ToString());
        }

        /// <summary>
        /// Retourne les balises Link à intégrer dans la vue (les fichiers doivenet être dansle même répèrtoire)
        /// </summary>
        /// <param name="urlHelper">UrlHeler à utiliser</param>
        /// <param name="jsFiles">Liste des fichier Js</param>
        /// <param name="path">Chemain des fichiers JS ex:"~/Content/"</param>
        /// <returns></returns>
        public static MvcHtmlString AlbAddCssToViews(UrlHelper urlHelper, string[] jsFiles, string path)
        {
            var resScripts = new StringBuilder();
            //jsFiles.ForEach(elem => resScripts.Append(string.Format("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\"></script>", urlHelper.Content(path + elem) + Environment.NewLine)));

            Array.ForEach(jsFiles, elem =>
            {
                resScripts.Append(string.Format(
                    "<link href=\"{0}{1}\" rel=\"stylesheet\" type=\"text/css\" />",
                    urlHelper.Content(path + (path.EndsWith("/") ? string.Empty : "/") + elem),
                    JsVersionSuffix()));
            });

            return MvcHtmlString.Create(resScripts.ToString());
        }

        public static MvcHtmlString IncludeKoComponent(this UrlHelper urlHelper, string componentName, bool useFullname = true) {
            return MvcHtmlString.Create(
                $"<script src=\"{urlHelper.Content($"~/knockout/components/{componentName}/{(useFullname ? (componentName + ".") : string.Empty)}initializer.js{JsVersionSuffix()}")}\" type=\"text/javascript\"></script>"
                + Environment.NewLine
                + $"<script src=\"{urlHelper.Content($"~/knockout/components/{componentName}/{(useFullname ? (componentName + ".") : string.Empty)}viewmodel.js{JsVersionSuffix()}")}\" type=\"text/javascript\"></script>");
        }

        public static MvcHtmlString JsVersionSuffix() {
            return new MvcHtmlString("?v=" + AlbOpConstants.JsCsVersion);
        }

        public static  IHtmlString AddScriptBundle(string name)
        {
            string version = BundleTable.EnableOptimizations ? string.Empty : $"?v={AlbOpConstants.JsCsVersion}";
            return Scripts.RenderFormat("<script src=\"{0}" + version + "\"></script>", name);
        }

        public static IHtmlString AddStyleBundle(string name)
        {
            string version = BundleTable.EnableOptimizations ? string.Empty : $"?v={AlbOpConstants.JsCsVersion}";
            return Scripts.RenderFormat("<link href=\"{0}" + version + "\" rel=\"stylesheet\" type=\"text/css\" />", name);
        }
    }
}
