using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yazgelder.Areas.Admin.Models;
using Yazgelder.Entity;
using Yazgelder.Entity.Models;

namespace Yazgelder.Controllers
{
    public class NewsController : Controller
    {
        private readonly Context _context;

        public NewsController(Context context)
        {
            _context = context;
        }

        public IActionResult Index(Guid cat, Guid tag)
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            var q = _context.News.Where(x => !x.IsDraft);

            var t = q.OrderByDescending(x => x.SenderDate)
                .Include(x => x.NewsTagsList).ThenInclude(x => x.Tag)
                .Include(x => x.NewsCategoriesList).ThenInclude(x => x.Categories)
                .Select(x => new News()
                {
                    Id = x.Id,
                    Body = x.Body,
                    LinkName = x.LinkName,
                    SenderDate = x.SenderDate,
                    Sender = x.Sender,
                    Type = x.Type,
                    NewsCategoriesList = x.NewsCategoriesList,
                    NewsTagsList = x.NewsTagsList,
                    Title = x.Title,
                    FileList = _context.Pictures.Where(y => y.RelationId == x.Id && y.RecordType == 6).Select(y => new FileModel() { FileName = y.Picture, Title = y.Title, Type = y.RecordType, SubType = y.RecordType2 }).ToList()
                }).ToList();
            if (cat != Guid.Empty)
            {
                t = t.Where(x => x.NewsCategoriesList.Any(y => y.CategoriesId == cat)).ToList();
            }
            else if (tag != Guid.Empty)
            {
                t = t.Where(x => x.NewsTagsList.Any(y => y.TagId == tag)).ToList();
            }
            return View(t);
        }

        public IActionResult Detail(Guid id)
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            var t = _context.News
                .Include(x => x.NewsTagsList).ThenInclude(x => x.Tag)
                .Include(x => x.NewsCategoriesList).ThenInclude(x => x.Categories)
                .FirstOrDefault(x => x.Id == id);
            t.FileList = _context.Pictures.Where(y => y.RelationId == t.Id && y.RecordType == 6).Select(y => new FileModel() { FileName = y.Picture, Title = y.Title, Type = y.RecordType, SubType = y.RecordType2 }).ToList();
            return View(t);
        }
    }
}