using System;
using System.Collections.Generic;
using System.Text;

namespace IS.Domain.Model
{
    public class Customer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public List<ProductForCustomer> Purchased { get; set; } = new List<ProductForCustomer>();
        public List<ProductForCustomer> Orders { get; set; } = new List<ProductForCustomer>();
        public bool Validate()
        {
            if (String.IsNullOrEmpty(Name)) return false;
            if (String.IsNullOrEmpty(Contact)) return false;
            return true;
        }
    }
}
