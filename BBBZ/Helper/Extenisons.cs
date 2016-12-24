using BBBZ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.IO;
using System.Web.Mvc;


public static class Extenisons
{

    #region general helper
    static ApplicationDbContext db = new ApplicationDbContext();

    public static int ParentsLength(Category c)
    {
        int lngth = -1;
        for (Category mover = c ; mover != null ; mover = mover.Parent)
            lngth++;
        return lngth;
    }
    public static string Dashis(int count)
    {
        string ans = "";
        for (int i = 0 ; i < count ; i++)
            ans += "– ";
        return ans;
    }

    public static List<BBBZ.Models.Language> GetAllLanguages()
    {
        return db.Languages.ToList();
    }
    public static List<BBBZ.Models.ViewLevel> GetAllViewLevels()
    {
        return db.ViewLevels.ToList();
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
    public static List<Content> GetAllContents()
    {
        return db.Contents.ToList();
    }
    #endregion


    #region CategoryHelper
    public static List<CategoryView> GetAllCategories(int without = -1)
    {
        return GetParentCatgory(without).FillWithChildren(without).ConvertToViewModel();
    }
    public static List<Category> GetParentCatgory(int without = -1)
    {
        return db.Categories.Where(x => x.Parent == null && x.ID != without).ToList();
    }
    public static List<Category> FillWithChildren(this List<Category> gs, int without = -1)
    {
        foreach (var g in gs)
        {
            g.SubCategories = db.Categories.Where(x => x.Parent != null && x.Parent.ID == g.ID && x.ID != without).ToList();
            FillWithChildren(g.SubCategories, without);
        }
        return gs;
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
    #endregion


    #region GroupHelper
    public static List<SelectableGroup> GetAllGroups(int without = -1)
    {
        return GetParentGroup(without).FillWithChildren(without).ConvertToViewModel();
    }
    public static List<Group> GetParentGroup(int without =-1)
    {
        return db.Groups.Where(x => x.Parent == null && x.ID == without).ToList();
    }
    public static List<Group> FillWithChildren(this List<Group> gs, int without = -1)
    {
        foreach (var g in gs)
        {
            g.Children = db.Groups.Where(x => x.Parent != null && x.Parent.ID == g.ID && x.ID != without).ToList();
            FillWithChildren(g.Children, without);
        }
        return gs;
    }
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
    #endregion


    #region MenuItemHelper
    public static List<MenuViewModel> GetAllMenuItems(List<Menu> ms = null, int without =-1)
    {
        return ((ms != null) ? ms : GetAllMenuItemParents()).FillWithChildren(without).ConvertToViewModel();
    }
    public static List<Menu> GetAllMenuItemParents()
    {
        return db.Menus.Where(x => x.Parent == null).ToList();
    }
    public static List<Menu> FillWithChildren(this List<Menu> gs, int without = -1)
    {
        foreach (var g in gs)
        {
            g.Children = db.Menus
                .Include(x => x.Parent)
                .Where(x => x.Parent != null && x.Parent.ID == g.ID && g.ID != without)
                .ToList();
            FillWithChildren(g.Children, without);
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



    public static List<Menu> GetChildernWithCheck(this List<Menu> gs, List<int> levels)
    {
        foreach (var g in gs)
        {
            g.Children = db.Menus
                .Include(x => x.Parent)
                .Include(x => x.Access)
                .Where(x => x.Parent != null && x.Parent.ID == g.ID && x.Published && x.Access != null && levels.Contains(x.Access.ID))
                .ToList();
            GetChildernWithCheck(g.Children, levels);
        }
        return gs;
    }
}