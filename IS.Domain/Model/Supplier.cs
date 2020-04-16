using System;
using System.Collections.Generic;
using System.Text;

namespace IS.Domain.Model
{
    public class Supplier : ICloneable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<RawMaterialsToOrder> RawMaterials { get; set; } = new List<RawMaterialsToOrder>();
        public string Transport { get; set; }
        public string Contact { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public bool Validate()
        {
            if (String.IsNullOrEmpty(Name)) return false;
            if (String.IsNullOrEmpty(Transport)) return false;
            if (String.IsNullOrEmpty(Contact)) return false;
            return true;
        }
    }
}
