using System;

namespace Yazgelder.Entity.Models
{
    public class Member : ImageBase
    {
        public Member() : base()
        {
        }

        public bool IsHonorary { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string TCIdentity { get; set; }
        public string Telephone { get; set; }
        public string Nationality { get; set; }
        public string MotherName { get; set; }
        public string FatherName { get; set; }
        public string Gender { get; set; }
        public string Job { get; set; }
        public string Email { get; set; }
        public string PlaceOfResidence { get; set; }
        public DateTime ApplicationDate { get; set; } = DateTime.Now;
        public DateTime ApprovalDate { get; set; } = DateTime.Now;
        public DateTime Birthday { get; set; } = DateTime.Now.Date;
        public string Reference { get; set; }
        public string Reference2 { get; set; }
        public bool Active { get; set; } = true;
    }
}