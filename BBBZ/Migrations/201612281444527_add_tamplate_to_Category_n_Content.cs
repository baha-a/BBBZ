namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_tamplate_to_Category_n_Content : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "Template", c => c.Int(nullable: false));
            AddColumn("dbo.Contents", "Template", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contents", "Template");
            DropColumn("dbo.Categories", "Template");
        }
    }
}
