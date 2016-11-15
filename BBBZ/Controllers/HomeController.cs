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
        ApplicationDbContext db = new ApplicationDbContext();

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
            var d = new List<Category>();
            var v = new Category() { Name = "Cat1" };
            
            v.SubCategories.Add(new Category() { Name = "SubCat1" });
            
            var v2 = new Category() { Name = "SubCat2" };
            v2.SubCategories.Add(new Category() { Name = "SubSubCat1" });
            v2.SubCategories.Add(new Category() { Name = "SubSubCat2" });
            v2.SubCategories[0].SubCategories.Add(new Category() { Name = "SubSubSubCat1" });
            v2.Course.Add(new Course() { Name = "Course1" });
            v2.Course.Add(new Course() { Name = "Course2" });
            v.SubCategories.Add(v2);
            
            v.SubCategories.Add(new Category() { Name = "SubCat3" });
            v.SubCategories.Add(new Category() { Name = "SubCat4" });
            d.Add(v);
            

            d.Add(new Category() { Name = "Cat2" });
            d.Add(new Category() { Name = "Cat3" });
            v = new Category() { Name = "Cat4" };
            v.SubCategories.Add(new Category() { Name = "SubCat1" });
            v.SubCategories.Add(new Category() { Name = "SubCat2" });
            v.Course.Add(new Course() { Name = "Course3" });
            v.Course.Add(new Course() { Name = "Course4" });
            d.Add(v);


            return View(d);
        }
    }
}




// how to user github with VS13 : http://michaelcrump.net/setting-up-github-to-work-with-visual-studio-2013-step-by-step/