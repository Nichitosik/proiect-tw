using AutoMapper;
using Microsoft.Build.Evaluation;
using Proiect_TW.BussinesLogic.DBModel.Seed;
using Proiect_TW.Domain.Entities.User;
using Proiect_TW.Domain.Entities.Users;
using Proiect_TW.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;



namespace Proiect_TW.BussinesLogic.Core
{
    public class AdminAPI
    {

        internal ProductResp AddProductAction(ProductData data)
        {
            Product product;
            if (data.Title != null && data.Description != null && data.Style != null && data.Type != null && data.Sizes != null)
            {
                List<bool> sizesList = new List<bool>(6) { false, false, false, false, false, false };

                foreach (string size in data.Sizes)
                {
                    switch (size)
                    {
                        case "XS":
                            sizesList[0] = true;
                            break;
                        case "S":
                            sizesList[1] = true;
                            break;
                        case "M":
                            sizesList[2] = true;
                            break;
                        case "L":
                            sizesList[3] = true;
                            break;
                        case "XL":
                            sizesList[4] = true;
                            break;
                        case "XXL":
                            sizesList[5] = true;
                            break;
                        default:
                            break;
                    }
                }
                using (var db = new ProductContext())
                {
                    product = db.Products.FirstOrDefault(u => u.Title == data.Description);
                }
                if (product != null)
                {
                    return new ProductResp { Status = false, StatusMsg = "This product already exists" };
                }
                var newProduct = new Product()
                {
                    Title = data.Title,
                    Description = data.Description,
                    Type = data.Type,
                    Style = data.Style,
                    XS = sizesList[0],
                    S = sizesList[1],
                    M = sizesList[2],
                    L = sizesList[3],
                    XL = sizesList[4],
                    XXL = sizesList[5],
                    Gender = data.Gender,
                    AgeCategory = data.AgeCategory,
                    Price = data.Price,
                    PublishTime = DateTime.Now,
                    Ip = data.Ip
                };
                using (var todo = new ProductContext())
                {
                    todo.Products.Add(newProduct);
                    todo.SaveChanges();
                }
                return new ProductResp { Status = true };

            }
            else
                return new ProductResp { Status = false, StatusMsg = "Please enter all information" };
        }
        internal ProductImagesResp AddProductImagesAction(ProductImagesData data)
        {
            if (data.ProductTitle != null && data.ImagePaths.Count > 0 && data.ImageNames.Count > 0 && data.ImagePaths.Count == data.ImageNames.Count)
            {
                for(int i = 0; i < data.ImagePaths.Count; i++)
                {
                    var newProductImage = new ProductImages()
                    {
                        ProductTitle = data.ProductTitle,
                        ImageName = data.ImageNames[i],
                        ImagePath = data.ImagePaths[i],
                        PublishTime = DateTime.Now
                    };
                    using (var todo = new ProductImagesContext())
                    {
                        todo.ProductImages.Add(newProductImage);
                        todo.SaveChanges();
                    }
                }
                return new ProductImagesResp { Status = true };

            }
            else
            {
                return new ProductImagesResp { Status = false, StatusMsg = "Please enter all information" };
            }
        }
        internal UsersResp GetUsersAction()
        {
            var users = new List<UDbTable>();
            int newUsers = 0;
            int onlineUsers = 0;
            using (var db = new UserContext())
            {
                users = db.Users.ToList();
            }
            foreach(UDbTable user in users)
            {
                DateTime dateTime1 = DateTime.Now;
                TimeSpan difference = dateTime1 - user.RegisterTime;
                if(difference.TotalHours <= 24)
                {
                    newUsers++;
                }
                if(user.IsOnline == true)
                {
                    onlineUsers++;
                }
            }
            return new UsersResp { Users = users, TotalUsers = users.Count, NewUsers = newUsers, OnlineUsers = onlineUsers };

        }
        internal List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            using (var db = new ProductContext())
            {
                products = db.Products.ToList();
            }
            return products;
        }
        public List<Feedback> GetFeedback()
        {
            List<Feedback> usersFeedback = new List<Feedback>();
            using (var db = new FeedbackContext())
            {
                usersFeedback = db.Feedback.ToList();
            }
            return usersFeedback;
        }
        public ULoginResp DelUser(string UserEmail)
        {
            using (var db = new UserContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    var user = db.Users.FirstOrDefault(e => e.Email == UserEmail);
                    if (user != null)
                    {
                        db.Users.Remove(user);
                        db.SaveChanges();
                        transaction.Commit();
                        return new ULoginResp { Status = true, StatusMsg = "User deleted succesfull" };
                    }
                }
            }
            return new ULoginResp { Status = false, StatusMsg = "Sometrhing get wrong" };
        }
        public ULoginResp DelProduct(string ProductTitle)
        {
            if(ProductTitle != null)
            {
                using (var db = new ProductContext())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        var product = db.Products.FirstOrDefault(e => e.Title == ProductTitle);
                        if (product != null)
                        {
                            string directoryPath = HttpContext.Current.Server.MapPath("~/assets/ProductsImages/" + ProductTitle);
                            if (Directory.Exists(directoryPath))
                            {
                                Directory.Delete(directoryPath, true); // true to delete subdirectories and files
                            }
                            db.Products.Remove(product);
                            db.SaveChanges();
                            transaction.Commit();
                        }
                    }
                }
                using (var db = new ProductImagesContext())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        var images = db.ProductImages.Where(e => e.ProductTitle == ProductTitle).ToList();
                        if (images != null)
                        {
                            foreach(var image in images) {
                                db.ProductImages.Remove(image);
                            }
                            db.SaveChanges();
                            transaction.Commit();
                            return new ULoginResp { Status = true, StatusMsg = "Product succesfull deleted" };
                        }
                    }
                }
            }
            
            return new ULoginResp { Status = false, StatusMsg = "Sometrhing get wrong" };
        }
    }
}

