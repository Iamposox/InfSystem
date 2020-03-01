using System;
using System.Collections.Generic;
using System.Text;

namespace IS.Domain.Model
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public double PassiveCosts { get; set; }
        public TimeSpan PreparationDuration { get; set; }
        public RawMaterials RequeredMaterials { get; set; }
    }
}
