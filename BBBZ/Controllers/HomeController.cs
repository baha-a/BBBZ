using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BBBZ.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BBBZ.Controllers
{
    public class HomeController: Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}




// how to user github with VS13 : http://michaelcrump.net/setting-up-github-to-work-with-visual-studio-2013-step-by-step/