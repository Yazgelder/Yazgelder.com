using System.Collections.Generic;

namespace Yazgelder.MesajPaneli.Models
{
    public class ReturnValue
    {
        public UserData userData = new UserData();
        public List<RefList> refList = new List<RefList>();
        public RefData refData = new RefData();
        public bool status;
        public string error;
        public string amount;
        public string type;
        public string credits;
        public string Ref;
        public bool passData;
        public RefundData refundData;
    }
}