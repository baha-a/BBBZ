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
    [Authorize(Roles = "admin,superadmin")]
    public class AdminController: BaseController
    {   
        private void deleteallCategories(Category[] cat)
        {
            foreach (var c in cat)
            {
                if (c.SubCategories.Count >= 0)
                    deleteallCategories(c.SubCategories.ToArray());
                db.Categories.Remove(c);
            }
        }

        public ActionResult Index()
        {
            //deleteallCategories(getCategoriesTree(db.Categories.ToList()).ToArray());
            //db.SaveChanges();

            var v = new AdminViewModel()
            {
                NewUsersToAccept = GetAllUserInRole("user_temp"),
                NewTeachersToAccept = GetAllUserInRole("teacher_temp"),
                NewAdminsToAccept = GetAllUserInRole("admin_temp"),

                PublicData = db.PublicData.ToList(),
                News = db.News.ToList(),

                Menus = db.Menus.Include(x=>x.MenuForRole).ToList(),

                JsonOBJ = generateJsonArrayFromMenus(),
            };

            return View(v);
        }

        private List<string> generateJsonArrayFromMenus()
        {
            List<Menu> menus = db.Menus.ToList();
            
            List<string> ans = new List<string>();

            foreach (var m in menus)
                ans.Add(generateJsonArrayFromMenusHelper(getCategoriesTree(db.MenuCategories.Where(x => x.Menu.ID == m.ID).Select(x => x.Category).ToList())));

            return ans;
        }

        public List<Category> getCategoriesTree(List<Category> cat)
        {
            foreach (var c in cat)
            {
                c.SubCategories = db.Categories.Where(x => x.Parent != null && x.Parent.ID == c.ID).ToList();
                getCategoriesTree(c.SubCategories);
            }
            return cat;
        }

        private string generateJsonArrayFromMenusHelper(List<Category> cat)
        {
            string ans = "[";
            foreach (var c in cat)
            {
                ans += "{\"id\":" + c.ID + ",\"name\":\"" + c.Key + "\", \"url\":\"" + c.Url + "\"";
                if (c.SubCategories.Count > 0)
                    ans += ", \"children\":" + generateJsonArrayFromMenusHelper(c.SubCategories);
                ans += "},";
            }
            if(ans.EndsWith(","))ans = ans.Remove(ans.Length - 1, 1);
            ans += "]";
            return ans;
        }

        [HttpPost]
        public ActionResult AcceptUser(string username, string type)
        {
            var user = db.Users.Single(x => x.UserName == username);
            var role = db.Roles.Single(x => x.Name.ToLower() == type.ToLower());

            changeRoleForUser(user, role.Name);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CancelUser(string username)
        {
            var user = db.Users.Single(x => x.UserName == username);

            db.Users.Remove(user);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult AddNews(AdminViewModel a)
        {
            db.News.Add(a.addNews);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteNews(int id)
        {
            db.News.Remove(db.News.Single(x => x.ID == id));
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddPublicData(AdminViewModel a)
        {
            if (ModelState.IsValid)
            {
                db.PublicData.Add(a.addPublicData);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeletePublicData(int id)
        {
            db.PublicData.Remove(db.PublicData.Single(x => x.ID == id));
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult AddMenu(string name)
        {
            db.Menus.Add(new Menu() { Name = name });
            db.SaveChanges();

            return Json("ok");
        }

        [HttpPost]
        public JsonResult SaveMenu(int id, string json)
        {
            // [{"id":1, "name":"wewe", "url":"http://google.com", "new":0, "edit":0, "children":[{},{},{},....]}]
            var r = JsonConvert.DeserializeObject<List<CateogriesJsonItem>>(json);

            var m = db.Menus.Single(x => x.ID == id);
            db.MenuCategories.RemoveRange(db.MenuCategories.Where(x => x.Menu.ID == id).ToList());

            int order = 1;
            Category c;
            foreach (var i in r)
            {
                if (i.New)
                {
                    c = new Category() { Key = i.name, Url = i.url };
                    c.SubCategories = generateSubCategories(c, i);
                    db.MenuCategories.Add(new MenuCategory() { Menu = m, Category = c, Order = order++ });
                }
                else if(i.edit)
                {
                    c = db.Categories.Single(x => x.ID == i.id);
                    c.Key = i.name;
                    c.Url = i.url;
                    c.SubCategories = generateSubCategories(c, i);
                    //db.MenuCategories.Single(x => x.Menu.ID == m.ID && x.Category.ID == c.ID).Order = order++;
                    db.MenuCategories.Add(new MenuCategory() { Menu = m, Category = c, Order = order++ });
                }
                else if (i.delete)
                {
                    //db.MenuCategories.Remove(db.MenuCategories.Single(x => x.Menu.ID == id && x.Category.ID == i.id));
                }
                else
                {
                    c = db.Categories.Single(x => x.ID == i.id);
                    c.SubCategories = generateSubCategories(c, i);
                    db.MenuCategories.Add(new MenuCategory() { Menu = m, Category = c, Order = order++ });
                }
            }

            ///// remove remined categories later

            db.SaveChanges();
            return Json("ok");
        }

        private List<Category> generateSubCategories(Category parent, CateogriesJsonItem t)
        {
            List<Category> ans = new List<Category>();
            Category c = null;
            foreach (var i in t.children)
            {
                if (i.New)
                {
                    c = new Category() { Parent = parent, Key = i.name, Url = i.url };
                    c.SubCategories = generateSubCategories(c, i);
                    ans.Add(c);
                }
                else if(i.edit)
                {
                    c = db.Categories.Single(x => x.ID == i.id);
                    c.Key = i.name;
                    c.Url = i.url;
                    c.SubCategories = generateSubCategories(c, i);
                    ans.Add(c);
                }
                else if(i.delete)
                {
                    parent.SubCategories.Remove(db.Categories.Single(x => x.ID == i.id));
                }
                else
                {
                    if (parent.ID == i.id)
                        continue;
                    c = db.Categories.Single(x => x.ID == i.id);
                    c.SubCategories = generateSubCategories(c, i);
                    ans.Add(c);
                }
            }
            return ans;
        }

        public List<ApplicationUser> GetAllUserInRole(string role)
        {
            string id = db.Roles.Single(x => x.Name == role).Id;
            return db.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(id)).ToList();
        }

        private bool changeRoleForUser(ApplicationUser user, string role)
        {
            var Manager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());

            var oldUser = Manager.FindById(user.Id);
            var oldRoleId = oldUser.Roles.SingleOrDefault().RoleId;
            var oldRoleName = db.Roles.SingleOrDefault(r => r.Id == oldRoleId).Name;

            if (oldRoleName != role)
            {
                Manager.RemoveFromRole(user.Id, oldRoleName);
                Manager.AddToRole(user.Id, role);
            }
            db.Entry(user).State = EntityState.Modified;
            return true;
        }
    }
}