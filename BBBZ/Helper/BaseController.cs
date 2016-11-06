using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

public abstract class BaseController: Controller
{
    private readonly IList<string> _supportedLocales;
    private readonly string _defaultLang;

    protected string Language { get; private set; }

    public BaseController()
    {
        // Get supported locales list
        _supportedLocales = LocalizationHelper.GetSupportedLocales();

        // Set default locale
        _defaultLang = _supportedLocales[0];
    }

    /// <summary>
    /// Apply locale to current thread
    /// </summary>
    /// <param name="lang">locale name</param>
    private void SetLang(string lang)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(lang);
        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(lang);

        Language = lang;
    }

    protected override void OnActionExecuted(ActionExecutedContext filterContext)
    {
        // Get locale from route values
        string lang = (string)filterContext.RouteData.Values["lang"] ?? _defaultLang;

        // If we haven't found appropriate culture - seet default locale then
        if (!_supportedLocales.Contains(lang))
            lang = _defaultLang;

        SetLang(lang);

        base.OnActionExecuted(filterContext);
    }
}