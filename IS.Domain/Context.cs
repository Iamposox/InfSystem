using Microsoft.EntityFrameworkCore;
using System;
using IS.Domain.Model;
using System.Linq;
using System.Collections.Generic;

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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role() { ID = 5, RoleName = $"Кассир" },
                new Role() { ID = 4, RoleName = $"Пекарь" },
                new Role() { ID = 3, RoleName = $"Бухгалтер" },
                new Role() { ID = 2, RoleName = $"Директор" },
                new Role() { ID = 1, RoleName = $"Админ" });
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    ID = 1,
                    Email = $"one@gmail.com",
                    Name = $"Влада",
                    Password = $"1",
                    RoleID = 1
                });
        }
        public DbSet<Assortment> Assortments { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Product> ProductsForCustomer { get; set; }
        public DbSet<RawMaterial> RawMaterials { get; set; }
        public DbSet<RawMaterialsToOrder> RawMaterialsToOrder { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

    }
}
