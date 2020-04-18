using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Config
{
    public class GuvenSmsConfig
    {
        public string KullaniciNo { get; set; }
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
        public string Orjinator { get; set; }
        public int ZamanAsimi { get; set; }
        public int Tip { get; set; }
    }
}