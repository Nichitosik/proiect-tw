using System;
using System.Collections.Generic;
using System.Web;


namespace Proiect_TW.Web.Models.Users
{
    public class AddProduct
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Style { get; set; }
        public List<string> Sizes { get; set; }
        //public List<HttpPostedFileBase> Photoes { get; set; }
    }
}