using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trend_Service.Models;
using Trend_Service.Models.Config;

namespace Trend_Service.Context
{
    public class TrendServiceContext : DbContext
    {
        public TrendServiceContext(DbContextOptions<TrendServiceContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new TrendConfig());
        }

        public DbSet<Trend> Trend { get; set; }
    }
}
