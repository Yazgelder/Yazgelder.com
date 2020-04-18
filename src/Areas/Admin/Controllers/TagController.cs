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
    [Route("/Admin/Tag")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class TagController : Controller
    {
        private readonly Context _context;
        private readonly ISysFunctions _sysFunctions;

        public TagController
            (
            Context context,
            ISysFunctions sysFunctions
            )
        {
            _context = context;
            _sysFunctions = sysFunctions;
        }

        [HttpGet("/Admin/Tag")]
        public IActionResult Index()
        {
            return View(_context.Tags.AsEnumerable().OrderByDescending(x => x.Name).ToList());
        }

        [HttpGet("/Admin/Tag/EditOrInsert/{id}")]
        public IActionResult EditOrInsert(Guid id)
        {
            Tag q = null;
            if (id != Guid.Empty)
            {
                q = _context.Tags.FirstOrDefault(x => x.Id == id);
            }

            if (q == null)
            {
                q = new Tag();
                q.Id = Guid.Empty;
            }
            return View(q);
        }

        [HttpPost("/Admin/Tag/EditOrInsert")]
        public IActionResult EditOrInsert(Tag model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool isNew = false;
            var q = _context.Tags.FirstOrDefault(x => x.Id == model.Id);

            if (q == null)
            {
                q = new Tag();
                isNew = true;
            }

            q.LinkName = model.LinkName;
            q.Name = model.Name;

            if (isNew)
            {
                _context.Tags.Add(q);
            }
            else
            {
                _context.Tags.Update(q);
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet("/Admin/Tag/Delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            var q = _context.Tags.FirstOrDefault(x => x.Id == id);
            if (q != null)
            {
                _context.Tags.Remove(q);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}