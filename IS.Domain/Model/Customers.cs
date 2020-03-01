using System;
using System.Collections.Generic;
using System.Text;

namespace IS.Domain.Model
{
    public class Customers
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public List<Product> Purchased { get; set; }
        public List<Product> Orders { get; set; }
    }
}
