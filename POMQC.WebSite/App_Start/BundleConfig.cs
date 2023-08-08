
using System.Web;
using System.Web.Optimization;

namespace POMQC.WebSite
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jquery.blockUI.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/Swiper-3.3.1/dist/js/swiper.js",
                        "~/Scripts/select2.js",
                        "~/Scripts/lightbox2/js/lightbox.js",
                        "~/Scripts/dropzone/dropzone.js",
                        "~/Scripts/jquery.table2excel.js",
                        "~/Scripts/jquery.ui.touch-punch.js",
                        "~/Scripts/jquery.inputpicker.js",
                        "~/Scripts/app/_constant.js",
                        "~/Scripts/app/dashboard.js",
                        "~/Scripts/app/sample.js",
                        "~/Scripts/app/quality.js",
                        "~/Scripts/app/meeting.js",
                        "~/Scripts/app/report.js",
                        "~/Scripts/app/inspection.js",
                        "~/Scripts/app/defect.js",
                        "~/Scripts/app/core.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

             bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/select2.js",
                        "~/Scripts/lightbox2/js/lightbox.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Content/bootstrap.css",
                "~/Scripts/Swiper-3.3.1/dist/css/swiper.css",
                "~/Content/bootstrap-responsive.css",
                "~/Scripts/dropzone/basic.css",
                "~/Scripts/dropzone/dropzone.css",
                "~/Content/select2.css",
                "~/Scripts/lightbox2/css/lightbox.css",
                "~/Scripts/jquery.inputpicker.css"
            ));


            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css",
                "~/Content/ng-grid.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.css",
                        "~/Content/themes/base/jquery.ui.all.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}