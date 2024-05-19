using Proiect_TW.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using Proiect_TW.BusinessLogic.Interfaces;
using System.Web;
using System.Web.Mvc;
using Proiect_TW.Domain.Entities.User;
using Proiect_TW.Domain.Entities.Users;
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
        public ActionResult Products(string button)
        {
            GetUser();
            List<Product> products = new List<Product>();
            List<List<string>> productImages = new List<List<string>>();
            switch (button)
            {
                case "Women":
                    products = _session.GetProductsForUser("Women's");
                    break;
                case "Men":
                    products = _session.GetProductsForUser("Men's");
                    break;
                case "Kids":
                    products = _session.GetProductsForUser("Kid's");
                    break;
                case "Accessories":
                    products = _session.GetProductsForUser("Accessories");
                    break;
                case "ForYou":
                    products = _session.GetProductsForYou(ViewBag.User.Gender, ViewBag.User.Age);
                    break;
            }
            productImages = _session.GetProductImages(products);
            ViewBag.Products = products;
            ViewBag.ProductImages = productImages;
            return View();
        }
    }
}