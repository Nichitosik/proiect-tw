using Proiect_TW.BusinessLogic.Core;
using Proiect_TW.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proiect_TW.Domain.Entities.User;
using System.Web;
using Proiect_TW.BussinesLogic.Interfaces;
using Proiect_TW.BussinesLogic.Core;
using Proiect_TW.Domain.Entities.Users;

namespace Proiect_TW.BussinesLogic
{
    public class SessionAdmin : AdminAPI, ISessionAdmin
    {
        public ProductResp AddProduct(ProductData data)
        {
            return AddProductAction(data);
        }
        public ProductImagesResp AddProductImages(ProductImagesData data) 
        {
            return AddProductImagesAction(data);
        }
        public UserMinimal GetUserByCookie(string apiCookieValue)
        {
            return UserCookie(apiCookieValue);
        }
        public UsersResp GetUsers()
        {
            return GetUsersAction();
        }
        public List<Product> GetAllProducts()
        {
            return GetProducts();
        }
        public List<List<string>> GetProductImages(List<Product> products)
        {
            return GetProductImagesPath(products);
        }

    }
}