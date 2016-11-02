namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_UsernameToTeacherAndStudent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "UserName", c => c.String());
            AddColumn("dbo.Teachers", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teachers", "UserName");
            DropColumn("dbo.Students", "UserName");
        }
    }
}
