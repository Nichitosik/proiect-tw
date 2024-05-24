using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_TW.Domain.Entities.Users
{
    public class ShopCProductData
    {
        public string UserEmail { get; set; }
        public string ProductTitle {  get; set; }
        public string Size { get; set; }
        public int Count { get; set; }
        public string Ip { get; set; }
        public DateTime DateTime { get; set; }

    }
}
