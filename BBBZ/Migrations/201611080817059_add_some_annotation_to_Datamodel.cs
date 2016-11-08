namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_some_annotation_to_Datamodel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.News", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.News", "Text", c => c.String(nullable: false));
            AlterColumn("dbo.PublicDatas", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.PublicDatas", "Text", c => c.String(nullable: false));
            AlterColumn("dbo.PublicDatas", "Language", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PublicDatas", "Language", c => c.String());
            AlterColumn("dbo.PublicDatas", "Text", c => c.String());
            AlterColumn("dbo.PublicDatas", "Title", c => c.String());
            AlterColumn("dbo.News", "Text", c => c.String());
            AlterColumn("dbo.News", "Title", c => c.String());
        }
    }
}
