using Proiect_TW.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using Proiect_TW.BusinessLogic.Interfaces;
using System.Web;
using System.Web.Mvc;
namespace Proiect_TW.Web.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly ISession _session;
        public ProductsController()
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
        // GET: Products
        public ActionResult Products()
        {
            GetUser();
            return View();
        }
        public ActionResult Men()
        {
            GetUser();
            return View();
        }
        public ActionResult Women()
        {
            GetUser();
            return View();
        }
        public ActionResult Kids()
        {
            GetUser();
            return View();
        }
        public ActionResult Accessories()
        {
            GetUser();
            return View();
        }
    }
}