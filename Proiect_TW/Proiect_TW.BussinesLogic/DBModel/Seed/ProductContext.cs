using Microsoft.Build.Evaluation;
using Proiect_TW.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_TW.BussinesLogic.DBModel.Seed
{
    internal class ProductContext : DbContext
    {
        public ProductContext() : base("name=Proiect_TW")
        {
        }

        public virtual DbSet<Product> Products { get; set; }
    }
}
