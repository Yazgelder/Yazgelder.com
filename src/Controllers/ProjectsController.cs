using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yazgelder.Areas.Admin.Models;
using Yazgelder.Entity;
using Yazgelder.Entity.Models;
using Yazgelder.Infrastructure;

namespace Yazgelder.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly Context _context;
        private readonly ISysFunctions _sysFunction;

        public ProjectsController(Context context, ISysFunctions sysFunction)
        {
            _context = context; _sysFunction = sysFunction;
        }

        public IActionResult Index()
        {
            var t = _context.Projects.OrderByDescending(x => x.Order).Select(x => new Projects()
            {
                Id = x.Id,
                Technology = x.Technology,
                Year = x.Year,
                Title = x.Title,
                FileList = _context.Pictures.Where(y => y.RelationId == x.Id && y.RecordType == 6).Select(y => new FileModel() { FileName = y.Picture, Title = y.Title, Type = y.RecordType, SubType = y.RecordType2 }).ToList()
            }).ToList();

            return View(t);
        }

        public IActionResult Detail(Guid id)
        {
            var t = _context.Projects.FirstOrDefault(x => x.Id == id);
            t.FileList = _context.Pictures.Where(y => y.RelationId == t.Id && y.RecordType == 6).Select(y => new FileModel() { FileName = y.Picture, Title = y.Title, Type = y.RecordType, SubType = y.RecordType2 }).ToList();
            return View(t);
        }

        public IActionResult Proposal()
        {
            return View(new ProjectProposal());
        }

        [ServiceFilter(typeof(ValidateReCaptchaAttribute))]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult ProposalPost(ProjectProposal model)
        {
            if (ModelState.IsValid)
            {
                ProjectProposal entity = new ProjectProposal
                {
                    ProjectExplanation = model.ProjectExplanation,
                    ProjectName = model.ProjectName,
                    SenderDate = model.SenderDate,
                    SenderMail = model.SenderMail,
                    SenderName = model.SenderName,
                    SenderSurname = model.SenderSurname
                };
                _context.ProjectProposal.Add(entity);

                _context.SaveChanges();

                _sysFunction.SendSmsAdmins($"Yeni Proje Önerisi: Proje Name:{entity.ProjectName},\n {entity.SenderName} {entity.SenderSurname} {entity.SenderMail}");

                return RedirectToAction("ContactOk", "Contact");
            }
            return View(model);
        }
    }
}