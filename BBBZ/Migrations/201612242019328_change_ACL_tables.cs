namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_ACL_tables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Assets", "Parent_ID", "dbo.Assets");
            DropForeignKey("dbo.Categories", "Asset_ID", "dbo.Assets");
            DropForeignKey("dbo.Contents", "Asset_ID", "dbo.Assets");
            DropForeignKey("dbo.Languages", "Access_ID", "dbo.ViewLevels");
            DropForeignKey("dbo.Languages", "Asset_ID", "dbo.Assets");
            DropForeignKey("dbo.Menus", "Asset_ID", "dbo.Assets");
            DropForeignKey("dbo.MenuTypes", "Asset_ID", "dbo.Assets");
            DropIndex("dbo.Assets", new[] { "Parent_ID" });
            DropIndex("dbo.Categories", new[] { "Asset_ID" });
            DropIndex("dbo.Contents", new[] { "Asset_ID" });
            DropIndex("dbo.Languages", new[] { "Access_ID" });
            DropIndex("dbo.Languages", new[] { "Asset_ID" });
            DropIndex("dbo.Menus", new[] { "Asset_ID" });
            DropIndex("dbo.MenuTypes", new[] { "Asset_ID" });
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Users = c.Byte(),
                        Groups = c.Byte(),
                        ViewLevels = c.Byte(),
                        Menus = c.Byte(),
                        Categorys = c.Byte(),
                        Contents = c.Byte(),
                        Languages = c.Byte(),
                        Newss = c.Byte(),
                        Questions = c.Byte(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Groups", t => t.ID)
                .Index(t => t.ID);
            
            DropColumn("dbo.Categories", "Asset_ID");
            DropColumn("dbo.Contents", "Asset_ID");
            DropColumn("dbo.Languages", "Access_ID");
            DropColumn("dbo.Languages", "Asset_ID");
            DropColumn("dbo.Menus", "Asset_ID");
            DropColumn("dbo.MenuTypes", "menuType");
            DropColumn("dbo.MenuTypes", "Asset_ID");
            DropTable("dbo.Assets");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Assets",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SiteLogin = c.Boolean(),
                        AdminLogin = c.Boolean(),
                        SuperUser = c.Boolean(),
                        AdminInterface = c.Boolean(),
                        Configure = c.Boolean(),
                        Create = c.Boolean(),
                        Delete = c.Boolean(),
                        Edit = c.Boolean(),
                        EditState = c.Boolean(),
                        EditOwn = c.Boolean(),
                        Parent_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.MenuTypes", "Asset_ID", c => c.Int());
            AddColumn("dbo.MenuTypes", "menuType", c => c.String(nullable: false));
            AddColumn("dbo.Menus", "Asset_ID", c => c.Int());
            AddColumn("dbo.Languages", "Asset_ID", c => c.Int());
            AddColumn("dbo.Languages", "Access_ID", c => c.Int());
            AddColumn("dbo.Contents", "Asset_ID", c => c.Int());
            AddColumn("dbo.Categories", "Asset_ID", c => c.Int());
            DropForeignKey("dbo.Permissions", "ID", "dbo.Groups");
            DropIndex("dbo.Permissions", new[] { "ID" });
            DropTable("dbo.Permissions");
            CreateIndex("dbo.MenuTypes", "Asset_ID");
            CreateIndex("dbo.Menus", "Asset_ID");
            CreateIndex("dbo.Languages", "Asset_ID");
            CreateIndex("dbo.Languages", "Access_ID");
            CreateIndex("dbo.Contents", "Asset_ID");
            CreateIndex("dbo.Categories", "Asset_ID");
            CreateIndex("dbo.Assets", "Parent_ID");
            AddForeignKey("dbo.MenuTypes", "Asset_ID", "dbo.Assets", "ID");
            AddForeignKey("dbo.Menus", "Asset_ID", "dbo.Assets", "ID");
            AddForeignKey("dbo.Languages", "Asset_ID", "dbo.Assets", "ID");
            AddForeignKey("dbo.Languages", "Access_ID", "dbo.ViewLevels", "ID");
            AddForeignKey("dbo.Contents", "Asset_ID", "dbo.Assets", "ID");
            AddForeignKey("dbo.Categories", "Asset_ID", "dbo.Assets", "ID");
            AddForeignKey("dbo.Assets", "Parent_ID", "dbo.Assets", "ID");
        }
    }
}
