using Proiect_TW.Atributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proiect_TW.BusinessLogic.Interfaces;
using Proiect_TW.BusinessLogic;
using Proiect_TW.Web.Controllers;

namespace Proiect_TW.Controllers
{
    public class AdminController : BaseController
    {
        private readonly ISession _session;
        public AdminController()
        {
            var bl = new BussinessLogic();
            _session = bl.GetSessionBL();
        }
        public void GetUser()
        {
            SessionStatus();
            var apiCookie = System.Web.HttpContext.Current.Request.Cookies["X-KEY"];

            string userStatus = (string)System.Web.HttpContext.Current.Session["LoginStatus"];
            if (userStatus != "logout")
            {
                var profile = _session.GetUserByCookie(apiCookie.Value);
                ViewBag.User = profile;
            }
            else if (userStatus == "logout")
            {
                ViewBag.User = null;
            }
        }
        // GET: Admin

        [AdminMod]

        public ActionResult AddProduct()
        {

            GetUser();
            return View();
        }
        public ActionResult Statistics()
        {
            GetUser();
            return View();
        }
        public ActionResult Users()
        {
            GetUser();
            return View();
        }
        public ActionResult Products()
        {
            GetUser();
            return View();
        }
    }
}