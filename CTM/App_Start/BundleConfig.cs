﻿using System;
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
                        "~/Scripts/jquery.unobtrusive*"));

            bundles.Add(new ScriptBundle("~/bundles/bundleJqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/bundleModernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bundleBootstrap").Include(
                       "~/Scripts/bootstrap.js"
                      //  "~/Scripts/respond.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/bundleCustomizedJS").Include(
                      "~/Scripts/jquery.dropdown.js",
                       //"~/Scripts/jquery.tagsinput.js",
                       //"~/Scripts/material.min.js",
                      //"~/Scripts/nouislider.min.js",
                      //"~/Scripts/material-kit.js",
                      //"~/Scripts/materialize.min.js",
                      "~/Scripts/sitemethod.js",
                      "~/Scripts/site.js"));

            bundles.Add(new StyleBundle("~/bundles/bundleCss")
                 .Include("~/Content/bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/Content/font-awesome.css", new CssRewriteUrlTransform())

                );

            var bundle = new StyleBundle("~/bundles/bundleCustomizedCSS");
        
            //bundle.Include("~/Content/material.css", new CssRewriteUrlTransform());
            //bundle.Include("~/Content/materialize.css", new CssRewriteUrlTransform());
            bundle.Include("~/Content/site.css", new CssRewriteUrlTransform());

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
            // Code removed for clarity.
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
