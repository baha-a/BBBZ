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
            IsAllowed(MyPermission.See_Categories);

            return View(GetAllCategories());
        }

        public ActionResult Details(int? id)
        {
            IsAllowed(MyPermission.See_Categories);

            if (id == null)
                return BadRequest();

            Category category = 
                db.Categories
                .Include(x=>x.Contents)
                .Include(x=>x.Parent)
                .Include(x => x.Access)
                .SingleOrDefault(x => x.ID == id);

            if (category == null)
                return HttpNotFound();
            category.SubCategories = db.Categories.Include(x => x.Parent).Where(x => x.Parent != null && x.Parent.ID == id).ToList();
            return View(category);
        }

        public ActionResult Create()
        {
            IsAllowed(MyPermission.Create_Categories);

            ViewBag.AllCategories = GetAllCategories();
            ViewBag.AllLanguages = db.GetAllLanguages();
            ViewBag.AllViewLevels = GetAllViewLevels();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            IsAllowed(MyPermission.Create_Categories);
            
            if (ModelState.IsValid)
            {
                category.Date = DateTime.Now;
                category.CreatedByUsername = User.Identity.Name;

                category.Parent = db.Categories.SingleOrDefault(x => x.ID == category.NewParentID_helper);
                category.Access = db.ViewLevels.SingleOrDefault(x => x.ID == category.NewAccessID_helper);

                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            ViewBag.AllCategories = GetAllCategories();
            ViewBag.AllLanguages = db.GetAllLanguages();
            ViewBag.AllViewLevels = GetAllViewLevels();
            
            return View(category);
        }

        public ActionResult Edit(int? id)
        {
            IsAllowed(MyPermission.Edit_Categories);

            if (id == null)
                return BadRequest();

            Category category =
                db.Categories
                .Include(x => x.Parent)
                .Include(x => x.Access)
                .SingleOrDefault(x => x.ID == id);
            
            if (category == null)
                return HttpNotFound();

            category.NewParentID_helper = category.Parent != null ? (int?)category.Parent.ID : null;

            ViewBag.AllCategories = GetAllCategories(category.ID);
            ViewBag.AllLanguages = db.GetAllLanguages();
            ViewBag.AllViewLevels = GetAllViewLevels();
            
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category cat)
        {
            IsAllowed(MyPermission.Edit_Categories);

            if (ModelState.IsValid)
            {
                var category = db.Categories.Include(x=>x.Parent).SingleOrDefault(c => c.ID == cat.ID);

                category.Title = cat.Title;
                category.Description = cat.Description;
                category.Image = cat.Image;
                category.Published = cat.Published;
                category.MetaData = cat.MetaData;
                category.MetaKey = cat.MetaKey;
                category.MetaDesc = cat.MetaDesc;
                category.Language = cat.Language;

                category.Parent = db.Categories.SingleOrDefault(x => x.ID == cat.NewParentID_helper);
                category.Access = db.ViewLevels.SingleOrDefault(x => x.ID == cat.NewAccessID_helper);
                
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AllCategories = GetAllCategories(cat.ID);
            ViewBag.AllLanguages = db.GetAllLanguages();
            ViewBag.AllViewLevels = GetAllViewLevels();

            return View(cat);
        }

        public ActionResult Delete(int? id)
        {
            IsAllowed(MyPermission.Delete_Categories);

            if (id == null)
                return BadRequest();
            Category category = db.Categories.Include(x => x.Access).SingleOrDefault(x => x.ID == id);
            if (category == null)
                return HttpNotFound();
            category.SubCategories = db.Categories.Include(x => x.Parent).Where(x => x.Parent != null && x.Parent.ID == id).ToList();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IsAllowed(MyPermission.Delete_Categories);

            Category category = db.Categories.Include(x => x.Contents).SingleOrDefault(x => x.ID == id);
            DeleteWithChildren(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void DeleteWithChildren(Category cat)
        {
            if (cat == null)
                return;

            var children =
                db.Categories
                .Include(x => x.Contents)
                .Include(x => x.Parent)
                .Where(x => x.Parent != null && x.Parent.ID == cat.ID)
                .ToList();

            foreach (var c in children)
                DeleteWithChildren(c);

            db.Categories.Remove(cat);
        }
    }
}
