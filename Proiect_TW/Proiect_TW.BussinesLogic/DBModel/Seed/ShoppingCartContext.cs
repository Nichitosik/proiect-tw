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
    internal class ShoppingCartContext: DbContext
    {
        public ShoppingCartContext() : base("name=Proiect_TW")
        {
        }

        public virtual DbSet<ShoppingCart> ShoppingCart { get; set; }
    }
}
