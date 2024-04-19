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


namespace Proiect_TW.BusinessLogic.Core
{
    public class UserAPI
    {
        internal ULoginResp UserLoginAction(ULoginData data)
        {
            UDbTable result;
            var validate = new EmailAddressAttribute();
            if(validate.IsValid(data.Email))
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
                return new ULoginResp { Status = false, StatusMsg = "Incorrect Email or Password"};   
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
                    LastIp = "194.1.20",
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

    }
}
