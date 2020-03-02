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
                RoleName = $"Kassir"
            };
            Roles.Add(roles);
            Role rolesTwo = new Role()
            {
                RoleName = $"Baker"
            };
            Roles.Add(rolesTwo);
            Role rolesThree = new Role()
            {
                RoleName = $"Director"
            };
            Roles.Add(rolesThree);
            Role rolesFour = new Role()
            {
                RoleName = $"Accountant"
            };
            Roles.Add(rolesFour);
            Roles.Add(new Role
            {
                RoleName = "Admin",
                Users = new List<User> { users }
            }); ; ;
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
