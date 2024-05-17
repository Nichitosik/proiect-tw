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
        public int Count { get; set; }
    }
}
