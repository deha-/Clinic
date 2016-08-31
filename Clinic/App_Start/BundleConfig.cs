using System.Web;
using System.Web.Optimization;

namespace Clinic
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/bootstrap/bootstrap.css",
                      "~/Content/site.css"));

            //hanka
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery/jquery-2.1.4.js",
                        "~/Scripts/jquery/jquery-ui.js",
                        "~/Scripts/jquery/jquery.validate.js",
                        "~/Scripts/jquery/additional-methods.js",
                        "~/Scripts/jquery/jquery.mask.js"));

            bundles.Add(new ScriptBundle("~/bundles/jsgrid").Include(
                        "~/Scripts/jsgrid/jsgrid.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css/jquery").Include(
                        "~/Content/css/jquery-ui.structure.css",
                        "~/Content/css/jquery-ui.theme.css",
                        "~/Content/css/jquery-ui.css"));

            bundles.Add(new StyleBundle("~/Content/css/jsgrid").Include(
                        "~/Content/css/jsgrid/jsgrid-theme.css",
                        "~/Content/css/jsgrid/jsgrid.css"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap").Include(
                        "~/Content/css/bootstrap/bootstrap-theme.css",
                        "~/Content/css/bootstrap/bootstrap.css"));
        }
    }
}
