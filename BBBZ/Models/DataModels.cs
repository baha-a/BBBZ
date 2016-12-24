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
        [Required]
        [RegularExpression("[A-Za-z]{2}")]
        public string Code { get; set; }
        [Required]
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
             Published = true;
	    }

	    public int ID { get; set; }
        [Required]
        [RegularExpression("[A-Za-z]*")]
        public string menuType { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsTopMenu { get; set; }
        public bool Published { get; set; }

        public List<Menu> Menus { get; set; }
        public Asset Asset { get; set; }
	}

    public partial class Menu
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        public string Alias { get; set; }
        public string Note { get; set; }
        public bool Published { get; set; }
        public bool OpenInSameWindow { get; set; }

        public string Langauge { get; set; }

        public Menu Parent { get; set; }

        public MenuType MenuType { get; set; }

        [Required]
        public string Type { get; set; }
        public string Url { get; set; }
        public int? CategoryID { get; set; }
        public int? ContentID { get; set; }

        public ViewLevel Access { get; set; }
        public Asset Asset { get; set; }
    }

    public partial class Category
    {
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
        public List<Content> Contents { get; set; }

        public ViewLevel Access { get; set; }
        public Asset Asset { get; set; }
    }

    public partial class Content
    {
        public int ID { get; set; }
        [Required]
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
        
        public string Language { get; set; }

        public List<CustomFieldValue> CustomFieldValues { get; set; }

        public ViewLevel Access { get; set; }
        public Asset Asset { get; set; }
    }

    public class CustomFieldValue
    {
        public int ID { get; set; }
        public Content Content{ get; set; }
        public CustomField CustomField { get; set; }

        public string Value { get; set; }
    }

    public class CustomField
    {
        public CustomField()
        {
            Values = new List<CustomFieldValue>();
        }

        public int ID { get; set; }
        [Required]
        public string Title { get; set; }

        public List<CustomFieldValue> Values { get; set; }

    }




    public class PublicData
    {
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
