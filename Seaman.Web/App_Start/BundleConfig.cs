using System.Web.Optimization;

namespace Seaman.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;
            
            

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/lodash.js",
                        "~/Scripts/jquery.slimscroll.js",
                        "~/Scripts/moment-with-locales.js",
                        "~/Scripts/moment-range.js",
                        "~/Scripts/spin.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/angular-material.css",
                      "~/Content/material-design-iconic-font.css",
                      "~/Content/bootstrap.css",
                      "~/Content/ui-bootstrap-csp.css",
                      "~/Content/ui.css",                      
                      "~/Content/main.css",
                      "~/Content/ui-grid/ui-grid-unstable.css",
                      "~/Content/loading-bar.css",
                      "~/Content/Site.css"));

            bundles.Add(new ScriptBundle("~/bundles/pdfmake")
                .IncludeDirectory("~/Scripts/pdfmake/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular.core")
                .Include("~/Scripts/angular/angular.js")
                .IncludeDirectory("~/Scripts/angular/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular.ui")
                .IncludeDirectory("~/Scripts/angular-ui", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/angular.app")
                .IncludeDirectory("~/app", "*.module.js", true)
                .IncludeDirectory("~/app", "*.js", true));
        }
    }
}
