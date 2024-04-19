﻿using Proiect_TW.Extension;
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
        public ActionResult Index()
        {
            GetUser();
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
        public ActionResult Products()
        {
            GetUser();
            return View();
        }
        public ActionResult SingleProduct()
        {
            GetUser();
            return View();
        }

    }
}