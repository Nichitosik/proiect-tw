using Proiect_TW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Optimization;
using System.Web.UI.WebControls;

namespace Proiect_TW
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/assets/css").Include(
                      "~/assets/css/bootstrap.min.css",
                      "~/assets/css/font-awesome.css",
                      "~/assets/css/templatemo-hexashop.css",
                      "~/assets/css/owl-carousel.css",
                      "~/assets/css/lightbox.css",
                      "~/assets/css/about-page.css"));

            bundles.Add(new StyleBundle("~/assets/css/_Layout_Blank")
                .Include("~/assets/css/bootstrap2.min.css",
                         "~/assets/css/icons.min.css",
                         "~/assets/css/app.min.css"));


            bundles.Add(new StyleBundle("~/assets/js/_Layout_Blank")
                .Include("~/assets/js/vendor.min.css",
                         "~/assets/js/app.min.js"));

            bundles.Add(new StyleBundle("~/assets/images/_Layout_Blank")
                .Include("~/assets/images/favicon.ico"));

            bundles.Add(new ScriptBundle("~/assets/js").Include(
              "~/assets/js/jquery-2.1.0.min.js",
              "~/assets/js/popper.js",
              "~/assets/js/bootstrap.min.js",
              "~/assets/js/owl-carousel.js",
              "~/assets/js/accordions.js",
              "~/assets/js/datepicker.js",
              "~/assets/js/scrollreveal.min.js",
              "~/assets/js/waypoints.min.js+",
              "~/assets/js/jquery.counterup.min.js",
              "~/assets/js/imgfix.min.js",
              "~/assets/js/slick.js",
              "~/assets/js/lightbox.js",
              "~/assets/js/isotope.js",
              "~/assets/js/custom.js",
              "~/assets/js/addedscript.js"));
        }
    }
}
