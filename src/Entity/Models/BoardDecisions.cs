using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Entity.Models
{
    public class BoardDecisions : ImageBase
    {
        public BoardDecisions() : base()
        {
        }

        public string Title { get; set; }
        public string Body { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string DateOrderNo { get; set; }
    }
}