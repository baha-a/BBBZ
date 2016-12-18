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
    public class CategoryController : BaseController
    {
        public ActionResult Index()
        {
            return View(db.Categories.Include(x => x.Access).ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Include(x => x.Access).SingleOrDefault(x => x.ID == id);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Date = DateTime.Now;
                category.CreatedByUsername = User.Identity.Name;
                if (string.IsNullOrEmpty(category.Alias))
                    category.Alias = category.Title.Replace(" ", "");
                category.Alias = category.Alias.ToLower();

                category.Parent = db.Categories.SingleOrDefault(x => x.ID == category.NewParentID_helper);
                category.Access = db.ViewLevels.SingleOrDefault(x => x.ID == category.NewAccessID_helper);

                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Include(x => x.Access).SingleOrDefault(x => x.ID == id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category cat)
        {
            if (ModelState.IsValid)
            {
                var category = db.Categories.SingleOrDefault(c => c.ID == cat.ID);

                category.Title = cat.Title;
                category.Description = cat.Description;
                category.Url = cat.Url;
                category.Published = cat.Published;
                category.MetaData = cat.MetaData;
                category.MetaKey = cat.MetaKey;
                category.MetaDesc = cat.MetaDesc;
                category.Language = cat.Language;

                if (string.IsNullOrEmpty(cat.Alias))
                    cat.Alias = cat.Title.Replace(" ", "");
                category.Alias = cat.Alias.ToLower();

                category.Parent = db.Categories.SingleOrDefault(x => x.ID == cat.NewParentID_helper);
                category.Access = db.ViewLevels.SingleOrDefault(x => x.ID == cat.NewAccessID_helper);
                
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cat);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Include(x => x.Access).SingleOrDefault(x => x.ID == id);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
