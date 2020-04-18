using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Entity.Models
{
    public class Pictures : Base
    {
        public Pictures() : base()
        {

        }

        public byte RecordType { get; set; }
        public byte RecordType2 { get; set; }
        public Guid RelationId { get; set; }
        public string Title { get; set; }
        public string Picture { get; set; }
        public byte Order { get; set; }
    }
}
