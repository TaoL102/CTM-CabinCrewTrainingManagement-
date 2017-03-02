using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Optimization;

namespace CTM
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/bundleJquery").Include(
                        "~/Scripts/jquery-{version}.js",
                         "~/Scripts/jquery.validate*", 
                         "~/Scripts/jquery-ui.js", 
                        "~/Scripts/jquery.unobtrusive*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/bundleModernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bundleBootstrap").Include(
                       "~/Scripts/bootstrap.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/bundleCustomizedJS").Include(           
                      "~/Scripts/jquery.dropdown.js",
                      "~/Scripts/site.js"));

            bundles.Add(new StyleBundle("~/bundles/bundleCss")
                 .Include("~/Content/bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/Content/font-awesome.css", new CssRewriteUrlTransform())

                );

            bundles.Add(new StyleBundle("~/bundles/bundleCustomizedCSS")
                .Include("~/Content/site.css")); 

            BundleTable.EnableOptimizations = false;
        }
    }
    class NonOrderingBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }

    }
}
