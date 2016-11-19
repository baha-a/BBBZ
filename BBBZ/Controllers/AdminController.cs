using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BBBZ.Models;

using System.Data.Entity;
using System.Web.Security;

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
            var v = new AdminViewModel() {
                
                NewUsersToAccept = GetAllUserInRole("user_temp"),
                NewTeachersToAccept = GetAllUserInRole("teacher_temp"),
                NewAdminsToAccept = GetAllUserInRole("admin_temp"),

                PublicData = db.PublicData.ToList(),
                News = db.News.ToList()
            };

            return View(v);
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
            if(ModelState.IsValid)
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
	}
}