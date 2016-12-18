using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BBBZ.Models
{
    public partial class Group
    {
        [NotMapped]
        public bool Selected { get; set; }
        [NotMapped]
        public int? helperID { get; set; }
    }
    public partial class Category
    {
        [NotMapped]
        [Display(Name="Parent")]
        public int? NewParentID_helper { get; set; }

        [NotMapped]
        [Display(Name = "Access")]
        public int? NewAccessID_helper { get; set; }
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
}