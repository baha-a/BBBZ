using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BBBZ.Models;
using System.Data.Entity;

namespace BBBZ.Controllers
{
    public class UsersController : BaseController
    {
        public ActionResult Index(string user)
        {
            var u = db.Users.SingleOrDefault(x => x.UserName == user);
            return View(u);
        }
	}
}