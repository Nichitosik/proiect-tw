using System;
using System.ComponentModel.DataAnnotations;

namespace Proiect_TW.Web.Models.Users
{
    public class UserRegister
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
    }
}