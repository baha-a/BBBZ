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
        public DbSet<Permission> Permissions { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<MenuType>  MenuTypes { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<CustomFieldValue> CustomFieldValues { get; set; }
        public DbSet<CustomField> CustomFields { get; set; }


        public DbSet<News> News { get; set; }
        public DbSet<PublicData> PublicData { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGroup>()
                .HasRequired(t => t.Groups)
                .WithMany()
                .WillCascadeOnDelete(true);

            
            modelBuilder.Entity<CustomFieldValue>()
                .HasRequired(x => x.CustomField)
                .WithMany()
                .WillCascadeOnDelete(true); 
            
            modelBuilder.Entity<CustomFieldValue>()
                 .HasRequired(x => x.Content)
                 .WithMany()
                 .WillCascadeOnDelete(true);


            base.OnModelCreating(modelBuilder);
        }
    }
}