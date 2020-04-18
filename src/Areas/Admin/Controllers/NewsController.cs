using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Yazgelder.Entity;
using Yazgelder.Entity.Models;
using Yazgelder.Infrastructure;

namespace Yazgelder.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("/Admin/News")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class NewsController : BaseController
    {
        private readonly Context _context;
        private readonly ISysFunctions _sysFunctions;

        public NewsController
            (
            Context context,
            ISysFunctions sysFunctions
            ) : base()
        {
            _context = context;
            _sysFunctions = sysFunctions;
        }

        [HttpGet("/Admin/News")]
        public IActionResult Index()
        {
            return View(_context.News.AsEnumerable().OrderByDescending(x => x.SenderDate).ToList());
        }

        [HttpGet("/Admin/News/EditOrInsert/{id}")]
        public IActionResult EditOrInsert(Guid id)
        {
            News q = null;
            if (id != Guid.Empty)
            {
                q = _context.News.Include(x => x.NewsTagsList).Include(x => x.NewsCategoriesList).FirstOrDefault(x => x.Id == id);
            }

            if (q == null)
            {
                q = new News();
                q.Id = Guid.Empty;
            }
            else
            {
                q.Categories = q.NewsCategoriesList.Select(x => x.CategoriesId).ToList();
                q.Tags = q.NewsTagsList.Select(x => x.TagId).ToList();
            }
            ViewBag.Categories = _context.Categories.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();
            ViewBag.Tags = _context.Tags.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();
            return View(q);
        }

        [HttpPost("/Admin/News/EditOrInsert")]
        public IActionResult EditOrInsert(News model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();
                ViewBag.Tags = _context.Tags.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();

                return View(model);
            }
            bool isNew = false;
            var q = _context.News.FirstOrDefault(x => x.Id == model.Id);

            if (q == null)
            {
                q = new News();
                isNew = true;
            }
            else
            {
                _context.NewsCategories.RemoveRange(_context.NewsCategories.Where(x => x.NewsId == model.Id));
                _context.NewsTags.RemoveRange(_context.NewsTags.Where(x => x.NewsId == model.Id));
                _context.SaveChanges();
                q = _context.News.FirstOrDefault(x => x.Id == model.Id);
            }

            q.Body = model.Body;
            q.IsDraft = model.IsDraft;
            q.LinkName = model.LinkName;
            q.Sender = model.Sender;
            q.SenderDate = model.SenderDate;
            q.Title = model.Title;
            q.Type = model.Type;

            if (isNew)
            {
                _context.News.Add(q);
            }
            else
            {
                _context.News.Update(q);
            }

            _context.NewsCategories.AddRange(model.Categories.Select(x => new NewsCategories() { Id = Guid.NewGuid(), NewsId = q.Id, CategoriesId = x }));

            _context.NewsTags.AddRange(model.Tags.Select(x => new NewsTags() { Id = Guid.NewGuid(), NewsId = q.Id, TagId = x }));

            FileSave(q, _context);

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet("/Admin/News/Delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            var q = _context.News.FirstOrDefault(x => x.Id == id);
            if (q != null)
            {
                _context.News.Remove(q);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}