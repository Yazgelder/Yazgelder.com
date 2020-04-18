namespace Yazgelder.MesajPaneli.Models
{
    public class RefList
    {
        public int id;
        public string tarih;
        public int adet;
        public string mesaj;
        public string baslik;
        public string durum;

        public string GetDurum()
        {
            string str;
            switch (this.durum)
            {
                case "1":
                    str = "Onay Bekliyor";
                    break;

                case "6":
                    str = "İleri Tarihli";
                    break;

                case "66":
                    str = "Taslak";
                    break;

                case "99":
                    str = "Hata";
                    break;

                case "":
                    str = "Hata";
                    break;

                case "JACK":
                    str = "Gönderiliyor";
                    break;

                case "VENUS":
                    str = "Gönderiliyor";
                    break;

                case "NUMERIK":
                    str = "Gönderiliyor";
                    break;

                case "BASLIKLI":
                    str = "Gönderiliyor";
                    break;

                case "4":
                    str = "Gönderildi";
                    break;

                default:
                    str = this.durum;
                    break;
            }
            return str;
        }
    }
}