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
                    result = db.Users.FirstOrDefault(u => u.Email == data.Email && u.Password == pass);
                }
                if (result == null)
                {
                    return new ULoginResp { Status = false, StatusMsg = "The Uswername or Password is Incorrect" };
                }
                using (var todo = new UserContext())
                {
                    result.LastIp = data.LoginIp;
                    result.LastLogin = data.LoginDateTime;
                    todo.SaveChanges();
                }
                return new ULoginResp { Status = true };

            }
            else
                return new ULoginResp { Status = false, StatusMsg = "Incorrect Email or Password" };
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
                    Level = URole.User
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
                            user = db.Users.FirstOrDefault(u => (u.Email == data.Email) && (u.Username == data.Username));

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
        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            using (var db = new ProductContext())
            {
                products = db.Products.ToList();
            }
            return products;
        }
        public List<Product> GetProductsByType(string type)
        {
            List<Product> allProducts = new List<Product>();
            List<Product> products = new List<Product>();
            allProducts = GetProducts();

            foreach(Product product in allProducts)
            {
                if (product.Type == type)
                {
                    products.Add(product);
                }
            }

            return products;
        }
        public List<Product> GetProductsByGender(string gender, int age)
        {
            List<Product> products = new List<Product>();
            products = GetProducts();
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

            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].AgeCategory != ageCategory)
                {
                    products.RemoveAt(i);
                }
            }
            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].Gender != productGender)
                {
                    products.RemoveAt(i);
                }
            }
            return products;
        }
        public List<List<string>> GetProductImagesPath(List<Product> products)
        {
            List<List<string>> imagesPath = new List<List<string>>();
            List<ProductImages> images = new List<ProductImages>();

            using (var db = new ProductImagesContext())
            {
                images = db.ProductImages.ToList();
            }

            // Inițializarea listelor pentru fiecare produs
            foreach (var product in products)
            {
                imagesPath.Add(new List<string>());
            }

            for (int i = 0; i < products.Count; i++)
            {
                foreach (ProductImages image in images)
                {
                    if (image.ProductTitle == products[i].Title)
                    {
                        imagesPath[i].Add(image.ImagePath);
                    }
                }
            }
            return imagesPath;
        }


    }
}
