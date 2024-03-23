using Proiect_TW.BusinessLogic.Entities.User;
using Proiect_TW.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace Proiect_TW.BusinessLogic.Interfaces
{
    public interface ISession
    {
        ULoginResp UserLogin(ULoginData data);

        //HttpCookie GenCookie(string loginCredential);
        //UserMinimal GetUserByCookie(AspNetHostingPermission apiCookieValue);
    }
}
