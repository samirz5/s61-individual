using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;
using UserService.Models.Config;

namespace UserService.Context
{
    public class UserContext: DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(p => new { p.UserName })
                .IsUnique(true);
            modelBuilder.ApplyConfiguration(new UserConfig());
        }

        /*protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.ApplyConfiguration(new TweetConfig());
        }*/

        public DbSet<User> User { get; set; }

    }
}
