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
    [Route("/Admin/Contact")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class ContactController : Controller
    {
        private readonly Context _context;
        private readonly ISysFunctions _sysFunctions;

        public ContactController
            (
            Context context,
            ISysFunctions sysFunctions
            )
        {
            _context = context;
            _sysFunctions = sysFunctions;
        }

        [HttpGet("/Admin/Contact")]
        public IActionResult Index()
        {
            return View(_context.Contact.AsEnumerable().OrderByDescending(x => x.Date).ToList());
        }

        [HttpGet("/Admin/Contact/Detail/{id}")]
        public IActionResult Detail(Guid id)
        {
            Contact q = null;
            if (id != Guid.Empty)
            {
                q = _context.Contact.FirstOrDefault(x => x.Id == id);
            }

            if (q == null)
            {
                q = new Contact();
            }
            return View(q);
        }

        [HttpGet("/Admin/Contact/delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            var q = _context.Contact.FirstOrDefault(x => x.Id == id);
            if (q != null)
            {
                _context.Contact.Remove(q);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}