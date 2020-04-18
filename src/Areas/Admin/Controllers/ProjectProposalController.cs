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
    [Route("/Admin/ProjectProposal")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class ProjectProposalController : Controller
    {
        private readonly Context _context;
        private readonly ISysFunctions _sysFunctions;

        public ProjectProposalController
            (
            Context context,
            ISysFunctions sysFunctions
            )
        {
            _context = context;
            _sysFunctions = sysFunctions;
        }

        [HttpGet("/Admin/ProjectProposal")]
        public IActionResult Index()
        {
            return View(_context.ProjectProposal.AsEnumerable().OrderByDescending(x => x.SenderDate).ToList());
        }

        [HttpGet("/Admin/ProjectProposal/Detail/{id}")]
        public IActionResult Detail(Guid id)
        {
            ProjectProposal q = null;
            if (id != Guid.Empty)
            {
                q = _context.ProjectProposal.FirstOrDefault(x => x.Id == id);
            }

            if (q == null)
            {
                q = new ProjectProposal();
                q.Id = Guid.NewGuid();
            }
            return View(q);
        }

        [HttpGet("/Admin/ProjectProposal/Delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            var q = _context.ProjectProposal.FirstOrDefault(x => x.Id == id);
            if (q != null)
            {
                _context.ProjectProposal.Remove(q);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}