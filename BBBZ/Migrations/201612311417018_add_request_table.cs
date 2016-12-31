namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_request_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        AccessId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropTable("dbo.PublicDatas");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PublicDatas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Text = c.String(nullable: false),
                        Language = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropTable("dbo.Requests");
        }
    }
}
