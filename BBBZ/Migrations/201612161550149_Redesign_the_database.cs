namespace BBBZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Redesign_the_database : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Items", "Category_ID", "dbo.Categories");
            DropForeignKey("dbo.Resources", "Language_ID", "dbo.Languages");
            DropForeignKey("dbo.Items", "Language_ID", "dbo.Languages");
            DropForeignKey("dbo.MenuForRoles", "Menu_ID", "dbo.Menus");
            DropIndex("dbo.Items", new[] { "Category_ID" });
            DropIndex("dbo.Resources", new[] { "Language_ID" });
            DropIndex("dbo.Items", new[] { "Language_ID" });
            DropIndex("dbo.MenuForRoles", new[] { "Menu_ID" });
            CreateTable(
                "dbo.Assets",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SiteLogin = c.Boolean(),
                        AdminLogin = c.Boolean(),
                        SuperUser = c.Boolean(),
                        AdminInterface = c.Boolean(),
                        Configure = c.Boolean(),
                        Create = c.Boolean(),
                        Delete = c.Boolean(),
                        Edit = c.Boolean(),
                        EditState = c.Boolean(),
                        EditOwn = c.Boolean(),
                        Parent_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Assets", t => t.Parent_ID)
                .Index(t => t.Parent_ID);
            
            CreateTable(
                "dbo.ViewLevels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        username = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Contents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Alias = c.String(),
                        IntroText = c.String(),
                        FullText = c.String(),
                        CreatedTime = c.DateTime(nullable: false),
                        Descrption = c.String(),
                        MetaDesc = c.String(),
                        MetaKey = c.String(),
                        MetaData = c.String(),
                        CreatedByUsername = c.String(),
                        Published = c.Boolean(nullable: false),
                        Access_ID = c.Int(),
                        Asset_ID = c.Int(),
                        Language_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ViewLevels", t => t.Access_ID)
                .ForeignKey("dbo.Assets", t => t.Asset_ID)
                .ForeignKey("dbo.Languages", t => t.Language_ID)
                .Index(t => t.Access_ID)
                .Index(t => t.Asset_ID)
                .Index(t => t.Language_ID);
            
            CreateTable(
                "dbo.CustomFields",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(),
                        DataType = c.String(),
                        Content_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Contents", t => t.Content_ID)
                .Index(t => t.Content_ID);
            
            CreateTable(
                "dbo.MenuTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        menuType = c.String(),
                        Title = c.String(),
                        Description = c.String(),
                        Asset_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Assets", t => t.Asset_ID)
                .Index(t => t.Asset_ID);
            
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Position = c.String(),
                        Note = c.String(),
                        Asset_ID = c.Int(),
                        Langauge_ID = c.Int(),
                        Menu_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Assets", t => t.Asset_ID)
                .ForeignKey("dbo.Languages", t => t.Langauge_ID)
                .ForeignKey("dbo.Menus", t => t.Menu_ID)
                .Index(t => t.Asset_ID)
                .Index(t => t.Langauge_ID)
                .Index(t => t.Menu_ID);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Subject = c.String(),
                        Text = c.String(),
                        From_username = c.String(),
                        To_username = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        username = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Image = c.String(),
                        RegisterDate = c.DateTime(nullable: false),
                        LastVisitDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.username);
            
            CreateTable(
                "dbo.GroupViewLevels",
                c => new
                    {
                        Group_ID = c.Int(nullable: false),
                        ViewLevel_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Group_ID, t.ViewLevel_ID })
                .ForeignKey("dbo.Groups", t => t.Group_ID, cascadeDelete: true)
                .ForeignKey("dbo.ViewLevels", t => t.ViewLevel_ID, cascadeDelete: true)
                .Index(t => t.Group_ID)
                .Index(t => t.ViewLevel_ID);
            
            CreateTable(
                "dbo.PermissionGroups",
                c => new
                    {
                        Permission_ID = c.Int(nullable: false),
                        Group_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Permission_ID, t.Group_ID })
                .ForeignKey("dbo.Permissions", t => t.Permission_ID, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.Group_ID, cascadeDelete: true)
                .Index(t => t.Permission_ID)
                .Index(t => t.Group_ID);
            
            CreateTable(
                "dbo.UserGroupGroups",
                c => new
                    {
                        UserGroup_ID = c.Int(nullable: false),
                        Group_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserGroup_ID, t.Group_ID })
                .ForeignKey("dbo.UserGroups", t => t.UserGroup_ID, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.Group_ID, cascadeDelete: true)
                .Index(t => t.UserGroup_ID)
                .Index(t => t.Group_ID);
            
            CreateTable(
                "dbo.ContentCategories",
                c => new
                    {
                        Content_ID = c.Int(nullable: false),
                        Category_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Content_ID, t.Category_ID })
                .ForeignKey("dbo.Contents", t => t.Content_ID, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.Category_ID, cascadeDelete: true)
                .Index(t => t.Content_ID)
                .Index(t => t.Category_ID);
            
            AddColumn("dbo.Categories", "Title", c => c.String());
            AddColumn("dbo.Categories", "Alias", c => c.String());
            AddColumn("dbo.Categories", "Description", c => c.String());
            AddColumn("dbo.Categories", "MetaDesc", c => c.String());
            AddColumn("dbo.Categories", "MetaKey", c => c.String());
            AddColumn("dbo.Categories", "MetaData", c => c.String());
            AddColumn("dbo.Categories", "Language", c => c.String());
            AddColumn("dbo.Categories", "CreatedByUsername", c => c.String());
            AddColumn("dbo.Categories", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Categories", "Published", c => c.Boolean(nullable: false));
            AddColumn("dbo.Categories", "Access_ID", c => c.Int());
            AddColumn("dbo.Categories", "Asset_ID", c => c.Int());
            AddColumn("dbo.Languages", "Code", c => c.String());
            AddColumn("dbo.Languages", "Title", c => c.String());
            AddColumn("dbo.Languages", "Description", c => c.String());
            AddColumn("dbo.Languages", "MetaKey", c => c.String());
            AddColumn("dbo.Languages", "MetaDesc", c => c.String());
            AddColumn("dbo.Languages", "SiteName", c => c.String());
            AddColumn("dbo.Languages", "Access_ID", c => c.Int());
            AddColumn("dbo.Languages", "Asset_ID", c => c.Int());
            AddColumn("dbo.Menus", "Title", c => c.String());
            AddColumn("dbo.Menus", "Alias", c => c.String());
            AddColumn("dbo.Menus", "Note", c => c.String());
            AddColumn("dbo.Menus", "Link", c => c.String());
            AddColumn("dbo.Menus", "Type", c => c.String());
            AddColumn("dbo.Menus", "Published", c => c.Boolean(nullable: false));
            AddColumn("dbo.Menus", "Home", c => c.Boolean(nullable: false));
            AddColumn("dbo.Menus", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.Menus", "OpenInSameWindow", c => c.Boolean(nullable: false));
            AddColumn("dbo.Menus", "Langauge_ID", c => c.Int());
            AddColumn("dbo.Menus", "Parent_ID", c => c.Int());
            AddColumn("dbo.Menus", "MenuType_ID", c => c.Int());
            CreateIndex("dbo.Categories", "Access_ID");
            CreateIndex("dbo.Categories", "Asset_ID");
            CreateIndex("dbo.Languages", "Access_ID");
            CreateIndex("dbo.Languages", "Asset_ID");
            CreateIndex("dbo.Menus", "Langauge_ID");
            CreateIndex("dbo.Menus", "Parent_ID");
            CreateIndex("dbo.Menus", "MenuType_ID");
            AddForeignKey("dbo.Categories", "Access_ID", "dbo.ViewLevels", "ID");
            AddForeignKey("dbo.Categories", "Asset_ID", "dbo.Assets", "ID");
            AddForeignKey("dbo.Languages", "Access_ID", "dbo.ViewLevels", "ID");
            AddForeignKey("dbo.Languages", "Asset_ID", "dbo.Assets", "ID");
            AddForeignKey("dbo.Menus", "Langauge_ID", "dbo.Languages", "ID");
            AddForeignKey("dbo.Menus", "Parent_ID", "dbo.Menus", "ID");
            AddForeignKey("dbo.Menus", "MenuType_ID", "dbo.MenuTypes", "ID");
            DropColumn("dbo.Categories", "Key");
            DropColumn("dbo.Languages", "ISOName");
            DropColumn("dbo.Languages", "FullName");
            DropColumn("dbo.Menus", "Name");
            DropTable("dbo.Items");
            DropTable("dbo.Resources");
            DropTable("dbo.MenuForRoles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MenuForRoles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Role = c.String(),
                        Menu_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Resources",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.String(),
                        Language_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ImgURl = c.String(),
                        Descrption = c.String(),
                        ActivationTime = c.DateTime(nullable: false),
                        Creator = c.String(),
                        CreationTime = c.DateTime(nullable: false),
                        Category_ID = c.Int(),
                        Language_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Menus", "Name", c => c.String());
            AddColumn("dbo.Languages", "FullName", c => c.String());
            AddColumn("dbo.Languages", "ISOName", c => c.String());
            AddColumn("dbo.Categories", "Key", c => c.String());
            DropForeignKey("dbo.Modules", "Menu_ID", "dbo.Menus");
            DropForeignKey("dbo.Modules", "Langauge_ID", "dbo.Languages");
            DropForeignKey("dbo.Modules", "Asset_ID", "dbo.Assets");
            DropForeignKey("dbo.Menus", "MenuType_ID", "dbo.MenuTypes");
            DropForeignKey("dbo.MenuTypes", "Asset_ID", "dbo.Assets");
            DropForeignKey("dbo.Menus", "Parent_ID", "dbo.Menus");
            DropForeignKey("dbo.Menus", "Langauge_ID", "dbo.Languages");
            DropForeignKey("dbo.Contents", "Language_ID", "dbo.Languages");
            DropForeignKey("dbo.Languages", "Asset_ID", "dbo.Assets");
            DropForeignKey("dbo.Languages", "Access_ID", "dbo.ViewLevels");
            DropForeignKey("dbo.CustomFields", "Content_ID", "dbo.Contents");
            DropForeignKey("dbo.ContentCategories", "Category_ID", "dbo.Categories");
            DropForeignKey("dbo.ContentCategories", "Content_ID", "dbo.Contents");
            DropForeignKey("dbo.Contents", "Asset_ID", "dbo.Assets");
            DropForeignKey("dbo.Contents", "Access_ID", "dbo.ViewLevels");
            DropForeignKey("dbo.Categories", "Asset_ID", "dbo.Assets");
            DropForeignKey("dbo.Categories", "Access_ID", "dbo.ViewLevels");
            DropForeignKey("dbo.UserGroupGroups", "Group_ID", "dbo.Groups");
            DropForeignKey("dbo.UserGroupGroups", "UserGroup_ID", "dbo.UserGroups");
            DropForeignKey("dbo.PermissionGroups", "Group_ID", "dbo.Groups");
            DropForeignKey("dbo.PermissionGroups", "Permission_ID", "dbo.Permissions");
            DropForeignKey("dbo.GroupViewLevels", "ViewLevel_ID", "dbo.ViewLevels");
            DropForeignKey("dbo.GroupViewLevels", "Group_ID", "dbo.Groups");
            DropForeignKey("dbo.Assets", "Parent_ID", "dbo.Assets");
            DropIndex("dbo.Modules", new[] { "Menu_ID" });
            DropIndex("dbo.Modules", new[] { "Langauge_ID" });
            DropIndex("dbo.Modules", new[] { "Asset_ID" });
            DropIndex("dbo.Menus", new[] { "MenuType_ID" });
            DropIndex("dbo.MenuTypes", new[] { "Asset_ID" });
            DropIndex("dbo.Menus", new[] { "Parent_ID" });
            DropIndex("dbo.Menus", new[] { "Langauge_ID" });
            DropIndex("dbo.Contents", new[] { "Language_ID" });
            DropIndex("dbo.Languages", new[] { "Asset_ID" });
            DropIndex("dbo.Languages", new[] { "Access_ID" });
            DropIndex("dbo.CustomFields", new[] { "Content_ID" });
            DropIndex("dbo.ContentCategories", new[] { "Category_ID" });
            DropIndex("dbo.ContentCategories", new[] { "Content_ID" });
            DropIndex("dbo.Contents", new[] { "Asset_ID" });
            DropIndex("dbo.Contents", new[] { "Access_ID" });
            DropIndex("dbo.Categories", new[] { "Asset_ID" });
            DropIndex("dbo.Categories", new[] { "Access_ID" });
            DropIndex("dbo.UserGroupGroups", new[] { "Group_ID" });
            DropIndex("dbo.UserGroupGroups", new[] { "UserGroup_ID" });
            DropIndex("dbo.PermissionGroups", new[] { "Group_ID" });
            DropIndex("dbo.PermissionGroups", new[] { "Permission_ID" });
            DropIndex("dbo.GroupViewLevels", new[] { "ViewLevel_ID" });
            DropIndex("dbo.GroupViewLevels", new[] { "Group_ID" });
            DropIndex("dbo.Assets", new[] { "Parent_ID" });
            DropColumn("dbo.Menus", "MenuType_ID");
            DropColumn("dbo.Menus", "Parent_ID");
            DropColumn("dbo.Menus", "Langauge_ID");
            DropColumn("dbo.Menus", "OpenInSameWindow");
            DropColumn("dbo.Menus", "Order");
            DropColumn("dbo.Menus", "Home");
            DropColumn("dbo.Menus", "Published");
            DropColumn("dbo.Menus", "Type");
            DropColumn("dbo.Menus", "Link");
            DropColumn("dbo.Menus", "Note");
            DropColumn("dbo.Menus", "Alias");
            DropColumn("dbo.Menus", "Title");
            DropColumn("dbo.Languages", "Asset_ID");
            DropColumn("dbo.Languages", "Access_ID");
            DropColumn("dbo.Languages", "SiteName");
            DropColumn("dbo.Languages", "MetaDesc");
            DropColumn("dbo.Languages", "MetaKey");
            DropColumn("dbo.Languages", "Description");
            DropColumn("dbo.Languages", "Title");
            DropColumn("dbo.Languages", "Code");
            DropColumn("dbo.Categories", "Asset_ID");
            DropColumn("dbo.Categories", "Access_ID");
            DropColumn("dbo.Categories", "Published");
            DropColumn("dbo.Categories", "Date");
            DropColumn("dbo.Categories", "CreatedByUsername");
            DropColumn("dbo.Categories", "Language");
            DropColumn("dbo.Categories", "MetaData");
            DropColumn("dbo.Categories", "MetaKey");
            DropColumn("dbo.Categories", "MetaDesc");
            DropColumn("dbo.Categories", "Description");
            DropColumn("dbo.Categories", "Alias");
            DropColumn("dbo.Categories", "Title");
            DropTable("dbo.ContentCategories");
            DropTable("dbo.UserGroupGroups");
            DropTable("dbo.PermissionGroups");
            DropTable("dbo.GroupViewLevels");
            DropTable("dbo.Profiles");
            DropTable("dbo.Messages");
            DropTable("dbo.Modules");
            DropTable("dbo.MenuTypes");
            DropTable("dbo.CustomFields");
            DropTable("dbo.Contents");
            DropTable("dbo.UserGroups");
            DropTable("dbo.Permissions");
            DropTable("dbo.Groups");
            DropTable("dbo.ViewLevels");
            DropTable("dbo.Assets");
            CreateIndex("dbo.MenuForRoles", "Menu_ID");
            CreateIndex("dbo.Items", "Language_ID");
            CreateIndex("dbo.Resources", "Language_ID");
            CreateIndex("dbo.Items", "Category_ID");
            AddForeignKey("dbo.MenuForRoles", "Menu_ID", "dbo.Menus", "ID");
            AddForeignKey("dbo.Items", "Language_ID", "dbo.Languages", "ID");
            AddForeignKey("dbo.Resources", "Language_ID", "dbo.Languages", "ID");
            AddForeignKey("dbo.Items", "Category_ID", "dbo.Categories", "ID");
        }
    }
}
