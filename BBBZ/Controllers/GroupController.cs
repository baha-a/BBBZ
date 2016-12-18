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
    public class GroupController : BaseController
    {
        public List<Group> FillWithChildren(List<Group> gs,int without=-1)
        {
            foreach (var g in gs)
            {
                g.Children = db.Groups.Where(x => x.Parent != null && x.Parent.ID == g.ID && x.ID != without).ToList();
                FillWithChildren(g.Children,without);
            }
            return gs;
        }


        // GET: /Group/
        public ActionResult Index()
        {
            return View(SelectableGroup.Convert(FillWithChildren(db.Groups.Where(p => p.Parent == null).ToList())));
        }

        // GET: /Group/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Include(x=>x.Access).Include(x => x.Parent).Include(x=>x.Children).SingleOrDefault(x => x.ID == id);
            
            Group mover = group;
            while (mover != null && mover.Parent != null)
            {
                mover.Parent = db.Groups.Include(x=>x.Parent).SingleOrDefault(x => x.ID == mover.Parent.ID);
                mover = mover.Parent;
            }

            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // GET: /Group/Create
        public ActionResult Create()
        {
            ViewBag.Groups = SelectableGroup.Convert(FillWithChildren(db.Groups.Where(p => p.Parent == null).ToList()));
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Group group)
        {
            if (ModelState.IsValid)
            {
                group.Parent = db.Groups.SingleOrDefault(x => x.ID == group.Parent.ID);
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group);
        }

        // GET: /Group/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            ViewBag.Groups = SelectableGroup.Convert(FillWithChildren(db.Groups.Where(p => p.Parent == null && p.ID != group.ID).ToList(), group.ID));
            return View(group);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Group group)
        {
            if (ModelState.IsValid)
            {
                var g = db.Groups.SingleOrDefault(x => x.ID == group.ID);
                g.Title = group.Title;
                g.Description = group.Description;
                g.Parent = (group.helperID == null) ? null : db.Groups.SingleOrDefault(x => x.ID == (int)group.helperID);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        // GET: /Group/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Include(x => x.Parent).SingleOrDefault(x => x.ID == id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);
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
