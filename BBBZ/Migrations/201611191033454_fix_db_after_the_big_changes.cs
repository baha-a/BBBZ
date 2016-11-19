namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_db_after_the_big_changes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Categories", "Menu_ID", "dbo.Menus");
            DropForeignKey("dbo.Menus", "Language_ID", "dbo.Languages");
            DropForeignKey("dbo.Categories", "Language_ID", "dbo.Languages");
            DropForeignKey("dbo.Categories", "Category_ID", "dbo.Categories");
            DropIndex("dbo.Categories", new[] { "Menu_ID" });
            DropIndex("dbo.Menus", new[] { "Language_ID" });
            DropIndex("dbo.Categories", new[] { "Language_ID" });
            DropIndex("dbo.Categories", new[] { "Category_ID" });
            CreateTable(
                "dbo.MenuCategories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Order = c.Int(nullable: false),
                        Category_ID = c.Int(),
                        Menu_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.Category_ID)
                .ForeignKey("dbo.Menus", t => t.Menu_ID)
                .Index(t => t.Category_ID)
                .Index(t => t.Menu_ID);
            
            CreateTable(
                "dbo.MenuForRoles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Role = c.String(),
                        Menu_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Menus", t => t.Menu_ID)
                .Index(t => t.Menu_ID);
            
            CreateTable(
                "dbo.Resources",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.String(),
                        Language_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Languages", t => t.Language_ID)
                .Index(t => t.Language_ID);
            
            AddColumn("dbo.Categories", "Key", c => c.String());
            AddColumn("dbo.Categories", "Parent_ID", c => c.Int());
            AddColumn("dbo.Items", "Key", c => c.String());
            AddColumn("dbo.Items", "HtmlCode", c => c.String());
            AddColumn("dbo.Languages", "ISOName", c => c.String());
            CreateIndex("dbo.Categories", "Parent_ID");
            AddForeignKey("dbo.Categories", "Parent_ID", "dbo.Categories", "ID");
            DropColumn("dbo.Categories", "Name");
            DropColumn("dbo.Categories", "Menu_ID");
            DropColumn("dbo.Categories", "Language_ID");
            DropColumn("dbo.Categories", "Category_ID");
            DropColumn("dbo.Items", "Name");
            DropColumn("dbo.Languages", "Name");
            DropColumn("dbo.Menus", "Language_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Menus", "Language_ID", c => c.Int());
            AddColumn("dbo.Languages", "Name", c => c.String());
            AddColumn("dbo.Items", "Name", c => c.String());
            AddColumn("dbo.Categories", "Category_ID", c => c.Int());
            AddColumn("dbo.Categories", "Language_ID", c => c.Int());
            AddColumn("dbo.Categories", "Menu_ID", c => c.Int());
            AddColumn("dbo.Categories", "Name", c => c.String());
            DropForeignKey("dbo.Categories", "Parent_ID", "dbo.Categories");
            DropForeignKey("dbo.Resources", "Language_ID", "dbo.Languages");
            DropForeignKey("dbo.MenuForRoles", "Menu_ID", "dbo.Menus");
            DropForeignKey("dbo.MenuCategories", "Menu_ID", "dbo.Menus");
            DropForeignKey("dbo.MenuCategories", "Category_ID", "dbo.Categories");
            DropIndex("dbo.Categories", new[] { "Parent_ID" });
            DropIndex("dbo.Resources", new[] { "Language_ID" });
            DropIndex("dbo.MenuForRoles", new[] { "Menu_ID" });
            DropIndex("dbo.MenuCategories", new[] { "Menu_ID" });
            DropIndex("dbo.MenuCategories", new[] { "Category_ID" });
            DropColumn("dbo.Languages", "ISOName");
            DropColumn("dbo.Items", "HtmlCode");
            DropColumn("dbo.Items", "Key");
            DropColumn("dbo.Categories", "Parent_ID");
            DropColumn("dbo.Categories", "Key");
            DropTable("dbo.Resources");
            DropTable("dbo.MenuForRoles");
            DropTable("dbo.MenuCategories");
            CreateIndex("dbo.Categories", "Category_ID");
            CreateIndex("dbo.Categories", "Language_ID");
            CreateIndex("dbo.Menus", "Language_ID");
            CreateIndex("dbo.Categories", "Menu_ID");
            AddForeignKey("dbo.Categories", "Category_ID", "dbo.Categories", "ID");
            AddForeignKey("dbo.Categories", "Language_ID", "dbo.Languages", "ID");
            AddForeignKey("dbo.Menus", "Language_ID", "dbo.Languages", "ID");
            AddForeignKey("dbo.Categories", "Menu_ID", "dbo.Menus", "ID");
        }
    }
}
