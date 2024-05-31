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
using System.Diagnostics;
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
                        }
                    }
                }
                using (var db = new ShoppingCartContext())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        var images = db.ShoppingCart.Where(e => e.ProductTitle == ProductTitle).ToList();
                        if (images != null)
                        {
                            foreach (var image in images)
                            {
                                db.ShoppingCart.Remove(image);
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
        public Statistics GetAllStaistics()
        {
            Statistics statistics = new Statistics();

            var users = new List<UDbTable>();
            int usersToday = 0;

            using (var db = new UserContext())
            {
                users = db.Users.ToList();
            }
            foreach(var user in users)
            {
                DateTime dateTime1 = DateTime.Now;
                TimeSpan difference = dateTime1 - user.RegisterTime;
                if (difference.TotalDays <= 1)
                {
                    usersToday++;
                }
            }
            using (var db = new OrderContext())
            {
                var orders = db.Orders.ToList();

                double TotalIncome = 0;
                int TotalSoldItems = 0;
                double AveragePrice = 0;

                double IncomeToday = 0;
                int SoldItemsToday = 0;
                double AveragePriceToday = 0;

                List<double> IncomeDaily = Enumerable.Repeat(0.0, 30).ToList();

                double MensIncome = 0;
                double WomensIncome = 0;
                double KidsIncome = 0;

                double MensSoldPercentage = 0;
                double WomensSoldPercentage = 0;
                double KidsSoldPercentage = 0;

                int MensSoldItems = 0;
                int WomensSoldItems = 0;
                int KidsSoldItems = 0;


                List<ProductStatistics> TopSoldItems = new List<ProductStatistics>();

                foreach (var order in orders)
                {
                    DateTime dateTime1 = DateTime.Now;
                    TimeSpan difference = dateTime1 - order.PublishTime;
                    if (difference.TotalDays <= 1)
                    {
                        IncomeToday += order.TotalPrice;
                    }
                    for(int i = 1; i <=30; i++)
                    {
                        if (difference.TotalDays <= i)
                        {
                            IncomeDaily[i - 1] += order.TotalPrice;
                            break;
                        }
                    }
                    
                    
                    TotalIncome += order.TotalPrice;

                    using (var db_OrderProducts = new OrderProductsContext())
                    {
                        var orderProducts = db_OrderProducts.OrderProducts.Where(p => p.OrderId == order.Id).ToList();
                        var top10MostSoldItems = db_OrderProducts.OrderProducts
                                    .OrderByDescending(p => p.Count)
                                    .Take(10)
                                    .ToList();
                        foreach(var product in top10MostSoldItems)
                        {
                            ProductStatistics topProduct = new ProductStatistics()
                            {
                                ProductId = product.Id,
                                ProductName = product.Title,
                                Price = product.ItemPrice,
                                Quantity = product.Count,
                                Amount = product.ItemPrice * product.Count
                            };

                            TopSoldItems.Add(topProduct);
                        }
                        foreach (var product in orderProducts)
                        {
                            using (var db_Products = new ProductContext())
                            {
                                var productType = db_Products.Products
                                                             .Where(p => p.Title == product.Title)
                                                             .Select(p => p.Type)
                                                             .FirstOrDefault();
                                switch (productType)
                                {
                                    case "Men's":
                                        {
                                            MensIncome += product.ItemPrice * product.Count;
                                            MensSoldItems++;
                                            break;
                                        }
                                    case "Women's":
                                        {
                                            WomensIncome += product.ItemPrice * product.Count;
                                            WomensSoldItems++;
                                            break;
                                        }
                                    case "Kid's":
                                        {
                                            KidsIncome += product.ItemPrice * product.Count;
                                            KidsSoldItems++;
                                            break;
                                        }
                                }

                            }

                            if (difference.TotalHours <= 24)
                            {
                                SoldItemsToday += product.Count;
                            }
                            TotalSoldItems += product.Count;
                        }
                    }
                }
                AveragePrice = (double)TotalIncome / TotalSoldItems;
                AveragePriceToday = (double)IncomeToday / SoldItemsToday;
                int totalSoldItems = MensSoldItems + WomensSoldItems + KidsSoldItems;

                if (totalSoldItems != 0)
                {
                    MensSoldPercentage = (double)MensSoldItems / totalSoldItems * 100;
                    WomensSoldPercentage = (double)WomensSoldItems / totalSoldItems * 100;
                    KidsSoldPercentage = (double)KidsSoldItems / totalSoldItems * 100;
                }


                statistics.AveragePrice = AveragePrice;
                statistics.AveragePriceToday = AveragePriceToday;
                statistics.IncomeToday = IncomeToday;
                statistics.SoldItemsToday = SoldItemsToday;
                statistics.NewUsersToday = usersToday;
                statistics.TotalIncome = TotalIncome;
                statistics.TotalSoldItems = TotalSoldItems;
                statistics.TotalUsers = users.Count;
                statistics.IncomeDaily = IncomeDaily;
                statistics.MensIncome = MensIncome;
                statistics.WomensIncome = WomensIncome;
                statistics.KidsIncome = KidsIncome;
                statistics.MensSoldPercentage = MensSoldPercentage;
                statistics.WomensSoldPercentage = WomensSoldPercentage;
                statistics.KidsSoldPercentage = KidsSoldPercentage;
                statistics.TopSalesProducts = TopSoldItems;


            }

            return statistics;
        }
    }
}

