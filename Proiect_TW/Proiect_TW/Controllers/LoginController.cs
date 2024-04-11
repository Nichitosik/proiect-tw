﻿using Proiect_TW.BusinessLogic;
using Proiect_TW.BusinessLogic.Entities.User;
using Proiect_TW.BusinessLogic.Interfaces;
using Proiect_TW.Domain.Entities.User;
using Proiect_TW.Web.Models.Users;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AutoMapper;
using System.Web.Helpers;


namespace Proiect_TW.Controllers
{
    public class LoginController : Controller
    {

        private readonly ISession _session;
        // GET: Register
        public LoginController()
        {
            var bl = new BussinessLogic();
            _session = bl.GetSessionBL();
        }
        public ActionResult Login()
        {
            return View();
        }

        // GET : Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login)
        {
            if (ModelState.IsValid)
            {
                ULoginData uData = new ULoginData
                {
                    Email = login.Email,
                    Password = login.Password,
                    LoginIp = Request.UserHostAddress,
                    LoginDateTime = DateTime.Now

                };



               ULoginResp loginResp = _session.UserLogin(uData);

                if (loginResp.Status)
                {
                    HttpCookie cookie = _session.GenCookie(login.Email);
                    ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("", loginResp.StatusMsg);
                    return View();
                }
            }
            return View();
        }



    }
}