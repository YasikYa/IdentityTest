using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using IdentityTest.Models;
using System.Data.Entity;
using IdentityTest.Entity;

namespace IdentityTest.Infrastructure
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext() : base("name=IdentityDb", false) { }

        //static AppIdentityDbContext()
        //{
        //    Database.SetInitializer<AppIdentityDbContext>(new IdentityDbInit());
        //}

        public static AppIdentityDbContext Create()
        {
            return new AppIdentityDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<File>().HasRequired(f => f.User).WithMany(u => u.Files).HasForeignKey(f => f.UserId);
        }

        public DbSet<File> Files { get; set; }
    }

    public class IdentityDbInit : DropCreateDatabaseIfModelChanges<AppIdentityDbContext>
    {
        protected override void Seed(AppIdentityDbContext context)
        {
            PerformInitialSetup(context);
        }

        public void PerformInitialSetup(AppIdentityDbContext context)
        {

        }
    }
}