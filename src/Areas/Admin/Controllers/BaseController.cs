using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Yazgelder.Areas.Admin.Models;
using Yazgelder.Entity;
using Yazgelder.Entity.Models;

namespace Yazgelder.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        public void FileSave(ImageBase model, Context context)
        {
            var imagList = Request.Form["FileList"];
            if (string.IsNullOrEmpty(imagList ) )
                return;
            model.FileList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileModel>>(imagList);

            var type = model.FileList.Select(x => new { x.Type, x.SubType }).Distinct().ToList();
            List<Pictures> a = new List<Pictures>();

            foreach (var item in type)
            {
                var fileList = context.Pictures.Where(x => x.RecordType == item.Type && x.RecordType2 == item.SubType && (x.RelationId == model.Id || x.RelationId == Guid.Empty));
                a.AddRange(fileList);
            }

            //Silinecekler
            var del = a.Where(x => model.FileList.Any(t => t.IsDeleted && t.Id == x.Id)).ToList();

            //dataDelete
            context.Pictures.RemoveRange(del);

            //Update
            var update = a.Where(x => model.FileList.Any(t => !t.IsDeleted && x.Id == t.Id)).ToList();
            foreach (var item in update)
            {
                var f = model.FileList.FirstOrDefault(x => x.Id == item.Id);
                if (item.RelationId == Guid.Empty)
                    item.RelationId = model.Id;
                item.RecordType2 = f.SubType;
                item.Title = f.Title;
            }
            context.Pictures.UpdateRange(update);
        }
    }
}