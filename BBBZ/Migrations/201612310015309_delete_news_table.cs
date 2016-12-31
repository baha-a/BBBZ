namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delete_news_table : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Permissions", "Newss");
            DropTable("dbo.News");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.News",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Text = c.String(nullable: false),
                        Photo = c.String(),
                        Date = c.DateTime(nullable: false),
                        Duration = c.Int(nullable: false),
                        Language = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Permissions", "Newss", c => c.Boolean());
        }
    }
}
