using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace BBBZ.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool Locked { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {        
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ViewLevel> ViewLevels { get; set; }
        public DbSet<Asset> Assets { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<MenuType>  MenuTypes { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Content> Contents { get; set; }

        public DbSet<CustomField> CustomFields { get; set; }


        public DbSet<News> News { get; set; }
        public DbSet<PublicData> PublicData { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder
            //    .Entity<Language>()
            //    .HasRequired(t => t.JohnsParentTable)
            //    .WithMany(t => t.JohnsChildTables)
            //    .HasForeignKey(d => d.JohnsParentTableId)
            //    .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}