using IS.Domain;
using IS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace IS.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {

#if DEBUG
            Seed();
#endif

        }

        private void Seed()
        {
            Context context = new Context();

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
                    Products = product
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
                RawMaterialsToOrder RMTO = new RawMaterialsToOrder()
                {
                    Supplier_ID = i,
                    ID = i,
                    Price = 1500 * i,
                    Name = $"SomeRawToOrder {i}"
                };
                context.Products.Add(product);
                context.Assortments.Add(As);
                context.Customers.Add(Cm);
                context.RawMaterials.Add(Rw);
                context.RawMaterialsToOrder.Add(RMTO);
                context.Suppliers.Add(sup);
            }
            context.SaveChanges();

            //var GetProduct = context.Products.Where(x => x.Name == "Smells like crap").First();

        }
    }
}
