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
            //if (this.Customers.Count() < 1)
            //{
            //    Seed();
            //}
        }
        private void SeedUser()
        {
            User user = new User()
            {
                Email = $"one@gmail.com",
                Name = $"Vlada",
                Password = $"1",
            };

            Users.Add(user);
            SaveChanges();
        }

        private void SeedRoles()
        {

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
            Role rolesFive = new Role()
            {
                ID = 1,
                RoleName = "Admin",
                Users = new List<User> { Users.Single(x => x.Name == "Vlada") }
            };
            Roles.Add(rolesFour);
            Roles.Add(rolesFive);
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
                Product productTwo = new Product()
                {
                    Name = $" crap {i}",
                    PassiveCosts = 99.9,
                    PreparationDuration = new TimeSpan(2, 30, 0),
                    RequeredMaterials = new List<RawMaterial>
                {
                    Rw
                }
                };
                Assortment As = new Assortment()
                {
                    InAssortment = 150 * i,
                    Product = product
                };
                Assortment AsTwo = new Assortment()
                {
                    InAssortment = 150 * i,
                    Product = productTwo
                };
                ProductForCustomer Pr = new ProductForCustomer()
                {
                    Price = 100 + 10 * i,
                    Product = As
                };
                ProductForCustomer PrTwo = new ProductForCustomer()
                {
                    Price = 100 + 10 * i,
                    Product = AsTwo
                };
                Customer Cm = new Customer()
                {
                    Name = $"Dirty bakery{i}",
                    Contact = $"890440588{i}",
                    Purchased = new List<ProductForCustomer>
                    {
                        Pr
                    },
                    Orders = new List<ProductForCustomer>
                    {
                        PrTwo
                    }
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
            //for (int i = 1; i < 11; i++)
            
            //    modelBuilder.Entity<RawMaterial>().HasData(new RawMaterial() { Name = $"Eggs {i}", Amount = 10+i, ID = i, ProductID = i});
            //for (int i = 1; i < 11; i++)
            //    modelBuilder.Entity<Product>().HasData
            //        (
            //        new Product() { Name = $"Sells like crap {i}", ID=i, PassiveCosts = 99.9, PreparationDuration = new TimeSpan(2, 30,0)},
            //        new Product() { Name = $"Crap {i}", ID = i, PassiveCosts = 99.9, PreparationDuration = new TimeSpan(2, 30, 0) }
            //        );
            //for (int i = 1; i < 11; i++)
            //    modelBuilder.Entity<Assortment>().HasData
            //        (
            //        new Assortment() { ID = i, InAssortment = 150*i, ProductID = i},
            //        new Assortment() { ID = i, InAssortment = 150 * i, ProductID = i }
            //        );
            
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
