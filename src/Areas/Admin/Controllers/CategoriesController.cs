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
    [Route("/Admin/Categories")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class CategoriesController : Controller
    {
        private readonly Context _context;
        private readonly ISysFunctions _sysFunctions;

        public CategoriesController
            (
            Context context,
            ISysFunctions sysFunctions
            )
        {
            _context = context;
            _sysFunctions = sysFunctions;
        }

        [HttpGet("/Admin/Categories")]
        public IActionResult Index()
        {
            return View(_context.Categories.AsEnumerable().OrderBy(x => x.Name).ToList());
        }

        [HttpGet("/Admin/Categories/EditOrInsert/{id}")]
        public IActionResult EditOrInsert(Guid id)
        {
            Categories q = null;
            if (id != Guid.Empty)
            {
                q = _context.Categories.FirstOrDefault(x => x.Id == id);
            }

            if (q == null)
            {
                q = new Categories();
                q.Id = Guid.Empty;
            }

            return View(q);
        }

        [HttpPost("/Admin/Categories/EditOrInsert")]
        public IActionResult EditOrInsert(Categories model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool isNew = false;
            var q = _context.Categories.FirstOrDefault(x => x.Id == model.Id);

            if (q == null)
            {
                q = new Categories();
                isNew = true;
            }

            q.Name = model.Name;

            if (isNew)
            {
                _context.Categories.Add(q);
            }
            else
            {
                _context.Categories.Update(q);
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet("/Admin/Categories/Delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            var q = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (q != null)
            {
                _context.Categories.Remove(q);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}