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

    private void SetLang(string lang)
    {
        try
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(lang);
        }
        catch { }
        Language = lang;
    }

    public string Language { get; private set; }    
    public string Username { get; private set; }
    public List<Group> MyGroups { get; private set; }
    public List<int> MyViewLevelIDs { get; private set; }
    public Permission MyPermission { get; private set; }


    public BaseController()
    {
        db = new ApplicationDbContext();
        LocalizationHelper.SetSupportedLocales(db.GetAllLanguages());
    }

    protected bool _checkforuserlockedFLAG = true;
    protected override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (Session["isAdminLayout"] == null)
            Session["isAdminLayout"] = false;

        string lang = (string)filterContext.RouteData.Values["lang"];// Get locale from route values
        SetLang(lang.ToLower());

        Language = lang;
        ViewBag.SiteTitle = "Virtual School";
        try
        {
            ViewBag.SiteTitle = db.Languages.FirstOrDefault(x => x.Code.ToLower() == Language.ToLower()).SiteName;
        }
        catch { }
        Username = User.Identity.Name;

        if (_checkforuserlockedFLAG && string.IsNullOrEmpty(Username) == false)
        {
            var u = db.Users.SingleOrDefault(x => x.UserName == Username);
            if (u != null && u.Locked)
                throw new HttpException(401, "your account had been locked");
        }

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
                    .Include(x => x.TheLanguage)
                    .Where(x =>
                        (x.TheLanguage == null || x.TheLanguage.Code == Language) &&
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
                            .Include(x => x.TheLanguage)
                            .Where(x =>
                                (x.TheLanguage == null || x.TheLanguage.Code == Language) &&
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
                .Where(x => x.ID == SettingManager.GuestGroupId)
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
    public List<CategoryView> GetAllCategories(int without = -1, bool fiterlanguage = false)
    {
        List<Category> tmp = null;
        if (MyPermission.See_Categories == true)
            tmp = GetAllParentCatgory(without);
        else
            tmp = db.Categories
                .Include(x => x.Access)
                .Include(x => x.TheLanguage)
                .Where(x =>
                    x.Parent == null &&
                    (x.TheLanguage == null || (fiterlanguage == false || x.TheLanguage.Code == Language)) &&
                    x.ID != without &&
                    x.Access != null && MyViewLevelIDs.Contains(x.Access.ID))
                    .ToList();

        return FillWithChildren(tmp, without).ConvertToViewModel();
    }
    public List<Category> GetAllParentCatgory(int without = -1, bool fiterlanguage = false)
    {
        return db.Categories
            .Include(x => x.Access)
            .Include(x=>x.Parent)
            .Include(x => x.TheLanguage)
            .Where(x =>
                (x.TheLanguage == null || (fiterlanguage == false || x.TheLanguage.Code == Language)) &&
                x.Parent == null && x.ID != without)
            .ToList();
    }
    public List<Category> FillWithChildren(List<Category> gs, int without = -1, bool fiterlanguage = false)
    {
        foreach (var g in gs)
        {
            g.SubCategories = 
                db.Categories
                .Include(x=>x.Access)
                .Include(x => x.Parent)
                .Include(x => x.TheLanguage)
                .Where(x =>
                    (x.TheLanguage == null || (fiterlanguage == false || x.TheLanguage.Code == Language)) &&
                    x.Parent != null && x.Parent.ID == g.ID && x.ID != without 
                    //&& x.Access != null && MyViewLevelIDs.Contains(x.Access.ID))
                    )
                .ToList();
            FillWithChildren(g.SubCategories, without);
        }
        return gs;
    }
    #endregion

    #region Contents helper
    public List<Content> GetAllContents()
    {
        if (MyPermission == null || MyViewLevelIDs == null)
            return new List<Content>();

        if (MyPermission.See_Contents == true)
            return db.Contents
                .Include(x => x.Access)
                .Include(x => x.Category)
                .Include(x => x.TheLanguage).ToList();

        return db.Contents
            .Include(x => x.Access)
            .Include(x => x.Category)
            .Include(x => x.TheLanguage)
            .Where(x =>
                (x.TheLanguage == null || x.TheLanguage.Code == Language) &&
                x.Access != null && MyViewLevelIDs.Contains(x.Access.ID)).ToList();
    }

    public List<Content> MarkTheVisited(List<Content> cons)
    {
        if (string.IsNullOrEmpty(Username) == false)
            cons.ForEach(x => x.Visited = db.ContentVisitLogs.Include(z=>z.Content).FirstOrDefault(z => z.Username == Username && z.Content.ID == x.ID) != null);
        return cons;
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
            db.Dispose(); 

        base.Dispose(disposing);
    }
}