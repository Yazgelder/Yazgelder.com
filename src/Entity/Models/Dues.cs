using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Entity.Models
{
    public class Dues : Base
    {
        public Dues() : base()
        {
            TallySheetList = new HashSet<TallySheet>();
        }

        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
        public decimal Money { get; set; }

        public virtual ICollection<TallySheet> TallySheetList { get; set; }

    }
}
