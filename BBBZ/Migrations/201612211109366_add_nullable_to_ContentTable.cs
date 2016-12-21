namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_nullable_to_ContentTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Menus", "CategoryID", c => c.Int());
            AlterColumn("dbo.Menus", "ContentID", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Menus", "ContentID", c => c.Int(nullable: false));
            AlterColumn("dbo.Menus", "CategoryID", c => c.Int(nullable: false));
        }
    }
}
