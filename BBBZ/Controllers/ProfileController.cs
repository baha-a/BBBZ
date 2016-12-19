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
    public class ProfileController : BaseController
    {

        public ActionResult Index(string username)
        {
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Index", "Home");

            if (username == User.Identity.Name)
                return Edit(username);
            return Details(username);
        }

        private ActionResult Details(string username)
        {
            if (username == null)
                return BadRequest();
            Profile profile = db.Profiles.SingleOrDefault(u => u.username == username);
            if (profile == null)
                return HttpNotFound();
            return View("Details", profile);
        }

        private ActionResult Edit(string username)
        {
            if (username == null)
                return BadRequest();

            if (username != User.Identity.Name)
                return Unauthorized();

            Profile profile = db.Profiles.SingleOrDefault(u => u.username == username);

            if (profile == null)
                return HttpNotFound();

            return View("Edit", profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Profile pro)
        {
            if (ModelState.IsValid)
            {
                if (pro.username != User.Identity.Name)
                    return Unauthorized();

                var p = db.Profiles.SingleOrDefault(x => x.username == pro.username);
                p.Name = pro.Name;
                p.LastName = pro.LastName;
                p.Email = pro.Email;
                p.Image = pro.Image;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(pro);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}

