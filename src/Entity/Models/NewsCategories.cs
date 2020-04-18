using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Entity.Models
{
    public class NewsCategories:Base
    {
        public NewsCategories():base()
        {

        }

        public Guid NewsId { get; set; }
        public Guid CategoriesId { get; set; }
        public News News { get; set; }
        public Categories Categories { get; set; }
    }
}
