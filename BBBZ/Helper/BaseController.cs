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

    protected override HttpNotFoundResult HttpNotFound(string statusDescription)
    {
        throw new HttpException(404, statusDescription);
    }

    protected new ActionResult HttpNotFound()
    {
        return RedirectToAction("Index", "Error", new { id = 404 });
    }

    protected ActionResult BadRequest()
    {
        return RedirectToAction("Index", "Error", new { id = 400 });
    }

    protected ActionResult Unauthorized()
    {
        return RedirectToAction("Index", "Error", new { id = 401 });
    }
}