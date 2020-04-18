using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Entity.Models
{
    public class Newsletter : Base
    {
        public Newsletter() : base() { }

        public string Mail { get; set; }
        public string IpAdress { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
