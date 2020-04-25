using System;
using System.Collections.Generic;
using System.Text;

namespace IS.Domain.Model
{
    public class User:ICloneable
    {
        public int ID { get; set; }
        public string Name {get;set;}
        public string Password { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; } 
        public bool Validate() 
        {
            if (String.IsNullOrEmpty(Name)) return false;
            if (String.IsNullOrEmpty(Password)) return false;
            if (String.IsNullOrEmpty(Email)) return false;
            return true;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
