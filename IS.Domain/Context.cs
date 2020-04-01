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

            SeedUser();
            SeedRoles();
           
            if (this.Customers.Count() < 1)
            {
                Seed();
            }
        }
        private void SeedUser()
        {
            User user = new User()
            {
                Email = $"one@gmail.com",
                Name = $"Vlada",
                Password = $"123qwerty456"
            };
            Users.Add(user);
            SaveChanges();
        }

        private void SeedRoles()
        {
            User users = Users.Single(x => x.Name == "Vlada");
            Role roles = new Role()
            {
                ID = 5,
                RoleName = $"Kassir"
            };
            Roles.Add(roles);
            Role rolesTwo = new Role()
            {
                ID = 4,
                RoleName = $"Baker"
            };
            Roles.Add(rolesTwo);
            Role rolesThree = new Role()
            {
                ID = 2,
                RoleName = $"Director"
            };
            Roles.Add(rolesThree);
            Role rolesFour = new Role()
            {
                ID = 3,
                RoleName = $"Accountant"
            };
            Roles.Add(rolesFour);
            Roles.Add(new Role
            {
                ID=1,
                RoleName = "Admin",
                Users = new List<User> { users }
            });
            SaveChanges();
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
                    InAssortment = 150*i,
                    Product = product
                };
                RawMaterialsToOrder RMTO = new RawMaterialsToOrder()
                {
                    Price = 1500 * i,
                    Material = Rw
                };
                RawMaterialsToOrder.Add(RMTO);
                Supplier sup = new Supplier()
                {
                    Name = $"Buyer{i}",
                    Contact = $"890440588{i}",
                    Transport = $"Wagon {i}",
                    RawMaterials = new List<RawMaterialsToOrder>
                    {
                        RMTO
                    }
                };
                Products.Add(product);
                Assortments.Add(As);
                Customers.Add(Cm);
                RawMaterials.Add(Rw);
                Suppliers.Add(sup);
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
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

    }
}
