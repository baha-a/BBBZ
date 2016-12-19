namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delete_Modules_table : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Modules", "Asset_ID", "dbo.Assets");
            DropForeignKey("dbo.Modules", "Langauge_ID", "dbo.Languages");
            DropForeignKey("dbo.Modules", "Menu_ID", "dbo.Menus");
            DropIndex("dbo.Modules", new[] { "Asset_ID" });
            DropIndex("dbo.Modules", new[] { "Langauge_ID" });
            DropIndex("dbo.Modules", new[] { "Menu_ID" });
            AddColumn("dbo.Menus", "Url", c => c.String());
            AddColumn("dbo.Menus", "SingleCategory_ID", c => c.Int());
            AddColumn("dbo.Menus", "SingleContent_ID", c => c.Int());
            AddColumn("dbo.MenuTypes", "IsTopMenu", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Menus", "SingleCategory_ID");
            CreateIndex("dbo.Menus", "SingleContent_ID");
            AddForeignKey("dbo.Menus", "SingleCategory_ID", "dbo.Categories", "ID");
            AddForeignKey("dbo.Menus", "SingleContent_ID", "dbo.Contents", "ID");
            DropColumn("dbo.Menus", "Order");
            DropTable("dbo.Modules");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Position = c.String(),
                        Note = c.String(),
                        Asset_ID = c.Int(),
                        Langauge_ID = c.Int(),
                        Menu_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Menus", "Order", c => c.Int(nullable: false));
            DropForeignKey("dbo.Menus", "SingleContent_ID", "dbo.Contents");
            DropForeignKey("dbo.Menus", "SingleCategory_ID", "dbo.Categories");
            DropIndex("dbo.Menus", new[] { "SingleContent_ID" });
            DropIndex("dbo.Menus", new[] { "SingleCategory_ID" });
            DropColumn("dbo.MenuTypes", "IsTopMenu");
            DropColumn("dbo.Menus", "SingleContent_ID");
            DropColumn("dbo.Menus", "SingleCategory_ID");
            DropColumn("dbo.Menus", "Url");
            CreateIndex("dbo.Modules", "Menu_ID");
            CreateIndex("dbo.Modules", "Langauge_ID");
            CreateIndex("dbo.Modules", "Asset_ID");
            AddForeignKey("dbo.Modules", "Menu_ID", "dbo.Menus", "ID");
            AddForeignKey("dbo.Modules", "Langauge_ID", "dbo.Languages", "ID");
            AddForeignKey("dbo.Modules", "Asset_ID", "dbo.Assets", "ID");
        }
    }
}
