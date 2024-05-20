using Proiect_TW.BusinessLogic.Entities.User;
using Proiect_TW.Domain.Entities.User;
using Proiect_TW.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;


namespace Proiect_TW.BusinessLogic.Interfaces
{
    public interface ISession
    {
        ULoginResp UserLogin(ULoginData data);
        URegisterResp UserRegister(URegisterData data);
        URecoverPasswordResp UserRecoverPassword(URecoverPasswordData data);
        UProfileEditResp UserProfileEdit(UProfileEditData data);
        HttpCookie GenCookie(string loginEmail);
        UserMinimal GetUserByCookie(string apiCookieValue);
        List<ProductWithPath> GetProductsForUser(string type);
        List<ProductWithPath> GetProductsForYou(string gender, int age);
        List<ProductWithPath> GetAllProducts();

    }
}
