using Proiect_TW.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_TW.BussinesLogic.DBModel.Seed
{
    internal class SessionContext: DbContext
    {
        public SessionContext() : base("name=Proiect_TW")
        {
        }

        public virtual DbSet<Session> Sessions { get; set; }
    }
}
