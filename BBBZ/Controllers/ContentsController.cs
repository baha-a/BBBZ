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
        // GET: /Content/
        public ActionResult Index()
        {
            return View(db.Contents.Include(x=>x.Access).Include(x=>x.Category).ToList());
        }

        // GET: /Content/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest();
            Content content = db.Contents.Include(x => x.Access).Include(x => x.Category).SingleOrDefault(x => x.ID == id);
            if (content == null)
                return HttpNotFound();
            return View(content);
        }

        // GET: /Content/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Content/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Content content)
        {
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

            return View(content);
        }

        // GET: /Content/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            Content content = db.Contents.Include(x => x.Category).SingleOrDefault(x => x.ID == id);
            if (content == null)
                return HttpNotFound();

            if (content.Category != null)
                content.CategoryID = content.Category.ID;
            if (content.Access != null)
                content.AccessID = content.Access.ID;

            return View(content);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Content con)
        {
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
                    content.MetaData = con.MetaData;
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
            return View(content);
        }

        // GET: /Content/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            Content content = db.Contents.Include(x => x.Access).Include(x => x.Category).SingleOrDefault(x => x.ID == id);
            if (content == null)
                return HttpNotFound();
            return View(content);
        }

        // POST: /Content/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Content content = db.Contents.Find(id);
            db.Contents.Remove(content);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
