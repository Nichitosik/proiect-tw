using Proiect_TW.Extension;
using System.Collections.Generic;
using System.Web.Mvc;
using Proiect_TW.Web.Controllers;
using Proiect_TW.BussinesLogic.DBModel.Seed;
using Proiect_TW.Domain.Entities.User;
using System.Linq;
using Proiect_TW.BusinessLogic;
using Proiect_TW.BusinessLogic.Interfaces;
using Proiect_TW.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Proiect_TW.Web.Models.Users;
using Proiect_TW.Domain.Entities.Users;
using Microsoft.Ajax.Utilities;
using System.Web.UI.WebControls;

namespace Proiect_TW.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ISession _session;
        public HomeController()
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
        // GET: Home
        public ActionResult Index(string button)
        {
            GetUser();
            if(ViewBag.User != null && button != null)
            {
                ULoginResp loginResp = _session.UserLogout(ViewBag.User.Email);
                if (loginResp.Status)
                {
                    if (button == "Logout")
                    {
                        return RedirectToAction("Login", "Login");
                    }
                    else if (button == "Register")
                    {
                        return RedirectToAction("Register", "Login");
                    }
                    else if (button == "RecoverPassword")
                    {
                        return RedirectToAction("RecoverPassword", "Login");
                    }
                }
            } 
            return View();
        }
        public ActionResult About()
        {
            GetUser();
            return View();
        }
        public ActionResult Contact()
        {
            GetUser();
            return View();
        }
        public ActionResult SingleProduct()
        {
            GetUser();
            return View();
        }
        public ActionResult UserProfile()
        {
            GetUser();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserProfile(UProfileEdit edit)
        {
            GetUser();
            if (ModelState.IsValid)
            {
                var data = Mapper.Map<UProfileEditData>(edit);
                data.LastIp = Request.UserHostAddress;
                data.ExistingEmail = ViewBag.User.Email;

                UProfileEditResp editResp = _session.UserProfileEdit(data);

                if (editResp.Status)
                {
                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    ModelState.AddModelError("", editResp.StatusMsg);
                    return View();
                }
            }
                return View();
        }
    }
}