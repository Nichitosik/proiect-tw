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

namespace Proiect_TW.Controllers
{
    public class AdminController : BaseController
    {
        private readonly ISessionAdmin _session;
        public AdminController()
        {
            var bl = new BussinessLogic();
            _session = bl.GetSessionAdmin();
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
        public ActionResult Users()
        {
            UsersResp users = _session.GetUsers();
            ViewBag.AllUsers = users;
            GetUser();
            return View();
        }
        public ActionResult Products()
        {
            GetUser();
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

                ProductResp productResp = _session.AddProduct(productData);

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
                    _session.AddProductImages(imagesData);


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