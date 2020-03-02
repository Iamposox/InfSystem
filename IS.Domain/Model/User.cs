using System;
using System.Collections.Generic;
using System.Text;

namespace IS.Domain.Model
{
    public class User
    {
        public int ID { get; set; }
        public string Name {get;set;}
        public string Password { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; } 
    }
}
