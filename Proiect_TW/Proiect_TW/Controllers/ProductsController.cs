using Proiect_TW.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using Proiect_TW.BusinessLogic.Interfaces;
using System.Web;
using System.Web.Mvc;
using Proiect_TW.Domain.Entities.User;
using Proiect_TW.Domain.Entities.Users;
using Proiect_TW.BusinessLogic.Entities.User;
using System.Web.UI.WebControls;
using Proiect_TW.Web.Models.Users;
using AutoMapper;
using Microsoft.Win32;
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
            List<ProductWithPath> products = new List<ProductWithPath>();
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
                case "AllProducts":
                    products = _session.GetAllProducts();
                    break;
            }
            ViewBag.Products = products;
            return View();
        }
        public ActionResult SingleProduct(string button)
        {
            var products = _session.GetAllProducts(); 
            foreach (ProductWithPath product in products)
            {
                if(product.Title == button)
                {
                    ViewBag.SingleProduct = product;
                }
            }
            GetUser();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SingleProduct(AddProductToShoppingCart data)
        {
            GetUser();
            var products = _session.GetAllProducts();
            foreach (ProductWithPath product in products)
            {
                if (product.Title == data.Title)
                {
                    ViewBag.SingleProduct = product;
                }
            }
            if (ModelState.IsValid)
            {
                ShopCProductData productData = new ShopCProductData
                {
                    UserEmail = ViewBag.User.Email,
                    Count = Convert.ToInt32(data.Count),
                    Size = data.Size,
                    ProductTitle = data.Title,
                    Ip = Request.UserHostAddress,
                    DateTime = DateTime.Now
                };


                ULoginResp loginResp = _session.UserAddProductShopC(productData);

                if (loginResp.Status)
                {

                    return RedirectToAction("ShoppingCart", "Products");
                }
                else
                {
                    ModelState.AddModelError("", loginResp.StatusMsg);
                    return View();
                }
            }
            return View();
        }
        public ActionResult ShoppingCart()
        {
            GetUser();
            var shoppingCartProducts = _session.GetAllShoppingCartProducts(ViewBag.User.Email);
            int totalPrice = 0;
            foreach(ShoppingCartProduct product in shoppingCartProducts)
            {
                totalPrice += int.Parse(product.Price);
            }
            ViewBag.ShoppingCartProducts = shoppingCartProducts;
            ViewBag.TotalPrice = totalPrice;

            return View();
        }
        public ActionResult Order()
        {
            GetUser();
            var shoppingCartProducts = _session.GetAllShoppingCartProducts(ViewBag.User.Email);
            int totalPrice = 0;
            foreach (ShoppingCartProduct product in shoppingCartProducts)
            {
                totalPrice += int.Parse(product.Price);
            }
            ViewBag.OrderProducts = shoppingCartProducts;
            ViewBag.TotalPrice = totalPrice;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Order(AddOrder data)
        {
            GetUser();
            if (ModelState.IsValid)
            {
                var orderData = Mapper.Map<OrderData>(data);
                orderData.Email = ViewBag.User.Email;
                orderData.Ip = Request.UserHostAddress;


                ULoginResp orderResp = _session.UserOrder(orderData);

                if (orderResp.Status)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", orderResp.StatusMsg);
                    return View();
                }
            }
            return View();
        }
    }

}