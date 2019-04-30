using nwBlog.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nwBlog.DataAccessLayer.EntityFramework
{
    public class DatabaseContext : DbContext
    {
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }

        public DbSet<BlogImage> BlogImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<LastVisit> LastVisits { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Like> Likes { get; set; }

        public DbSet<SentMail> SentMails { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public DatabaseContext() : base("blogDb")
        {
            Database.SetInitializer(new MyIntializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }



    }
}
