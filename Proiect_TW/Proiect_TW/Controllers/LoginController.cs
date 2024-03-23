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
        // GET : Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin data)
        {
            if (ModelState.IsValid)
            {
                ULoginData uData = new ULoginData
                {
                    Credential = data.Credential,
                    Password = data.Password,
                    LoginIp = Request.UserHostAddress,
                    LoginDateTime = DateTime.Now
                };
                //ULoginResp resp = _session.UserLogin(uData);

                //if (resp.Status)
                //{
                //    //ADD COOKIE

                //    return RedirectToAction("Index", "Home");
                //}
                //else
                //{
                //    ModelState.AddModelError("", resp.StatusMsg);
                //    return View();
                //}

                if (uData.Credential == "username" && uData.Password == "password")
                {
                    return RedirectToAction("Index", "Home");

                }
            }
            return View();
        }

        // GET: Login

        public ActionResult LoginIndex()
        {
            return View();
        }
        //O sa adaug mai tarziu
        //public ActionResult LoginError()
        //{
        //    return View();
        //}

    }
}