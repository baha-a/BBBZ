namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_permission_tabel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Permissions", "See_Categories", c => c.Boolean());
            AddColumn("dbo.Permissions", "Create_Categories", c => c.Boolean());
            AddColumn("dbo.Permissions", "Delete_Categories", c => c.Boolean());
            AddColumn("dbo.Permissions", "Edit_Categories", c => c.Boolean());
            AddColumn("dbo.Permissions", "See_Contents", c => c.Boolean());
            AddColumn("dbo.Permissions", "Create_Contents", c => c.Boolean());
            AddColumn("dbo.Permissions", "Delete_Contents", c => c.Boolean());
            AddColumn("dbo.Permissions", "Edit_Contents", c => c.Boolean());
            AlterColumn("dbo.Permissions", "Users", c => c.Boolean());
            AlterColumn("dbo.Permissions", "Groups", c => c.Boolean());
            AlterColumn("dbo.Permissions", "ViewLevels", c => c.Boolean());
            AlterColumn("dbo.Permissions", "Menus", c => c.Boolean());
            AlterColumn("dbo.Permissions", "Languages", c => c.Boolean());
            AlterColumn("dbo.Permissions", "Newss", c => c.Boolean());
            AlterColumn("dbo.Permissions", "Questions", c => c.Boolean());
            DropColumn("dbo.Permissions", "Categorys");
            DropColumn("dbo.Permissions", "Contents");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Permissions", "Contents", c => c.Byte());
            AddColumn("dbo.Permissions", "Categorys", c => c.Byte());
            AlterColumn("dbo.Permissions", "Questions", c => c.Byte());
            AlterColumn("dbo.Permissions", "Newss", c => c.Byte());
            AlterColumn("dbo.Permissions", "Languages", c => c.Byte());
            AlterColumn("dbo.Permissions", "Menus", c => c.Byte());
            AlterColumn("dbo.Permissions", "ViewLevels", c => c.Byte());
            AlterColumn("dbo.Permissions", "Groups", c => c.Byte());
            AlterColumn("dbo.Permissions", "Users", c => c.Byte());
            DropColumn("dbo.Permissions", "Edit_Contents");
            DropColumn("dbo.Permissions", "Delete_Contents");
            DropColumn("dbo.Permissions", "Create_Contents");
            DropColumn("dbo.Permissions", "See_Contents");
            DropColumn("dbo.Permissions", "Edit_Categories");
            DropColumn("dbo.Permissions", "Delete_Categories");
            DropColumn("dbo.Permissions", "Create_Categories");
            DropColumn("dbo.Permissions", "See_Categories");
        }
    }
}
