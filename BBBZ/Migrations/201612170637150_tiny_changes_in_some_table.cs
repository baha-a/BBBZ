namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tiny_changes_in_some_table : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PermissionGroups", "Permission_ID", "dbo.Permissions");
            DropForeignKey("dbo.PermissionGroups", "Group_ID", "dbo.Groups");
            DropIndex("dbo.PermissionGroups", new[] { "Permission_ID" });
            DropIndex("dbo.PermissionGroups", new[] { "Group_ID" });
            AddColumn("dbo.Groups", "Parnet_ID", c => c.Int());
            AddColumn("dbo.Languages", "MetaData", c => c.String());
            CreateIndex("dbo.Groups", "Parnet_ID");
            AddForeignKey("dbo.Groups", "Parnet_ID", "dbo.Groups", "ID");
            DropTable("dbo.Permissions");
            DropTable("dbo.PermissionGroups");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PermissionGroups",
                c => new
                    {
                        Permission_ID = c.Int(nullable: false),
                        Group_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Permission_ID, t.Group_ID });
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.Groups", "Parnet_ID", "dbo.Groups");
            DropIndex("dbo.Groups", new[] { "Parnet_ID" });
            DropColumn("dbo.Languages", "MetaData");
            DropColumn("dbo.Groups", "Parnet_ID");
            CreateIndex("dbo.PermissionGroups", "Group_ID");
            CreateIndex("dbo.PermissionGroups", "Permission_ID");
            AddForeignKey("dbo.PermissionGroups", "Group_ID", "dbo.Groups", "ID", cascadeDelete: true);
            AddForeignKey("dbo.PermissionGroups", "Permission_ID", "dbo.Permissions", "ID", cascadeDelete: true);
        }
    }
}
