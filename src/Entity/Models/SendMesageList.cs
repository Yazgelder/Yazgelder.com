using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Entity.Models
{
    public class SendMesageList:Base
    {
        public SendMesageList():base()
        {

        }
        public string To { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime SendingDate { get; set; } = DateTime.Now;
    }
}
