namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_errors : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserGroups", "Groups_ID", "dbo.Groups");
            DropForeignKey("dbo.UserGroups", "Groups_ID", "dbo.Groups");
            DropIndex("dbo.UserGroups", new[] { "Groups_ID" });
            DropIndex("dbo.UserGroups", new[] { "Groups_ID" });
            AddColumn("dbo.UserGroups", "Group_ID", c => c.Int());
            AlterColumn("dbo.UserGroups", "Groups_ID", c => c.Int(nullable: false));
            CreateIndex("dbo.UserGroups", "Groups_ID");
            CreateIndex("dbo.UserGroups", "Group_ID");
            AddForeignKey("dbo.UserGroups", "Groups_ID", "dbo.Groups", "ID", cascadeDelete: true);
            AddForeignKey("dbo.UserGroups", "Group_ID", "dbo.Groups", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserGroups", "Group_ID", "dbo.Groups");
            DropForeignKey("dbo.UserGroups", "Groups_ID", "dbo.Groups");
            DropIndex("dbo.UserGroups", new[] { "Group_ID" });
            DropIndex("dbo.UserGroups", new[] { "Groups_ID" });
            AlterColumn("dbo.UserGroups", "Groups_ID", c => c.Int());
            DropColumn("dbo.UserGroups", "Group_ID");
            CreateIndex("dbo.UserGroups", "Groups_ID");
            CreateIndex("dbo.UserGroups", "Groups_ID");
            AddForeignKey("dbo.UserGroups", "Groups_ID", "dbo.Groups", "ID");
            AddForeignKey("dbo.UserGroups", "Groups_ID", "dbo.Groups", "ID");
        }
    }
}
