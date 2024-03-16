using Proiect_TW.BussinesLogic.Entities.User;
using Proiect_TW.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace Proiect_TW.BussinesLogic.Interfaces
{
    public interface ISession
    {
        ULoginResp UserLogin(ULoginData data);

        //HttpCookie GenCookie(string loginCredential);
        //UserMinimal GetUserByCookie(AspNetHostingPermission apiCookieValue);
    }
}
