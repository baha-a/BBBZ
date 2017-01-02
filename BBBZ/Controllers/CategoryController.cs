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
        public ActionResult Show(int? id)
        {
            if (id == null)
                return BadRequest();

            Category category = 
                db.Categories
                .Include(x => x.Access)
                //.Include(x => x.Contents)
                .Include(x => x.Parent)
                .Include(x => x.TheLanguage)
                .SingleOrDefault(x => x.ID == id && x.Published);

            if (category == null)
                return HttpNotFound();

            if (category.Template == CategorysTemplate.NotSet)
                return RedirectToAction("Details", "Category", new { id = id });

            if (MyPermission.See_Categories == true || (category.Access != null && MyViewLevelIDs.Contains(category.Access.ID)))
            {

                if (category.Template == CategorysTemplate.ArticleList)
                {
                    category.Contents = 
                        db.Contents.Include(x=>x.TheLanguage).Include(x=>x.Access).Include(x=>x.Category)
                        .Where(x=> 
                                x.Published &&
                                x.Category != null && x.Category.ID == id &&
                                (x.TheLanguage == null || x.TheLanguage.Code == Language) &&
                                    x.Access != null && MyViewLevelIDs.Contains(x.Access.ID))
                                    .ToList();
                    return View("ArticleList",category);
                }

                category.SubCategories =
                     db.Categories
                        .Include(x => x.Parent)
                        .Include(x => x.Access)
                        .Include(x => x.TheLanguage)
                        .Where(x =>
                            x.Published &&
                            (x.TheLanguage == null || x.TheLanguage.Code == Language) &&
                            x.Parent != null && x.Parent.ID == category.ID &&
                            x.Access != null && MyViewLevelIDs.Contains(x.Access.ID))
                        .ToList();


                ViewBag.NotEnrolledWith = 
                     db.Categories
                        .Include(x => x.Parent)
                        .Include(x => x.Access)
                        .Include(x => x.TheLanguage)
                        .Where(x =>
                            x.Published &&
                            (x.TheLanguage == null || x.TheLanguage.Code == Language) &&
                            x.Parent != null && x.Parent.ID == category.ID &&
                            (x.Access == null || MyViewLevelIDs.Contains(x.Access.ID) == false))
                        .ToList();

                category.Contents = MarkTheVisited(
                    db.Contents.Include(x=>x.TheLanguage).Include(x=>x.Access).Include(x=>x.Category)
                        .Where(x=> 
                                x.Published &&
                                x.Category != null && x.Category.ID == id &&
                                (x.TheLanguage == null || x.TheLanguage.Code == Language) &&
                                    x.Access != null && MyViewLevelIDs.Contains(x.Access.ID))
                                .ToList());

                if(category.Template == CategorysTemplate.LessonOneByOne)
                    return View(CategorysTemplate.Default.ToString(), category);
                
                return View(category.Template.ToString(), category);
            }

            return Unauthorized();
        }

        public ActionResult Enroll(int? id)
        {
            if (id == null)
                return BadRequest();

            Category category = 
                db.Categories
                .Include(x => x.Access)
                .Include(x => x.Parent)
                .Include(x => x.TheLanguage)
                .SingleOrDefault(x => x.ID == id && x.Published);

            if (category == null)
                return HttpNotFound();

            ViewBag.Wait = false;
            if (string.IsNullOrEmpty(Username)==false && category.Access != null)
            {
                if (db.Requests.SingleOrDefault(x => x.AccessId == category.Access.ID && x.Username == Username) == null)
                {
                    db.Requests.Add(new Request() { Username = Username, Date = DateTime.Now, AccessId = category.Access.ID });
                    db.SaveChanges();
                }
                ViewBag.Wait = true;
            }

            return View(CategorysTemplate.Default.ToString(), category);
        }

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
        public ActionResult Create(Category category,HttpPostedFileBase Uploader)
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

                Uploader.UploadCategoryImage(category);
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

            if (category.Parent != null)
                category.NewParentID_helper = category.Parent.ID;

            if (category.Access != null)
                category.NewAccessID_helper = category.Access.ID;

            ViewBag.AllCategories = GetAllCategories(category.ID);
            ViewBag.AllLanguages = db.GetAllLanguages();
            ViewBag.AllViewLevels = GetAllViewLevels();
            
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category cat,HttpPostedFileBase Uploader)
        {
            IsAllowed(MyPermission.Edit_Categories);

            if (ModelState.IsValid)
            {
                var category = db.Categories.Include(x=>x.TheLanguage).Include(x=>x.Access).Include(x=>x.Parent).SingleOrDefault(c => c.ID == cat.ID);
                if (category == null)
                    return HttpNotFound();

                category.Title = cat.Title;
                category.Description = cat.Description;
                category.Published = cat.Published;
                category.MetaKey = cat.MetaKey;
                category.MetaDesc = cat.MetaDesc;
                category.Language = cat.Language;
                category.Template = cat.Template;


                if (cat.NewParentID_helper == null)
                    category.Parent = null;
                else
                    category.Parent = db.Categories.SingleOrDefault(x => x.ID == cat.NewParentID_helper);

                if (category.Access == null || cat.NewAccessID_helper != category.Access.ID)
                    category.Access = db.ViewLevels.SingleOrDefault(x => x.ID == cat.NewAccessID_helper);

                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();

                Uploader.UploadCategoryImage(category);
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

            Category category = db.Categories.Include(x => x.Parent).Include(x => x.Access).SingleOrDefault(x => x.ID == id);
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
            if (category == null)
                return HttpNotFound();
            
            DeleteWithChildren(category);
            
                db.SaveChanges();
            
            return RedirectToAction("Index");
        }

        private void DeleteWithChildren(Category cat)
        {
            if (cat == null)
                return;
            cat.Contents.Clear();

            var children =
                db.Categories
                .Include(x => x.Contents)
                .Include(x => x.Parent)
                .Where(x => x.Parent != null && x.Parent.ID == cat.ID)
                .ToList();

            foreach (var c in children)
                DeleteWithChildren(c);

            db.Entry(cat).State = EntityState.Deleted;
            db.Categories.Remove(cat);

            cat.DeleteImage();
        }
    }
}
