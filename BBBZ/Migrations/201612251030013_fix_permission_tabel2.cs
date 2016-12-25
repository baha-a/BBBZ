namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_permission_tabel2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Permissions", "Media", c => c.Boolean());
            AddColumn("dbo.Permissions", "AdminPanel", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Permissions", "AdminPanel");
            DropColumn("dbo.Permissions", "Media");
        }
    }
}
