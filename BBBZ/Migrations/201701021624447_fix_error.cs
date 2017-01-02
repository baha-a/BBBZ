namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_error : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Permissions", "ID", "dbo.Groups");
            DropIndex("dbo.Permissions", new[] { "ID" });
            CreateIndex("dbo.Permissions", "ID");
            AddForeignKey("dbo.Permissions", "ID", "dbo.Groups", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Permissions", "ID", "dbo.Groups");
            DropIndex("dbo.Permissions", new[] { "ID" });
            CreateIndex("dbo.Permissions", "ID");
            AddForeignKey("dbo.Permissions", "ID", "dbo.Groups", "ID");
        }
    }
}
