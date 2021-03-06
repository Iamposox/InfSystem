﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IS.Domain.Model
{
    public class RawMaterial:ICloneable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
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
