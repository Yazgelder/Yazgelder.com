using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Entity.Models
{
    public class Link : ImageBase
    {
        public Link() : base()
        {
        }

        public string Name { get; set; }
        public string LinkName { get; set; }
        public int Count { get; set; }
    }
}