using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Yazgelder.Entity;
using Yazgelder.Entity.Models;
using Yazgelder.Infrastructure;

namespace Yazgelder.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("/Admin/MemberRequest")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class MemberRequestController : Controller
    {
        private readonly Context _context;
        private readonly ISysFunctions _sysFunctions;

        public MemberRequestController
            (
            Context context,
            ISysFunctions sysFunctions
            )
        {
            _context = context;
            _sysFunctions = sysFunctions;
        }

        [HttpGet("/Admin/MemberRequest")]
        public IActionResult Index()
        {
            return View(_context.MemberRequest.AsEnumerable().OrderBy(x => x.Surname).ThenBy(x => x.Name).ToList());
        }

        [HttpGet("/Admin/MemberRequest/EditOrInsert/{id}")]
        public IActionResult EditOrInsert(Guid id)
        {
            MemberRequest q = null;
            if (id != Guid.Empty)
            {
                q = _context.MemberRequest.FirstOrDefault(x => x.Id == id);
            }

            if (q == null)
            {
                q = new MemberRequest();
                q.Id = Guid.Empty;
            }
            return View(q);
        }

        [HttpPost("/Admin/MemberRequest/EditOrInsert")]
        public IActionResult EditOrInsert(MemberRequest model)
        {
            if (!ModelState.IsValid)
                return View(model);
            bool isNew = false;
            var q = _context.MemberRequest.FirstOrDefault(x => x.Id == model.Id);

            if (q == null)
            {
                q = new MemberRequest();
                isNew = true;
            }

            q.ApplicationDate = model.ApplicationDate;
            q.Email = model.Email;
            q.Name = model.Name;
            q.Gender = model.Gender;
            q.IsHonorary = model.IsHonorary;
            q.Job = model.Job;
            q.Reference = model.Reference;
            q.Reference2 = model.Reference2;
            q.Surname = model.Surname;
            q.Telephone = model.Telephone;
            q.Birthday = model.Birthday.Date;

            if (isNew)
            {
                _context.MemberRequest.Add(q);
            }
            else
            {
                _context.MemberRequest.Update(q);
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet("/Admin/MemberRequest/Delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            var q = _context.MemberRequest.FirstOrDefault(x => x.Id == id);
            if (q != null)
            {
                _context.MemberRequest.Remove(q);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}