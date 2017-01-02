using BBBZ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.IO;
using System.Web.Mvc;
using System.Web.Hosting;


public static class Extenisons
{
    #region general helper
    public static string Dashis(int count)
    {
        string ans = "";
        for (int i = 0 ; i < count ; i++)
            ans += "– ";
        return ans;
    }

    public static List<Language> GetAllLanguages(this ApplicationDbContext db)
    {
        return db.Languages.ToList();
    }
    public static List<string> GetAllMenuTypes()
    {
        return new List<string>(new string[] 
        {
            "SinglePage",
            "Category",
            "Link"
        });
    }
    public static List<CustomField> GetAllCustomFields(this ApplicationDbContext db)
    {
        return db.CustomFields.ToList();
    }

    public static string getString(List<int> x)
    {
        string str ="";
        x.ForEach(z => str += z + "\r\n");
        return str;
    }


    public static string[] MargeWith(this string x,string[] list)
    {
        string[] res = new string[list.Length + 1];
        res[0] = x;
        for (int i = 0 ; i < list.Length ; i++)
            res[i+1] = list[i];

        return res;
    }

    #endregion

    #region CategoryHelper
    public static int ParentsLength(Category c)
    {
        int lngth = -1;
        for (Category mover = c ; mover != null ; mover = mover.Parent)
            lngth++;
        return lngth;
    }
    public static List<CategoryView> ConvertToViewModel(this List<Category> gs, int level = 0)
    {
        List<CategoryView> a = new List<CategoryView>();
        foreach (var i in gs)
        {
            a.Add(new CategoryView() { ID = i.ID, Title = Extenisons.Dashis(level) + i.Title, theCategory = i});
            a.AddRange(ConvertToViewModel(i.SubCategories, level + 1));
        }
        return a;
    }

    public static bool UploadCategoryImage(this HttpPostedFileBase Uploader, Category cat)
    {
        if (cat != null && Uploader != null)
        {
            string extension = Path.GetExtension(Uploader.FileName);
            if (extension == ".jpg" || extension == ".png")
            {
                cat.Image = "/CateogryImages/" + cat.ID + extension;
                Uploader.SaveAs(HostingEnvironment.MapPath("~/CateogryImages/").CheckFolder() + cat.ID + extension);
                return true;
            }
        }
        return false;
    }
    #endregion

    public static string UploadMessageAttachment(this HttpPostedFileBase Uploader, Message msg)
    {
        if (msg != null && Uploader != null)
        {
                string path = "/MessageAttachment/" + msg.From_username + "_" + msg.To_username + "/" ;
                Uploader.SaveAs(HostingEnvironment.MapPath(path).CheckFolder() + Path.GetFileName(Uploader.FileName));
                return path + Path.GetFileName(Uploader.FileName);
        }
        return "";
    }

    #region GroupHelper
    public static List<SelectableGroup> ConvertToViewModel(this List<Group> gs, int level = 0)
    {
        List<SelectableGroup> a = new List<SelectableGroup>();
        foreach (var i in gs)
        {
            a.Add(new SelectableGroup() { ID = i.ID, Selected = false, Text = Extenisons.Dashis(level) + i.Title, Group = i });
            a.AddRange(ConvertToViewModel(i.Children, level + 1));
        }
        return a;
    }

    public static string getString(this List<Group> x)
    {
        string str = "";
        x.ForEach(z => str += z.Title + "\r\n");
        return str;
    }
    #endregion


    #region Permission helper

    public static Permission CalculatePermissions(this Permission per, Permission g, bool first = false)
    {
        per.Users               = Check(per.Users               , g.Users               , first);
        per.Groups              = Check(per.Groups              , g.Groups              , first);
        per.ViewLevels          = Check(per.ViewLevels          , g.ViewLevels          , first);
        per.Menus               = Check(per.Menus               , g.Menus               , first);
        per.Languages           = Check(per.Languages           , g.Languages           , first);

        per.See_Categories      = Check(per.See_Categories      , g.See_Categories      , first);
        per.Create_Categories   = Check(per.Create_Categories   , g.Create_Categories   , first);
        per.Edit_Categories     = Check(per.Edit_Categories     , g.Edit_Categories     , first);
        per.Delete_Categories   = Check(per.Delete_Categories   , g.Delete_Categories   , first);

        per.See_Contents        = Check(per.See_Contents        , g.See_Contents        , first);
        per.Create_Contents     = Check(per.Create_Contents     , g.Create_Contents     , first);
        per.Edit_Contents       = Check(per.Edit_Contents       , g.Edit_Contents       , first);
        per.Delete_Contents     = Check(per.Delete_Contents     , g.Delete_Contents     , first);

        per.AdminPanel          = Check(per.AdminPanel          , g.AdminPanel          , first);
        per.Media               = Check(per.Media               , g.Media               , first);

        return per;
    }

    public static bool? Check(bool? b, bool? d, bool first = false)
    {
        if (b == null || (first && b == false && d == true))
            return d;
        return b;
    }

    public static bool HasNull(this Permission per)
    {
        return
            per.Users == null ||
            per.Groups == null ||
            per.Menus == null ||
            per.ViewLevels == null ||
            per.Languages == null ||

            per.Questions == null ||

            per.Media == null ||
            per.AdminPanel == null ||

            per.See_Categories == null ||
            per.Create_Categories == null ||
            per.Edit_Categories == null ||
            per.Delete_Categories == null ||

            per.See_Contents == null ||
            per.Create_Contents == null ||
            per.Edit_Contents == null ||
            per.Delete_Contents == null;
    }
    public static bool IsAllTrue(this Permission per)
    {
        return
            per.Users == true &&
            per.Groups == true &&
            per.Menus == true &&
            per.ViewLevels == true &&
            per.Languages == true &&

            per.Questions == true &&

            per.Media == true &&
            per.AdminPanel == true &&

            per.See_Categories == true &&
            per.Create_Categories == true &&
            per.Edit_Categories == true &&
            per.Delete_Categories == true &&

            per.See_Contents == true &&
            per.Create_Contents == true &&
            per.Edit_Contents == true &&
            per.Delete_Contents == true;
    }
    #endregion



    #region MenuItemHelper
    public static List<MenuViewModel> GetAllMenuItems(this ApplicationDbContext db,List<Menu> ms = null, int without =-1)
    {
        return ((ms != null) ? ms : db.GetAllMenuItemParents()).FillWithChildren(db,without).ConvertToViewModel();
    }
    public static List<Menu> GetAllMenuItemParents(this ApplicationDbContext db,int without =-1)
    {
        return db.Menus.Include(x => x.Access).Where(x => x.Parent == null && x.ID != without).ToList();
    }
    public static List<Menu> FillWithChildren(this List<Menu> gs,ApplicationDbContext db, int without = -1)
    {
        foreach (var g in gs)
        {
            g.Children = db.Menus
                .Include(x => x.Parent)
                .Include(x => x.Access)  
                .Where(x => x.Parent != null && x.Parent.ID == g.ID && g.ID != without)
                .ToList();
            FillWithChildren(g.Children,db, without);
        }
        return gs;
    }
    public static List<MenuViewModel> ConvertToViewModel(this List<Menu> gs, int level = 0)
    {
        List<MenuViewModel> a = new List<MenuViewModel>();
        foreach (var i in gs)
        {
            a.Add(new MenuViewModel() { ID = i.ID, Title = Extenisons.Dashis(level) + i.Title, Menu = i });
            a.AddRange(ConvertToViewModel(i.Children, level + 1));
        }
        return a;
    }
    #endregion


    #region fileHelper
    public static string AddBackslashFirst(this string x)
    {
        if (string.IsNullOrEmpty(x) == false)
            if (x[0] != '\\')
                x = "\\" + x;
        return x;
    }
    public static string AddBackslash(this string x)
    {
        if (string.IsNullOrEmpty(x) == false)
            if (x[x.Length - 1] != '\\')
                x += "\\";
        return x;
    }

    public static string CheckFolder(this string path)
    {
        if (Directory.Exists(path) == false)
            Directory.CreateDirectory(path);

        return path;
    }
    public static MvcHtmlString UploadFile(this HtmlHelper helper, string name, object htmlAttributes = null)
    {
        TagBuilder input = new TagBuilder("input");
        input.Attributes.Add("type", "file");
        input.Attributes.Add("id", helper.ViewData.TemplateInfo.GetFullHtmlFieldId(name));
        input.Attributes.Add("name", helper.ViewData.TemplateInfo.GetFullHtmlFieldName(name));

        if (htmlAttributes != null)
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            input.MergeAttributes(attributes);
        }

        return new MvcHtmlString(input.ToString());
    }
    #endregion
}