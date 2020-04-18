using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Entity.Models
{
    public class Categories:Base
    {
        public Categories() : base()
        {
            NewsCategoriesList = new HashSet<NewsCategories>();
        }

        public string Name { get; set; }

        public virtual ICollection<NewsCategories> NewsCategoriesList { get; set; }

    }
}
