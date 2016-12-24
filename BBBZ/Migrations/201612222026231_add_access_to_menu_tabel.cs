namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_access_to_menu_tabel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Menus", "Access_ID", c => c.Int());
            AddColumn("dbo.Menus", "Asset_ID", c => c.Int());
            CreateIndex("dbo.Menus", "Access_ID");
            CreateIndex("dbo.Menus", "Asset_ID");
            AddForeignKey("dbo.Menus", "Access_ID", "dbo.ViewLevels", "ID");
            AddForeignKey("dbo.Menus", "Asset_ID", "dbo.Assets", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Menus", "Asset_ID", "dbo.Assets");
            DropForeignKey("dbo.Menus", "Access_ID", "dbo.ViewLevels");
            DropIndex("dbo.Menus", new[] { "Asset_ID" });
            DropIndex("dbo.Menus", new[] { "Access_ID" });
            DropColumn("dbo.Menus", "Asset_ID");
            DropColumn("dbo.Menus", "Access_ID");
        }
    }
}
