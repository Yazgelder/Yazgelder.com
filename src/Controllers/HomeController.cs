using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Yazgelder.Entity;
using Yazgelder.Entity.Models;
using Yazgelder.Infrastructure;
using Yazgelder.Models;

namespace Yazgelder.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Context _context;
        private readonly ISysFunctions _sysFunctions;

        public HomeController(
            ILogger<HomeController> logger,
            ISysFunctions sysFunctions,
            Context context)
        {
            _logger = logger;
            _context = context;
            _sysFunctions = sysFunctions;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PostLink(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException("id");

            var f = _context.Link.FirstOrDefault(x => x.Id == id);
            if (f != null)
            {
                f.Count++;
                _context.Link.Update(f);
                _context.SaveChanges();
            }
            return Redirect(f.LinkName);
        }

        public IActionResult SiteMap()
        {
            List<SiteMapModel> list = new List<SiteMapModel>();
            //Statik Sayfalar
            //Ana Sayfa
            list.Add(new SiteMapModel() { Url = "Home/Index" });
            list.Add(new SiteMapModel() { Url = "Haberler/Index" });
            list.Add(new SiteMapModel() { Url = "Hakkimizda/Index" });
            list.Add(new SiteMapModel() { Url = "Hakkimizda/Hikayemiz" });
            list.Add(new SiteMapModel() { Url = "Hakkimizda/Tuzuk" });
            list.Add(new SiteMapModel() { Url = "Hakkimizda/MisyonVeHedefler" });
            list.Add(new SiteMapModel() { Url = "Hakkimizda/YonetimKuruluKararlari" });
            list.Add(new SiteMapModel() { Url = "Hakkimizda/YonetimKurulu" });
            list.Add(new SiteMapModel() { Url = "Hakkimizda/HizmetSartlari" });
            list.Add(new SiteMapModel() { Url = "Hakkimizda/GizlilikPolitikasi" });
            list.Add(new SiteMapModel() { Url = "Iletisim" });
            list.Add(new SiteMapModel() { Url = "Projeler" });
            list.Add(new SiteMapModel() { Url = "Projeler/Guru" });
            list.Add(new SiteMapModel() { Url = "Projeler/HayatiKodla" });
            list.Add(new SiteMapModel() { Url = "Projeler/YazilimciEvi" });
            list.Add(new SiteMapModel() { Url = "Uyelik" });
            list.Add(new SiteMapModel() { Url = "Uyelik/GenelKurulUyesi" });
            list.Add(new SiteMapModel() { Url = "Uyelik/FahriUye" });

            //Taglar
            foreach (var item in _context.Tags)
            {
                list.Add(new SiteMapModel() { Url = "Haberler/Tag/" + item.LinkName });
            }

            //NewsList
            foreach (var item in _context.News)
            {
                list.Add(new SiteMapModel() { Url = "Haberler/Detay/" + item.LinkName, Image = "images/Update/", ImageAlt = item.Title });
            }

            //Categories
            foreach (var item in _context.NewsCategories.Select(x => x.Categories).Distinct())
            {
                list.Add(new SiteMapModel() { Url = "Haberler/Kategori/" + item });
            }

            //kurul
            foreach (var item in _context.BoardDecisions)
            {
                list.Add(new SiteMapModel() { Url = $@"Hakkimizda/YonetimKuruluKararDetay/{item.Number}/{item.Title}", Image = "images/blog-details.jpg", ImageAlt = item.Number + " " + item.Title });
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
            sb.Append(@"<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9""  xmlns:image=""http://www.google.com/schemas/sitemap-image/1.1""   xmlns:video=""http://www.google.com/schemas/sitemap-video/1.1"">");
            foreach (var item in list)
            {
                sb.Append($"<url>");
                sb.Append($"<loc>http://www.yazgelder.org/{item.Url}</loc>");
                if (!string.IsNullOrEmpty(item.Image))
                {
                    sb.Append($"<image:image>");
                    sb.Append($"<image:loc>http://www.yazgelder.org/{item.Image}</image:loc>");
                    if (!string.IsNullOrEmpty(item.ImageAlt))
                        sb.Append($"<image:caption>{item.ImageAlt}</image:caption>");
                    sb.Append($"</image:image>");
                }
                sb.Append($"</url>");
            }
            sb.Append(@"</urlset>");

            return Content(sb.ToString(), "application/xml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [ServiceFilter(typeof(ValidateReCaptchaAttribute))]
        [HttpPost]
        public IActionResult Newsletter(Newsletter model)
        {
            if (ModelState.IsValid)
            {
                var f = new Newsletter
                {
                    Date = DateTime.Now,
                    IpAdress = "",
                    Mail = model.Mail
                };
                _context.Newsletter.Add(f);
                _context.SaveChanges();
                return Redirect("/Contact/ContactOK");
            }
            return View(model);
        }
    }
}