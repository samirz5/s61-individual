using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trend_Service.Models.Config
{
    public class TrendConfig : IEntityTypeConfiguration<Trend>
    {
        public void Configure(EntityTypeBuilder<Trend> builder)
        {
            builder.HasKey(prop => prop.Id);
            builder.Property(prop => prop.TweetId).IsRequired();
            builder.Property(prop => prop.Name).IsRequired();
            builder.Property(prop => prop.CreatedDate).IsRequired();
        }
    }
}
