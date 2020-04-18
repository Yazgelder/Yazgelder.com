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
    public class MemberController : Controller
    {
        private readonly Context _context;
        private readonly ISysFunctions _sysFunction;

        public MemberController(Context context, ISysFunctions sysFunction)
        {
            _context = context;
            _sysFunction = sysFunction;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GeneralAssembly()
        {
            return View(new MemberRequest());
        }

        [ServiceFilter(typeof(ValidateReCaptchaAttribute))]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult GeneralAssembly(MemberRequest model)
        {
            return SaveData(model, false);
        }

        public IActionResult Honorary()
        {
            return View(new MemberRequest());
        }

        [ServiceFilter(typeof(ValidateReCaptchaAttribute))]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Honorary(MemberRequest model)
        {
            return SaveData(model, true);
        }

        public IActionResult MemberRequestAccept()
        {
            return View();
        }

        private IActionResult SaveData(MemberRequest model, bool isHonorary)
        {
            if (ModelState.IsValid)
            {
                MemberRequest r = new MemberRequest();
                r.ApplicationDate = DateTime.Now;
                r.Email = model.Email;
                r.Gender = model.Gender;
                r.IsHonorary = isHonorary;
                r.Name = model.Name;
                r.Job = model.Job;
                r.Reference = model.Reference;
                r.Reference2 = model.Reference2;
                r.Surname = model.Surname;
                r.Telephone = model.Telephone;

                _context.MemberRequest.Add(r);
                _context.SaveChanges();

                _sysFunction.SendSmsAdmins($"Yeni üye talebi: Fahri:{r.IsHonorary},\n {r.Name} {r.Surname} {r.Email}");
                _sysFunction.SendMemberRequestMail(r);

                return RedirectToAction("MemberRequestAccept");
            }
            return View(model);
        }
    }
}