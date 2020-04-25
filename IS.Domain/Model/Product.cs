using System;
using System.Collections.Generic;
using System.Text;

namespace IS.Domain.Model
{
    public class Product:ICloneable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double PassiveCosts { get; set; }
        public TimeSpan PreparationDuration { get; set; }
        public List<RawMaterial> RequeredMaterials { get; set; }
        public bool Validate() 
        {
            if (String.IsNullOrEmpty(Name)) return false;
            return true;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
