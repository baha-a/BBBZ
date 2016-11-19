namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class majors_changes_for_db_stracture : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Attendances", "Lecture_ID", "dbo.Lectures");
            DropForeignKey("dbo.Courses", "Category_ID", "dbo.Categories");
            DropForeignKey("dbo.Classes", "Course_ID", "dbo.Courses");
            DropForeignKey("dbo.Questions", "Course_ID", "dbo.Courses");
            DropForeignKey("dbo.Attendances", "Student_ID", "dbo.Enrollments");
            DropForeignKey("dbo.Enrollments", "Class_ID", "dbo.Classes");
            DropForeignKey("dbo.Messages", "Enroll_ID", "dbo.Enrollments");
            DropForeignKey("dbo.Enrollments", "Student_ID", "dbo.Students");
            DropForeignKey("dbo.Lectures", "Class_ID", "dbo.Classes");
            DropForeignKey("dbo.Classes", "Teacher_ID", "dbo.Teachers");
            DropForeignKey("dbo.Questions", "Exam_ID", "dbo.Exams");
            DropForeignKey("dbo.Lectures", "Quiz_ID", "dbo.Exams");
            DropIndex("dbo.Attendances", new[] { "Lecture_ID" });
            DropIndex("dbo.Courses", new[] { "Category_ID" });
            DropIndex("dbo.Classes", new[] { "Course_ID" });
            DropIndex("dbo.Questions", new[] { "Course_ID" });
            DropIndex("dbo.Attendances", new[] { "Student_ID" });
            DropIndex("dbo.Enrollments", new[] { "Class_ID" });
            DropIndex("dbo.Messages", new[] { "Enroll_ID" });
            DropIndex("dbo.Enrollments", new[] { "Student_ID" });
            DropIndex("dbo.Lectures", new[] { "Class_ID" });
            DropIndex("dbo.Classes", new[] { "Teacher_ID" });
            DropIndex("dbo.Questions", new[] { "Exam_ID" });
            DropIndex("dbo.Lectures", new[] { "Quiz_ID" });
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Language_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Languages", t => t.Language_ID)
                .Index(t => t.Language_ID);
            
            CreateTable(
                "dbo.ItemCategories",
                c => new
                    {
                        Item_ID = c.Int(nullable: false),
                        Category_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Item_ID, t.Category_ID })
                .ForeignKey("dbo.Items", t => t.Item_ID, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.Category_ID, cascadeDelete: true)
                .Index(t => t.Item_ID)
                .Index(t => t.Category_ID);
            
            AddColumn("dbo.Categories", "Url", c => c.String());
            AddColumn("dbo.Categories", "Menu_ID", c => c.Int());
            AddColumn("dbo.News", "Language", c => c.String(nullable: false));
            CreateIndex("dbo.Categories", "Menu_ID");
            AddForeignKey("dbo.Categories", "Menu_ID", "dbo.Menus", "ID");
            DropTable("dbo.Attendances");
            DropTable("dbo.Lectures");
            DropTable("dbo.Classes");
            DropTable("dbo.Courses");
            DropTable("dbo.Questions");
            DropTable("dbo.Enrollments");
            DropTable("dbo.Messages");
            DropTable("dbo.Students");
            DropTable("dbo.Teachers");
            DropTable("dbo.Exams");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Exams",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
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
                .PrimaryKey(t => t.ID);
            
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
                .PrimaryKey(t => t.ID);
            
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
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Category_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
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
                .PrimaryKey(t => t.ID);
            
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
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Attendances",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Mark = c.Int(nullable: false),
                        Lecture_ID = c.Int(),
                        Student_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.Menus", "Language_ID", "dbo.Languages");
            DropForeignKey("dbo.Categories", "Menu_ID", "dbo.Menus");
            DropForeignKey("dbo.ItemCategories", "Category_ID", "dbo.Categories");
            DropForeignKey("dbo.ItemCategories", "Item_ID", "dbo.Items");
            DropIndex("dbo.Menus", new[] { "Language_ID" });
            DropIndex("dbo.Categories", new[] { "Menu_ID" });
            DropIndex("dbo.ItemCategories", new[] { "Category_ID" });
            DropIndex("dbo.ItemCategories", new[] { "Item_ID" });
            DropColumn("dbo.News", "Language");
            DropColumn("dbo.Categories", "Menu_ID");
            DropColumn("dbo.Categories", "Url");
            DropTable("dbo.ItemCategories");
            DropTable("dbo.Menus");
            DropTable("dbo.Items");
            CreateIndex("dbo.Lectures", "Quiz_ID");
            CreateIndex("dbo.Questions", "Exam_ID");
            CreateIndex("dbo.Classes", "Teacher_ID");
            CreateIndex("dbo.Lectures", "Class_ID");
            CreateIndex("dbo.Enrollments", "Student_ID");
            CreateIndex("dbo.Messages", "Enroll_ID");
            CreateIndex("dbo.Enrollments", "Class_ID");
            CreateIndex("dbo.Attendances", "Student_ID");
            CreateIndex("dbo.Questions", "Course_ID");
            CreateIndex("dbo.Classes", "Course_ID");
            CreateIndex("dbo.Courses", "Category_ID");
            CreateIndex("dbo.Attendances", "Lecture_ID");
            AddForeignKey("dbo.Lectures", "Quiz_ID", "dbo.Exams", "ID");
            AddForeignKey("dbo.Questions", "Exam_ID", "dbo.Exams", "ID");
            AddForeignKey("dbo.Classes", "Teacher_ID", "dbo.Teachers", "ID");
            AddForeignKey("dbo.Lectures", "Class_ID", "dbo.Classes", "ID");
            AddForeignKey("dbo.Enrollments", "Student_ID", "dbo.Students", "ID");
            AddForeignKey("dbo.Messages", "Enroll_ID", "dbo.Enrollments", "ID");
            AddForeignKey("dbo.Enrollments", "Class_ID", "dbo.Classes", "ID");
            AddForeignKey("dbo.Attendances", "Student_ID", "dbo.Enrollments", "ID");
            AddForeignKey("dbo.Questions", "Course_ID", "dbo.Courses", "ID");
            AddForeignKey("dbo.Classes", "Course_ID", "dbo.Courses", "ID");
            AddForeignKey("dbo.Courses", "Category_ID", "dbo.Categories", "ID");
            AddForeignKey("dbo.Attendances", "Lecture_ID", "dbo.Lectures", "ID");
        }
    }
}
