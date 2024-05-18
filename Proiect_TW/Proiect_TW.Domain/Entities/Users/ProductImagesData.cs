using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_TW.Domain.Entities.Users
{
    public class ProductImagesData
    {
        public string ProductTitle { get; set; }
        public List<string> ImageNames { get; set; }
        public List<string> ImagePaths { get; set; }
    }
}
