using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Entity.Models
{
    public class Tag : Base
    {
        public Tag() : base()
        {
            NewsTagsList = new HashSet<NewsTags>();
        }

        public string Name { get; set; }
        public string LinkName { get; set; }
        public virtual ICollection<NewsTags> NewsTagsList { get; set; }
    }
}
