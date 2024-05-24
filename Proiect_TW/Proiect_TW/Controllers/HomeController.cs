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
using Microsoft.Win32;
using Microsoft.Build.Evaluation;
using System;

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UserFeedback feedback)
        {
            GetUser();
            if(feedback.Description == null)
            {
                ModelState.AddModelError("", "Please enter a description");
                return View();
            }
            if (ModelState.IsValid)
            {
                if(ViewBag.User != null)
                {
                    UFeedbackData feedbackData = new UFeedbackData();
                    feedbackData.Description = feedback.Description;
                    feedbackData.Email = ViewBag.User.Email;
                    feedbackData.Ip = Request.UserHostAddress;
                    feedbackData.PublishTime = DateTime.Now;

                    UFeedbackResp feedbackResp = _session.UserFeedback(feedbackData);

                    if (feedbackResp.Status)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", feedbackResp.StatusMsg);
                        return RedirectToAction("Login", "Login");
                    }
                }
                return RedirectToAction("Login", "Login");
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