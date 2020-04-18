using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yazgelder.Entity;
using Yazgelder.Entity.Models;
using Yazgelder.Infrastructure;

namespace Yazgelder.Controllers
{
    public class ContactController : Controller
    {
        private readonly Context _context;
        private readonly ISysFunctions _sysFunction;

        public ContactController(Context context, ISysFunctions sysFunction)
        {
            _context = context;
            _sysFunction = sysFunction;
        }

        public IActionResult Index()
        {
            return View(new Contact());
        }

        [ServiceFilter(typeof(ValidateReCaptchaAttribute))]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Index(Contact model)
        {
            if (ModelState.IsValid)
            {
                Contact r = new Contact
                {
                    Date = DateTime.Now,
                    EMail = model.EMail,
                    Message = model.Message,
                    Name = model.Name,
                    Surname = model.Surname,
                    Type = 1
                };
                _context.Contact.Add(r);
                _context.SaveChanges();
                _sysFunction.SendSmsAdmins($"Yeni iletişim:{r.Name} {r.Surname} \nİletişim Formu");
                _sysFunction.SendContractAdminMail(r);

                return RedirectToAction("ContactOk");
            }
            return View(model);
        }

        public IActionResult ContactOk()
        {
            return View();
        }
    }
}