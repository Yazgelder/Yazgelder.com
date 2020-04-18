using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Yazgelder.Areas.Admin.Models;
using Yazgelder.Entity;
using Yazgelder.Entity.Models;

namespace Yazgelder.Controllers
{
    public class EventsController : Controller
    {
        private readonly Context _context;

        public EventsController(Context context)
        {
            _context = context;
        }

        public IActionResult Index(Guid cat, Guid tag)
        {
            var q = _context.Events;

            var t = q.OrderByDescending(x => x.StartDate)
                .Select(x => new Events()
                {
                    Id = x.Id,
                    Comment = x.Comment,
                    EndDate = x.EndDate,
                    StartDate = x.StartDate,
                    IsCancel = x.IsCancel,
                    Title = x.Title,
                    FileList = _context.Pictures.Where(y => y.RelationId == x.Id && y.RecordType == 3).Select(y => new FileModel() { FileName = y.Picture, Title = y.Title, Type = y.RecordType, SubType = y.RecordType2 }).ToList()
                }).ToList();

            return View(t);
        }

        public IActionResult Detail(Guid id)
        {
            var t = _context.Events
                .FirstOrDefault(x => x.Id == id);
            t.FileList = _context.Pictures.Where(y => y.RelationId == t.Id && y.RecordType == 3).Select(y => new FileModel() { FileName = y.Picture, Title = y.Title, Type = y.RecordType, SubType = y.RecordType2 }).ToList();
            return View(t);
        }
    }
}