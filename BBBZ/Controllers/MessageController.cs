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
                Inbox = db.Messages.Where(x => x.To_username == user).OrderByDescending(x=>x.Date).ToList(),
                Outbox = db.Messages.Where(x => x.From_username == user).OrderByDescending(x => x.Date).ToList()
            });
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        public ActionResult Create(string touser = "")
        {
            if(string.IsNullOrEmpty(Username) == true)
            {
                return Unauthorized();
            }

            return View(new Message(){
                To_username = touser,
                From_username = Username
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Message message, HttpPostedFileBase Uploader)
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

                message.Attachment = Uploader.UploadMessageAttachment(message);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(message);
        }
    }
}
