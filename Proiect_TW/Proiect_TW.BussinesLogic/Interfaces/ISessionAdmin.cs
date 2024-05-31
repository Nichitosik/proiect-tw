using Proiect_TW.Domain.Entities.User;
using Proiect_TW.Domain.Entities.Users;
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
        ProductImagesResp AddProductImages(ProductImagesData data);
        UsersResp GetUsers();
        List<Product> GetAllProducts();
        List<Feedback> GetUsersFeedback();
        ULoginResp DeleteUser(string UserEmail);
        ULoginResp DeleteProduct(string Title);
        Statistics GetStatistics();


    }
}
