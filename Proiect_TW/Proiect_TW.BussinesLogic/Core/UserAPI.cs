using Proiect_TW.BusinessLogic.Entities.User;
using Proiect_TW.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Proiect_TW.BussinesLogic.DBModel.Seed;
using Proiect_TW.Domain.Enums;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Web;
using AutoMapper;
using Proiect_TW.Helpers;
using Proiect_TW.Domain.Entities.Users;
using System.Net.Http.Headers;
using NuGet.Protocol.Plugins;
using Microsoft.Build.Evaluation;


namespace Proiect_TW.BusinessLogic.Core
{
    public class UserAPI
    {
        internal ULoginResp UserLoginAction(ULoginData data)
        {
            UDbTable result;
            var validate = new EmailAddressAttribute();
            if (validate.IsValid(data.Email))
            {
                var pass = LoginHelper.HashGen(data.Password);
                using (var db = new UserContext())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            result = db.Users.FirstOrDefault(u => u.Email == data.Email && u.Password == pass);

                            if (result == null)
                            {
                                return new ULoginResp { Status = false, StatusMsg = "The Email or Password is Incorrect" };
                            }

                            result.Password = pass;
                            result.LastIp = data.LoginIp;
                            result.LastLogin = data.LoginDateTime;
                            result.IsOnline = true;
                            db.SaveChanges();
                            transaction.Commit();

                            return new ULoginResp { Status = true };
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();

                            return new ULoginResp { Status = false, StatusMsg = "An error occurred while updating the password." };
                        }
                    }
                }
            }
            else
                return new ULoginResp { Status = false, StatusMsg = "Incorrect Email or Password" };
        }
        internal ULoginResp UserLogoutAction(string email)
        {
            var validate = new EmailAddressAttribute();
            if (!validate.IsValid(email))
            {
                return new ULoginResp { Status = false, StatusMsg = "The provided email is not valid." };
            }

            using (var db = new SessionContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var sessionToRemove = db.Sessions.FirstOrDefault(e => e.Username == email);
                        if (sessionToRemove != null)
                        {
                            db.Sessions.Remove(sessionToRemove);
                            db.SaveChanges();
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new ULoginResp { Status = false, StatusMsg = "An error occurred while the logout process." };
                    }
                }
            }

            using (var db = new UserContext())
            {
                try
                {
                    var userToUpdate = db.Users.FirstOrDefault(u => u.Email == email);
                    if (userToUpdate != null)
                    {
                        userToUpdate.IsOnline = false;
                        db.SaveChanges();
                        return new ULoginResp { Status = true, StatusMsg = "Logout successful" };
                    }
                    else
                    {
                        return new ULoginResp { Status = false, StatusMsg = "The provided email does not exist." };
                    }
                }
                catch (Exception ex)
                {
                    return new ULoginResp { Status = false, StatusMsg = "An error occurred while the logout process." };
                }
            }
        }

        internal URegisterResp UserRegisterAction(URegisterData data)
        {
            UDbTable user;
            var validate = new EmailAddressAttribute();
            if (validate.IsValid(data.Email))
            {
                if (data.Age < 0 || data.Age > 120)
                {
                    return new URegisterResp { Status = false, StatusMsg = "The age is not valid" };
                }
                if(data.Password != data.RepeatPassword)
                {
                    return new URegisterResp { Status = false, StatusMsg = "The repeat password is not correct" };
                }
                var pass = LoginHelper.HashGen(data.Password);
                using (var db = new UserContext())
                {
                    user = db.Users.FirstOrDefault(u => u.Email == data.Email);
                }
                if (user != null)
                {
                    return new URegisterResp { Status = false, StatusMsg = "This user already exists" };
                }
                var newUser = new UDbTable()
                {
                    Email = data.Email,
                    Username = data.Username,
                    Password = pass,
                    Age = data.Age,
                    Gender = data.Gender,
                    LastIp = "19",
                    LastLogin = DateTime.Now,
                    Level = URole.User,
                    RegisterTime = DateTime.Now
                };
                using (var todo = new UserContext())
                {
                    todo.Users.Add(newUser);
                    todo.SaveChanges();
                }
                return new URegisterResp { Status = true };

            }
            else
            {
                return new URegisterResp { Status = false, StatusMsg = "Email is not valid" };
            }
        }
        internal URecoverPasswordResp UserRecoverPasswordAction(URecoverPasswordData data)
        {
            UDbTable user;
            var validate = new EmailAddressAttribute();
            if (validate.IsValid(data.Email))
            {
                var pass = LoginHelper.HashGen(data.Password);
                using (var db = new UserContext())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            user = db.Users.FirstOrDefault(u => u.Email == data.Email && u.Username == data.Username);

                            if (user == null)
                            {
                                return new URecoverPasswordResp { Status = false, StatusMsg = "Invalid Email or Username" };
                            }

                            if (data.Password == data.RepeatPassword)
                            {
                                user.Password = pass;
                                user.LastIp = data.RecoverPasswordIp;
                                user.LastLogin = data.RecoverDateTime;
                                db.SaveChanges();
                            }
                            transaction.Commit();

                            return new URecoverPasswordResp { Status = true };
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();

                            return new URecoverPasswordResp { Status = false, StatusMsg = "An error occurred while updating the password." };
                        }
                    }
                }
            }
            else
            {
                return new URecoverPasswordResp { Status = false, StatusMsg = "Email is not valid" };
            }
        }
        internal UProfileEditResp UserProfileEditAction(UProfileEditData data)
        {
            UDbTable result;
            var validate = new EmailAddressAttribute();
            if (validate.IsValid(data.Email))
            {
                using (var db = new UserContext())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            result = db.Users.FirstOrDefault(u => u.Email == data.ExistingEmail);

                            if (result == null)
                            {
                                return new UProfileEditResp { Status = false, StatusMsg = "An error occurred while updating the profile data." };
                            }
                            result.Email = data.Email;
                            result.Username = data.Username;
                            result.Age = data.Age;
                            result.Gender = data.Gender;
                            result.LastIp = data.LastIp;
                            db.SaveChanges();
                            transaction.Commit();

                            return new UProfileEditResp { Status = true };
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();

                            return new UProfileEditResp { Status = false, StatusMsg = "An error occurred while updating the password." };
                        }
                    }
                }
            }
            else
            {
                return new UProfileEditResp { Status = false, StatusMsg = "Please enter a valid email" };
            }
        }

        internal HttpCookie Cookie(string loginEmail)
        {
            var apiCookie = new HttpCookie("X-KEY")
            {
                Value = CookieGenerator.Create(loginEmail)
            };

            using (var db = new SessionContext())
            {
                Session curent;
                var validate = new EmailAddressAttribute();
                if (validate.IsValid(loginEmail))
                {
                    curent = (from e in db.Sessions where e.Username == loginEmail select e).FirstOrDefault();
                }
                else
                {
                    curent = (from e in db.Sessions where e.Username == loginEmail select e).FirstOrDefault();
                }

                if (curent != null)
                {
                    curent.CookieString = apiCookie.Value;
                    curent.ExpireTime = DateTime.Now.AddMinutes(60);
                    using (var todo = new SessionContext())
                    {
                        todo.Entry(curent).State = EntityState.Modified;
                        todo.SaveChanges();
                    }
                }
                else
                {
                    db.Sessions.Add(new Session
                    {
                        Username = loginEmail,
                        CookieString = apiCookie.Value,
                        ExpireTime = DateTime.Now.AddMinutes(60)
                    });
                    db.SaveChanges();
                    List<Session> sessiolist = new List<Session>();
                    using (var transaction = db.Database.BeginTransaction())
                    {
                            sessiolist = db.Sessions.ToList();
                            if (sessiolist != null)
                            {
                                foreach (Session session in sessiolist)
                                {
                                    DateTime currentTime = DateTime.Now;
                                    if (session.ExpireTime < currentTime) // Check if the session has expired
                                    {
                                        db.Sessions.Remove(session);
                                        using (var userdb = new UserContext())
                                        {
                                            var user = userdb.Users.Where(u => u.Email == session.Username).FirstOrDefault();
                                        if (user != null)
                                            {
                                                user.IsOnline = false;
                                                db.SaveChanges();
                                            }
                                        }
                                        
                                    }
                                }

                                db.SaveChanges();
                                transaction.Commit();
                            }
                    }
                }
            }

            return apiCookie;
        }
        public UserMinimal UserCookie(string cookie)
        {
            Session session;
            UDbTable curentUser;

            using (var db = new SessionContext())
            {
                session = db.Sessions.FirstOrDefault(s => s.CookieString == cookie && s.ExpireTime > DateTime.Now);
            }

            if (session == null) return null;
            using (var db = new UserContext())
            {
                var validate = new EmailAddressAttribute();
                if (validate.IsValid(session.Username))
                {
                    curentUser = db.Users.FirstOrDefault(u => u.Email == session.Username);
                }
                else
                {
                    curentUser = db.Users.FirstOrDefault(u => u.Username == session.Username);
                }
            }

            if (curentUser == null) return null;
            var userminimal = Mapper.Map<UserMinimal>(curentUser);

            return userminimal;
        }
        public List<ProductWithPath> GetProductsByType(string type)
        {
            List<Product> products = new List<Product>();
            List<ProductWithPath> productsWithPath = new List<ProductWithPath>();

            using (var db = new ProductContext())
            {
                products = db.Products.Where(p => p.Type == type).ToList();
            }
            foreach (Product product in products)
            {
                List<string> imagePaths = new List<string>();
                imagePaths = this.GetProductImagesPath(product);
                var productWithPath = Mapper.Map<ProductWithPath>(product);
                productWithPath.ImagesPath = imagePaths;
                productsWithPath.Add(productWithPath);
            }

            return productsWithPath;
        }
        public List<ProductWithPath> GetProductsByGender(string gender, int age)
        {
            string ageCategory = "";
            string productGender = "";

            switch (age)
            {
                case int n when (n >= 0 && n <= 4):
                    ageCategory = "0-4";
                    break;
                case int n when (n >= 5 && n <= 9):
                    ageCategory = "5-9";
                    break;
                case int n when (n >= 10 && n <= 14):
                    ageCategory = "10-14";
                    break;
                case int n when (n >= 15 && n <= 19):
                    ageCategory = "15-19";
                    break;
                case int n when (n >= 20 && n <= 29):
                    ageCategory = "20-29";
                    break;
                case int n when (n >= 30 && n <= 39):
                    ageCategory = "30-39";
                    break;
                case int n when (n >= 40 && n <= 49):
                    ageCategory = "40-49";
                    break;
                case int n when (n >= 50 && n <= 59):
                    ageCategory = "50-59";
                    break;
                case int n when (n >= 60 && n <= 69):
                    ageCategory = "60-69";
                    break;
                case int n when (n >= 70 && n <= 79):
                    ageCategory = "70-79";
                    break;
                case int n when (n >= 80 && n <= 120):
                    ageCategory = "80-120";
                    break;
            }
            switch (gender)
            {
                case "Male":
                    productGender = "Male";
                    break;
                case "Female":
                    productGender = "Female";
                    break;
            }

            List<Product> products = new List<Product>();
            List<ProductWithPath> productsWithPath = new List<ProductWithPath>();
            using (var db = new ProductContext())
            {
                products = db.Products.Where(p => p.AgeCategory == ageCategory && p.Gender == productGender).ToList();
            }

            foreach (Product product in products)
            {
                List<string> imagePaths = new List<string>();
                imagePaths = this.GetProductImagesPath(product);
                var productWithPath = Mapper.Map<ProductWithPath>(product);
                productWithPath.ImagesPath = imagePaths;
                productsWithPath.Add(productWithPath);
            }

            return productsWithPath;
        }
        public List<ProductWithPath> GetProducts()
        {
            List<Product> products = new List<Product>();
            List<ProductWithPath> productsWithPath = new List<ProductWithPath>();
            using (var db = new ProductContext())
            {
                products = db.Products.ToList();
            }
            for (int i = 0; i <products.Count; i++)
            {
                List<string> imagePaths = new List<string>();
                imagePaths = this.GetProductImagesPath(products[i]);
                var productWithPath = Mapper.Map<ProductWithPath>(products[i]);
                productWithPath.ImagesPath = imagePaths;
                if (products[i].XS == true)
                {
                    productWithPath.Sizes.Add("XS");
                } 
                if (products[i].S == true)
                {
                    productWithPath.Sizes.Add("S");
                }  
                if (products[i].M == true)
                {
                    productWithPath.Sizes.Add("M");
                }                
                if (products[i].L == true)
                {
                    productWithPath.Sizes.Add("L");
                }    
                if (products[i].XL == true)
                {
                    productWithPath.Sizes.Add("XL");
                }    
                if (products[i].XXL == true)
                {
                    productWithPath.Sizes.Add("XXL");
                }    
                if (products[i].XXL == true)
                {
                    productWithPath.Sizes.Add("XS");
                }
                productsWithPath.Add(productWithPath);
            }

            return productsWithPath;
        }
        public List<string> GetProductImagesPath(Product product)
        {
            List<string> imagesPath = new List<string>();
            List<ProductImages> images;

            using (var db = new ProductImagesContext())
            {
                images = db.ProductImages.Where(p => p.ProductTitle == product.Title).ToList();
            }

            foreach (var image in images)
            {
                imagesPath.Add(image.ImagePath);
            }
            return imagesPath;
        }
        public UFeedbackResp UserFeedbackAction(UFeedbackData data)
        {

            var validate = new EmailAddressAttribute();
            if (validate.IsValid(data.Email))
            {
                var newFeedbak = new Feedback()
                {
                    Email = data.Email,
                    Description = data.Description,
                    PublishTime = DateTime.Now,
                    Ip = data.Ip
                };
                using (var todo = new FeedbackContext())
                {
                    todo.Feedback.Add(newFeedbak);
                    todo.SaveChanges();
                }
                return new UFeedbackResp { Status = true };

            }
            else
            {
                return new UFeedbackResp { Status = false, StatusMsg = "Email is not valid" };
            }
        }
        public ULoginResp AddProductToSC(ShopCProductData data)
        {
            var validate = new EmailAddressAttribute();
            if (validate.IsValid(data.UserEmail))
            {
                ShoppingCart product;
                using (var db = new ShoppingCartContext())
                {
                    product = db.ShoppingCart.FirstOrDefault(p => p.ProductTitle == data.ProductTitle && p.UserEmail == data.UserEmail);
                }
                if (product == null)
                {
                    var shoppingCart = new ShoppingCart()
                    {
                        UserEmail = data.UserEmail,
                        ProductTitle = data.ProductTitle,
                        Size = data.Size,
                        Count = data.Count,
                        PublishTime = DateTime.Now,
                        Ip = data.Ip
                    };
                    using (var todo = new ShoppingCartContext())
                    {
                        todo.ShoppingCart.Add(shoppingCart);
                        todo.SaveChanges();
                    }
                }
                return new ULoginResp { Status = true };

            }
            return new ULoginResp { Status = false, StatusMsg = "An error ocured during adding the product in Shopping Cart" };
        }
        public List<ShoppingCartProduct> GetShoppingCartProducts(string UserEmail)
        {
            List<ShoppingCart> shoppingCartProduct = new List<ShoppingCart>();
            List<ShoppingCartProduct> result = new List<ShoppingCartProduct>();

            using (var db = new ShoppingCartContext())
            {
                shoppingCartProduct = db.ShoppingCart.Where(p => p.UserEmail == UserEmail).ToList();
            }

            foreach (var product in shoppingCartProduct)
            {
                Product prod = null;
                using (var db = new ProductContext())
                {
                    prod = db.Products.FirstOrDefault(p => p.Title == product.ProductTitle);
                }
                var imagesPath = this.GetProductImagesPath(prod);

                var resultProd = new ShoppingCartProduct()
                {
                Title = prod.Title,
                Description = prod.Description,
                Type = prod.Type,
                Style = prod.Style,
                Count = product.Count,
                Price = prod.Price,
                Size = product.Size,
                ImagesPath = imagesPath
                };

                result.Add(resultProd);
            }

            return result;
        }
        public ULoginResp UserOrderAction(OrderData data)
        {
            UDbTable user;
            var validate = new EmailAddressAttribute();
            if (validate.IsValid(data.Email))
            {
                var Products = this.GetShoppingCartProducts(data.Email);
                int totalPrice = 0;
                foreach(var product in Products)
                {
                    totalPrice += product.Count * int.Parse(product.Price);
                }
                var newOrder = new Order()
                {
                    Email = data.Email,
                    NameSurname = data.NameSurname,
                    PhoneNumber = data.PhoneNumber,
                    City = data.City,
                    Street = data.Street,
                    Building = data.Building,
                    Appartment = int.Parse(data.Appartment),
                    PostalCode = data.PostalCode,
                    PaymentMethod = data.PaymentMethod,
                    PublishTime = DateTime.Now,
                    Ip = data.Ip,
                    TotalPrice = totalPrice
                };
                using (var todo = new OrderContext())
                {
                    todo.Orders.Add(newOrder);
                    todo.SaveChanges();
                    // Get the OrderId of the newly saved Order
                    int orderId = newOrder.Id;

                    foreach (var product in Products)
                    {
                        var newOrderProduct = new OrderProducts()
                        {
                            OrderId = orderId,
                            Email = data.Email,
                            Title = product.Title,
                            ItemPrice = int.Parse(product.Price),
                            Count = product.Count,
                            MainImagePath = product.ImagesPath[0]
                        };
                        using (var todoOP = new OrderProductsContext())
                        {
                            todoOP.OrderProducts.Add(newOrderProduct);
                            todoOP.SaveChanges();

                        }
                    }
                    todo.SaveChanges();
                    return new ULoginResp { Status = true, StatusMsg = "Purchase was made succesfully" };
                }
            }
            else
            {
                return new ULoginResp() { Status = false, StatusMsg = "Something went wrong" };
            }
        }
        public List<OrderWithProducts> GetOrders(string email)
        {
            List<OrderWithProducts> ordersList = new List<OrderWithProducts>();
            using (var db = new OrderContext())
            {
                List<Order> orders;
                if (email != null)
                {
                    orders = db.Orders.Where(p => p.Email == email).ToList();
                }
                else{
                    orders = db.Orders.ToList();
                }
                if (orders != null)
                {
                    for(int i=0; i<orders.Count; i++)
                    {
                        OrderWithProducts order = new OrderWithProducts()
                        {
                            Id = orders[i].Id,
                            NameSurname = orders[i].NameSurname,
                            PhoneNumber = orders[i].PhoneNumber,
                            City = orders[i].City,
                            Street = orders[i].Street,
                            Building = orders[i].Building,
                            Appartment = orders[i].Appartment,
                            PostalCode = orders[i].PostalCode,
                            PaymentMethod = orders[i].PaymentMethod,
                            Email = orders[i].Email,
                            TotalPrice = orders[i].TotalPrice,
                            PublishTime = orders[i].PublishTime,
                            Products = new List<ProductWithPath>() // Ensure Products is initialized
                        };

                        using (var dbOProducts = new OrderProductsContext())
                        {
                            var currentOrderId = orders[i].Id;
                            var productsTitle = dbOProducts.OrderProducts
                                                           .Where(p => p.OrderId == currentOrderId)
                                                           .Select(p => p.Title)
                                                           .ToList();

                            foreach (var title in productsTitle)
                            {
                                using (var dbProducts = new ProductContext())
                                {
                                    var product = dbProducts.Products
                                                            .Where(p => p.Title == title)
                                                            .FirstOrDefault();
                                    if (product != null)
                                    {
                                        List<string> imagePaths = this.GetProductImagesPath(product);
                                        ProductWithPath productWithPath = Mapper.Map<ProductWithPath>(product);
                                        productWithPath.ImagesPath = imagePaths;

                                        order.Products.Add(productWithPath);
                                    }
                                }
                            }
                        }
                        ordersList.Add(order);
                    }
                }
                
                
            }

            return ordersList;
        }
    }

}
