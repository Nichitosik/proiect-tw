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
using System.Web.DynamicData;

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Users(SearchSort option)
        {
            GetUser();
            UsersResp allusers = _sessionAdmin.GetUsers();
            UsersResp users = new UsersResp
            {
                Users = new List<UDbTable>(),
                TotalUsers = 0
            }; 
            if (option.Email != null)
            {
                foreach (UDbTable user in allusers.Users)
                {
                    if (user.Email.Length >= option.Email.Length && user.Email.IndexOf(option.Email, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        users.Users.Add(user);
                        users.TotalUsers++;
                        if(user.IsOnline == true)
                        {
                            users.OnlineUsers++;
                        }
                        DateTime currentTime = DateTime.Now;
                        TimeSpan timeDifference = currentTime - user.RegisterTime;
                        if (timeDifference.TotalHours < 24)
                        {
                            users.NewUsers++;
                        }
                    }
                }
                ViewBag.AllUsers = users;
            }
            else
            {
                users = allusers;
                users.TotalUsers = allusers.TotalUsers;
                users.OnlineUsers = allusers.OnlineUsers;
                users.NewUsers = allusers.NewUsers;
            }
            if (option.SortOption != "All")
            {
                switch (option.SortOption)
                {
                    case "New":
                        {
                            users.Users = ((UsersResp)users).Users.OrderByDescending(p => p.RegisterTime).ToList();
                            break;
                        }
                    case "Age":
                        {
                            users.Users = ((UsersResp)users).Users.OrderBy(p => p.Age).ToList();
                            break;
                        }
                    case "Gender":
                        {
                            users.Users = ((UsersResp)users).Users.OrderBy(p => p.Gender).ToList();
                            break;
                        }
                    case "Username":
                        {
                            users.Users = ((UsersResp)users).Users.OrderBy(p => p.Username).ToList();
                            break;
                        }
                }
            }
            ViewBag.AllUsers = users;

            return View();
        }
        public ActionResult Users(string email)
        {

            GetUser();
            ULoginResp responce = new ULoginResp();
            if (email != null)
            {
                responce = _sessionAdmin.DeleteUser(email);
                if (responce.Status == true)
                {
                    UsersResp users = _sessionAdmin.GetUsers();
                    ViewBag.AllUsers = users;
                    return View();
                }
            }
            else
            {
                UsersResp users = _sessionAdmin.GetUsers();
                ViewBag.AllUsers = users;
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

            int NewFeddbacks = 0;
            foreach(var feedback in usersFeedback)
            {
                DateTime currentTime = DateTime.Now;
                TimeSpan timeDifference = currentTime - feedback.PublishTime;
                if (timeDifference.TotalHours < 24)
                {
                    NewFeddbacks++;
                }
            }
            
            ViewBag.AllFeedbacks = usersFeedback;
            ViewBag.NewFeedbacks = NewFeddbacks;
            ViewBag.TotalFeedbacks = usersFeedback.Count();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Products(SearchSort option)
        {
            GetUser();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UsersFeedback(SearchSort option)
        {
            GetUser();
            var usersFeedback = _sessionAdmin.GetUsersFeedback();
            int NewFeddbacks = 0;
            List<Feedback> selectedFeedbacks = new List<Feedback>();

            if (option.Email != null)
            {
                foreach (Feedback feedback in usersFeedback)
                {
                    if (feedback.Email.Length >= option.Email.Length && feedback.Email.IndexOf(option.Email, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        selectedFeedbacks.Add(feedback);
                        DateTime currentTime = DateTime.Now;
                        TimeSpan timeDifference = currentTime - feedback.PublishTime;
                        if (timeDifference.TotalHours < 24)
                        {
                            NewFeddbacks++;
                        }
                    }
                }
                ViewBag.AllFeedbacks = selectedFeedbacks;
                ViewBag.NewFeedbacks = NewFeddbacks;
                ViewBag.TotalFeedbacks = selectedFeedbacks.Count();
            }
            else
            {
                foreach (var feedback in usersFeedback)
                {
                    DateTime currentTime = DateTime.Now;
                    TimeSpan timeDifference = currentTime - feedback.PublishTime;
                    if (timeDifference.TotalHours < 24)
                    {
                        NewFeddbacks++;
                    }
                }
                selectedFeedbacks = usersFeedback;
                ViewBag.AllFeedbacks = selectedFeedbacks;
                ViewBag.NewFeedbacks = NewFeddbacks;
                ViewBag.TotalFeedbacks = usersFeedback.Count();
            }
            if (option.SortOption != "All")
            {
                switch (option.SortOption)
                {
                    case "Id":
                        {
                            selectedFeedbacks = ((List<Feedback>)selectedFeedbacks).OrderBy(p => p.Id).ToList();
                            break;
                        }
                    case "Email":
                        {
                            selectedFeedbacks = ((List<Feedback>)selectedFeedbacks).OrderBy(p => p.Email).ToList();
                            break;
                        }
                    case "New":
                        {
                            selectedFeedbacks = ((List<Feedback>)selectedFeedbacks).OrderByDescending(p => p.PublishTime).ToList();
                            break;
                        }
                }
                ViewBag.AllFeedbacks = selectedFeedbacks;
            }

            return View();
        }
        public ActionResult Orders()
        {
            GetUser();
            var orders = _session.GetOrdersByEmail(null);
            int NewOrders = 0;
            foreach(var order in orders)
            {
                DateTime currentTime = DateTime.Now;
                TimeSpan timeDifference = currentTime - order.PublishTime;

                if (timeDifference.TotalHours < 24)
                {
                    NewOrders++;
                }
            }
            ViewBag.AllOrders = orders;
            ViewBag.NewOrders = NewOrders;
            ViewBag.TotalOrders = orders.Count;
          

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Orders(SearchSort option)
        {
            GetUser();
            var orders = _session.GetOrdersByEmail(null);
            int NewOrders = 0;
            List<OrderWithProducts> selectedOrders = new List<OrderWithProducts>();

            if (option.Email != null)
            {
                foreach (OrderWithProducts order in orders)
                {
                    if (order.Email.Length >= option.Email.Length && order.Email.IndexOf(option.Email, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        selectedOrders.Add(order);
                        DateTime currentTime = DateTime.Now;
                        TimeSpan timeDifference = currentTime - order.PublishTime;
                        if (timeDifference.TotalHours < 24)
                        {
                            NewOrders++;
                        }
                    }
                }
                ViewBag.AllOrders = selectedOrders;
                ViewBag.NewOrders = NewOrders;
                ViewBag.TotalOrders = selectedOrders.Count();
            }
            else
            {
                foreach (var order in orders)
                {
                    DateTime currentTime = DateTime.Now;
                    TimeSpan timeDifference = currentTime - order.PublishTime;
                    if (timeDifference.TotalHours < 24)
                    {
                        NewOrders++;
                    }
                }
                ViewBag.NewOrders = NewOrders;
                ViewBag.TotalOrders = orders.Count();
            }
            if (option.SortOption != "All")
            {
                switch (option.SortOption)
                {
                    case "Total Price":
                        {
                            selectedOrders = ((List<OrderWithProducts>)selectedOrders).OrderBy(p => p.TotalPrice).ToList();
                            break;
                        }
                    case "Id":
                        {
                            selectedOrders = ((List<OrderWithProducts>)selectedOrders).OrderBy(p => p.Id).ToList();
                            break;
                        }
                    case "Name and Surname":
                        {
                            selectedOrders = ((List<OrderWithProducts>)selectedOrders).OrderBy(p => p.NameSurname).ToList();
                            break;
                        }
                    case "Username":
                        {
                            selectedOrders = ((List<OrderWithProducts>)selectedOrders).OrderBy(p => p.TotalPrice).ToList();
                            break;
                        }
                    case "New":
                        {
                            selectedOrders = ((List<OrderWithProducts>)selectedOrders).OrderByDescending(p => p.PublishTime).ToList();
                            break;
                        }
                }
                ViewBag.AllOrders = selectedOrders;
            }

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