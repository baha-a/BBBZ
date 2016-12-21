using BBBZ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.IO;

public class CategoryView
{
    public int ID { get; set; }
    public string Title { get; set; }
}

public static class Extenisons
{
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

    public static List<CategoryView> GetAllCategories()
    {
        return GetParentCatgory().FillWithChildren().ConvertToViewModel();
    }

    public static List<Category> GetParentCatgory()
    {
        return db.Categories.Where(x => x.Parent == null).ToList();
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
            a.Add(new CategoryView() { ID = i.ID, Title = Extenisons.Dashis(level) + i.Title });
            a.AddRange(ConvertToViewModel(i.SubCategories, level + 1));
        }
        return a;
    }

    private static BBBZ.Models.ApplicationDbContext db
    {
        get { return new BBBZ.Models.ApplicationDbContext(); }
    }

    public static List<SelectableGroup> GetAllGroups()
    {
        return db.Groups.Where(x => x.Parent == null).ToList().FillWithChildren().ConvertToViewModel();
    }
    public static List<Group> FillWithChildren(this List<Group> gs)
    {
        foreach (var g in gs)
        {
            g.Children = db.Groups.Where(x => x.Parent != null && x.Parent.ID == g.ID).ToList();
            FillWithChildren(g.Children);
        }
        return gs;
    }
    public static List<SelectableGroup> ConvertToViewModel(this List<Group> gs,int level = 0)
    {
        List<SelectableGroup> a = new List<SelectableGroup>();
        foreach (var i in gs)
        {
            a.Add(new SelectableGroup() { ID = i.ID, Selected = false, Text = Extenisons.Dashis(level) + i.Title , Group = i});
            a.AddRange(ConvertToViewModel(i.Children, level + 1));
        }
        return a;
    }


    public static List<MenuViewModel> GetAllMenuItems(List<Menu> ms=null)
    {
        return ((ms != null) ? ms : GetAllMenuItemParents()).FillWithChildren().ConvertToViewModel();
    }

    private static List<Menu> GetAllMenuItemParents()
    {
        return db.Menus.Where(x => x.Parent == null).ToList();
    }

    public static List<Menu> FillWithChildren(this List<Menu> gs)
    {
        foreach (var g in gs)
        {
            g.Menus = db.Menus
                .Include(x => x.Parent)
                .Where(x => x.Parent != null && x.Parent.ID == g.ID)
                .ToList();
            FillWithChildren(g.Menus);
        }
        return gs;
    }
    public static List<MenuViewModel> ConvertToViewModel(this List<Menu> gs, int level = 0)
    {
        List<MenuViewModel> a = new List<MenuViewModel>();
        foreach (var i in gs)
        {
            a.Add(new MenuViewModel() { ID = i.ID, Title = Extenisons.Dashis(level) + i.Title, Menu = i });
            a.AddRange(ConvertToViewModel(i.Menus, level + 1));
        }
        return a;
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

    public static string AddBackslashFirst(this string x)
    {
        if(string.IsNullOrEmpty(x) == false)
            if (x[0] != '\\')
                x = "\\" + x;
        return x;
    }
    public static string AddBackslash(this string x)
    {
        if(string.IsNullOrEmpty(x) == false)
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
}