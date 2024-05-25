using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proiect_TW.Web.Models.Users
{
    public class Order
    {
        public string NameSurname { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Building { get; set; }
        public string Appartment { get; set; }
        public string PostalCode { get; set; }
        public string PaymentMethod { get; set; }
    }
}