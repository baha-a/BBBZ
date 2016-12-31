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
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            IsAllowed(MyPermission.Groups); 
        }

        public List<Group> FillWithChildren(List<Group> gs,int without=-1)
        {
            foreach (var g in gs)
            {
                g.Children = db.Groups.Where(x => x.Parent != null && x.Parent.ID == g.ID && x.ID != without).ToList();
                FillWithChildren(g.Children,without);
            }
            return gs;
        }


        public ActionResult Index()
        {
            return View(SelectableGroup.Convert(FillWithChildren(db.Groups.Where(p => p.Parent == null).ToList())));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
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
                group.Parent = db.Groups.SingleOrDefault(x => x.ID == group.helperID);
                group.Permission = new Permission();
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
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
                var g = db.Groups.Include(x=>x.Parent).SingleOrDefault(x => x.ID == group.ID);
                g.Title = group.Title;
                g.Description = group.Description;

                if (group.helperID == null || group.helperID == -1)
                {
                    if (g.Parent != null)
                        g.Parent.Children.Remove(g);
                }
                else
                    g.Parent = db.Groups.SingleOrDefault(x => x.ID == (int)group.helperID);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
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
            Group group = db.Groups.Include(x=>x.Access).SingleOrDefault(x => x.ID == id);
            DeleteWithChildren(group);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void DeleteWithChildren(Group grp)
        {
            if (grp == null)
                return;
            var children = db.Groups.Include(x=>x.Access).Where(x => x.Parent != null && x.Parent.ID == grp.ID).ToList();
            foreach (var c in children)
                DeleteWithChildren(c);
            db.Groups.Remove(grp);
        }



        public ActionResult Setting()
        {
            ViewBag.newUserGroup = GroupSetting.NewUserGroupId;
            ViewBag.guestGroup = GroupSetting.GuestGroupId;

            return View(GetAllGroups());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Setting(int? newUserGroup, int? guestGroup)
        {
            GroupSetting.GuestGroupId = guestGroup;
            GroupSetting.NewUserGroupId = newUserGroup;
            return RedirectToAction("Setting");
        }

        public ActionResult Permission(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Permission p =  db.Permissions.SingleOrDefault(x => x.ID == id);
            if (p == null)
            {
                var g = db.Groups.SingleOrDefault(x=>x.ID==id);
                if (g == null)
                    return HttpNotFound();
                else
                {
                    p = new Permission();
                    g.Permission = p;
                    db.SaveChanges();
                }
            }
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Permission(Permission per)
        {
            if (ModelState.IsValid)
            {
                Permission p = db.Permissions.SingleOrDefault(x => x.ID == per.ID);

                p.Users = per.Users;
                p.Groups = per.Groups;
                p.ViewLevels = per.ViewLevels;
                p.Menus = per.Menus;
                p.Languages = per.Languages;

                p.Questions = per.Questions;

                p.Media = per.Media;
                p.AdminPanel = per.AdminPanel;

                p.See_Categories = per.See_Categories;
                p.Create_Categories = per.Create_Categories;
                p.Edit_Categories = per.Edit_Categories;
                p.Delete_Categories = per.Delete_Categories;

                p.See_Contents = per.See_Contents;
                p.Create_Contents = per.Create_Contents;
                p.Edit_Contents = per.Edit_Contents;
                p.Delete_Contents = per.Delete_Contents;


                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(per);
        }
    }
}
