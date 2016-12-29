using BBBZ.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BBBZ.Controllers
{
    public class MediaController : BaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            IsAllowed(MyPermission.Media);
        }


        private string  GoToMyPath()
        {
            return (Server.MapPath("\\Files\\" + User.Identity.Name).Replace("\\", "/")).CheckFolder();
        }
        private DirectoryInfo GoToMyFolder(string path)
        {
            try
            {
                return new DirectoryInfo(GoToMyPath() + path.AddBackslash().Replace("\\", "/"));
            }
            catch 
            {
                throw new HttpException(404,"File or Folder not found\r\nPath:"+ path); 
            }
        }

        public ActionResult Index(string path = "\\",string folder="")
        {
            if (string.IsNullOrEmpty(path))
                path = "\\";

            var d = GoToMyFolder(path.AddBackslash() + folder);
            return View("Index",new MediaViewModel()
            {
                Files = d.GetFiles().Select(x => x.Name).ToArray(),
                Folders = d.GetDirectories().Select(x => x.Name).ToArray(),
                CurrnetPath = path.AddBackslash() + folder.AddBackslash()
            });
        }

        public ActionResult Back(string path)
        {
            if (path.Length > 1)
            {
                int i = path.LastIndexOf("\\");
                if (i == path.Length - 1)
                    i = path.LastIndexOf("\\", i - 1);
                path = path.Remove(i + 1);
            }
            return RedirectToAction("Index", new { path = path });
        }

        public ActionResult DeleteFile(string file, string path)
        {
            GoToMyFolder(path)
                .GetFiles()
                .SingleOrDefault(x => x.Name == file)
                .Delete();
            return RedirectToAction("Index", new { path = path });
        }

        public ActionResult RenameFile(string file, string path, string name)
        {
            GoToMyFolder(path)
                .GetFiles()
                .SingleOrDefault(x => x.Name == file)
                .MoveTo(name);
            return RedirectToAction("Index", new { path = path });
        }


        public ActionResult CreateFolder(string folder, string path)
        {
            GoToMyFolder(path).CreateSubdirectory(folder);
            return RedirectToAction("Index", new { path = path });
        }
        
        public ActionResult DeleteFolder(string folder, string path)
        {
            GoToMyFolder(path)
                .GetDirectories()
                .SingleOrDefault(x => x.Name == folder)
                .Delete(true);
            return RedirectToAction("Index", new { path = path });
        }

        public ActionResult RenameFolder(string folder, string path, string name)
        {
            GoToMyFolder(path)
                .GetDirectories()
                .SingleOrDefault(x => x.Name == folder)
                .MoveTo(name);
            return RedirectToAction("Index", new { path = path });
        }


        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase Uploader, string path)
        {
            using (var sr = Uploader.InputStream)
            {
                using (FileStream sw = new FileStream(GoToMyFolder(path).FullName + Uploader.FileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    byte[] bufr = new byte[1024 * 1024];

                    for (int readed=0 ; (readed = sr.Read(bufr, 0, bufr.Length)) > 0 ; sw.Write(bufr, 0, readed))
                        ;
                }
            }
            return RedirectToAction("Index", new { path = path });
        }


        [HttpPost]
        public ActionResult UploadProfileImage(HttpPostedFileBase Uploader)
        {
            if (Uploader != null)
            {
                var pro = db.Profiles.SingleOrDefault(x => x.username == Username);
                if (pro != null)
                {
                    string extension = Path.GetExtension(Uploader.FileName);
                    if (extension == ".jpg" || extension == ".png")
                    {
                        pro.Image = "/ProfileImages/" + Username + extension;
                        Uploader.SaveAs(Server.MapPath("~/ProfileImages/").CheckFolder() + Username + extension);
                        db.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Index", "Profile", new { username = Username });
        }
        
        [HttpPost]
        public JsonResult UploadCategoryImage(HttpPostedFileBase Uploader,int id)
        {
            if (Uploader != null)
            {
                var pro = db.Categories.SingleOrDefault(x => x.ID == id);
                if (pro != null)
                {
                    string extension = Path.GetExtension(Uploader.FileName);
                    if (extension == ".jpg" || extension == ".png")
                    {
                        pro.Image = "/Cateogries/" + id + extension;
                        Uploader.SaveAs(Server.MapPath("~/Cateogries/").CheckFolder() + id + extension);
                        db.SaveChanges();
                    }
                }
            }
            return Json("ok");
        }
	}
}