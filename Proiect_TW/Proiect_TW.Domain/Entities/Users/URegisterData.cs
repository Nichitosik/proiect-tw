using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_TW.Domain.Entities.User
{
    public class URegisterData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string LastIp { get; set; }
        public DateTime LoginDateTime { get; set; }
        public DateTime RegisterDateTime { get; set; }

    }
}
