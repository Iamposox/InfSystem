using System;
using System.Collections.Generic;
using System.Text;

namespace IS.Domain.Model
{
    public class Assortment
    {
        public int ID { get; set; }
        public int InAssortment { get; set; }
        public Product Products { get; set; }
    }
}
