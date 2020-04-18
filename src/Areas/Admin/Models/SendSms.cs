using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Areas.Admin.Models
{
    public class SendSms
    {
        public bool IsBulk { get; set; }
        public string Numbers { get; set; }
        public string Message { get; set; }
        public bool IsResmi { get; set; }
        public bool IsStartNameSurname { get; set; }
    }
}