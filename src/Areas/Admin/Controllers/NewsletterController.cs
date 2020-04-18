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
    [Route("/Admin/Newsletter")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class NewsletterController : Controller
    {
        private readonly Context _context;
        private readonly ISysFunctions _sysFunctions;

        public NewsletterController
            (
            Context context,
            ISysFunctions sysFunctions
            )
        {
            _context = context;
            _sysFunctions = sysFunctions;
        }

        [HttpGet("/Admin/Newsletter")]
        public IActionResult Index()
        {
            return View(_context.Newsletter.AsEnumerable().OrderByDescending(x => x.Date).ToList());
        }

        [HttpGet("/Admin/Newsletter/EditOrInsert/{id}")]
        public IActionResult EditOrInsert(Guid id)
        {
            Newsletter q = null;
            if (id != Guid.Empty)
            {
                q = _context.Newsletter.FirstOrDefault(x => x.Id == id);
            }

            if (q == null)
            {
                q = new Newsletter();
                q.Id = Guid.Empty;
            }
            return View(q);
        }

        [HttpPost("/Admin/Newsletter/EditOrInsert")]
        public IActionResult EditOrInsert(Newsletter model)
        {
            if (!ModelState.IsValid)
                return View(model);
            bool isNew = false;
            var q = _context.Newsletter.FirstOrDefault(x => x.Id == model.Id);

            if (q == null)
            {
                q = new Newsletter();
                isNew = true;
            }

            q.Date = model.Date;
            q.IpAdress = model.IpAdress;
            q.Mail = model.Mail;

            if (isNew)
            {
                _context.Newsletter.Add(q);
            }
            else
            {
                _context.Newsletter.Update(q);
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet("/Admin/Newsletter/Delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            var q = _context.Newsletter.FirstOrDefault(x => x.Id == id);
            if (q != null)
            {
                _context.Newsletter.Remove(q);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}