using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Areas.Admin.Models
{
    public class FileModel
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        public bool IsDeleted { get; set; }
        public byte Type { get; set; }
        public byte SubType { get; set; }
    }
}