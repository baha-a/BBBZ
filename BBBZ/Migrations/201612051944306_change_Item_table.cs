namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_Item_table : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.ItemCategories", newName: "Items");
            //DropForeignKey("dbo.ItemCategories", "Item_ID", "dbo.Items");
            //DropForeignKey("dbo.ItemCategories", "Category_ID", "dbo.Categories");
            //DropIndex("dbo.ItemCategories", new[] { "Item_ID" });
            //DropIndex("dbo.ItemCategories", new[] { "Category_ID" });
            
            DropTable("dbo.ItemCategories");
            AddColumn("dbo.Items", "Title", c => c.String());
            AddColumn("dbo.Items", "ImgURl", c => c.String());
            AddColumn("dbo.Items", "Descrption", c => c.String());
            AddColumn("dbo.Items", "ActivationTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Items", "Creator", c => c.String());
            AddColumn("dbo.Items", "CreationTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Items", "Category_ID", c => c.Int());
            AddColumn("dbo.Items", "Language_ID", c => c.Int());
            CreateIndex("dbo.Items", "Category_ID");
            CreateIndex("dbo.Items", "Language_ID");
            AddForeignKey("dbo.Items", "Category_ID", "dbo.Categories", "ID");
            AddForeignKey("dbo.Items", "Language_ID", "dbo.Languages", "ID");
            DropColumn("dbo.Items", "Key");
            DropColumn("dbo.Items", "Url");
            DropColumn("dbo.Items", "HtmlCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Items", "HtmlCode", c => c.String());
            AddColumn("dbo.Items", "Url", c => c.String());
            AddColumn("dbo.Items", "Key", c => c.String());
            DropForeignKey("dbo.Items", "Language_ID", "dbo.Languages");
            DropForeignKey("dbo.Items", "Category_ID", "dbo.Categories");
            DropIndex("dbo.Items", new[] { "Language_ID" });
            DropIndex("dbo.Items", new[] { "Category_ID" });
            DropColumn("dbo.Items", "Language_ID");
            DropColumn("dbo.Items", "Category_ID");
            DropColumn("dbo.Items", "CreationTime");
            DropColumn("dbo.Items", "Creator");
            DropColumn("dbo.Items", "ActivationTime");
            DropColumn("dbo.Items", "Descrption");
            DropColumn("dbo.Items", "ImgURl");
            DropColumn("dbo.Items", "Title");

            CreateTable(
                "dbo.ItemCategories",
                c => new
                {
                    Item_ID = c.Int(nullable : false),
                    Category_ID = c.Int(nullable : false),
                })
                .PrimaryKey(t => new { t.Item_ID, t.Category_ID })
                .ForeignKey("dbo.Items", t => t.Item_ID, cascadeDelete : true)
                .ForeignKey("dbo.Categories", t => t.Category_ID, cascadeDelete : true)
                .Index(t => t.Item_ID)
                .Index(t => t.Category_ID);

            //CreateIndex("dbo.ItemCategories", "Category_ID");
            //CreateIndex("dbo.ItemCategories", "Item_ID");
            //AddForeignKey("dbo.ItemCategories", "Category_ID", "dbo.Categories", "ID", cascadeDelete: true);
            //AddForeignKey("dbo.ItemCategories", "Item_ID", "dbo.Items", "ID", cascadeDelete: true);
            //RenameTable(name: "dbo.Items", newName: "ItemCategories");
        }
    }
}
