namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_message_attachment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "Attachment", c => c.String());
            AlterColumn("dbo.Messages", "To_username", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Messages", "To_username", c => c.String());
            DropColumn("dbo.Messages", "Attachment");
        }
    }
}
