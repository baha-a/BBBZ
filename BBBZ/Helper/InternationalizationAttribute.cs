using System.Web.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

    /// <summary>
    /// Set language that is defined in route parameter "lang"
    /// </summary>
public class InternationalizationAttribute: ActionFilterAttribute
{
    private readonly IList<string> _supportedLocales;
    private readonly string _defaultLang;

    public InternationalizationAttribute()
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
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        // Get locale from route values
        string lang = (string)filterContext.RouteData.Values["lang"] ?? _defaultLang;

        // If we haven't found appropriate culture - seet default locale then
        if (!_supportedLocales.Contains(lang))
            lang = _defaultLang;

        SetLang(lang);
    }
}