using BBBZ.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

using System.Data.Entity;

public abstract class BaseController: Controller
{
    private readonly IList<string> _supportedLocales;
    private readonly string _defaultLang;

    public string Language { get; private set; }

    protected ApplicationDbContext db;

    public BaseController()
    {
        // Get supported locales list
        _supportedLocales = LocalizationHelper.GetSupportedLocales();

        // Set default locale
        _defaultLang = _supportedLocales[0];

        db = new ApplicationDbContext();
        var m = db.Menus.ToList()[0];
        ViewBag.Menu = getCategoriesTree(db.MenuCategories.Where(x => x.Menu.ID == m.ID).Select(x => x.Category).ToList());
    }

    private List<Category> getCategoriesTree(List<Category> cat)
    {
        foreach (var c in cat)
        {
            c.SubCategories = db.Categories.Where(x => x.Parent != null && x.Parent.ID == c.ID).ToList();
            getCategoriesTree(c.SubCategories);
        }
        return cat;
    }

    /// Apply locale to current thread
    private void SetLang(string lang)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(lang);
        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(lang);

        Language = lang;
    }

    protected override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        // Get locale from route values
        string lang = (string)filterContext.RouteData.Values["lang"] ?? _defaultLang;

        lang = lang.ToLower();
        // If we haven't found appropriate culture - seet default locale then
        if (!_supportedLocales.Contains(lang))
            lang = _defaultLang;

        SetLang(lang);
        
        base.OnActionExecuting(filterContext);
    }
}