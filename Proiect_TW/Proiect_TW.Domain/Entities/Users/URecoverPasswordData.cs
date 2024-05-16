using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_TW.Domain.Entities.User
{
    public class URecoverPasswordData
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        public DateTime RecoverDateTime { get; set; }
        public string RecoverPasswordIp { get; set; }
    }
}
