using Login_Service.Models;
using Login_Service.Models.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login_Service.Context
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
        : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfig());
        }

        public DbSet<User> User { get; set; }
    }
}
