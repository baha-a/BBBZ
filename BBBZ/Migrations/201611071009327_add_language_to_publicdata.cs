namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_language_to_publicdata : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PublicDatas", "Language", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PublicDatas", "Language");
        }
    }
}
