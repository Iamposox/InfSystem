using System;
using System.Collections.Generic;
using System.Text;

namespace IS.Domain.Model
{
    public class Role
    {
        public int ID { get; set; }
        public string RoleName { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }
}
