namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_relation_between_tables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CustomFields", "Content_ID", "dbo.Contents");
            DropForeignKey("dbo.Contents", "Language_ID", "dbo.Languages");
            DropForeignKey("dbo.MenuCategories", "Category_ID", "dbo.Categories");
            DropForeignKey("dbo.Menus", "Langauge_ID", "dbo.Languages");
            DropForeignKey("dbo.MenuCategories", "Menu_ID", "dbo.Menus");
            DropForeignKey("dbo.Menus", "SingleCategory_ID", "dbo.Categories");
            DropForeignKey("dbo.Menus", "SingleContent_ID", "dbo.Contents");
            DropIndex("dbo.CustomFields", new[] { "Content_ID" });
            DropIndex("dbo.Contents", new[] { "Language_ID" });
            DropIndex("dbo.MenuCategories", new[] { "Category_ID" });
            DropIndex("dbo.Menus", new[] { "Langauge_ID" });
            DropIndex("dbo.MenuCategories", new[] { "Menu_ID" });
            DropIndex("dbo.Menus", new[] { "SingleCategory_ID" });
            DropIndex("dbo.Menus", new[] { "SingleContent_ID" });
            CreateTable(
                "dbo.CustomFieldValues",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        Content_ID = c.Int(),
                        CustomField_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Contents", t => t.Content_ID)
                .ForeignKey("dbo.CustomFields", t => t.CustomField_ID)
                .Index(t => t.Content_ID)
                .Index(t => t.CustomField_ID);
            
            AddColumn("dbo.Contents", "Language", c => c.String());
            AddColumn("dbo.CustomFields", "Title", c => c.String());
            AddColumn("dbo.Menus", "Langauge", c => c.String());
            AddColumn("dbo.Menus", "CategoryID", c => c.Int(nullable: false));
            AddColumn("dbo.Menus", "ContentID", c => c.Int(nullable: false));
            DropColumn("dbo.Contents", "Language_ID");
            DropColumn("dbo.CustomFields", "Name");
            DropColumn("dbo.CustomFields", "Value");
            DropColumn("dbo.CustomFields", "Content_ID");
            DropColumn("dbo.Menus", "Link");
            DropColumn("dbo.Menus", "Home");
            DropColumn("dbo.Menus", "Langauge_ID");
            DropColumn("dbo.Menus", "SingleCategory_ID");
            DropColumn("dbo.Menus", "SingleContent_ID");
            DropTable("dbo.MenuCategories");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MenuCategories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Order = c.Int(nullable: false),
                        Category_ID = c.Int(),
                        Menu_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Menus", "SingleContent_ID", c => c.Int());
            AddColumn("dbo.Menus", "SingleCategory_ID", c => c.Int());
            AddColumn("dbo.Menus", "Langauge_ID", c => c.Int());
            AddColumn("dbo.Menus", "Home", c => c.Boolean(nullable: false));
            AddColumn("dbo.Menus", "Link", c => c.String());
            AddColumn("dbo.CustomFields", "Content_ID", c => c.Int());
            AddColumn("dbo.CustomFields", "Value", c => c.String());
            AddColumn("dbo.CustomFields", "Name", c => c.String());
            AddColumn("dbo.Contents", "Language_ID", c => c.Int());
            DropForeignKey("dbo.CustomFieldValues", "CustomField_ID", "dbo.CustomFields");
            DropForeignKey("dbo.CustomFieldValues", "Content_ID", "dbo.Contents");
            DropIndex("dbo.CustomFieldValues", new[] { "CustomField_ID" });
            DropIndex("dbo.CustomFieldValues", new[] { "Content_ID" });
            DropColumn("dbo.Menus", "ContentID");
            DropColumn("dbo.Menus", "CategoryID");
            DropColumn("dbo.Menus", "Langauge");
            DropColumn("dbo.CustomFields", "Title");
            DropColumn("dbo.Contents", "Language");
            DropTable("dbo.CustomFieldValues");
            CreateIndex("dbo.Menus", "SingleContent_ID");
            CreateIndex("dbo.Menus", "SingleCategory_ID");
            CreateIndex("dbo.MenuCategories", "Menu_ID");
            CreateIndex("dbo.Menus", "Langauge_ID");
            CreateIndex("dbo.MenuCategories", "Category_ID");
            CreateIndex("dbo.Contents", "Language_ID");
            CreateIndex("dbo.CustomFields", "Content_ID");
            AddForeignKey("dbo.Menus", "SingleContent_ID", "dbo.Contents", "ID");
            AddForeignKey("dbo.Menus", "SingleCategory_ID", "dbo.Categories", "ID");
            AddForeignKey("dbo.MenuCategories", "Menu_ID", "dbo.Menus", "ID");
            AddForeignKey("dbo.Menus", "Langauge_ID", "dbo.Languages", "ID");
            AddForeignKey("dbo.MenuCategories", "Category_ID", "dbo.Categories", "ID");
            AddForeignKey("dbo.Contents", "Language_ID", "dbo.Languages", "ID");
            AddForeignKey("dbo.CustomFields", "Content_ID", "dbo.Contents", "ID");
        }
    }
}
