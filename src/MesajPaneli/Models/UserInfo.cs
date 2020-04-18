namespace Yazgelder.MesajPaneli.Models
{
    public class UserInfo
    {
        public string name;
        public string pass;
        public string newpass;

        public UserInfo(string namex, string passx)
        {
            this.name = namex;
            this.pass = passx;
        }

        public UserInfo(string namex, string passx, string newpassx)
        {
            this.name = namex;
            this.pass = passx;
            this.newpass = newpassx;
        }
    }
}