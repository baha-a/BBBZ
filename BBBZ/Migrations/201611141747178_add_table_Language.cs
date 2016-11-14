namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_table_Language : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FullName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Courses", "Language_ID", c => c.Int());
            CreateIndex("dbo.Courses", "Language_ID");
            AddForeignKey("dbo.Courses", "Language_ID", "dbo.Languages", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Courses", "Language_ID", "dbo.Languages");
            DropIndex("dbo.Courses", new[] { "Language_ID" });
            DropColumn("dbo.Courses", "Language_ID");
            DropTable("dbo.Languages");
        }
    }
}
