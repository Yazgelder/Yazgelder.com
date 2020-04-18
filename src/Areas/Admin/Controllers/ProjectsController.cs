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
    [Route("/Admin/Projects")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class ProjectsController : BaseController
    {
        private readonly Context _context;
        private readonly ISysFunctions _sysFunctions;

        public ProjectsController
            (
            Context context,
            ISysFunctions sysFunctions
            ) : base()
        {
            _context = context;
            _sysFunctions = sysFunctions;
        }

        [HttpGet("/Admin/Projects")]
        public IActionResult Index()
        {
            return View(_context.Projects.AsEnumerable().OrderByDescending(x => x.Year).ToList());
        }

        [HttpGet("/Admin/Projects/EditOrInsert/{id}")]
        public IActionResult EditOrInsert(Guid id)
        {
            Projects q = null;
            if (id != Guid.Empty)
            {
                q = _context.Projects.FirstOrDefault(x => x.Id == id);
            }

            if (q == null)
            {
                q = new Projects();
                q.Id = Guid.Empty;
            }
            return View(q);
        }

        [HttpPost("/Admin/Projects/EditOrInsert")]
        public IActionResult EditOrInsert(Projects model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool isNew = false;
            var q = _context.Projects.FirstOrDefault(x => x.Id == model.Id);

            if (q == null)
            {
                q = new Projects();
                isNew = true;
            }

            q.Content = model.Content;
            q.Order = model.Order;
            q.Technology = model.Technology;
            q.Title = model.Title;
            q.Year = model.Year;

            if (isNew)
            {
                _context.Projects.Add(q);
            }
            else
            {
                _context.Projects.Update(q);
            }

            FileSave(q, _context);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet("/Admin/Projects/Delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            var q = _context.Projects.FirstOrDefault(x => x.Id == id);
            if (q != null)
            {
                _context.Projects.Remove(q);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}