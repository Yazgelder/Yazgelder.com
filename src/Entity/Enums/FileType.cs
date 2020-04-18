using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yazgelder.Entity.Enums
{
    public enum FileType :byte
    {
        None=0,
        BoardDecisions=1,
        Events=2,
        Link=3,
        Member=4,
        News=5,
        Projects=6,
        CompanyLogo=7,
        Company=8,
        CompanyComment=9,
    }
}
