using System;
using System.Collections.Generic;
using System.Text;

namespace IS.Domain.Model
{
    public class Supplier
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<RawMaterialsToOrder> RawMaterials { get; set; } = new List<RawMaterialsToOrder>();
        public string Transport { get; set; }
        public string Contact { get; set; }
    }
}
