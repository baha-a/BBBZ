using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BBBZ.Models
{
    public partial class Content
    {
        public Content()
        {
            CustomFieldValues = new List<CustomFieldValue>();
            Published = true;
        }

        [NotMapped]
        public int? CategoryID { get; set; }

        [NotMapped]
        public int? AccessID { get; set; }
    }

    public partial class Menu
    {
        public Menu()
        {
            Children = new List<Menu>();
            OpenInSameWindow = true;
            Published = true;
        }

        [NotMapped]
        public List<Menu> Children{ get; set; }
    }
    public partial class Group
    {
        [NotMapped]
        public bool Selected { get; set; }
        [NotMapped]
        public int? helperID { get; set; }
    }
    public partial class Category
    {
        public Category()
        {
            SubCategories = new List<Category>();
            Contents = new List<Content>();
            Published = true;
        }

        [NotMapped]
        [Display(Name="Parent")]
        public int? NewParentID_helper { get; set; }

        [NotMapped]
        [Display(Name = "Access")]
        public int? NewAccessID_helper { get; set; }

        [NotMapped]
        public List<Category> SubCategories { get; set; }
    }

    public class SelectableGroup
    {
        public SelectableGroup()
        {
            Group = new Group();
        }

        public int ID { get; set; }
        public string Text { get; set; }
        public bool Selected { get; set; }
        public Group Group { get; set; }

        public static List<SelectableGroup> Convert(List<Group> g, int level = 0)
        {
            List<SelectableGroup> a = new List<SelectableGroup>();
            foreach (var i in g)
            {
                a.Add(new SelectableGroup() { ID = i.ID, Selected = i.Selected, Text = Extenisons.Dashis(level) + i.Title, Group = i });
                a.AddRange(Convert(i.Children, level + 1));
            }
            return a;
        }
    }

    public class UserManagerDataView
    {
        public UserManagerDataView()
        {
            AllGroups = new List<SelectableGroup>();
            AssignedGroups = new List<Group>();
        }
        public RegisterViewModel TheUser { get; set; }
        public bool Locked { get; set; }
        public List<SelectableGroup> AllGroups { get; set; }
        public List<Group> AssignedGroups { get; set; }
    }

    public class ViewLevelViewModel
    {
        public List<SelectableGroup> Groups { get; set; }
        public ViewLevel theViewlevel { get; set; }

        public ViewLevelViewModel()
        {
            Groups = new List<SelectableGroup>();
            theViewlevel = new ViewLevel();
        }

        public ViewLevelViewModel Initialize(List<Group> g, ViewLevel vl = null)
        {
            Groups = SelectableGroup.Convert(g);

            if (vl != null)
            {
                theViewlevel = vl;
                SelectTheViewLevelsGroup();
            }

            return this;
        }

        public ViewLevelViewModel SelectTheViewLevelsGroup()
        {
            Groups.ForEach(x => x.Selected = theViewlevel.Groups.SingleOrDefault(y => y.ID == x.Group.ID) != null);
            return this;
        }
    }

    public class GroupViewModel
    {
        public List<SelectableGroup> Groups { get; set; }
        public SelectableGroup SelectedParent{ get; set; }
        public Group Group { get; set; }

        public GroupViewModel() { }
        public GroupViewModel(List<Group> gs, Group g)
        {
            Groups = SelectableGroup.Convert(gs);
            Group = g;

            if (Group != null)
            {
                var t = Groups.SingleOrDefault(x => x.ID == Group.ID);
                if (t != null)
                    Groups.Remove(t);
            }
        }
    }

    public class MessageViewModel
    {
        public List<Message> Inbox { get; set; }
        public List<Message> Outbox { get; set; }
    }


    public class MenuViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public Menu Menu{ get; set; }
    }
    public class MenuItemViewModel
    {
        public MenuItemViewModel()
        {
            TheMenu = new Menu() { Type="SinglePage"};
        }
        public Menu TheMenu { get; set; }
        public List<MenuViewModel> AllMenus { get; set; }
        
        public List<MenuType> AllMenuTypes { get; set; }
        public MenuType TheMenuType { get; set; }

        public List<Content> AllContents{ get; set; }
        public List<Category> AlllCategories { get; set; }


        [Display(Name = "Parent Menu")]
        public int? selectedMenuTypeID { get; set; }
        [Display(Name = "Parent Menu Item")]
        public int? selectedParentID { get; set; }

        [Display(Name = "Category")]
        public int? selectedCategoryID { get; set; }
        [Display(Name = "Content")]
        public int? selectedContentID { get; set; }
        
        [Display(Name = "Access")]
        public int? selectedAccessID { get; set; }

        public bool AddThisToMenuType { get; set; }
        
        [Required]
        public string ItemType { get; set; }

        public int? id { get; set; }
    }





    public class MediaViewModel
    {
        public string[] Files { get; set; }
        public string[] Folders { get; set; }
        public string CurrnetPath { get; set; }
    }





    
    
    
    public class HomeViewModel
    {
        public List<PublicData> PublicData { get; set; }
        public List<News> News { get; set; }
    }
    public class AdminViewModel
    {
        public List<ApplicationUser> NewUsersToAccept { get; set; }
        public List<ApplicationUser> NewTeachersToAccept { get; set; }
        public List<ApplicationUser> NewAdminsToAccept { get; set; }

        public List<PublicData> PublicData { get; set; }
        public List<News> News { get; set; }


        public List<Menu> Menus { get; set; }

        public List<Category> Categories { get; set; }

        public Menu addMenu { get; set; }
        public News addNews { get; set; }
        public PublicData addPublicData { get; set; }

        public List<string> JsonOBJ { get; set; }
    }

    public class CateogriesJsonItem
    {
        public CateogriesJsonItem()
        {
            children = new List<CateogriesJsonItem>();

            //// [{"id":1, "name":"wewe", "url":"http://google.com", "new":0, "edit":0, "children":[{},{},{},....]}]
        }

        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }

        public bool New { get; set; }
        public bool edit { get; set; }
        public bool delete { get; set; }
        
        public List<CateogriesJsonItem> children{ get; set; }
    }


    public class SideMenuViewModel
    {
        public MenuType MenuType { get; set; }
        public List<Menu> MenuItems { get; set; }
    }


    public class SharedLayoutViewModel
    {
        public SharedLayoutViewModel()
        {
            TopMenuItems = new List<Menu>();
            SideMenus = new List<SideMenuViewModel>();
            IsAdmin = false;
        }

        public List<Menu> TopMenuItems { get; set; }
        public List<SideMenuViewModel> SideMenus { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class CategoryView
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public Category theCategory { get; set; }
    }
}