using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coravel.Invocable;
using Yazgelder.Infrastructure;

namespace Yazgelder.Jobs
{
    public class SmsReportSmsSenderJobs : IInvocable
    {
        private ISysFunctions _sysFunctions;

        public SmsReportSmsSenderJobs(ISysFunctions sysFunctions)
        {
            _sysFunctions = sysFunctions;
        }

        public async Task Invoke()
        {
            _sysFunctions.SendSmsReport();
        }
    }
}