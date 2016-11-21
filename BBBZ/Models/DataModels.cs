using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BBBZ.Models
{
    public class Language
    {
        public Language()
        {
            Resources = new List<Resource>();
        }

        public int ID { get; set; }
        public string ISOName { get; set; }
        public string FullName { get; set; }

        public List<Resource> Resources { get; set; }
    }

    public class Resource
    {
        public int ID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public Language Language { get; set; }
    }




    public class MenuForRole
    {
        public int ID { get; set; }
        public string Role { get; set; }
        public Menu Menu { get; set; }
    }

    public class Menu
    {
        public Menu()
        {
            MenuForRole = new List<MenuForRole>();
            MenuCategories = new List<MenuCategory>();
        }

        public int ID { get; set; }
        public string Name { get; set; }


        public List<MenuForRole> MenuForRole { get; set; }
        public List<MenuCategory> MenuCategories { get; set; }
    }

    public class MenuCategory 
    {
        public int ID { get; set; }
        public Menu Menu { get; set; }
        public Category Category { get; set; }
        public int Order { get; set; }
    }

    public class Category
    {
        public Category()
        {
            SubCategories = new List<Category>();
            Items = new List<Item>();
            MenuCategories = new List<MenuCategory>();
        }

        public int ID { get; set; }
        public string Key { get; set; }
        public string Url { get; set; }

        public Category Parent { get; set; }
        public List<MenuCategory> MenuCategories { get; set; }
        public List<Category> SubCategories { get; set; }
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        public Item()
        {
            Categories = new List<Category>();
        }

        public int ID { get; set; }
        public string Key { get; set; }
        public string Url { get; set; }

        public List<Category> Categories { get; set; }

        public string HtmlCode { get; set; }

        //public List<Class> Classes { get; set; }
        //public List<Question> QuestionsBank { get; set; }
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







    
    //public class Teacher
    //{
    //    public int ID { get; set; }
    //    public string Name { get; set; }

    //    public List<Class> Classes { get; set; }

    //    public string UserName { get; set; }
    //}

    //public class Student
    //{
    //    public int ID { get; set; }
    //    public string Name { get; set; }

    //    public List<Enrollment> Enrollments { get; set; }

    //    public string UserName { get; set; }
    //}


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

    //public class Message
    //{
    //    public int ID { get; set; }
    //    public int Text { get; set; }
    //    public DateTime Date { get; set; }

    //    public bool Readed { get;set;}

    //    public string File { get; set; }

    //    public Enrollment Enroll { get; set; }
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
