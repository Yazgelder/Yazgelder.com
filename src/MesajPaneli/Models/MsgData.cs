using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.MesajPaneli.Models
{
    public class msgdata
    {
        public List<string> telList = new List<string>();
        public string tel;
        public string msg;

        public msgdata(string telx, string msgx)
        {
            this.tel = telx;
            this.msg = msgx;
        }

        public msgdata(List<string> telx, string msgx)
        {
            this.telList = telx;
            this.msg = msgx;
        }
    }
}