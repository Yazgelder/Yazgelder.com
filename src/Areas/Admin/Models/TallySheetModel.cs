using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Areas.Admin.Models
{
    public class TallySheetModel
    {
        public int OrderNo { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string Exp { get; set; }
        public Guid? MemberId { get; set; }
        public string Member { get; set; }
        public Guid? DuesId { get; set; }
        public string Dues { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsEdit { get; set; } = false;

    }
}
