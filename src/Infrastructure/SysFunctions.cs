using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Yazgelder.Config;
using System.Net;
using System.Net.Mail;
using Yazgelder.Entity;
using Yazgelder.Entity.Models;
using System.Text;
using System.Security.Cryptography;
using Yazgelder.Models;
using RestSharp;
using Yazgelder.MesajPaneli;
using Yazgelder.MesajPaneli.Models;

namespace Yazgelder.Infrastructure
{
    public class SysFunctions : ISysFunctions
    {
        private readonly IWebHostEnvironment _env;
        private readonly IOptions<MailConfig> _mailConfig;
        private readonly IOptions<GuvenSmsConfig> _guvenSmsConfig;
        private readonly IOptions<MesajPaneliConfig> _smsVitriniConfig;
        private readonly IOptions<ReportConfig> _reportConfig;
        private readonly Context _context;

        public SysFunctions(IWebHostEnvironment env,
            IOptions<MailConfig> mailConfig,
            Context context,
            IOptions<GuvenSmsConfig> guvenSmsConfig,
            IOptions<MesajPaneliConfig> smsVitriniConfig,
            IOptions<ReportConfig> reportConfig)
        {
            _env = env;
            _mailConfig = mailConfig;
            _context = context;
            _guvenSmsConfig = guvenSmsConfig;
            _smsVitriniConfig = smsVitriniConfig;
            _reportConfig = reportConfig;
        }

        public string SaveUserProfilePhoto(string base64, string userId)
        {
            var file = Path.Combine(_env.WebRootPath, "Fotos");

            if (!Directory.Exists(file))
                Directory.CreateDirectory(file);

            file = Path.Combine(file, userId + ".jpg");
            if (base64.Contains("base64,"))
            {
                var index = base64.IndexOf("base64,");
                base64 = base64.Substring(index + 7);
            }
            using (var data = new MemoryStream(Convert.FromBase64String(base64)))
            {
                using var smlData = StreamImageConvertAndResizeAndCompress(data);
                File.WriteAllBytes(file, smlData.ToArray());
            }

            return userId.ToString() + ".jpg";
        }

        public string GetString(string htmlString, int length)
        {
            if (string.IsNullOrEmpty(htmlString))
                return "";
            var t = Regex.Replace(htmlString, "<.*?>", " ");
            if (t.Length > length)
                return t.Substring(0, length);
            else
                return t;
        }

        public MemoryStream StreamImageConvertAndResizeAndCompress(Stream data)
        {
            data.Position = 0;
            using Image<Rgba32> image = Image.Load(data);
            ResizeOptions resizeOptions = new ResizeOptions();
            resizeOptions.Size = new SixLabors.Primitives.Size(500, 500);
            resizeOptions.Position = AnchorPositionMode.Center;
            resizeOptions.Mode = ResizeMode.Max;

            image.Mutate(x => x
                 .Resize(resizeOptions)
                );
            var memoryStream = new MemoryStream();

            image.SaveAsJpeg(memoryStream,
                new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder()
                {
                    Quality = 100
                }
                ); // Automatic encoder selected based on extension.
            memoryStream.Position = 0;
            return memoryStream;
        }

        public bool SendMail(string title, string to, string message)
        {
            try
            {
                Guid messageId = Guid.NewGuid();

                //Sablon değişkenleri
                message = message.Replace("{{messageId}}", messageId.ToString());
                message = message.Replace("{{mailAddress}}", to);

                MailMessage mesaj = new MailMessage();
                mesaj.From = new MailAddress(_mailConfig.Value.MailAdres);
                mesaj.To.Add(to);
                mesaj.Subject = title;
                mesaj.Body = message;

                mesaj.IsBodyHtml = true; // giden mailin içeriği html olmasını istiyorsak true kalması lazım
                SmtpClient client = new SmtpClient(_mailConfig.Value.MailServer, _mailConfig.Value.Port)
                {
                    Credentials = new NetworkCredential(_mailConfig.Value.MailAdres, _mailConfig.Value.Password),
                    EnableSsl = _mailConfig.Value.UseSsl
                };
                client.Send(mesaj);

                _context.SendMesageList.Add(new Entity.Models.SendMesageList() { Id = messageId, Message = message, Title = title, SendingDate = DateTime.Now, To = to });
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                //
            }
            return true;
        }

        public string GetSablon(string sablon)
        {
            var text = "";
            using (StreamReader sr = new StreamReader(System.IO.Path.Combine(_env.WebRootPath, "mail", "sablon.html")))
            {
                text = sr.ReadToEnd();
            }
            using (StreamReader sr = new StreamReader(System.IO.Path.Combine(_env.WebRootPath, "mail", sablon + ".html")))
            {
                text = text.Replace("{{content}}", sr.ReadToEnd());
            }

            return text;
        }

        public string GetMd5Hash(string input)
        {
            StringBuilder sBuilder = new StringBuilder();
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.

                // Loop through each byte of the hashed data
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
            }
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public void SendMemberRequestMail(MemberRequest r)
        {
            var mail = GetSablon("1register");
            SendMail("Yazgelder Ön Kaydınız Başarıyla Tamamlanmıştır.", r.Email, mail);

            SendMemberRequestAdminMail(r);
        }

        public void SendMemberRequestAdminMail(MemberRequest r)
        {
            var mail = GetSablon("0bos");
            mail = mail.Replace("{{title}}", "Yeni Üye Başvurusu");

            StringBuilder sb = new StringBuilder();
            sb.Append(@"<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"">");
            sb.Append($@"<tr><td>Id</td><td>{r.Id}</td></tr>");
            sb.Append($@"<tr><td>Name</td><td>{r.Name}</td></tr>");
            sb.Append($@"<tr><td>Surname</td><td>{r.Surname}</td></tr>");
            sb.Append($@"<tr><td>ApplicationDate</td><td>{r.ApplicationDate.ToString()}</td></tr>");
            sb.Append($@"<tr><td>Email</td><td>{r.Email}</td></tr>");
            sb.Append($@"<tr><td>Gender</td><td>{r.Gender}</td></tr>");
            sb.Append($@"<tr><td>Name</td><td>{r.Telephone}</td></tr>");
            sb.Append($@"<tr><td>IsHonorary</td><td>{r.IsHonorary}</td></tr>");
            sb.Append($@"<tr><td>Job</td><td>{r.Job}</td></tr>");
            sb.Append($@"<tr><td>Name</td><td>{r.Reference}</td></tr>");
            sb.Append($@"<tr><td>Name</td><td>{r.Reference2}</td></tr>");
            sb.Append(@"</table>");

            mail = mail.Replace("{{body}}", sb.ToString());

            var q = _mailConfig.Value.AdminMails.Split(";", StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in q)
            {
                SendMail("Yeni Üye Başvurusu.", item, mail);
            }
        }

        public void SendContractAdminMail(Contact r)
        {
            var mail = GetSablon("0bos");
            mail = mail.Replace("{{title}}", "İletişim");

            StringBuilder sb = new StringBuilder();
            sb.Append(@"<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"">");
            sb.Append($@"<tr><td>Id</td><td>{r.Id}</td></tr>");
            sb.Append($@"<tr><td>Name</td><td>{r.Name}</td></tr>");
            sb.Append($@"<tr><td>Surname</td><td>{r.Surname}</td></tr>");
            sb.Append($@"<tr><td>Date</td><td>{r.Date.ToString()}</td></tr>");
            sb.Append($@"<tr><td>Type</td><td>{r.Type}</td></tr>");
            sb.Append($@"<tr><td>Message</td><td>{r.Message}</td></tr>");
            sb.Append(@"</table>");

            mail = mail.Replace("{{body}}", sb.ToString());

            var q = _mailConfig.Value.AdminMails.Split(";", StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in q)
            {
                SendMail("İletişim.", item, mail);
            }
        }

        public List<SmsInfo> GetSmsInfos()
        {
            var list = new List<SmsInfo>();

            list.Add(GetGuvenSmsInfo());
            list.Add(GetMesajPanaliSmsInfo());

            return list;
        }

        public void SendSms(List<string> numbers, string message, byte smsType)
        {
            string title = "";
            switch (smsType)
            {
                case 0:
                default:
                    title = _guvenSmsConfig.Value.Orjinator;
                    SendGuvenSms(numbers, message);
                    break;

                case 1:
                    title = _smsVitriniConfig.Value.Baslik;
                    SendMesajPaneliSms(numbers, message);
                    break;
            }
            var entity = numbers.Select(x => new SendMesageList() { To = x, Id = Guid.NewGuid(), SendingDate = DateTime.Now, Message = message, Title = title }).ToList();
            _context.SendMesageList.AddRange(entity);
            _context.SaveChanges();
        }

        public void SendSmsReport()
        {
            var q = this.GetSmsInfos();
            StringBuilder sb = new StringBuilder();
            sb.Append("Merhaba Hacım. Kalan Smsler; \n");
            foreach (var item in q)
            {
                sb.Append($"{item.Ordinator}={item.SmsCount}\n");
            }
            sb.Append("Hayırlı Haftalar.");
            var ls = _reportConfig.Value.SmsSenderNumbers.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            this.SendSms(ls, sb.ToString(), 0);
        }

        public void SendSmsAdmins(string message)
        {
            var ls = _reportConfig.Value.SmsSenderNumbers.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            if (message.Length > 120)
                message = message.Substring(0, 120);
            this.SendSms(ls, message, 0);
        }

        public void SendBirthdaySms()
        {
            var s = _context.Member.Where(x => x.Active).ToList();
            var l = s.Where(x => x.Birthday.Day == DateTime.Now.Day && x.Birthday.Month == DateTime.Now.Month).ToList();
            foreach (var item in l)
            {
                this.SendSms(new List<string>() { item.Telephone }, $"Sayın {item.Name} {item.Surname}. Doğum gününüzü en içten dileklerimizle kutlar nice senelerde sizlerin yanında olmaktan kıvanç duyarız.", 1);
            }
        }

        private void SendGuvenSms(List<string> numbers, string message)
        {
            string str;
            message = message.Replace("@", "|01|");
            message = message.Replace("£", "|02|");
            message = message.Replace("$", "|03|");
            message = message.Replace("€", "|05|");
            message = message.Replace("_", "|14|");
            message = message.Replace("!", "|26|");
            message = message.Replace("'", "|27|");
            message = message.Replace("#", "|28|");
            message = message.Replace("%", "|30|");
            message = message.Replace("&", "|31|");
            message = message.Replace("(", "|33|");
            message = message.Replace(")", "|34|");
            message = message.Replace("*", "|35|");
            message = message.Replace("+", "|36|");
            message = message.Replace("-", "|38|");
            message = message.Replace("/", "|39|");
            message = message.Replace(":", "|40|");
            message = message.Replace(";", "|41|");
            message = message.Replace("<", "|42|");
            message = message.Replace("=", "|43|");
            message = message.Replace(">", "|44|");
            message = message.Replace("?", "|45|");
            message = message.Replace("{", "|46|");
            message = message.Replace("}", "|47|");
            message = message.Replace("~", "|49|");
            message = message.Replace("^", "|51|");
            message = message.Replace("ö", "|62|");
            message = message.Replace("ü", "|63|");
            message = message.Replace("ç", "|64|");
            message = message.Replace("ş", "|65|");
            message = message.Replace("ğ", "|66|");
            message = message.Replace("ı", "|67|");
            message = message.Replace("Ö", "|68|");
            message = message.Replace("Ü", "|69|");
            message = message.Replace("Ç", "|70|");
            message = message.Replace("Ş", "|71|");
            message = message.Replace("İ", "|72|");
            message = message.Replace("Ğ", "|73|");
            message = message.Replace("\n", "|61|");
            var t = string.Join(',', numbers.ToArray());
            using (StreamReader sr = new StreamReader(System.IO.Path.Combine(_env.WebRootPath, "SoapSchema", "TekSmsiBirdenCokNumarayaGonder.xml")))
            {
                str = sr.ReadToEnd();
                str = str.Replace("{{kullanicino}}", _guvenSmsConfig.Value.KullaniciNo);
                str = str.Replace("{{kullaniciadi}}", _guvenSmsConfig.Value.KullaniciAdi);
                str = str.Replace("{{sifre}}", _guvenSmsConfig.Value.Sifre);
                str = str.Replace("{{orjinator}}", _guvenSmsConfig.Value.Orjinator);
                str = str.Replace("{{numaralar}}", t);
                str = str.Replace("{{mesaj}}", message);
                str = str.Replace("{{zaman}}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                var client = new RestClient("http://www.guvensms.com/webservis/service.php");
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.Body = new RequestBody("text/xml", "", str);
                IRestResponse response = client.Execute(request);
            }
        }

        private void SendMesajPaneliSms(List<string> numbers, string message)
        {
            smsData MesajPaneli = new smsData();
            MesajPaneli.user = new UserInfo(_smsVitriniConfig.Value.Name, _smsVitriniConfig.Value.Pass);
            MesajPaneli.msgBaslik = _smsVitriniConfig.Value.Baslik;
            MesajPaneli.msgData.Add(new msgdata(numbers, message));
            MesajPaneli.tr = true;
            ReturnValue ReturnData = MesajPaneli.DoPost("http://api.mesajpaneli.com/json_api/", true, true);
        }

        private SmsInfo GetGuvenSmsInfo()
        {
            var info = new SmsInfo();
            info.Ordinator = _guvenSmsConfig.Value.Orjinator;

            var t = GetGuvenSmsInfoRequest();
            int.TryParse(t, out var i);
            info.SmsCount = i;
            return info;
        }

        private SmsInfo GetMesajPanaliSmsInfo()
        {
            var info = new SmsInfo();
            info.Ordinator = _smsVitriniConfig.Value.Baslik;
            smsData MesajPaneli = new smsData();
            MesajPaneli.user = new UserInfo(_smsVitriniConfig.Value.Name, _smsVitriniConfig.Value.Pass);

            ReturnValue ReturnData = MesajPaneli.DoPost("http://api.mesajpaneli.com/json_api/login/", true, true);
            info.SmsCount = ReturnData.userData.orjinli;

            return info;
        }

        private string GetGuvenSmsInfoRequest()
        {
            var c = "Toplam SMS =";
            var str = "";
            using (StreamReader sr = new StreamReader(System.IO.Path.Combine(_env.WebRootPath, "SoapSchema", "UyeBilgisiSorgula.xml")))
            {
                str = sr.ReadToEnd();
                str = str.Replace("{{kullanicino}}", _guvenSmsConfig.Value.KullaniciNo);
                str = str.Replace("{{kullaniciadi}}", _guvenSmsConfig.Value.KullaniciAdi);
                str = str.Replace("{{sifre}}", _guvenSmsConfig.Value.Sifre);

                var client = new RestClient("http://www.guvensms.com/webservis/service.php");
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.Body = new RequestBody("text/xml", "", str);
                IRestResponse response = client.Execute(request);
                var t = response.Content;
                t = t.Substring(t.IndexOf(c) + c.Length, t.IndexOf("\n", t.IndexOf(c)) - t.IndexOf(c) - c.Length);
                return t;
            }
        }
    }
}