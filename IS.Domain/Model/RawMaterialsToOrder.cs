using System;
using System.Collections.Generic;
using System.Text;

namespace IS.Domain.Model
{
    public class RawMaterialsToOrder
    {
        public int ID { get; set; }
        public double Price { get; set; }
        public RawMaterial Material { get; set; }
    }
}
