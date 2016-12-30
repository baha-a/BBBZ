namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_contentvisitlog : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserGroups", "Group_ID", "dbo.Groups");
            DropIndex("dbo.UserGroups", new[] { "Group_ID" });
            DropColumn("dbo.UserGroups", "Group_ID");

            DropForeignKey("dbo.CustomFieldValues", "Content_ID", "dbo.Contents");
            DropForeignKey("dbo.CustomFieldValues", "CustomField_ID1", "dbo.CustomFields");
            DropIndex("dbo.CustomFieldValues", new[] { "Content_ID" });
            DropIndex("dbo.CustomFieldValues", new[] { "CustomField_ID1" });

            RenameColumn(table: "dbo.CustomFieldValues", name: "CustomField_ID1", newName: "CustomField_ID");
            CreateTable(
                "dbo.ContentVisitLogs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Date = c.DateTime(nullable: false),
                        Content_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Contents", t => t.Content_ID, cascadeDelete: true)
                .Index(t => t.Content_ID);
            
            AlterColumn("dbo.CustomFieldValues", "CustomField_ID", c => c.Int(nullable: false));
            CreateIndex("dbo.CustomFieldValues", "Content_ID");
            CreateIndex("dbo.CustomFieldValues", "CustomField_ID");
            //AddForeignKey("dbo.UserGroups", "Groups_ID", "dbo.Groups", "ID", cascadeDelete : true);
            AddForeignKey("dbo.CustomFieldValues", "Content_ID", "dbo.Contents", "ID", cascadeDelete: true);
            AddForeignKey("dbo.CustomFieldValues", "CustomField_ID", "dbo.CustomFields", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {

        }
    }
}
