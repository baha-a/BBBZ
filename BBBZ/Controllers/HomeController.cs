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
            return View(db.PublicData.Where(x => x.Language == Language).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }


        public ActionResult Courses()
        {
            //var d = new List<Category>();
            //var v = new Category() { Name = "first year" };
            
            //v.SubCategories.Add(new Category() { Name = "first semester" });

            //var v2 = new Category() { Name = "second semester" };
            //v2.SubCategories.Add(new Category() { Name = "programming" });
            //v2.SubCategories.Add(new Category() { Name = "database" });
            //v2.SubCategories[0].SubCategories.Add(new Category() { Name = "beginner programming" });
            //v2.SubCategories[0].SubCategories.Add(new Category() { Name = "Advance programming" });
            //v2.Items.Add(new Item() { Name = "Course1" });
            //v2.Items.Add(new Item() { Name = "Course2" });
            //v.SubCategories.Add(v2);
            
            //v.SubCategories.Add(new Category() { Name = "SubCat3" });
            //v.SubCategories.Add(new Category() { Name = "SubCat4" });
            //d.Add(v);
            

            //d.Add(new Category() { Name = "second year" });
            //d.Add(new Category() { Name = "thired year" });
            //v = new Category() { Name = "fourth year" };
            //v.SubCategories.Add(new Category() { Name = "SubCat1" });
            //v.SubCategories.Add(new Category() { Name = "SubCat2" });
            //v.Items.Add(new Item() { Name = "Course3" });
            //v.Items.Add(new Item() { Name = "Course4" });
            //d.Add(v);

            


            return View();
        }
    }
}




// how to user github with VS13 : http://michaelcrump.net/setting-up-github-to-work-with-visual-studio-2013-step-by-step/