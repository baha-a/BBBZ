using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBBZ.Models
{
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