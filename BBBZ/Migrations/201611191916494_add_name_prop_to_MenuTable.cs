namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_name_prop_to_MenuTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Menus", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Menus", "Name");
        }
    }
}
