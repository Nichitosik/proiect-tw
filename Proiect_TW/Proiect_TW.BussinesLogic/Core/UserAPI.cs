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
                using (var db = new UserContext())
                {
                    result = db.Users.FirstOrDefault(u => u.Email == data.Email && u.Password == data.Password);
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
                    Password = data.Password,
                    LastIp = "0.0.0.1",
                    LastLogin = DateTime.Now,
                    Level = 0
                };
                //try
                //{
                    using (var todo = new UserContext())
                    {
                        todo.Users.Add(newUser);
                        todo.SaveChanges();
                    }
                    return new URegisterResp { Status = true };
                //}
                //catch (DbEntityValidationException ex)
                //{
                //    foreach (var validationErrors in ex.EntityValidationErrors)
                //    {
                //        foreach (var validationError in validationErrors.ValidationErrors)
                //        {
                //            Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                //        }
                //    }
                //    // Handle the validation errors appropriately
                //    return new URegisterResp { Status = false, StatusMsg = "Validation failed for one or more entities" };
                //}
            }
            else
            {
                return new URegisterResp { Status = false, StatusMsg = "Email is not valid" };
            }
        }

    //    internal URegisterResp UserRegisterAction(URegisterData data)
    //    {
    //        UDbTable user;
    //        var validate = new EmailAddressAttribute();
    //        if (validate.IsValid(data.Email))
    //        {
    //            using (var db = new UserContext())
    //            {
    //                user = db.Users.FirstOrDefault(u => u.Email == data.Email);
    //            }
    //            if (user != null)
    //            {
    //                return new URegisterResp { Status = false, StatusMsg = "This user already exists" };
    //            }
    //            var newUser = new UDbTable()
    //            {
    //                Email = data.Email,
    //                Username = data.Username,
    //                Password = data.Password,
    //                LastIp = data.LoginIp,
    //                LastLogin = data.LoginDateTime,
    //            };
    //            using (var todo = new UserContext())
    //            {
    //                todo.Users.Add(newUser);
    //                todo.SaveChanges();
    //            }
    //            return new URegisterResp { Status = true };

    //        }
    //        else
    //            return new URegisterResp { Status = false, StatusMsg = "Email is not valid" };
    //    }
    }
}
