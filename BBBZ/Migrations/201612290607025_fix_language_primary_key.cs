namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_language_primary_key : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Menus", "Language", c => c.String(maxLength: 128));
            AlterColumn("dbo.Categories", "Language", c => c.String(maxLength: 128));
            AlterColumn("dbo.Contents", "Language", c => c.String(maxLength: 128));
            AlterColumn("dbo.Languages", "Code", c => c.String(nullable: false, maxLength: 128));
            DropPrimaryKey("dbo.Languages");
            AddPrimaryKey("dbo.Languages", "Code");
            CreateIndex("dbo.Contents", "Language");
            CreateIndex("dbo.Categories", "Language");
            CreateIndex("dbo.Menus", "Language");
            AddForeignKey("dbo.Contents", "Language", "dbo.Languages", "Code");
            AddForeignKey("dbo.Categories", "Language", "dbo.Languages", "Code");
            AddForeignKey("dbo.Menus", "Language", "dbo.Languages", "Code");
            DropColumn("dbo.Categories", "MetaData");
            DropColumn("dbo.Contents", "MetaData");
            DropColumn("dbo.Languages", "ID");
            DropColumn("dbo.Languages", "MetaData");
            DropColumn("dbo.Menus", "Langauge");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Menus", "Langauge", c => c.String());
            AddColumn("dbo.Languages", "MetaData", c => c.String());
            AddColumn("dbo.Languages", "ID", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Contents", "MetaData", c => c.String());
            AddColumn("dbo.Categories", "MetaData", c => c.String());
            DropForeignKey("dbo.Menus", "Language", "dbo.Languages");
            DropForeignKey("dbo.Categories", "Language", "dbo.Languages");
            DropForeignKey("dbo.Contents", "Language", "dbo.Languages");
            DropIndex("dbo.Menus", new[] { "Language" });
            DropIndex("dbo.Categories", new[] { "Language" });
            DropIndex("dbo.Contents", new[] { "Language" });
            DropPrimaryKey("dbo.Languages");
            AddPrimaryKey("dbo.Languages", "ID");
            AlterColumn("dbo.Languages", "Code", c => c.String(nullable: false));
            AlterColumn("dbo.Contents", "Language", c => c.String());
            AlterColumn("dbo.Categories", "Language", c => c.String());
            DropColumn("dbo.Menus", "Language");
        }
    }
}
