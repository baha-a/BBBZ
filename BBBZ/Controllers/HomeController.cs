﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BBBZ.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading;

namespace BBBZ.Controllers
{
    [InternationalizationAttribute]
    public class HomeController: Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            //db.PublicData.ToList()
            return View(new List<PublicData>());
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}




// how to user github with VS13 : http://michaelcrump.net/setting-up-github-to-work-with-visual-studio-2013-step-by-step/