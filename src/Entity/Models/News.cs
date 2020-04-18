using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yazgelder.Entity.Models
{
    public class News : ImageBase
    {
        public News() : base()
        {
            NewsCategoriesList = new HashSet<NewsCategories>();
            NewsTagsList = new HashSet<NewsTags>();
        }

        public string Title { get; set; }
        public string Body { get; set; }
        public byte Type { get; set; }
        public string Sender { get; set; }
        public DateTime SenderDate { get; set; } = DateTime.Now;
        public bool IsDraft { get; set; } = true;
        public string LinkName { get; set; }

        [NotMapped]
        public List<Guid> Categories { get; set; }

        [NotMapped]
        public List<Guid> Tags { get; set; }

        public virtual ICollection<NewsCategories> NewsCategoriesList { get; set; }
        public virtual ICollection<NewsTags> NewsTagsList { get; set; }
    }
}