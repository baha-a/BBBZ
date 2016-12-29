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
    public class ContentsController : BaseController
    {
        public ActionResult Index()
        {
            IsAllowed(MyPermission.See_Contents);
            return View(db.Contents.Include(x=>x.Access).Include(x=>x.Category).ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest();
            Content content = db.Contents.Include(x => x.Access).Include(x => x.Category).SingleOrDefault(x => x.ID == id);
            if (content == null)
                return HttpNotFound();
            content.CustomFieldValues = db.CustomFieldValues.Include(x => x.Content).Include(x => x.CustomField).Where(x => x.Content.ID == content.ID).ToList();
            return View(content);
        }

        public ActionResult Create()
        {
            IsAllowed(MyPermission.Create_Contents);
            
            ViewBag.AllCategories = GetAllCategories();
            ViewBag.AllViewLevels = GetAllViewLevels();
            ViewBag.AllLanguages = db.GetAllLanguages();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Content content)
        {
            IsAllowed(MyPermission.Create_Contents);
            if (ModelState.IsValid)
            {
                content.CreatedByUsername = User.Identity.Name;
                content.CreatedTime = DateTime.Now;

                if (content.CategoryID != null)
                    content.Category = db.Categories.SingleOrDefault(x => x.ID == content.CategoryID);

                if (content.AccessID != null)
                    content.Access = db.ViewLevels.SingleOrDefault(x => x.ID == content.AccessID);

                db.Contents.Add(content);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AllCategories = GetAllCategories();
            ViewBag.AllViewLevels = GetAllViewLevels();
            ViewBag.AllLanguages = db.GetAllLanguages();

            return View(content);
        }

        public ActionResult Edit(int? id)
        {
            IsAllowed(MyPermission.Edit_Contents);
            if (id == null)
                return BadRequest();

            Content content = db.Contents.Include(x => x.Category).SingleOrDefault(x => x.ID == id);
            if (content == null)
                return HttpNotFound();

            if (content.Category != null)
                content.CategoryID = content.Category.ID;
            if (content.Access != null)
                content.AccessID = content.Access.ID;

            ViewBag.AllCategories = GetAllCategories();
            ViewBag.AllViewLevels = GetAllViewLevels();
            ViewBag.AllLanguages = db.GetAllLanguages();

            return View(content);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Content con)
        {
            IsAllowed(MyPermission.Edit_Contents);
            var content  = con;
            if (ModelState.IsValid)
            {
                if (con != null)
                {
                    content = db.Contents.Include(x=>x.Access).Include(x => x.Category).SingleOrDefault(x => x.ID == con.ID);

                    content.Title = con.Title;
                    content.FullText = con.FullText;
                    content.IntroText = con.IntroText;
                    content.Descrption = con.Descrption;

                    content.Language = con.Language;
                    content.MetaDesc = con.MetaDesc;
                    content.MetaKey = con.MetaKey;
                    content.Published = con.Published;

                    if (con.CategoryID != null)
                    {
                        if (content.Category == null || content.Category.ID != con.CategoryID)
                            content.Category = db.Categories.SingleOrDefault(x => x.ID == con.CategoryID);
                    }
                    else
                        if (content.Category != null)
                            content.Category = null;

                    if (con.AccessID != null)
                    {
                        if (content.Access == null || content.Access.ID != con.AccessID)
                            content.Access = db.ViewLevels.SingleOrDefault(x => x.ID == con.AccessID);
                    }
                    else
                        if (content.Access != null)
                            content.Access = null;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.AllCategories = GetAllCategories();
            ViewBag.AllViewLevels = GetAllViewLevels();
            ViewBag.AllLanguages = db.GetAllLanguages();

            return View(content);
        }

        public ActionResult Delete(int? id)
        {
            IsAllowed(MyPermission.Delete_Contents);
            if (id == null)
                return BadRequest();
            Content content = db.Contents.Include(x => x.Access).Include(x => x.Category).Include(x => x.CustomFieldValues).SingleOrDefault(x => x.ID == id);
            if (content == null)
                return HttpNotFound();
            return View(content);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IsAllowed(MyPermission.Delete_Contents); 
            Content content = db.Contents.Find(id);
            db.Contents.Remove(content);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Custom(int? id)
        {
            IsAllowed(MyPermission.Edit_Contents);
            if (id == null)
                return BadRequest();

            var c = db.Contents.SingleOrDefault(x => x.ID == id);
            if (c == null)
                return HttpNotFound();
            ViewBag.ID = id;
            ViewBag.ContentTitle = c.Title;


            var cfv = db.CustomFieldValues.Include(x => x.Content).Include(x => x.CustomField).Where(x => x.Content.ID == id).ToList();
            if (cfv == null)
                return HttpNotFound();

            ViewBag.AllCustomFields = db.GetAllCustomFields();

            return View(cfv);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddCFV(int? customid, int? contentid, string value)
        {
            if (customid == null || contentid == null)
                return BadRequest();

            var con = db.Contents.SingleOrDefault(x => x.ID == contentid);
            var cus = db.CustomFields.SingleOrDefault(x => x.ID == customid);
            if (con == null || cus == null)
                return HttpNotFound();

            db.CustomFieldValues.Add(new CustomFieldValue()
            {
                Value = value,
                Content = con,
                CustomField = cus
            });

            db.SaveChanges();

            return RedirectToAction("Custom", new { id = contentid });
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditCFV(int? contentid, int? id, string value)
        {
            if (id == null || contentid == null)
                return BadRequest();

            var vl = db.CustomFieldValues.SingleOrDefault(x => x.ID == id);
            if (vl == null)
                return HttpNotFound();

            vl.Value = value;
            db.SaveChanges();
            return RedirectToAction("Custom", new { id = contentid });
        }

        [HttpPost]
        public ActionResult DeleteCFV(int? id, int? contentid)
        {
            if (id == null)
                return BadRequest();

            var vl = db.CustomFieldValues.SingleOrDefault(x => x.ID == id);
            if (vl == null)
                return HttpNotFound();

            db.CustomFieldValues.Remove(vl);
            db.SaveChanges();
            return RedirectToAction("Custom", new { id = contentid });
        }
    }
}
