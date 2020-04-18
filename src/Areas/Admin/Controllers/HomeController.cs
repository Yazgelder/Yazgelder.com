using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Yazgelder.Areas.Admin.Models;
using Yazgelder.Entity;
using Yazgelder.Infrastructure;

namespace Yazgelder.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("/Admin/home")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class HomeController : Controller
    {
        private ISysFunctions _sysFunctions;
        private Context _context;

        public HomeController(ISysFunctions sysFunctions, Context context)

        {
            _sysFunctions = sysFunctions;
            _context = context;
        }

        [HttpGet("/Admin/home")]
        public IActionResult Index()
        {
            return View(new SendSms());
        }

        [HttpPost("/Admin/home/SendMessage")]
        public IActionResult SendMessage(SendSms post)
        {
            if (post.IsStartNameSurname)
            {
                if (post.IsBulk)
                {
                    var t = _context.Member.Where(x => x.Active).ToList();
                    foreach (var item in t)
                    {
                        if (item.Telephone.Length == 10)
                        {
                            _sysFunctions.SendSms(new List<string>() { item.Telephone }, $"Sayın {item.Name} {item.Surname} ;\n {post.Message}", post.IsResmi ? (byte)1 : (byte)0);
                        }
                    }
                }
                else
                {
                    var numbers = post.Numbers.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                    foreach (var tt in numbers)
                    {
                        var t = _context.Member.FirstOrDefault(x => x.Active && x.Telephone == tt);
                        if (t != null)
                        {
                            _sysFunctions.SendSms(new List<string>() { tt }, $"Sayın {t.Name} {t.Surname} ;\n {post.Message}", post.IsResmi ? (byte)1 : (byte)0);
                        }
                    }
                }
            }
            else
            {
                List<string> numbers;

                if (post.IsBulk)
                {
                    numbers = _context.Member.Where(x => x.Active).Select(x => x.Telephone).ToList().Where(x => x.Length == 10).ToList();
                }
                else
                {
                    numbers = post.Numbers.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                }

                if (numbers != null && numbers.Any())
                    _sysFunctions.SendSms(numbers, post.Message, post.IsResmi ? (byte)1 : (byte)0);
            }

            return Redirect("/Admin/home");
        }
    }
}