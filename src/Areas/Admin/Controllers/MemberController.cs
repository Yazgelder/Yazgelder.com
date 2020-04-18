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
    [Route("/Admin/Member")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class MemberController : BaseController
    {
        private readonly Context _context;
        private readonly ISysFunctions _sysFunctions;

        public MemberController
            (
            Context context,
            ISysFunctions sysFunctions
            ) : base()
        {
            _context = context;
            _sysFunctions = sysFunctions;
        }

        [HttpGet("/Admin/Member")]
        public IActionResult Index()
        {
            return View(_context.Member.AsEnumerable().OrderBy(x => x.Surname).ThenBy(x => x.Name).ToList());
        }

        [HttpGet("/Admin/Member/EditOrInsert/{id}")]
        public IActionResult EditOrInsert(Guid id)
        {
            Member q = null;
            if (id != Guid.Empty)
            {
                q = _context.Member.FirstOrDefault(x => x.Id == id);
            }

            if (q == null)
            {
                q = new Member();
                q.Id = Guid.Empty;
            }
            return View(q);
        }

        [HttpPost("/Admin/Member/EditOrInsert")]
        public IActionResult EditOrInsert(Member model)
        {
            if (!ModelState.IsValid)
                return View(model);
            bool isNew = false;
            var q = _context.Member.FirstOrDefault(x => x.Id == model.Id);

            if (q == null)
            {
                q = new Member();
                isNew = true;
            }

            q.Active = model.Active;
            q.ApplicationDate = model.ApplicationDate;
            q.ApprovalDate = model.ApprovalDate;
            q.Email = model.Email;
            q.Name = model.Name;
            q.FatherName = model.FatherName;
            q.Gender = model.Gender;
            q.IsHonorary = model.IsHonorary;
            q.Job = model.Job;
            q.MotherName = model.MotherName;
            q.Nationality = model.Nationality;
            q.PlaceOfResidence = model.PlaceOfResidence;
            q.Reference = model.Reference;
            q.Reference2 = model.Reference2;
            q.Surname = model.Surname;
            q.TCIdentity = model.TCIdentity;
            q.Telephone = model.Telephone;
            q.Birthday = model.Birthday.Date;
            if (isNew)
            {
                _context.Member.Add(q);
            }
            else
            {
                _context.Member.Update(q);
            }

            FileSave(q, _context);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet("/Admin/Member/Delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            var q = _context.Member.FirstOrDefault(x => x.Id == id);
            if (q != null)
            {
                _context.Member.Remove(q);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}