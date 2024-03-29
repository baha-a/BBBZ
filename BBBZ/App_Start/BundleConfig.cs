﻿using System.Web;
using System.Web.Optimization;

namespace BBBZ
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-rtl").Include(
                      "~/Scripts/bootstrap-rtl.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/myscript").Include(
                        "~/Scripts/myscript.js"));

            bundles.Add(new ScriptBundle("~/bundles/tinymce").Include(
                        "~/Scripts/tinymce/jquery.tinymce.min.js",
                        "~/Scripts/tinymce/tinymce.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/nestable").Include(
                        "~/Scripts/jquery.nestable.js",
                        "~/Scripts/jquery.nestablePlus.js"));



            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/css-rtl").Include(
                        "~/Content/bootstrap-rtl.css"));





            bundles.Add(new StyleBundle("~/Content/admin").Include(
                      "~/Content/admin/modal.css",
                      "~/Content/admin/font-awesome.min.css",
                      "~/Content/admin/ace-fonts.css",
                      "~/Content/admin/ace.min.css",
                      "~/Content/admin/template.css"));

            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                        "~/Content/admin/bootstrap.min.js",
                        "~/Content/admin/template.js",
                        "~/Content/admin/ace-elements.min.js",
                        "~/Content/admin/ace.min.js"
                        ));
        }
    }
}
