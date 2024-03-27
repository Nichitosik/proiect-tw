using Proiect_TW.BusinessLogic.Entities.User;
using Proiect_TW.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Proiect_TW.BusinessLogic.Core
{
    public class UserAPI
    {
        internal ULoginResp UserLoginAction(ULoginData data)
        {
            //UDbTable result;
            var validate = new EmailAddressAttribute();
            if(validate.IsValid(data.Email) && data.Email == "gheorghe.cristian@isa.utm.md" && data.Password == "password")
            {



                //using (var db = new UserContext())
                //{
                //    result = db.Users.FirstOrDefault(using => u.Email == data.Email && using.Password == data.Password);
                //}
                //if (result == null)
                //{
                //    return new ULoginResp { Status = false, StatusMsg = "The Uswername or Password is Incorrect" };
                //}
                //using (var todo = new UserContext())
                //{
                //    result.LastIp = data.LoginIp;
                //    result.LastLogin = data.LoginDateTime;
                //    todo.SaveChanges();
                //}
                return new ULoginResp { Status = true };

            }
            else
                return new ULoginResp { Status = false, StatusMsg = "Incorrect Email or Password"};   
        }
    }
}
