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
using Newtonsoft.Json;

namespace BBBZ.Controllers
{
    //[Authorize(Roles = "admin")]
    public class AdminController: BaseController
    {
        public ActionResult Index(bool onlyOnce = false)
        {
            IsAllowed(MyPermission.AdminPanel);

            if (Session["isAdminLayout"] == null)
                Session["isAdminLayout"] = true;
            else
                Session["isAdminLayout"] = !(Session["isAdminLayout"].ToString() == true.ToString());

            return RedirectToAction("Index", "Home");
        }
    }
}