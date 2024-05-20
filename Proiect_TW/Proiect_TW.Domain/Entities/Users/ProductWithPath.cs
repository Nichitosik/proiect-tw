using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_TW.Domain.Entities.Users
{
    public class ProductWithPath
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Type { get; set; }
        public string Style { get; set; }
        public DateTime PublishTime { get; set; }
        public int SoldItems { get; set; }
        public string SalesIncome { get; set; }
        public List<string> ImagesPath { get; set; }
    }
}
