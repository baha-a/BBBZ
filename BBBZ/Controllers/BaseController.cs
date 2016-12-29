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
    protected ApplicationDbContext db;
    private readonly IList<string> _supportedLocales;
    private readonly string _defaultLang;
    private void SetLang(string lang)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(lang);
        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(lang);

        Language = lang;
    }


    public string Language { get; private set; }
    
    public string Username { get; private set; }
    public List<Group> MyGroups { get; private set; }
    public List<int> MyViewLevelIDs { get; private set; }
    public Permission MyPermission { get; private set; }

    public List<Content> GetAllContents()
    {
        if (MyPermission == null || MyViewLevelIDs == null)
            return new List<Content>();

        if (MyPermission.See_Contents == true)
            return db.Contents.ToList();
        return db.Contents.Include(x => x.Access).Where(x => x.Access != null && MyViewLevelIDs.Contains(x.Access.ID)).ToList();
    }
    

    public BaseController()
    {
        _supportedLocales = LocalizationHelper.GetSupportedLocales();
        _defaultLang = _supportedLocales[0];

        db = new ApplicationDbContext();
    }

    protected override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (Session["isAdminLayout"] == null)
            Session["isAdminLayout"] = false;

        string lang = (string)filterContext.RouteData.Values["lang"] ?? _defaultLang;// Get locale from route values

        lang = lang.ToLower();
        if (!_supportedLocales.Contains(lang)) // If we haven't found appropriate culture - set default locale then
            lang = _defaultLang;

        SetLang(lang);

        Username = User.Identity.Name;
        MyGroups = FetchGroupsFor(Username);
        MyViewLevelIDs = FetchViewLevelFor(MyGroups);
        MyPermission = FetchPermissionFor(MyGroups);

        ViewBag.Permission = MyPermission;

        ViewBag.LayoutViewModel = new SharedLayoutViewModel()
        {
            TopMenuItems = BuildTheTopMenu(),
            SideMenus = BuildTheSideMenu()
        };

        base.OnActionExecuting(filterContext);
    }


    #region Menu builder
    public List<Menu> BuildTheTopMenu()
    {
        List<Menu>  toplist  = new List<Menu>();
            var topMenuTypeIds  = db.MenuTypes.Where(x => x.IsTopMenu && x.Published).Select(x => x.ID).ToList();
            topMenuTypeIds.ForEach(x => toplist.AddRange(fill(x, MyViewLevelIDs)));
        return toplist;
    }
    public List<SideMenuViewModel> BuildTheSideMenu()
    {
        List<SideMenuViewModel> sidelist = new List<SideMenuViewModel>();
            db.MenuTypes
                .Where(x => !x.IsTopMenu && x.Published)
                .ToList()
                .ForEach(m => sidelist.Add(new SideMenuViewModel(){
                    MenuType = m,
                    MenuItems = fill(m.ID, MyViewLevelIDs) }));
        return sidelist;
    }

    private List<Menu> fill(int menuTypeId, List<int> levels)
    {
        var itms = db.Menus
                    .Include(x => x.MenuType)
                    .Include(x => x.Access)
                    .Where(x =>
                        x.Access != null &&
                        levels.Contains(x.Access.ID) &&
                        x.Published &&

                        x.MenuType != null &&
                        x.MenuType.ID == menuTypeId)
                    .ToList();

        return  GetChildernWithCheck(itms, levels);
    }
    public List<Menu> GetChildernWithCheck(List<Menu> gs, List<int> levels)
    {
        foreach (var g in gs)
        {
            g.Children = db.Menus
                            .Include(x => x.Parent)
                            .Include(x => x.Access)
                            .Where(x =>
                                x.Access != null &&
                                levels.Contains(x.Access.ID) &&
                                x.Published &&

                                x.Parent != null &&
                                x.Parent.ID == g.ID)
                            .ToList();

            GetChildernWithCheck(g.Children, levels);
        }
        return gs;
    }
    #endregion

    #region get my groups
    public List<Group> FetchGroupsFor(string username)
    {
        List<Group>myGroups = null;

        if (string.IsNullOrEmpty(username))
            myGroups = db.Groups
                .Where(x => x.ID == GroupSetting.GuestGroupId)
                .Include(x => x.Parent)
                .Include(x => x.Access)
                .ToList();
        else
            myGroups = db.UserGroups
                .Where(x => x.username == username)
                .Select(x => x.Groups)
                .Include(x => x.Access)
                .Include(x => x.Parent)
                .ToList();

        return myGroups;
    }
    #endregion

    #region ViewLevel helper
    public List<ViewLevel> GetAllViewLevels()
    {
        if (MyPermission == null || MyViewLevelIDs == null)
            return new List<ViewLevel>();

        if (MyPermission.ViewLevels == true)
            return db.ViewLevels.ToList();
        return db.ViewLevels.Where(x => MyViewLevelIDs.Contains(x.ID)).ToList();
    }

    private List<int> FetchViewLevelFor(List<Group> groups)
    {
        List<ViewLevel> levels = new List<ViewLevel>();

        groups.ForEach(x => getAllMyViewLevel(levels, x));

        return levels.Select(x => x.ID).Distinct().ToList();
    }
    private void getAllMyViewLevel(List<ViewLevel> levels, Group g)
    {
        if (g == null)
            return;
        levels.AddRange(g.Access);
        if (g.Parent != null)
        getAllMyViewLevel(levels,
            db.Groups
            .Include(x => x.Parent)
            .Include(x => x.Access)
            .SingleOrDefault(x => x.ID == g.Parent.ID));
    }
    #endregion

    #region Permission helper
    public Permission FetchPermissionFor(List<Group> groups)
    {
        Permission tmp = new Permission();

        foreach (var g in groups)
        {
            tmp.CalculatePermissions(FetchPermissionForGroupWithParent(g), true);
            if (tmp.IsAllTrue())
                return tmp;
        }

        return tmp;
    }
    public Permission FetchPermissionForGroupWithParent(Group group)
    {
        Permission per = new Permission();
        if (per.CalculatePermissions(group.Permission).HasNull())
            if (group.Parent != null)
                per.CalculatePermissions(FetchPermissionForGroupWithParent(group.Parent));

        return per;
    }

    protected virtual bool IsAllowed(bool? tag)
    {
        if (tag != true)
            throw new HttpException(401, "not allowed to see this");
        return true;
    }
    #endregion


    #region Category helper
    public List<CategoryView> GetAllCategories(int without = -1)
    {
        List<Category> tmp = null;
        if (MyPermission.See_Categories == true)
            tmp = GetAllParentCatgory(without);
        else
            tmp = db.Categories.Include(x=>x.TheLanguage).Include(x => x.Access).Where(x => x.ID != without && x.Access != null && MyViewLevelIDs.Contains(x.Access.ID)).ToList();
        return FillWithChildren(tmp, without).ConvertToViewModel();
    }
    public List<Category> GetAllParentCatgory(int without = -1)
    {
        return db.Categories
            .Include(x => x.Access)
            .Include(x=>x.Parent)
            .Include(x => x.TheLanguage)
            .Where(x => x.Parent == null && x.ID != without)
            .ToList();
    }
    public List<Category> FillWithChildren(List<Category> gs,  int without = -1)
    {
        foreach (var g in gs)
        {
            g.SubCategories = 
                db.Categories
                .Include(x=>x.Access)
                .Include(x => x.Parent)
                .Include(x => x.TheLanguage)
                .Where(x => x.Parent != null && x.Parent.ID == g.ID && x.ID != without 
                    //&& (MyPermission.See_Categories == true ? true : MyViewLevelIDs.Contains(x.Access.ID))
                    )
                .ToList();
            FillWithChildren(g.SubCategories, without);
        }
        return gs;
    }
    #endregion


    #region Group helper
    public List<SelectableGroup> GetAllGroups(int without = -1)
    {
        if (MyPermission.Groups == true)
            return FillWithChildren(GetParentGroup(without), without).ConvertToViewModel();

        return MyGroups.ConvertToViewModel();
    }
    public List<Group> GetParentGroup(int without = -1)
    {
        return db.Groups.Where(x => x.Parent == null && x.ID != without).ToList();
    }
    public List<Group> FillWithChildren(List<Group> gs, int without = -1)
    {
        foreach (var g in gs)
        {
            g.Children = db.Groups.Where(x => x.Parent != null && x.Parent.ID == g.ID && x.ID != without).ToList();
            FillWithChildren(g.Children, without);
        }
        return gs;
    }
    #endregion


    #region Errors Thrower
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
    #endregion


    protected override void Dispose(bool disposing)
    {
        if (disposing)
            db.Dispose(); // this may create error on debugging -only- , so I Comment it for a while

        base.Dispose(disposing);
    }
}