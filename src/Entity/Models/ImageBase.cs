using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Yazgelder.Areas.Admin.Models;

namespace Yazgelder.Entity.Models
{
    public class ImageBase : Base
    {
        [NotMapped]
        public List<FileModel> FileList { get; set; }

        public ImageBase() : base()
        {
        }
    }
}