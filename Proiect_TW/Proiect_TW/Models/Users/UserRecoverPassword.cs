using System;


namespace Proiect_TW.Web.Models.Users
{
    public class UserRecoverPassword
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
    }
}