using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweet_Service.Models.Config
{
    public class TweetConfig : IEntityTypeConfiguration<Tweet>
    {
        public void Configure(EntityTypeBuilder<Tweet> builder)
        {
            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.CreatedDate)
                .HasColumnType("TIMESTAMP(0)")
                .IsRequired();

            builder.Property(prop => prop.UserId)
                .IsRequired();

            builder.Property(prop => prop.UpdatedDate);
            builder.Property(prop => prop.HeartCount);
            builder.Property(prop => prop.Message);
          
        }
    }
}
