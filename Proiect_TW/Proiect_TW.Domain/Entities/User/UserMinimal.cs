﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proiect_TW.Domain.Enums;

namespace Proiect_TW.Domain.Entities.User
{
    internal class UserMinimal
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime LastLogin { get; set; }
        public string LasIp { get; set; }
        public URole Level { get; set;}
        
    }
}
