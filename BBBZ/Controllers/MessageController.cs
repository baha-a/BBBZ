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
    public class MessageController : BaseController
    {
        public ActionResult Index()
        {
            string user = User.Identity.Name;
            return View(new MessageViewModel()
            {
                Inbox = db.Messages.Where(x => x.To_username == user).ToList(),
                Outbox = db.Messages.Where(x => x.From_username == user).ToList()
            });
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Message message)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.SingleOrDefault(x => x.UserName == message.To_username) == null)
                {
                    ModelState.AddModelError("To_username", "username not found");
                    return View(message);
                }
                message.Date = DateTime.Now;
                message.From_username = User.Identity.Name;
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(message);
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
