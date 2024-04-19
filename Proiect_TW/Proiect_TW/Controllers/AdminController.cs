using Proiect_TW.Atributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proiect_TW.Controllers
{
    public class AdminController : Controller
    {
        [AdminMod]
        // GET: Admin
        public ActionResult AddProduct()
        {
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }
    }
}