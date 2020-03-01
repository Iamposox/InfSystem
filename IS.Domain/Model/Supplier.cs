using System;
using System.Collections.Generic;
using System.Text;

namespace IS.Domain.Model
{
    public class Supplier
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<RawMaterials> RawMaterials { get; set; }
        public string Transport { get; set; }
        public string Contact { get; set; }
    }
}
