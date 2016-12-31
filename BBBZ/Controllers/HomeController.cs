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
            return View(new HomeViewModel(){
                PublicData = db.PublicData.Where(x => x.Language == Language).ToList(),
            });
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Show()
        {
            return View();
        }
    }
}