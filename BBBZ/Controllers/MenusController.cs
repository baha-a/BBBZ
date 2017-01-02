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
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            IsAllowed(MyPermission.Menus);
        }

        public ActionResult Index()
        {
            return View(db.MenuTypes.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest();
            MenuType menutype = db.MenuTypes.Find(id);
            if (menutype == null)
                return HttpNotFound();
            return View(menutype);
        }

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

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return BadRequest();
            MenuType menutype = db.MenuTypes.Find(id);
            if (menutype == null)
                return HttpNotFound();
            return View(menutype);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MenuType menut)
        {
            if (ModelState.IsValid)
            {
                var men = db.MenuTypes.SingleOrDefault(x => x.ID == menut.ID);
                if (men == null)
                    return HttpNotFound();

                men.Title= menut.Title;
                men.Description = menut.Description; 
                men.IsTopMenu = menut.IsTopMenu;
                men.Published = menut.Published;

                db.Entry(men).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menut);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            MenuType menutype = db.MenuTypes.Find(id);
            if (menutype == null)
                return HttpNotFound();
            return View(menutype);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MenuType menutype = db.MenuTypes.Include(x => x.Menus).SingleOrDefault(x => x.ID == id);
            db.MenuTypes.Remove(menutype);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
