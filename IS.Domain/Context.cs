using Microsoft.EntityFrameworkCore;
using System;
using IS.Domain.Model;

namespace IS.Domain
{

    /// <summary>
    /// Default way to connect Operate with Database
    /// </summary>
    public class Context : DbContext
    {
        private const string ConnectionString = "Data Source=Database.db";

        public Context()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
        }

        public DbSet<Assortment> Assortments { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<RawMaterial> RawMaterials { get; set; }
        public DbSet<RawMaterialsToOrder> RawMaterialsToOrder { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
    }
}
