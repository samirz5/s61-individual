using Microsoft.EntityFrameworkCore;
using Tweet_Service.Models;
using Tweet_Service.Models.Config;

namespace Tweet_Service.Context
{
    public class TweetServiceContext: DbContext
    {
        public TweetServiceContext(DbContextOptions<TweetServiceContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new TweetConfig());
            builder.ApplyConfiguration(new MentionConfig());
        }
      
        public DbSet<Tweet> Tweet { get; set; }
        public DbSet<Mention> Mention { get; set; }

    }
}
