using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_TW.Domain.Entities.User
{
    public class ULoginResp
    {
        public UDbTable User { get; set; }
        public bool Status { get; set; }
        public string StatusMsg { get; set; }
    }
}
