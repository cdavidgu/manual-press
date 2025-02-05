using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }  
        public string Email { get; set; }  
        public string Pass { get; set; }  
        public string Role { get; set; }

        public override string ToString ()
        {
            return string.Format ("[Users: Id={0}, Name={1}, Email={2}, Pass={3}, Role={4}]",
                Id, Name, Email,Pass,Role);
        }
    }
}