using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_TW.Domain.Entities.User
{
    public class ProductData
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Style { get; set; }
        public string Type { get; set; }
        public List<string> Sizes { get; set; }
        public string Ip { get; set; }
        public DateTime PublishTime { get; set; }
    }
}
