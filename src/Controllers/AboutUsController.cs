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
    public class AboutUsController : Controller
    {
        private readonly Context _context;

        public AboutUsController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Charter()
        {
            return View();
        }

        public IActionResult MissionAndObjectives()
        {
            return View();
        }

        public IActionResult BoardOfDirectors()
        {
            return View();
        }

        public IActionResult TermsOfService()
        {
            return View();
        }

        public IActionResult PrivacyPolicy()
        {
            return View();
        }

        public IActionResult OurStory()
        {
            return View();
        }

        public IActionResult BoardDecisions()
        {
            var t = _context.BoardDecisions.OrderByDescending(x => x.Date).ThenByDescending(x => x.DateOrderNo).Select(x => new BoardDecisions()
            {
                Id = x.Id,
                Body = x.Body,
                Date = x.Date,
                DateOrderNo = x.DateOrderNo,
                Number = x.Number,
                Title = x.Title,
                FileList = _context.Pictures.Where(y => y.RelationId == x.Id && y.RecordType == 1).Select(y => new FileModel() { FileName = y.Picture, Title = y.Title, Type = y.RecordType, SubType = y.RecordType2 }).ToList()
            }).ToList();

            return View(t);
        }

        public IActionResult BoardDecisionsDetail(Guid id)
        {
            var t = _context.BoardDecisions.FirstOrDefault(x => x.Id == id);
            t.FileList = _context.Pictures.Where(y => y.RelationId == t.Id && y.RecordType == 1).Select(y => new FileModel() { FileName = y.Picture, Title = y.Title, Type = y.RecordType, SubType = y.RecordType2 }).ToList();
            return View(t);
        }
    }
}