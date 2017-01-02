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
    public class ViewLevelController : BaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            IsAllowed(MyPermission.ViewLevels);
        }

        public List<Group> FillWithChildren(List<Group> gs)
        {
            foreach (var g in gs)
            {
                g.Children = db.Groups.Where(x => x.Parent != null && x.Parent.ID == g.ID).ToList();
                FillWithChildren(g.Children);
            }
            return gs;
        }

        public ActionResult Index()
        {
            return View(db.ViewLevels.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewLevel viewlevel = db.ViewLevels.Find(id);
            if (viewlevel == null)
            {
                return HttpNotFound();
            }
            viewlevel.Groups = db.Groups.Where(x => x.Access.FirstOrDefault(y => y.ID == viewlevel.ID) != null).ToList();
            return View(viewlevel);
        }
        
        public ActionResult Create()
        {
            return View(new ViewLevelViewModel().Initialize(FillWithChildren(db.Groups.Where(x=>x.Parent == null).ToList())));
        }

        [HttpPost]
        public ActionResult Create(ViewLevelViewModel vl)
        {
            if (ModelState.IsValid)
            {
                db.ViewLevels.Add(AssignTheSelectedGroupsToTheViewLevelAndReturnIt(vl.theViewlevel,vl.Groups));
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vl);
        }
        public ViewLevel AssignTheSelectedGroupsToTheViewLevelAndReturnIt(ViewLevel vl, List<SelectableGroup> sg)
        {
            foreach (var g in sg)
                if (g.Selected)
                {
                    if (vl.Groups.SingleOrDefault(x => x.ID == g.ID) == null)
                        vl.Groups.Add(db.Groups.SingleOrDefault(y => y.ID == g.ID));
                }
                else
                {
                    if (vl.Groups.SingleOrDefault(x => x.ID == g.ID) != null)
                        vl.Groups.Remove(db.Groups.SingleOrDefault(y => y.ID == g.ID));
                }
            return vl;
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ViewLevel viewlevel = db.ViewLevels.Find(id);
            if (viewlevel == null)
            {
                return HttpNotFound();
            }
            viewlevel.Groups = db.Groups.Include(x => x.Access).Where(x => x.Access.FirstOrDefault(y => y.ID == viewlevel.ID) != null).ToList();

            return View(new ViewLevelViewModel().Initialize(FillWithChildren(db.Groups.Where(x => x.Parent == null).ToList()), viewlevel));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ViewLevelViewModel vl)
        {
            if (ModelState.IsValid)
            {
                var vll = db.ViewLevels.SingleOrDefault(x => x.ID == vl.theViewlevel.ID);
                vll.Title = vl.theViewlevel.Title;
                vll.Description = vl.theViewlevel.Description;
                vll.Groups = db.Groups.Include(x=>x.Access).Where(x => x.Access.FirstOrDefault(y => y.ID == vll.ID) != null).ToList();

                AssignTheSelectedGroupsToTheViewLevelAndReturnIt(vll, vl.Groups);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vl);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            ViewLevel viewlevel = db.ViewLevels.Find(id);
            if (viewlevel == null)
                return HttpNotFound();
            viewlevel.Groups = db.Groups.Include(x => x.Access).Where(x => x.Access.FirstOrDefault(y => y.ID == viewlevel.ID) != null).ToList();
            return View(viewlevel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewLevel viewlevel = db.ViewLevels.Include(x=>x.Groups).SingleOrDefault(x => x.ID == id);

            if (viewlevel == null)
                return HttpNotFound();

            db.Contents.Include(x => x.Access)
                .Where(x => x.Access != null && x.Access.ID == id).ToList()
                .ForEach(x => x.Access = null);

            db.Categories.Include(x => x.Access)
                .Where(x => x.Access != null && x.Access.ID == id).ToList()
                .ForEach(x => x.Access = null);

            db.Menus.Include(x => x.Access)
                .Where(x => x.Access != null && x.Access.ID == id).ToList()
                .ForEach(x => x.Access = null);


            db.ViewLevels.Remove(viewlevel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
