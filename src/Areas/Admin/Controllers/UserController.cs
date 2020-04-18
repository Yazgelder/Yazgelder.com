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
    [Route("/Admin/User")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class UserController : Controller
    {
        private readonly Context _context;
        private readonly ISysFunctions _sysFunctions;

        public UserController
            (
            Context context,
            ISysFunctions sysFunctions
            )
        {
            _context = context;
            _sysFunctions = sysFunctions;
        }

        [HttpGet("/Admin/User")]
        public IActionResult Index()
        {
            return View(_context.User.AsEnumerable().OrderByDescending(x => x.UserName).ToList());
        }

        [HttpGet("/Admin/User/EditOrInsert/{id}")]
        public IActionResult EditOrInsert(Guid id)
        {
            User q = null;
            if (id != Guid.Empty)
            {
                q = _context.User.FirstOrDefault(x => x.Id == id);
            }

            if (q == null)
            {
                q = new User();
                q.Id = Guid.Empty;
            }
            return View(q);
        }

        [HttpPost("/Admin/User/EditOrInsert")]
        public IActionResult EditOrInsert(User model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool isNew = false;
            var q = _context.User.FirstOrDefault(x => x.Id == model.Id);

            if (q == null)
            {
                q = new User();
                isNew = true;
            }

            q.UserName = model.UserName;

            if (!string.IsNullOrEmpty(model.Password))
                q.Password = _sysFunctions.GetMd5Hash(model.Password);

            if (isNew)
            {
                _context.User.Add(q);
            }
            else
            {
                _context.User.Update(q);
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet("/Admin/User/Delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            var q = _context.User.FirstOrDefault(x => x.Id == id);
            if (q != null)
            {
                _context.User.Remove(q);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}