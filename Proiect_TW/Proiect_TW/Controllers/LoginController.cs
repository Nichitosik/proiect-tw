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


namespace Proiect_TW.Controllers
{
    public class LoginController : Controller
    {

        private readonly ISession _session;
        // GET: Register
        public LoginController()
        {
            var bl = new BusinesLogic();
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
                var data = Mapper.Map<ULoginData>(login);

                data.LoginIp = Request.UserHostAddress;
                data.LoginDateTime = DateTime.Now;

               var userLogin = _session.UserLogin(data);

                if (userLogin.Status)
                {
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("", userLogin.StatusMsg);
                    return View();
                }
            }
            return View();
        }

        // GET: Login


        //O sa adaug mai tarziu
        //public ActionResult LoginError()
        //{
        //    return View();
        //}

    }
}