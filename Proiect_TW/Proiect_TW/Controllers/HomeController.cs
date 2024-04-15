using Proiect_TW.Extension;
using System.Collections.Generic;
using System.Web.Mvc;
using Proiect_TW.Web.Controllers;
using Proiect_TW.BussinesLogic.DBModel.Seed;
using Proiect_TW.Domain.Entities.User;
using System.Linq;
using Proiect_TW.BusinessLogic;
using Proiect_TW.BusinessLogic.Interfaces;


namespace Proiect_TW.Controllers
{
    public class HomeController : BaseController
    {

        // GET: Home
        public ActionResult Index()
        {
            SessionStatus();
            
            string userStatus = (string)System.Web.HttpContext.Current.Session["LoginStatus"];
            if (userStatus != "login")
            {
                TempData["UserLevel"] = null;
            }
            //string email = (string)System.Web.HttpContext.Current.Session["Email"];

            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Products()
        {
            return View();
        }
        public ActionResult SingleProduct()
        {
            return View();
        }

    }
}