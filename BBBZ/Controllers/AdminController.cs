using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BBBZ.Models;

using System.Data.Entity;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BBBZ.Controllers
{
    [Authorize(Roles = "admin,superadmin")]
    public class AdminController: BaseController
    {
        ApplicationDbContext db;

        public AdminController()
        {
            db = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var v = new AdminViewModel()
            {
                NewUsersToAccept = GetAllUserInRole("user_temp"),
                NewTeachersToAccept = GetAllUserInRole("teacher_temp"),
                NewAdminsToAccept = GetAllUserInRole("admin_temp"),

                PublicData = db.PublicData.ToList(),
                News = db.News.ToList(),

                Menus = db.Menus.Include(x=>x.MenuForRole).Include(x=>x.MenuCategories).ToList(),
            };

            return View(v);
        }

        [HttpPost]
        public ActionResult AcceptUser(string username, string type)
        {
            var user = db.Users.Single(x => x.UserName == username);
            var role = db.Roles.Single(x => x.Name.ToLower() == type.ToLower());

            changeRoleForUser(user, role.Name);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CancelUser(string username)
        {
            var user = db.Users.Single(x => x.UserName == username);

            db.Users.Remove(user);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult AddNews(AdminViewModel a)
        {
            db.News.Add(a.addNews);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteNews(int id)
        {
            db.News.Remove(db.News.Single(x => x.ID == id));
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddPublicData(AdminViewModel a)
        {
            if (ModelState.IsValid)
            {
                db.PublicData.Add(a.addPublicData);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeletePublicData(int id)
        {
            db.PublicData.Remove(db.PublicData.Single(x => x.ID == id));
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        public List<ApplicationUser> GetAllUserInRole(string role)
        {
            string id = db.Roles.Single(x => x.Name == role).Id;
            return db.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(id)).ToList();
        }

        private bool changeRoleForUser(ApplicationUser user, string role)
        {
            var Manager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());

            var oldUser = Manager.FindById(user.Id);
            var oldRoleId = oldUser.Roles.SingleOrDefault().RoleId;
            var oldRoleName = db.Roles.SingleOrDefault(r => r.Id == oldRoleId).Name;

            if (oldRoleName != role)
            {
                Manager.RemoveFromRole(user.Id, oldRoleName);
                Manager.AddToRole(user.Id, role);
            }
            db.Entry(user).State = EntityState.Modified;
            return true;
        }
    }
}