using Proiect_TW.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_TW.BussinesLogic.DBModel.Seed
{
    internal class ProductImagesContext : DbContext
    {
        public ProductImagesContext() : base("name=Proiect_TW")
        {
        }

        public virtual DbSet<ProductImages> ProductImages { get; set; }
    }
}
