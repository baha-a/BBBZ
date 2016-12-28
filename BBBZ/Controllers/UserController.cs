using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BBBZ.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BBBZ.Controllers
{
    public class UserController : BaseController
    {
        public UserManager<ApplicationUser> UserManager { get; private set; }
        public UserController()
        {
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            IsAllowed(MyPermission.Users);
        }


        public ActionResult Index()
        {
            List<UserManagerDataView> rg = new List<UserManagerDataView>();
            db.Users.ToList().ForEach(x =>
                rg.Add(new UserManagerDataView()
                {
                    TheUser = new RegisterViewModel() { UserName = x.UserName },
                    Locked = x.Locked,
                    AssignedGroups = db.UserGroups.Where(y => y.username == x.UserName).Select(y => y.Groups).ToList()
                }));
            return View(rg);
        }

        public ActionResult Create()
        {
            return View(new UserManagerDataView() { Locked = false, TheUser = null, AllGroups = GetAllGroups() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserManagerDataView us)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = us.TheUser.UserName, Locked = us.Locked };

                var result = UserManager.Create(user, us.TheUser.Password);
                if (result.Succeeded)
                {
                    db.Profiles.Add(new Profile() { username = user.UserName, RegisterDate = DateTime.Now, LastVisitDate = DateTime.Now });

                    foreach (var g in us.AllGroups)
                    {
                        if (g.Selected)
                        {
                            var group = db.Groups.SingleOrDefault(x => x.ID == g.ID);

                            if (db.UserGroups.SingleOrDefault(x => x.Groups.ID == group.ID && x.username == user.UserName) == null)
                                db.UserGroups.Add(new UserGroup() { username = user.UserName, Groups = group });
                        }
                    }

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error);
                }
            }

            return View(us);
        }

        public ActionResult Edit(string username)
        {
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Index");

            var u = db.Users.SingleOrDefault(x => x.UserName == username);
            var g = db.UserGroups.Where(y => y.username == u.UserName).Select(y => y.Groups).ToList();

            var gs = GetAllGroups();
            gs.ForEach(x => x.Selected = g.SingleOrDefault(z => z.ID == x.ID) != null);

            return View(new UserManagerDataView()
            {
                Locked = u.Locked,
                TheUser = new RegisterViewModel() { UserName = u.UserName },
                AllGroups = gs
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserManagerDataView us)
        {
            var user = db.Users.SingleOrDefault(x => x.UserName == us.TheUser.UserName);
            if (user == null)
                return HttpNotFound();

            user.Locked = us.Locked;

            db.UserGroups.RemoveRange(db.UserGroups.Where(x=>x.username == user.UserName));

            foreach (var g in us.AllGroups)
            {
                if (g.Selected)
                {
                    var group = db.Groups.SingleOrDefault(x => x.ID == g.ID);
                    db.UserGroups.Add(new UserGroup() { username = user.UserName, Groups = group });
                }
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        public ActionResult Lock(string username, bool Locked)
        {
            if (ModelState.IsValid)
            {
                var u = db.Users.SingleOrDefault(x => x.UserName == username);
                if (u == null)
                    return HttpNotFound();
                u.Locked = Locked;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }
	}
}