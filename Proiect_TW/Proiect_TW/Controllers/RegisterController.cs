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

namespace Proiect_TW.Web.Controllers
{
    public class RegisterController : Controller
    {

        private readonly ISession _session;
        // GET: Register
        public RegisterController()
        {
            var bl = new BussinessLogic();
            _session = bl.GetSessionBL();
        }
        public ActionResult Register()
        {
            return View();
        }
        // GET: SignIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserRegister register)
        {
            if (ModelState.IsValid)
            {
                URegisterData uData = new URegisterData
                {
                    Username = register.Username,
                    Password = register.Password,
                    Email = register.Email
                };



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

    }
}