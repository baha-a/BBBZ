using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BBBZ.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading;

namespace BBBZ.Controllers
{
    public class HomeController: BaseController
    {
        public ActionResult Index()
        {
            if (SettingManager.StartupMenuItem != null)
            {
                var m = db.Menus.SingleOrDefault(x => x.ID == SettingManager.StartupMenuItem);
                if (m != null)
                {
                    if (m.Type.ToLower() == "singlepage")
                        return RedirectToAction("Show", "Contents", new { id = m.ContentID });
                    else if (m.Type.ToLower() == "category")
                        return RedirectToAction("Show", "Category", new { id = m.CategoryID });
                    else if (m.Type.ToLower() == "link")
                        return Redirect(m.Url);
                }
            }

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}