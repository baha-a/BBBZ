namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_Typo_in_Table_Group : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Groups", "Parent_ID", "dbo.Groups");
            DropIndex("dbo.Groups", new[] { "Parent_ID" });
            RenameColumn(table: "dbo.Groups", name: "Parent_ID", newName: "Parent_ID");
            CreateIndex("dbo.Groups", "Parent_ID");
            AddForeignKey("dbo.Groups", "Parent_ID", "dbo.Groups", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Groups", "Parent_ID", "dbo.Groups");
            DropIndex("dbo.Groups", new[] { "Parent_ID" });
            RenameColumn(table: "dbo.Groups", name: "Parent_ID", newName: "Parent_ID");
            CreateIndex("dbo.Groups", "Parent_ID");
            AddForeignKey("dbo.Groups", "Parent_ID", "dbo.Groups", "ID");
        }
    }
}
