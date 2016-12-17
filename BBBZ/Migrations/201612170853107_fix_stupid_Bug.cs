namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_stupid_Bug : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserGroupGroups", "UserGroup_ID", "dbo.UserGroups");
            DropForeignKey("dbo.UserGroupGroups", "Group_ID", "dbo.Groups");
            DropIndex("dbo.UserGroupGroups", new[] { "UserGroup_ID" });
            DropIndex("dbo.UserGroupGroups", new[] { "Group_ID" });
            DropTable("dbo.UserGroupGroups");

            AddColumn("dbo.UserGroups", "Groups_ID", c => c.Int());
            CreateIndex("dbo.UserGroups", "Groups_ID");
            AddForeignKey("dbo.UserGroups", "Groups_ID", "dbo.Groups", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserGroups", "Groups_ID", "dbo.Groups");
            DropIndex("dbo.UserGroups", new[] { "Groups_ID" });
            DropColumn("dbo.UserGroups", "Groups_ID");
            //CreateTable("UserGroupGroups");
            //CreateIndex("dbo.UserGroupGroups", "Group_ID");
            //CreateIndex("dbo.UserGroupGroups", "UserGroup_ID");
            //AddForeignKey("dbo.UserGroupGroups", "Group_ID", "dbo.Groups", "ID", cascadeDelete: true);
            //AddForeignKey("dbo.UserGroupGroups", "UserGroup_ID", "dbo.UserGroups", "ID", cascadeDelete: true);
        }
    }
}
