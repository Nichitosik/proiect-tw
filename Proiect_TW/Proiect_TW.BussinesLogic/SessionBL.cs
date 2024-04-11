﻿using Proiect_TW.BusinessLogic.Core;
using Proiect_TW.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proiect_TW.Domain.Entities.User;
using Proiect_TW.BusinessLogic.Entities.User;
using System.Web;

namespace Proiect_TW.BusinessLogic
{
    public class SessionBL : UserAPI, ISession
    {
        public ULoginResp UserLogin(ULoginData data)
        {
            return UserLoginAction(data);
        }
        public URegisterResp UserRegister(URegisterData data)
        {
            return UserRegisterAction(data);
        }
        public HttpCookie GenCookie(string loginEmail)
        {
            return Cookie(loginEmail);
        }

        public UserMinimal GetUserByCookie(string apiCookieValue)
        {
            return UserCookie(apiCookieValue);
        }
    }
}