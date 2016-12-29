namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class require_title_for_ViewLevel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ViewLevels", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Groups", "Title", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Groups", "Title", c => c.String());
            AlterColumn("dbo.ViewLevels", "Title", c => c.String());
        }
    }
}
