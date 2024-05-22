using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_TW.Domain.Entities.Users
{
    public class UFeedbackData
    {
        public string Email { get; set; }
        public string Description { get; set; }
        public string Ip { get; set; }
        public DateTime PublishTime { get; set; }
    }
}
