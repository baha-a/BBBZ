using BBBZ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class CategoryView
{
    public int ID { get; set; }
    public string Title { get; set; }
}

public static class Extenisons
{
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
        return db.Categories.ToList().FillWithChildren().ConvertToViewModel();
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
}