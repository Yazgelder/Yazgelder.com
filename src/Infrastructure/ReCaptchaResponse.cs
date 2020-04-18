using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Infrastructure
{
    public class ReCaptchaResponse
    {
        public bool success { get; set; }
        public string challenge_ts { get; set; }
        public string hostname { get; set; }
        public string[] errorcodes { get; set; }
    }
}
