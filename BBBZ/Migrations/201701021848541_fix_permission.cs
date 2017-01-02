namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_permission : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Permissions", "Questions");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Permissions", "Questions", c => c.Boolean());
        }
    }
}
