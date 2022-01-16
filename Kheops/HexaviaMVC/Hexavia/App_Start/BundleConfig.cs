using System.Web.Optimization;

namespace Hexavia
{
    public class BundleConfig
    {
        // Pour plus d'informations sur le regroupement, visitez https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/Js/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapjs").Include(
                      "~/Content/Js/umd/popper.min.js",
                      "~/Content/Js/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/styles").Include(
                      "~/Content/font-awesome.css",
                      "~/Content/Css/bootstrap.css",
                     "~/Content/Css/site.css"));

            // bundle sidebar - google maps
            bundles.Add(new StyleBundle("~/Content/sidebarGmap").Include(
                      "~/Content/Css/gmaps-sidebar/gmaps-sidebar.css"));

            bundles.Add(new StyleBundle("~/bundles/leafletcss").Include(
                      "~/Content/Plugins/leaflet/leaflet.css",
                      "~/Content/Plugins/leaflet/leaflet.draw.css"));

            bundles.Add(new ScriptBundle("~/bundles/leafletjs").Include(
                     "~/Content/Plugins/leaflet/leaflet.js",
                     "~/Content/Plugins/leaflet/leaflet.draw.js",
                     "~/Content/Plugins/leaflet/leaflet.draw.fr.js"
                     ));
            // bundle sweetalert2            
            bundles.Add(new ScriptBundle("~/bundles/sweetalert2").Include(
                     "~/Content/Js/sweetalert2.all.min.js"));
            // bundle hexavia-leaflet
            bundles.Add(new ScriptBundle("~/bundles/hexaviaLeafletMap").Include(
                   "~/Content/Js/hexavia-leafletmap.js"
                   ));
            // bundle hexavia - google maps
            bundles.Add(new ScriptBundle("~/bundles/hexaviaGmap").Include(
                  "~/Content/Js/hexavia-swal.js",
                  "~/Content/Js/hexavia-autocomplete.js",
                   "~/Content/Js/hexavia-common.js",
                  "~/Content/Js/hexavia-gmap.js"
                  ));

            // bundle sidebar - google maps
            bundles.Add(new ScriptBundle("~/bundles/sidebarGmap").Include(
                  "~/Content/Js/gmaps-sidebar/jquery-sidebar.js"
                  ));

            // bundle bootstap select css
            bundles.Add(new StyleBundle("~/Content/bootstrap-select").Include(
                      //"~/Content/Css/bootstrap-select.css"
                      "~/Content/Css/bootstrap-multiselect.css"
                      ));
            // bundle bootstap select js
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-select").Include(
                 //"~/Content/Js/bootstrap-select.js"
                 "~/Content/Js/bootstrap-multiselect.js"
                 ));
            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                       "~/Content/Js/jquery-ui-1.12.1.js"));

            bundles.Add(new StyleBundle("~/Content/jquery-ui").Include(
                     "~/Content/themes/base/jquery-ui.css"));

            // bundle jspdf
            bundles.Add(new ScriptBundle("~/bundles/jspdf").Include(
                   "~/Content/Js/jspdf.min.js"
                   ));
            // bundle html2canvas
            bundles.Add(new ScriptBundle("~/bundles/html2canvas").Include(
                   "~/Content/Js/html2canvas.min.js"
                   ));
            // bundle remove special characters
            bundles.Add(new ScriptBundle("~/bundles/specialCharacter").Include(
                   "~/Content/Js/replace_method.js"
                   ));

            System.Web.Optimization.BundleTable.EnableOptimizations = System.Configuration.ConfigurationManager.AppSettings["bundle"]?.ToLower() == true.ToString().ToLower();

        }
    }
}
