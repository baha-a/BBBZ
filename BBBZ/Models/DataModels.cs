using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BBBZ.Models
{
#region User and ACL
    public partial class Group
    {
        public Group()
        {
            Children = new List<Group>();
            Users = new List<UserGroup>();
            Access = new List<ViewLevel>();
        }

        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        
        public Group Parent { get; set; }
        public List<Group> Children { get; set; }

        public List<UserGroup> Users { get; set; }
        public List<ViewLevel> Access { get; set; }
    }

    public class UserGroup
    {
        public int ID { get; set; }
        public string username { get; set; }
        public Group Groups { get; set; }
    }
     
    public class Profile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string username { get; set; }

        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Image { get; set;}
        public DateTime RegisterDate{ get; set; }
        public DateTime LastVisitDate{ get; set; }
    }

    public class Message
    {
        public int ID { get; set; }

        public DateTime Date { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }

        public string From_username { get; set; }
        public string To_username { get; set; }
    }


    public class ViewLevel
    {
        public ViewLevel()
        {
            Groups = new List<Group>();
        }
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public List<Group> Groups { get; set; }
    }

    public class Asset
    {
        // this model follows the Nested Set Model to store data Hierhically
        // the root id must be 0 and it has no parent, root is the global configuration

        public Asset()
        {
            Children = new List<Asset>();
        }

        public int ID { get; set; }

        public bool? SiteLogin { get; set; }      //  only Global Configuration
        public bool? AdminLogin { get; set; }     //  only Global Configuration
        public bool? SuperUser { get; set; }      //  Global Configuration and Component Manager
        public bool? AdminInterface { get; set; } //  Global Configuration and Component Manager

        public bool? Configure { get; set; }      //  Global Configuration and Component Manager
        public bool? Create { get; set; }         //  Global Configuration, Component Manager and Category level
        public bool? Delete { get; set; }         //  all level above plus Article level
        public bool? Edit { get; set; }           //  all level above plus Article level
        public bool? EditState { get; set; }      //  all level above plus Article level
        public bool? EditOwn { get; set; }        //  Global Configuration, Component Manager and Category level

        public Asset Parent { get; set; }
        public List<Asset> Children { get; set; }
    }

#endregion


    public class Language
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MetaDesc { get; set; }
        public string MetaKey { get; set; }
        public string MetaData { get; set; }
        public string SiteName { get; set; }

        public Asset Asset { get; set; }
        public ViewLevel Access { get; set; }
    }
    
    public class MenuType
	{
        public MenuType ()
	    {
             Menus = new List<Menu>();
	    }

	    public int ID { get; set; }
        public string menuType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsTopMenu { get; set; }
        public bool Published { get; set; }

        public List<Menu> Menus { get; set; }
        public Asset Asset { get; set; }
	}
    public class Menu
    {
        public Menu()
        {
            Menus = new List<Menu>();
            MenuCategories = new List<MenuCategory>();
            OpenInSameWindow = true;
        }

        public int ID { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public string Note { get; set; }
        public string Link { get; set; } /////////////////  ex.:  /index.php?view=artical&id=3  or /index.php?view=cateogry&id=5
        public string Type { get; set; } ///////////////// The type of link: Component, URL, Alias, Separator or Heading.
        public bool Published { get; set; }
        public bool Home { get; set; }

        public string Url { get; set; }
        public bool OpenInSameWindow { get; set; }

        public Language Langauge { get; set; }
        
        public Menu Parent { get; set; }
        public List<Menu> Menus { get; set; }

        public MenuType MenuType { get; set; }

        public List<MenuCategory> MenuCategories { get; set; }
        public Category SingleCategory { get; set; }
        public Content SingleContent { get; set; }
    }

    public class MenuCategory 
    {
        public int ID { get; set; }
        public Menu Menu { get; set; }
        public Category Category { get; set; }
        public int Order { get; set; }
    }

    public partial class Category
    {
        public Category()
        {
            SubCategories = new List<Category>();
            Contents = new List<Content>();
            MenuCategories = new List<MenuCategory>();
            Published = true;
        }

        public int ID { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string MetaDesc { get; set; }
        public string MetaKey { get; set; }
        public string MetaData { get; set; }
        public string Language { get; set; }
        [Display(Name="Creator")]
        public string CreatedByUsername { get; set; }
        public DateTime Date { get; set; }
        public bool Published { get; set; }

        public Category Parent { get; set; }
        public List<Category> SubCategories { get; set; }
        public List<Content> Contents { get; set; }

        public List<MenuCategory> MenuCategories { get; set; }

        public ViewLevel Access { get; set; }
        public Asset Asset { get; set; }
    }

    public class Content
    {
        public Content() 
        {
            CustomFields = new List<CustomField>(); 
        }

        public int ID { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public string IntroText { get; set; }
        [AllowHtml]
        public string FullText { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Descrption { get; set; }
        public string MetaDesc { get; set; }
        public string MetaKey { get; set; }
        public string MetaData { get; set; }
        [Display(Name = "Creator")]
        public string CreatedByUsername { get; set; }
        public bool Published { get; set; }
        public Category Category { get; set; }
        public Language Language { get; set; }

        public List<CustomField> CustomFields { get; set; }

        public ViewLevel Access { get; set; }
        public Asset Asset { get; set; }
    }

    public class CustomField
    {
        public int ID { get; set; }
        public Content Content { get; set; }

        public string Name { get; set; }
        public string Value { get; set; }
        public string DataType { get; set; }
    }







    public class PublicData
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        [RegularExpression("[A-Za-z]{2}")]
        public string Language { get; set; }
    }

    public class News
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
        public string Photo { get; set; }

        public DateTime Date { get; set; }
        [Range(1, 360)]
        public int Duration { get; set; }

        [Required]
        [RegularExpression("[A-Za-z]{2}")]
        public string Language { get; set; }
    }
    

    //public class Enrollment
    //{
    //    public int ID { get; set; }
    //    public DateTime StartDate { get; set; }
    //    public DateTime FinishDate { get; set; }

    //    public int FinalMark { get; set; }

    //    public Student Student { get; set; }
    //    public Class Class { get; set; }

    //    public List<Attendance> Attendances { get; set; }
    //    public List<Message> Messages { get; set; }
    //}

    //public class Class
    //{
    //    public int ID { get; set; }
    //    public string Name { get; set; }
    //    public string StartDate { get; set; }
    //    public string EndDate { get; set; }

    //    public Item Course { get; set; }
    //    public Teacher Teacher { get; set; }
    //    public List<Lecture> Lectures{ get; set; }
    //    public List<Enrollment> Enrollments { get; set; }
    //}

    //public class Attendance
    //{
    //    public int ID { get; set; }
    //    public Enrollment Student { get; set; }
    //    public Lecture Lecture { get; set; }

    //    public int Mark { get; set; }
    //}

    //public class Lecture
    //{
    //    public int ID { get; set; }
    //    public DateTime Date { get; set; }

    //    public string Video { get; set; }
    //    public List<string> Files { get; set; }
    //    public string Note { get; set; }

    //    public bool isFinalExam { get; set; }
    //    public Exam Quiz { get; set; }

    //    public Class Class { get; set; }
    //    public List<Attendance> Attendances { get; set; }
    //}

    //public class Exam
    //{
    //    public int ID { get; set; }
    //    public DateTime StartDate { get; set; }
    //    public List<Question> Questions{ get; set; }
    //}

    //public class Question
    //{
    //    public int ID { get; set; }
    //    public string QuestionText { get; set; }

    //    public string CorrectAnswer { get; set; }

    //    public string option1 { get; set; }
    //    public string option2 { get; set; }
    //    public string option3 { get; set; }
    //    public string option4 { get; set; }

    //    public Item Course { get; set; }
    //}
}
