using System;
using System.Collections.Generic;
using System.Text;

namespace IS.Domain.Model
{
    public class Assortment
    {
        public int ID { get; set; }
        public double InAssortment { get; set; }
        public Product Product { get; set; }
        public bool Validate() 
        {
            if (Product == null) return false;
            return true;
        }
    }
}
