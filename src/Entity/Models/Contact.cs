using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Entity.Models
{
    public class Contact : Base
    {
        public Contact() : base() { }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EMail { get; set; }
        public string Message { get; set; }
        public byte Type { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
