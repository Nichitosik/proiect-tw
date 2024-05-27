using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_TW.Domain.Entities.Users
{
    public class OrderWithProducts
    {
        public int Id { get; set; }
        public string NameSurname { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Building { get; set; }
        public int Appartment { get; set; }
        public string PostalCode { get; set; }
        public string PaymentMethod { get; set; }
        public string Email { get; set; }
        public int TotalPrice { get; set; }
        public DateTime PublishTime { get; set; }
        public List<ProductWithPath> Products { get; set; }
    }
}
