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

            RawMaterial Rw = new RawMaterial()
            {
                Name = "Eggs",
                Amount = 10
            };
            for (int i = 0; i < 999; i++)
            {
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
                context.Products.Add(product);
            }
            context.SaveChanges();

            var GetProduct = context.Products.Where(x => x.Name == "Smells like crap").First();

        }
    }
}
