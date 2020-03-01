using System;
using System.Collections.Generic;
using System.Text;

namespace IS.Domain.Model
{
    public class RawMaterialsToOrder
    {
        public int ID { get; set; }
        public int Supplier_ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
