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
            Database.EnsureDeleted();
            Database.EnsureCreated();
            if (this.Customers.Count() < 1)
            {
                Seed();
            }
        }

        private void Seed()
        {
            for (int i = 0; i < 100; i++)
            {
                RawMaterial Rw = new RawMaterial()
                {
                    Name = "Eggs",
                    Amount = 10
                };
                Product product = new Product()
                {
                    Amount = 20,
                    Name = $"Smells like crap {i}",
                    PassiveCosts = 99.9,
                    PreparationDuration = new TimeSpan(2, 30, 0),
                    RequeredMaterials = new List<RawMaterial>
                {
                    Rw
                }
                };
                Customer Cm = new Customer()
                {
                    Name = $"Dirty bakery{i}",
                    Contact = $"890440588{i}",
                    Purchased = new List<Product>
                    {
                        product
                    },
                    Orders = new List<Product>
                    {
                        product
                    }
                };
                Assortment As = new Assortment()
                {
                    InAssortment = 15000,
                    Products = product,
                };
                Supplier sup = new Supplier()
                {
                    Name = $"Buyer{i}",
                    Contact = $"890440588{i}",
                    Transport = $"Wagon {i}",
                    RawMaterials = new List<RawMaterial>
                    {
                        Rw
                    }
                };
                Users user = new Users()
                {
                    Email = $"one{i}@gmail.com",
                    Name = $"Vlada{i}",
                    Password = $"123qwerty456"
                };
                Role roles = new Role()
                {
                    RoleName = $"Kassir{i}"
                };
                //RawMaterialsToOrder RMTO = new RawMaterialsToOrder()
                //{
                //    Supplier_ID = i,
                //    Price = 1500 * i,
                //    Name = $"SomeRawToOrder {i}"
                //};
                //RawMaterialsToOrder.Add(RMTO);
                Products.Add(product);
                Assortments.Add(As);
                Customers.Add(Cm);
                RawMaterials.Add(Rw);
                Suppliers.Add(sup);
                Users.Add(user);
                Role.Add(roles);
            }
            SaveChanges();
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
        public DbSet<Users> Users { get; set; }
        public DbSet<Role> Role { get; set; }
        //public DbSet<RoleUsers> RoleUs { get; set; }
    }
}
