using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Entity.Models
{
    public class NewsTags:Base
    {
        public NewsTags():base()
        {

        }

        public Guid NewsId { get; set; }
        public Guid TagId { get; set; }
        public News News { get; set; }
        public Tag Tag { get; set; }
    }
}
