using Proiect_TW.Domain.Entities.User;
using Proiect_TW.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_TW.BussinesLogic.DBModel.Seed
{
    public class FeedbackContext : DbContext
    {
        public FeedbackContext() :
            base("name=Proiect_TW")
        {

        }
        public virtual DbSet<Feedback> Feedback { get; set; }
    }
}
