namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class I_dont_know_why_database_whant_update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contents", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.CustomFields", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Languages", "Code", c => c.String(nullable: false));
            AlterColumn("dbo.Languages", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Menus", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Menus", "Type", c => c.String(nullable: false));
            AlterColumn("dbo.MenuTypes", "menuType", c => c.String(nullable: false));
            AlterColumn("dbo.MenuTypes", "Title", c => c.String(nullable: false));
            DropColumn("dbo.CustomFields", "DataType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CustomFields", "DataType", c => c.String());
            AlterColumn("dbo.MenuTypes", "Title", c => c.String());
            AlterColumn("dbo.MenuTypes", "menuType", c => c.String());
            AlterColumn("dbo.Menus", "Type", c => c.String());
            AlterColumn("dbo.Menus", "Title", c => c.String());
            AlterColumn("dbo.Languages", "Title", c => c.String());
            AlterColumn("dbo.Languages", "Code", c => c.String());
            AlterColumn("dbo.CustomFields", "Title", c => c.String());
            AlterColumn("dbo.Contents", "Title", c => c.String());
        }
    }
}
