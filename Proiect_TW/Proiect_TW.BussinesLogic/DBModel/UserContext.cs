using Proiect_TW.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_TW.BussinesLogic.DBModel
{
    class UserContext: DbContext
    {
        public UserContext() : base("name=eUseControl") { }

        public virtual DbSet<UDbTable> Users { get; set; }
    }
}
