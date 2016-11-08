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

        public List<Student> Students { get; set; }
        public List<Teacher> Teachers { get; set; }

        public List<PublicData> PublicData { get; set; }
        public List<News> News { get; set; }


        public News addNews { get; set; }
        public PublicData addPublicData { get; set; }
    }

}