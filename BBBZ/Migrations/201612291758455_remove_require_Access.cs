namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_require_Access : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Categories", "Access_ID", "dbo.ViewLevels");
            DropForeignKey("dbo.Contents", "Access_ID", "dbo.ViewLevels");
            DropForeignKey("dbo.Menus", "Access_ID", "dbo.ViewLevels");
            DropIndex("dbo.Categories", new[] { "Access_ID" });
            DropIndex("dbo.Contents", new[] { "Access_ID" });
            DropIndex("dbo.Menus", new[] { "Access_ID" });
            AlterColumn("dbo.Categories", "Access_ID", c => c.Int());
            AlterColumn("dbo.Contents", "Access_ID", c => c.Int());
            AlterColumn("dbo.Menus", "Access_ID", c => c.Int());
            CreateIndex("dbo.Categories", "Access_ID");
            CreateIndex("dbo.Contents", "Access_ID");
            CreateIndex("dbo.Menus", "Access_ID");
            AddForeignKey("dbo.Categories", "Access_ID", "dbo.ViewLevels", "ID");
            AddForeignKey("dbo.Contents", "Access_ID", "dbo.ViewLevels", "ID");
            AddForeignKey("dbo.Menus", "Access_ID", "dbo.ViewLevels", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Menus", "Access_ID", "dbo.ViewLevels");
            DropForeignKey("dbo.Contents", "Access_ID", "dbo.ViewLevels");
            DropForeignKey("dbo.Categories", "Access_ID", "dbo.ViewLevels");
            DropIndex("dbo.Menus", new[] { "Access_ID" });
            DropIndex("dbo.Contents", new[] { "Access_ID" });
            DropIndex("dbo.Categories", new[] { "Access_ID" });
            AlterColumn("dbo.Menus", "Access_ID", c => c.Int(nullable: false));
            AlterColumn("dbo.Contents", "Access_ID", c => c.Int(nullable: false));
            AlterColumn("dbo.Categories", "Access_ID", c => c.Int(nullable: false));
            CreateIndex("dbo.Menus", "Access_ID");
            CreateIndex("dbo.Contents", "Access_ID");
            CreateIndex("dbo.Categories", "Access_ID");
            AddForeignKey("dbo.Menus", "Access_ID", "dbo.ViewLevels", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Contents", "Access_ID", "dbo.ViewLevels", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Categories", "Access_ID", "dbo.ViewLevels", "ID", cascadeDelete: true);
        }
    }
}
