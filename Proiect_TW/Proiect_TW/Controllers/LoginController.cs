using Proiect_TW.BusinessLogic;
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
using Proiect_TW.Web.Controllers;
using Proiect_TW.Domain.Enums;
using Proiect_TW.BussinesLogic.DBModel.Seed;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Common.CommandTrees;
using AutoMapper.Configuration.Conventions;


namespace Proiect_TW.Web.Controllers
{
    public class LoginController : BaseController
    {

        private readonly ISession _session;
        // GET: Register
        public LoginController()
        {
            var bl = new BussinessLogic();
            _session = bl.GetSessionBL();
        }
        public ActionResult Login(string button)
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult RecoverPassword()
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserRegister register)
        {
            if (ModelState.IsValid)
            {

                var uData = Mapper.Map<URegisterData>(register);

                URegisterResp registerResp = _session.UserRegister(uData);

                if (registerResp.Status)
                {
                    return RedirectToAction("Login", "Login");

                }
                else
                {
                    ModelState.AddModelError("", registerResp.StatusMsg);
                    return View();
                }
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecoverPassword(UserRecoverPassword data)
        {
            if (ModelState.IsValid)
            {
                URecoverPasswordData uData = new URecoverPasswordData
                {
                    Email = data.Email,
                    Username = data.Username,
                    Password = data.Password,
                    RepeatPassword = data.RepeatPassword,
                    RecoverPasswordIp = "192.23.4",
                    RecoverDateTime = DateTime.Now
                };
                URecoverPasswordResp recoverPasswordResp = _session.UserRecoverPassword(uData);
                if (recoverPasswordResp.Status)
                {
                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    ModelState.AddModelError("", recoverPasswordResp.StatusMsg);
                    return View();
                }
            }
            return View();
        }



    }
}