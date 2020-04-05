using System;
using System.Collections.Generic;
using System.Text;

namespace IS.Domain.Model
{
    public class ProductForCustomer
    {
        public int ID { get; set; }
        public double Price { get; set; }
        public Assortment Product { get; set; }
    }
}
