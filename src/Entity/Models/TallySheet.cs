using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Entity.Models
{
    public class TallySheet : Base
    {
        public TallySheet() : base()
        {

        }

        public int OrderNo { get; set; }    
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string Exp { get; set; }
        public Guid? MemberId { get; set; }
        public Guid? DuesId { get; set; }
        public Member Member { get; set; }
        public Dues Dues { get; set; }

    }
}
