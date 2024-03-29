﻿using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace BBBZ.Models
{
    public class ApplicationUser: IdentityUser
    {
        public bool Locked { get; set; }
    }

    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ViewLevel> ViewLevels { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<MenuType> MenuTypes { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<CustomFieldValue> CustomFieldValues { get; set; }
        public DbSet<CustomField> CustomFields { get; set; }
        public DbSet<ContentVisitLog> ContentVisitLogs { get; set; }

        public DbSet<Request> Requests { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGroup>()
                .HasRequired(t => t.Groups)
                .WithMany(t => t.Users)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Permission>()
                .HasRequired(t => t.Group)
                .WithOptional(x=>x.Permission)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<CustomFieldValue>()
                .HasRequired(x => x.CustomField)
                .WithMany(x => x.Values)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<CustomFieldValue>()
                 .HasRequired(x => x.Content)
                 .WithMany(x => x.CustomFieldValues)
                 .WillCascadeOnDelete(true);


            modelBuilder.Entity<ContentVisitLog>()
                .HasRequired(x => x.Content)
                .WithMany(x => x.Log)
                .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}