namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_relation_between_Content_and_Category : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ContentCategories", "Content_ID", "dbo.Contents");
            DropForeignKey("dbo.ContentCategories", "Category_ID", "dbo.Categories");
            DropIndex("dbo.ContentCategories", new[] { "Content_ID" });
            DropIndex("dbo.ContentCategories", new[] { "Category_ID" });
            DropTable("dbo.ContentCategories");
            AddColumn("dbo.Contents", "Category_ID", c => c.Int());
            CreateIndex("dbo.Contents", "Category_ID");
            AddForeignKey("dbo.Contents", "Category_ID", "dbo.Categories", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contents", "Category_ID", "dbo.Categories");
            DropIndex("dbo.Contents", new[] { "Category_ID" });
            DropColumn("dbo.Contents", "Category_ID");
            CreateTable("dbo.ContentCategories",
                    c => new
                    {
                        Content_ID = c.Int(nullable : false),
                        Category_ID = c.Int(nullable : false),
                    });
            CreateIndex("dbo.ContentCategories", "Category_ID");
            CreateIndex("dbo.ContentCategories", "Content_ID");
            AddForeignKey("dbo.ContentCategories", "Category_ID", "dbo.Categories", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ContentCategories", "Content_ID", "dbo.Contents", "ID", cascadeDelete: true);
        }
    }
}
