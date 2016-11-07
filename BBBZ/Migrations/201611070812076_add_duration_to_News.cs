namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_duration_to_News : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "Duration", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "Duration");
        }
    }
}
