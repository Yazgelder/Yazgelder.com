using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Yazgelder.Entity.Models;
using Yazgelder.Models;

namespace Yazgelder.Infrastructure
{
    public interface ISysFunctions
    {
        MemoryStream StreamImageConvertAndResizeAndCompress(Stream data);

        string GetString(string htmlString, int length);

        bool SendMail(string title, string to, string message);

        string GetSablon(string sablon);

        void SendMemberRequestMail(MemberRequest r);

        void SendContractAdminMail(Contact r);

        string GetMd5Hash(string input);

        List<SmsInfo> GetSmsInfos();

        void SendSms(List<string> numbers, string message, byte smsType);

        void SendSmsReport();

        void SendSmsAdmins(string message);

        void SendBirthdaySms();
    }
}