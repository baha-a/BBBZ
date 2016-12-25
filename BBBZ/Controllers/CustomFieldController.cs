using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BBBZ.Models;

namespace BBBZ.Controllers
{
    public class CustomFieldController : BaseController
    {
        public ActionResult Index()
        {
            IsAllowed(MyPermission.Edit_Contents);
            return View(db.CustomFields.ToList());
        }

        [HttpPost]
        public ActionResult Create(string title)
        {
            IsAllowed(MyPermission.Edit_Contents);

            if (string.IsNullOrEmpty(title))
                ModelState.AddModelError("", "Title required");
            else if(db.CustomFields.SingleOrDefault(x=>x.Title==title) != null)
                ModelState.AddModelError("", "Title used");
            else
            {
                db.CustomFields.Add(new CustomField() { Title = title });
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(int? id, string title)
        {
            IsAllowed(MyPermission.Edit_Contents);

            if (id == null)
                return BadRequest();
            CustomField cf = db.CustomFields.SingleOrDefault(x => x.ID == id);
            if (cf == null)
                return HttpNotFound();

            cf.Title = title;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            IsAllowed(MyPermission.Edit_Contents);

            if (id == null)
                return BadRequest();

            CustomField customfield = db.CustomFields.Include(x => x.Values).SingleOrDefault(x => x.ID == id);
            if (customfield == null)
                return HttpNotFound();

            db.CustomFields.Remove(customfield);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
