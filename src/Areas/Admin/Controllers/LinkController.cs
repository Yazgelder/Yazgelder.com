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
    [Route("/Admin/Link")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class LinkController : Controller
    {
        private readonly Context _context;
        private readonly ISysFunctions _sysFunctions;

        public LinkController
            (
            Context context,
            ISysFunctions sysFunctions
            )
        {
            _context = context;
            _sysFunctions = sysFunctions;
        }

        [HttpGet("/Admin/Link")]
        public IActionResult Index()
        {
            return View(_context.Link.AsEnumerable().OrderBy(x => x.LinkName).ToList());
        }

        [HttpGet("/Admin/Link/EditOrInsert/{id}")]
        public IActionResult EditOrInsert(Guid id)
        {
            Link q = null;
            if (id != Guid.Empty)
            {
                q = _context.Link.FirstOrDefault(x => x.Id == id);
            }

            if (q == null)
            {
                q = new Link();
                q.Id = Guid.Empty;
            }
            return View(q);
        }

        [HttpPost("/Admin/Link/EditOrInsert")]
        public IActionResult EditOrInsert(Link model)
        {
            if (!ModelState.IsValid)
                return View(model);
            bool isNew = false;
            var q = _context.Link.FirstOrDefault(x => x.Id == model.Id);

            if (q == null)
            {
                q = new Link();
                isNew = true;
            }

            q.Count = model.Count;
            q.LinkName = model.LinkName;
            q.Name = model.Name;

            if (isNew)
            {
                _context.Link.Add(q);
            }
            else
            {
                _context.Link.Update(q);
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet("/Admin/Link/Delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            var q = _context.Link.FirstOrDefault(x => x.Id == id);
            if (q != null)
            {
                _context.Link.Remove(q);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}