using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Entity.Models
{
    public class ProjectProposal : ImageBase
    {
        public ProjectProposal() : base()
        {
        }

        public string SenderMail { get; set; }
        public string SenderName { get; set; }
        public string SenderSurname { get; set; }
        public string ProjectName { get; set; }
        public string ProjectExplanation { get; set; }
        public DateTime SenderDate { get; set; } = DateTime.Now;
    }
}