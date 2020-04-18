using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Entity.Models
{
    public class Events : ImageBase
    {
        public Events() : base()
        {
        }

        public string Title { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;
        public string Comment { get; set; }
        public bool IsCancel { get; set; }
    }
}