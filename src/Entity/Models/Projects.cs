using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Entity.Models
{
    public class Projects : ImageBase
    {
        public Projects() : base()
        {
        }

        public string Title { get; set; }
        public string Content { get; set; }
        public string Technology { get; set; }
        public int Year { get; set; }
        public int Order { get; set; }
    }
}