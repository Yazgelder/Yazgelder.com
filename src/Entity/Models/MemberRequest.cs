using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Entity.Models
{
    public class MemberRequest : Base
    {
        public MemberRequest() : base()
        {
        }

        public bool IsHonorary { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Telephone { get; set; }

        public string Gender { get; set; }

        [Required]
        public string Job { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public DateTime ApplicationDate { get; set; } = DateTime.Now;
        public DateTime Birthday { get; set; } = DateTime.Now.Date;
        public string Reference { get; set; }
        public string Reference2 { get; set; }
    }
}