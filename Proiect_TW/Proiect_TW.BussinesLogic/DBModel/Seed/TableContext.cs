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
    public class TableContext: DbContext
    {
        public TableContext() : base("name=Proiect_TW")
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<UDbTable> Users { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<ProductImages> ProductImages { get; set; }

    }
}
