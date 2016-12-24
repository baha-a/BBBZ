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
        public ActionResult Index()
        {
            return View(db.Languages.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Language language = db.Languages.Find(id);
            if (language == null)
            {
                return HttpNotFound();
            }
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

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Language language = db.Languages.Find(id);
            if (language == null)
            {
                return HttpNotFound();
            }
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

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Language language = db.Languages.Find(id);
            if (language == null)
            {
                return HttpNotFound();
            }
            return View(language);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Language language = db.Languages.Find(id);
            db.Languages.Remove(language);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
