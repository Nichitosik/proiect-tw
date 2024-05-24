using Proiect_TW.Atributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proiect_TW.BusinessLogic.Interfaces;
using Proiect_TW.BusinessLogic;
using Proiect_TW.Web.Controllers;
using Proiect_TW.BussinesLogic.Interfaces;
using Proiect_TW.Domain.Entities.User;
using Proiect_TW.Web.Models.Users;
using AutoMapper;
using Microsoft.Win32;
using Proiect_TW.Domain.Entities.Users;
using System.IO;
using System.Web.UI.WebControls;
using Microsoft.Web.XmlTransform;
using System.ServiceModel.PeerResolvers;
using System.Net.Http.Headers;
using System.Data.SqlTypes;
using Proiect_TW.BussinesLogic;

namespace Proiect_TW.Controllers
{
    public class AdminController : BaseController
    {
        private readonly ISessionAdmin _sessionAdmin;
        private readonly ISession _session;
        public AdminController()
        {
            var blAdmin = new BussinessLogic();
            var bl = new BussinessLogic();
            _sessionAdmin = blAdmin.GetSessionAdmin();
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
        // GET: Admin

        [AdminMod]

        public ActionResult AddProduct()
        {
            GetUser();
            return View();
        }
        public ActionResult Statistics()
        {
            GetUser();
            return View();
        }
        public ActionResult Users(string button)
        {
            GetUser();
            ULoginResp responce = new ULoginResp();
            if(button != null)
            {
                responce = _sessionAdmin.DeleteUser(button);
            }
            if(responce != null)
            {
                UsersResp users = _sessionAdmin.GetUsers();
                ViewBag.AllUsers = users;
                return View();
            }
            return View();
        }
        public void GetProducts()
        {
            GetUser();
            List<ProductWithPath> productWithPaths = new List<ProductWithPath>();
            productWithPaths = _session.GetAllProducts();
            ViewBag.AllProducts = productWithPaths;
        }
        public void GetUsersFeedback()
        {
            GetUser();
            List<Feedback> usersFeedback = new List<Feedback>();
            usersFeedback = _sessionAdmin.GetUsersFeedback();
            ViewBag.AllUsersFeedback = usersFeedback;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Products(SearchSort option)
        {
            GetProducts();
            if (option.ProductTitle != null)
            {
                List<ProductWithPath> products = new List<ProductWithPath>();
                foreach(ProductWithPath product in ViewBag.AllProducts)
                {
                    if (product.Title.Length >= option.ProductTitle.Length && product.Title.IndexOf(option.ProductTitle, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        products.Add(product);
                    }

                }
                ViewBag.AllProducts = products;
            }
            if (option.SortOption != "All")
            {
                switch (option.SortOption)
                {
                    case "Price":
                        ViewBag.AllProducts = ((List<ProductWithPath>)ViewBag.AllProducts).OrderBy(p => p.Price).ToList();
                        break;
                    case "Sold Items":
                        ViewBag.AllProducts = ((List<ProductWithPath>)ViewBag.AllProducts).OrderBy(p => p.SoldItems).ToList();
                        break;
                    case "Sales Income":
                        ViewBag.AllProducts = ((List<ProductWithPath>)ViewBag.AllProducts).OrderBy(p => p.SalesIncome).ToList();
                        break;
                    case "Publish Time":
                        ViewBag.AllProducts = ((List<ProductWithPath>)ViewBag.AllProducts).OrderBy(p => p.PublishTime).ToList();
                        break;
                }
            }

            return View();
        }
        public ActionResult Products(string button)
        {
            GetUser();
            ULoginResp responce = new ULoginResp();
            if (button != null)
            {
                responce = _sessionAdmin.DeleteProduct(button);
            }
            if (responce != null)
            {
                GetProducts();
                return View();
            }
            return View();
        }
        public ActionResult UsersFeedback()
        {
            GetUsersFeedback();
            return View();
        }

        // GET : Product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct(AddProduct product, HttpPostedFileBase[] ImageFiles)
        {
            bool fileValidation = true;
            foreach(HttpPostedFileBase File in ImageFiles)
            {
                if (File == null || File.ContentLength <= 0)
                {
                    fileValidation = false;
                }
            }
            if (ModelState.IsValid && fileValidation == true)
            {
                var productData = Mapper.Map<ProductData>(product);
                productData.Ip = Request.UserHostAddress;
                productData.PublishTime = DateTime.Now;

                ProductResp productResp = _sessionAdmin.AddProduct(productData);

                if (productResp.Status)
                {
                    string directoryPath = Server.MapPath("~/assets/ProductsImages/" + product.Title);

                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    List<string> filePaths = new List<string>();
                    List<string> fileNames = new List<string>();
                    string fileName;
                    string filePath;
                    foreach (HttpPostedFileBase File in ImageFiles)
                    {
                        fileName = Path.GetFileName(File.FileName);
                        filePath = (Server.MapPath(@"~/assets/ProductsImages/"+ product.Title + "/" + fileName));

                        fileNames.Add(fileName);
                        filePaths.Add(product.Title + "/" + fileName);

                        File.SaveAs(Server.MapPath(@"~/assets/ProductsImages/" + product.Title + "/" + fileName));
                    }
                    var imagesData = new ProductImagesData()
                    {
                        ProductTitle = product.Title,
                        ImageNames = fileNames,
                        ImagePaths = filePaths
                    };
                    _sessionAdmin.AddProductImages(imagesData);


                    return RedirectToAction("Index", "Home");
                    // Salvează fișierul pe server
                }
                else
                {
                    ModelState.AddModelError("", productResp.StatusMsg);
                    return View();
                }
            }
            return View();
        }
    }
}