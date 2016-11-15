namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_Category : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Courses", "Language_ID", "dbo.Languages");
            DropForeignKey("dbo.Courses", "Semester_Id", "dbo.Semesters");
            DropIndex("dbo.Courses", new[] { "Language_ID" });
            DropIndex("dbo.Courses", new[] { "Semester_Id" });
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Language_ID = c.Int(),
                        Category_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Languages", t => t.Language_ID)
                .ForeignKey("dbo.Categories", t => t.Category_ID)
                .Index(t => t.Language_ID)
                .Index(t => t.Category_ID);
            
            AddColumn("dbo.Courses", "Category_ID", c => c.Int());
            CreateIndex("dbo.Courses", "Category_ID");
            AddForeignKey("dbo.Courses", "Category_ID", "dbo.Categories", "ID");
            DropColumn("dbo.Courses", "Language_ID");
            DropColumn("dbo.Courses", "Semester_Id");
            DropTable("dbo.Semesters");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Semesters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Courses", "Semester_Id", c => c.Int());
            AddColumn("dbo.Courses", "Language_ID", c => c.Int());
            DropForeignKey("dbo.Categories", "Category_ID", "dbo.Categories");
            DropForeignKey("dbo.Categories", "Language_ID", "dbo.Languages");
            DropForeignKey("dbo.Courses", "Category_ID", "dbo.Categories");
            DropIndex("dbo.Categories", new[] { "Category_ID" });
            DropIndex("dbo.Categories", new[] { "Language_ID" });
            DropIndex("dbo.Courses", new[] { "Category_ID" });
            DropColumn("dbo.Courses", "Category_ID");
            DropTable("dbo.Categories");
            CreateIndex("dbo.Courses", "Semester_Id");
            CreateIndex("dbo.Courses", "Language_ID");
            AddForeignKey("dbo.Courses", "Semester_Id", "dbo.Semesters", "Id");
            AddForeignKey("dbo.Courses", "Language_ID", "dbo.Languages", "ID");
        }
    }
}
