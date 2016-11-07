using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BBBZ.Models
{
    public class Teacher
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<Class> Classes { get; set; }

        public string UserName { get; set; }
    }

    public class Student
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<Enrollment> Enrollments { get; set; }

        public string UserName { get; set; }
    }


    public class Semester
    {
        public int Id { get; set; }
        public string Name{ get; set; }
        public int Year { get; set; }

        public List<Course> Courses { get; set; }
    }

    public class Course
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public Semester Semester { get; set; }
        public List<Class> Classes { get; set; }

        public List<Question> QuestionsBank { get; set; }
    }
    
    public class Enrollment
    {
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }

        public int FinalMark { get; set; }

        public Student Student { get; set; }
        public Class Class { get; set; }

        public List<Attendance> Attendances { get; set; }
        public List<Message> Messages { get; set; }
    }

    public class Message
    {
        public int ID { get; set; }
        public int Text { get; set; }
        public DateTime Date { get; set; }

        public bool Readed { get;set;}

        public string File { get; set; }

        public Enrollment Enroll { get; set; }
    }

    public class Class
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public Course Course { get; set; }
        public Teacher Teacher { get; set; }
        public List<Lecture> Lectures{ get; set; }
        public List<Enrollment> Enrollments { get; set; }
    }

    public class Attendance
    {
        public int ID { get; set; }
        public Enrollment Student { get; set; }
        public Lecture Lecture { get; set; }

        public int Mark { get; set; }
    }

    public class Lecture
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }

        public string Video { get; set; }
        public List<string> Files { get; set; }
        public string Note { get; set; }

        public bool isFinalExam { get; set; }
        public Exam Quiz { get; set; }

        public Class Class { get; set; }
        public List<Attendance> Attendances { get; set; }
    }

    public class Exam
    {
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public List<Question> Questions{ get; set; }
    }

    public class Question
    {
        public int ID { get; set; }
        public string QuestionText { get; set; }

        public string CorrectAnswer { get; set; }

        public string option1 { get; set; }
        public string option2 { get; set; }
        public string option3 { get; set; }
        public string option4 { get; set; }

        public Course Course { get; set; }
    }

    
    public class PublicData
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Language { get; set; }
    }

    public class News 
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Photo { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        public int Duration { get; set; }
    }
}
