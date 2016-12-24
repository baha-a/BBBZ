namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_aliases : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Categories", "Alias");
            DropColumn("dbo.Contents", "Alias");
            DropColumn("dbo.Menus", "Alias");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Menus", "Alias", c => c.String());
            AddColumn("dbo.Contents", "Alias", c => c.String());
            AddColumn("dbo.Categories", "Alias", c => c.String());
        }
    }
}
