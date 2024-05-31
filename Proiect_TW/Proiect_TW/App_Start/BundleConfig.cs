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
            bundles.Add(new StyleBundle("~/assets/css/template1").Include(
          "~/assets/css/css_template1/bootstrap.min.css",
          "~/assets/css/css_template1/font-awesome.css",
          "~/assets/css/css_template1/templatemo-hexashop.css",
          "~/assets/css/css_template1/owl-carousel.css",
          "~/assets/css/css_template1/lightbox.css"));

            bundles.Add(new ScriptBundle("~/assets/js/template1").Include(
              "~/assets/js/js_template1/jquery-2.1.0.min.js",
              "~/assets/js/js_template1/popper.js",
              "~/assets/js/js_template1/bootstrap.min.js",
              "~/assets/js/js_template1/owl-carousel.js",
              "~/assets/js/js_template1/accordions.js",
              "~/assets/js/js_template1/datepicker.js",
              "~/assets/js/js_template1/scrollreveal.min.js",
              "~/assets/js/js_template1/waypoints.min.js",
              "~/assets/js/js_template1/jquery.counterup.min.js",
              "~/assets/js/js_template1/imgfix.min.js",
              "~/assets/js/js_template1/slick.js",
              "~/assets/js/js_template1/lightbox.js",
              "~/assets/js/js_template1/isotope.js",
              "~/assets/js/js_template1/custom.js",
              "~/assets/js/js_template1/quantity.js"));


            bundles.Add(new ScriptBundle("~/assets/js").Include(
                "~/assets/js/addedscript.js",
                "~/assets/js/AddProduct.js"));

            bundles.Add(new StyleBundle("~/assets/css/template2")
                .Include("~/assets/css/css_template2/bootstrap.css",
                         "~/assets/css/css_template2/bootstrap.min.css",
                         //"~/assets/css/css_template2/app-rtl.css",
                         //"~/assets/css/css_template2/app-rtl.min.css",
                         //"~/assets/css/css_template2/app-rtl.min.css.map",
                         "~/assets/css/css_template2/app.css",
                         "~/assets/css/css_template2/app.min.css",
                         "~/assets/css/css_template2/app.min.css.map",
                         "~/assets/css/css_template2/bootstrap.min.css.map",
                         "~/assets/css/css_template2/icons.css",
                         "~/assets/css/css_template2/icons.min.css",
                         "~/assets/css/css_template2/icons.min.css.map",
                         "~/assets/css/css_template2/jquery-jvectormap-1.2.2.css",
                         "~/assets/css/css_template2/dropzone.min.css"
                         ));
            bundles.Add(new StyleBundle("~/assets/js/template2")
                .Include("~/assets/js/js_template2/jquery-jvectormap-world-mill-en.js",
                         "~/assets/js/js_template2/jquery-jvectormap-1.2.2.min.js",
                         //"~/assets/js/js_template2/apexcharts.min.js",
                         "~/assets/js/js_template2/jquery.sparkline.min.js",
                         "~/assets/js/js_template2/jquery.peity.min.js",
                         "~/assets/js/js_template2/dashboard-2.init.js",
                         "~/assets/js/js_template2/app.min.js",
                         "~/assets/js/js_template2/vendor.min.js",
                         "~/assets/js/js_template2/dropzone.min.js",
                         "~/assets/js/js_template2/form-fileuploads.init.js",
                         "~/assets/js/js_template2/script.js"
                         ));




            bundles.Add(new StyleBundle("~/assets/css/login")
                .Include("~/assets/css/css_template2/bootstrap.css",
                         "~/assets/css/css_template2/icons.min.css",
                         "~/assets/css/css_template2/app.min.css",
                         "~/assets/css/css_template2/loginstyle.css"
                         ));

            bundles.Add(new StyleBundle("~/assets/js/login")
                .Include("~/assets/js/js_template2/app.min.js",
                         "~/assets/js/js_template2/vendor.min.js"));

            bundles.Add(new StyleBundle("~/assets/images")
                .Include("~/assets/images/favicon.ico"));
        }
    }
}
