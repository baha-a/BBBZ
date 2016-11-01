namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitalDB : DbMigration
    {
        public override void Up()
        {

            CreateTable(
                "dbo.Attendances",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Mark = c.Int(nullable: false),
                        Lecture_ID = c.Int(),
                        Student_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Lectures", t => t.Lecture_ID)
                .ForeignKey("dbo.Enrollments", t => t.Student_ID)
                .Index(t => t.Lecture_ID)
                .Index(t => t.Student_ID);
            
            CreateTable(
                "dbo.Lectures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Video = c.String(),
                        Note = c.String(),
                        isFinalExam = c.Boolean(nullable: false),
                        Class_ID = c.Int(),
                        Quiz_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Classes", t => t.Class_ID)
                .ForeignKey("dbo.Exams", t => t.Quiz_ID)
                .Index(t => t.Class_ID)
                .Index(t => t.Quiz_ID);
            
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StartDate = c.String(),
                        EndDate = c.String(),
                        Course_ID = c.Int(),
                        Teacher_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.Course_ID)
                .ForeignKey("dbo.Teachers", t => t.Teacher_ID)
                .Index(t => t.Course_ID)
                .Index(t => t.Teacher_ID);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Semester_Id = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Semesters", t => t.Semester_Id)
                .Index(t => t.Semester_Id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        QuestionText = c.String(),
                        CorrectAnswer = c.String(),
                        option1 = c.String(),
                        option2 = c.String(),
                        option3 = c.String(),
                        option4 = c.String(),
                        Course_ID = c.Int(),
                        Exam_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.Course_ID)
                .ForeignKey("dbo.Exams", t => t.Exam_ID)
                .Index(t => t.Course_ID)
                .Index(t => t.Exam_ID);
            
            CreateTable(
                "dbo.Semesters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Enrollments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        FinishDate = c.DateTime(nullable: false),
                        FinalMark = c.Int(nullable: false),
                        Class_ID = c.Int(),
                        Student_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Classes", t => t.Class_ID)
                .ForeignKey("dbo.Students", t => t.Student_ID)
                .Index(t => t.Class_ID)
                .Index(t => t.Student_ID);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Text = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Readed = c.Boolean(nullable: false),
                        File = c.String(),
                        Enroll_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Enrollments", t => t.Enroll_ID)
                .Index(t => t.Enroll_ID);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Exams",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Lectures", "Quiz_ID", "dbo.Exams");
            DropForeignKey("dbo.Questions", "Exam_ID", "dbo.Exams");
            DropForeignKey("dbo.Classes", "Teacher_ID", "dbo.Teachers");
            DropForeignKey("dbo.Lectures", "Class_ID", "dbo.Classes");
            DropForeignKey("dbo.Enrollments", "Student_ID", "dbo.Students");
            DropForeignKey("dbo.Messages", "Enroll_ID", "dbo.Enrollments");
            DropForeignKey("dbo.Enrollments", "Class_ID", "dbo.Classes");
            DropForeignKey("dbo.Attendances", "Student_ID", "dbo.Enrollments");
            DropForeignKey("dbo.Courses", "Semester_Id", "dbo.Semesters");
            DropForeignKey("dbo.Questions", "Course_ID", "dbo.Courses");
            DropForeignKey("dbo.Classes", "Course_ID", "dbo.Courses");
            DropForeignKey("dbo.Attendances", "Lecture_ID", "dbo.Lectures");
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Lectures", new[] { "Quiz_ID" });
            DropIndex("dbo.Questions", new[] { "Exam_ID" });
            DropIndex("dbo.Classes", new[] { "Teacher_ID" });
            DropIndex("dbo.Lectures", new[] { "Class_ID" });
            DropIndex("dbo.Enrollments", new[] { "Student_ID" });
            DropIndex("dbo.Messages", new[] { "Enroll_ID" });
            DropIndex("dbo.Enrollments", new[] { "Class_ID" });
            DropIndex("dbo.Attendances", new[] { "Student_ID" });
            DropIndex("dbo.Courses", new[] { "Semester_Id" });
            DropIndex("dbo.Questions", new[] { "Course_ID" });
            DropIndex("dbo.Classes", new[] { "Course_ID" });
            DropIndex("dbo.Attendances", new[] { "Lecture_ID" });
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Exams");
            DropTable("dbo.Teachers");
            DropTable("dbo.Students");
            DropTable("dbo.Messages");
            DropTable("dbo.Enrollments");
            DropTable("dbo.Semesters");
            DropTable("dbo.Questions");
            DropTable("dbo.Courses");
            DropTable("dbo.Classes");
            DropTable("dbo.Lectures");
            DropTable("dbo.Attendances");
        }
    }
}
