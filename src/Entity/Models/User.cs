using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Entity.Models
{
    public class User : Base
    {
        public User() : base()
        {

        }


        public string UserName { get; set; }
        public string Password { get; set; } 
    }
}
