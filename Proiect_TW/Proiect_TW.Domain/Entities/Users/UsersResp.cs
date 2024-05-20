using Proiect_TW.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_TW.Domain.Entities.Users
{
    public class UsersResp
    {
        public List<UDbTable> Users { get; set; }
        public int TotalUsers { get; set; }
        public int NewUsers { get; set; }//new users in last 24 hours
        public int OnlineUsers { get; set; }
        public int DeletedUsers { get; set; }
    }
}
