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
    public class MenusController : BaseController
    {
        // GET: /Menus/
        public ActionResult Index()
        {
            return View(db.MenuTypes.ToList());
        }

        // GET: /Menus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            MenuType menutype = db.MenuTypes.Find(id);
            if (menutype == null)
            {
                return HttpNotFound();
            }
            return View(menutype);
        }

        // GET: /Menus/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MenuType menut)
        {
            if (ModelState.IsValid)
            {
                db.MenuTypes.Add(menut);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(menut);
        }

        // GET: /Menus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            MenuType menutype = db.MenuTypes.Find(id);
            if (menutype == null)
            {
                return HttpNotFound();
            }
            return View(menutype);
        }

        // POST: /Menus/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MenuType menut)
        {
            if (ModelState.IsValid)
            {
                db.Entry(menut).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menut);
        }

        // GET: /Menus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            MenuType menutype = db.MenuTypes.Find(id);
            if (menutype == null)
            {
                return HttpNotFound();
            }
            return View(menutype);
        }

        // POST: /Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MenuType menutype = db.MenuTypes.Find(id);
            db.MenuTypes.Remove(menutype);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
