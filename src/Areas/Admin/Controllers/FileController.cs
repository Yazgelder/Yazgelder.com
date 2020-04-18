using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yazgelder.Areas.Admin.Models;
using Yazgelder.Entity;
using Yazgelder.Infrastructure;

namespace Yazgelder.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("/Admin/File")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class FileController : Controller
    {
        private readonly Context _context;
        private readonly ISysFunctions _sysFunctions;
        private Microsoft.AspNetCore.Hosting.IWebHostEnvironment _hostingEnvironment;

        public FileController
        (
            Context context,
            ISysFunctions sysFunctions,
            Microsoft.AspNetCore.Hosting.IWebHostEnvironment environment
        )
        {
            _hostingEnvironment = environment;
            _context = context;
            _sysFunctions = sysFunctions;
        }

        [HttpGet("/Admin/File/{id}")]
        public IActionResult Index(Guid id, byte type)
        {
            var t = _context
                .Pictures
                .Where(x => x.RelationId == id && x.RecordType == type)
                .Select(x => new FileModel() { Id = x.Id, FileName = x.Picture, IsDeleted = false, Title = x.Title, Type = type, SubType = x.RecordType2 })
                .ToList();
            if (t == null || !t.Any())
                t = new List<FileModel>();
            return Json(t);
        }

        [HttpPost("/Admin/File/{id}")]
        public IActionResult Index(Guid id, [FromForm] IFormFile file, byte type, byte type2, string title = "")
        {
            var p = new System.IO.DirectoryInfo(System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "photo", type.ToString()));
            if (!p.Exists)
            {
                p.Create();
            }
            var fName = file.FileName;

            var f = new System.IO.FileInfo(System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "photo", type.ToString(), fName));
            while (f.Exists)
            {
                fName = "_" + fName;
                f = new System.IO.FileInfo(System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "photo", type.ToString(), fName));
            }
            using (var fileStream = new FileStream(f.FullName, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            var pName = f.FullName.Replace(_hostingEnvironment.WebRootPath, "");
            var e = new Entity.Models.Pictures() { Id = Guid.NewGuid(), Picture = pName, RecordType = type, RelationId = id, Title = title, RecordType2 = type2 };
            _context
                .Pictures.Add(e);
            _context.SaveChanges();

            return Json(new FileModel() { Id = e.Id, FileName = e.Picture, IsDeleted = false, Title = e.Title, Type = e.RecordType, SubType = e.RecordType2 });
        }
    }
}