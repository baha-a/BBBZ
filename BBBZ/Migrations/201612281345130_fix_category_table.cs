namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_category_table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "Image", c => c.String());
            AlterColumn("dbo.Categories", "Title", c => c.String(nullable: false));
            DropColumn("dbo.Categories", "Url");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "Url", c => c.String());
            AlterColumn("dbo.Categories", "Title", c => c.String());
            DropColumn("dbo.Categories", "Image");
        }
    }
}
