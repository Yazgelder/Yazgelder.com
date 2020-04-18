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
    [Route("/Admin/Events")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class EventsController : BaseController
    {
        private readonly Context _context;
        private readonly ISysFunctions _sysFunctions;

        public EventsController
            (
            Context context,
            ISysFunctions sysFunctions
            ) : base()
        {
            _context = context;
            _sysFunctions = sysFunctions;
        }

        [HttpGet("/Admin/Events")]
        public IActionResult Index()
        {
            return View(_context.Events.AsEnumerable().OrderByDescending(x => x.StartDate).ThenBy(x => x.EndDate).ToList());
        }

        [HttpGet("/Admin/Events/EditOrInsert/{id}")]
        public IActionResult EditOrInsert(Guid id)
        {
            Events q = null;
            if (id != Guid.Empty)
            {
                q = _context.Events.FirstOrDefault(x => x.Id == id);
            }

            if (q == null)
            {
                q = new Events();
                q.Id = Guid.Empty;
            }
            return View(q);
        }

        [HttpPost("/Admin/Events/EditOrInsert")]
        public IActionResult EditOrInsert(Events model)
        {
            if (!ModelState.IsValid)
                return View(model);
            bool isNew = false;
            var q = _context.Events.FirstOrDefault(x => x.Id == model.Id);

            if (q == null)
            {
                q = new Events();
                isNew = true;
            }

            q.Comment = model.Comment;
            q.EndDate = model.EndDate;
            q.IsCancel = model.IsCancel;
            q.StartDate = model.StartDate;
            q.Title = model.Title;

            if (isNew)
            {
                _context.Events.Add(q);
            }
            else
            {
                _context.Events.Update(q);
            }
            FileSave(q, _context);

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet("/Admin/Events/Delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            var q = _context.Events.FirstOrDefault(x => x.Id == id);
            if (q != null)
            {
                _context.Events.Remove(q);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}