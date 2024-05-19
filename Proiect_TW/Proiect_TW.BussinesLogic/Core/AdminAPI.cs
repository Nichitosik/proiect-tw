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
        internal UsersResp GetUsersAction()
        {
            var users = new List<UDbTable>();
            using (var db = new UserContext())
            {
                users = db.Users.ToList();
            }
            return new UsersResp { Users = users, Count = users.Count};

        }
    }
}

