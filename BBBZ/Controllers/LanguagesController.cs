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
    public class LanguagesController : BaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            IsAllowed(MyPermission.Languages);
        }

        public ActionResult Index()
        {
            return View(db.Languages.ToList());
        }

        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();
            Language language = db.Languages.SingleOrDefault(x => x.Code == id);
            if (language == null)
                return HttpNotFound();
            return View(language);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Language language)
        {
            if (ModelState.IsValid)
            {
                db.Languages.Add(language);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(language);
        }

        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();
            Language language = db.Languages.SingleOrDefault(x => x.Code == id);
            if (language == null)
                return HttpNotFound();
            return View(language);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Language language)
        {
            if (ModelState.IsValid)
            {
                db.Entry(language).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(language);
        }

        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();
            Language language = db.Languages.SingleOrDefault(x => x.Code == id);
            if (language == null)
                return HttpNotFound();
            return View(language);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();
            Language language = db.Languages.SingleOrDefault(x => x.Code == id);

            db.Categories.Where(x => x.Language == id).ToList().ForEach(x => x.TheLanguage = null);
            db.Contents.Where(x => x.Language == id).ToList().ForEach(x => x.TheLanguage = null);
            db.Menus.Where(x => x.Language == id).ToList().ForEach(x => x.TheLanguage = null);

            db.Languages.Remove(language);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
