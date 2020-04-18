using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Config
{
    public class MailConfig 
    {
        public string MailServer { get; set; }
        public string MailAdres { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public string SenderName { get; set; }
        public bool UseSsl { get; set; }
        public string Domain { get; set; }
        public string AdminMails { get; set; }
    } 
}
