using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Yazgelder.Areas.Admin.Models;
using Yazgelder.Entity;
using Yazgelder.Entity.Models;
using Yazgelder.Infrastructure;

namespace Yazgelder.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("/Admin/BoardDecisions")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class BoardDecisionsController : BaseController
    {
        private readonly Context _context;
        private readonly ISysFunctions _sysFunctions;

        public BoardDecisionsController
            (
            Context context,
            ISysFunctions sysFunctions
            )
        {
            _context = context;
            _sysFunctions = sysFunctions;
        }

        [HttpGet("/Admin/BoardDecisions")]
        public IActionResult Index()
        {
            return View(_context.BoardDecisions.AsEnumerable().OrderByDescending(x => x.Date).ToList());
        }

        [HttpGet("/Admin/BoardDecisions/EditOrInsert/{id}")]
        public IActionResult EditOrInsert(Guid id)
        {
            BoardDecisions q = null;
            if (id != Guid.Empty)
            {
                q = _context.BoardDecisions.FirstOrDefault(x => x.Id == id);
            }

            if (q == null)
            {
                q = new BoardDecisions();
                q.Id = Guid.Empty;
            }
            return View(q);
        }

        [HttpPost("/Admin/BoardDecisions/EditOrInsert")]
        public IActionResult EditOrInsert(BoardDecisions model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool isNew = false;
            var q = _context.BoardDecisions.FirstOrDefault(x => x.Id == model.Id);

            if (q == null)
            {
                q = new BoardDecisions();
                isNew = true;
            }

            q.Body = model.Body;
            q.Date = model.Date;
            q.DateOrderNo = model.DateOrderNo;
            q.Number = model.Number;
            q.Title = model.Title;

            if (isNew)
            {
                _context.BoardDecisions.Add(q);
            }
            else
            {
                _context.BoardDecisions.Update(q);
            }
            FileSave(q, _context);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet("/Admin/BoardDecisions/Delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            var q = _context.BoardDecisions.FirstOrDefault(x => x.Id == id);
            if (q != null)
            {
                _context.BoardDecisions.Remove(q);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}