using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Entity.Models
{
    public class Base
    {
        [Key]
        public Guid Id { get; set; }

        public Base()
        {
            Id = Guid.NewGuid();
        }

    }
}
