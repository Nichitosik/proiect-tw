using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_TW.Domain.Entities.Users
{
    public class ShoppingCartProduct
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Type { get; set; }
        public string Style { get; set; }
        public string Size { get; set; }
        public int SoldItems { get; set; }
        public string SalesIncome { get; set; }
        public List<string> ImagesPath { get; set; }
        public int Count { get; set; }
        public ShoppingCartProduct()
        {
            ImagesPath = new List<string>();
        }
    }
}
