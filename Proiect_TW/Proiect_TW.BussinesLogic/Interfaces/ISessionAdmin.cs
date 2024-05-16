using Proiect_TW.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_TW.BussinesLogic.Interfaces
{
    public interface ISessionAdmin
    {
        ProductResp AddProduct(ProductData data);
        UserMinimal GetUserByCookie(string apiCookieValue);
    }
}
