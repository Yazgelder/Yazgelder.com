using Yazgelder.MesajPaneli.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Text;

namespace Yazgelder.MesajPaneli
{
    public class smsData
    {
        public List<msgdata> msgData = new List<msgdata>();
        public Tarih tarih = new Tarih();
        public UserInfo user;
        public string msgBaslik;
        public string refno;
        public int limit;
        public bool tr;

        private string Base64Encode(string plainText)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
        }

        private string Base64Decode(string encodedString)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encodedString));
        }

        private string Serialize()
        {
            return JsonConvert.SerializeObject((object)this);
        }

        public ReturnValue DoPost(string URL, bool isPostBase64, bool isReturnBase64)
        {
            string str1 = "data=";
            string str2 = !isPostBase64 ? str1 + this.Serialize() : str1 + this.Base64Encode(this.Serialize());
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
            ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)((param0, param1, param2, param3) => true);
            httpWebRequest.Method = "post";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            string encodedString = (string)null;
            try
            {
                using (Stream requestStream = httpWebRequest.GetRequestStream())
                {
                    using (StreamWriter streamWriter = new StreamWriter(requestStream))
                        streamWriter.Write(str2);
                }
                using (HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    Console.WriteLine();
                    encodedString = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                }
                return (ReturnValue)JsonConvert.DeserializeObject<ReturnValue>(!isReturnBase64 ? encodedString : this.Base64Decode(encodedString));
            }
            catch (Exception ex)
            {
                return new ReturnValue()
                {
                    status = false,
                    error = ex.Message
                };
            }
        }
    }
}