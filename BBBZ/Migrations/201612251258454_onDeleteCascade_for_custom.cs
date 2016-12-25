namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class onDeleteCascade_for_custom : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CustomFieldValues", "Content_ID", "dbo.Contents");
            DropForeignKey("dbo.CustomFieldValues", "Content_ID", "dbo.Contents");
            DropForeignKey("dbo.CustomFieldValues", "CustomField_ID", "dbo.CustomFields");
            DropIndex("dbo.CustomFieldValues", new[] { "Content_ID" });
            DropIndex("dbo.CustomFieldValues", new[] { "Content_ID" });
            DropIndex("dbo.CustomFieldValues", new[] { "CustomField_ID" });
            AddColumn("dbo.CustomFieldValues", "CustomField_ID1", c => c.Int(nullable: false));
            AddColumn("dbo.CustomFieldValues", "Content_ID1", c => c.Int());
            AlterColumn("dbo.CustomFieldValues", "Content_ID", c => c.Int(nullable: false));
            CreateIndex("dbo.CustomFieldValues", "Content_ID");
            CreateIndex("dbo.CustomFieldValues", "Content_ID1");
            CreateIndex("dbo.CustomFieldValues", "CustomField_ID1");
            AddForeignKey("dbo.CustomFieldValues", "Content_ID", "dbo.Contents", "ID", cascadeDelete: true);
            AddForeignKey("dbo.CustomFieldValues", "Content_ID1", "dbo.Contents", "ID");
            AddForeignKey("dbo.CustomFieldValues", "CustomField_ID1", "dbo.CustomFields", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomFieldValues", "CustomField_ID1", "dbo.CustomFields");
            DropForeignKey("dbo.CustomFieldValues", "Content_ID1", "dbo.Contents");
            DropForeignKey("dbo.CustomFieldValues", "Content_ID", "dbo.Contents");
            DropIndex("dbo.CustomFieldValues", new[] { "CustomField_ID1" });
            DropIndex("dbo.CustomFieldValues", new[] { "Content_ID1" });
            DropIndex("dbo.CustomFieldValues", new[] { "Content_ID" });
            AlterColumn("dbo.CustomFieldValues", "Content_ID", c => c.Int());
            DropColumn("dbo.CustomFieldValues", "Content_ID1");
            DropColumn("dbo.CustomFieldValues", "CustomField_ID1");
            CreateIndex("dbo.CustomFieldValues", "CustomField_ID");
            CreateIndex("dbo.CustomFieldValues", "Content_ID");
            CreateIndex("dbo.CustomFieldValues", "Content_ID");
            AddForeignKey("dbo.CustomFieldValues", "CustomField_ID", "dbo.CustomFields", "ID");
            AddForeignKey("dbo.CustomFieldValues", "Content_ID", "dbo.Contents", "ID");
            AddForeignKey("dbo.CustomFieldValues", "Content_ID", "dbo.Contents", "ID");
        }
    }
}
