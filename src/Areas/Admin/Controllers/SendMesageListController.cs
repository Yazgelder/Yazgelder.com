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
    [Route("/Admin/SendMesageList")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class SendMesageListController : Controller
    {
        private readonly Context _context;
        private readonly ISysFunctions _sysFunctions;

        public SendMesageListController
            (
            Context context,
            ISysFunctions sysFunctions
            )
        {
            _context = context;
            _sysFunctions = sysFunctions;
        }

        [HttpGet("/Admin/SendMesageList")]
        public IActionResult Index()
        {
            return View(_context.SendMesageList.AsEnumerable().OrderByDescending(x => x.SendingDate).ToList());
        }

        [HttpGet("/Admin/SendMesageList/Detail/{id}")]
        public IActionResult Detail(Guid id)
        {
            SendMesageList q = null;
            if (id != Guid.Empty)
            {
                q = _context.SendMesageList.FirstOrDefault(x => x.Id == id);
            }

            if (q == null)
                q = new SendMesageList();

            return View();
        }

        [HttpGet("/Admin/SendMesageList/Delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            var q = _context.SendMesageList.FirstOrDefault(x => x.Id == id);
            if (q != null)
            {
                _context.SendMesageList.Remove(q);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}