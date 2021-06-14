using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweet_Service.Models.Config
{
    public class MentionConfig : IEntityTypeConfiguration<Mention>
    {
        public void Configure(EntityTypeBuilder<Mention> builder)
        {
            builder.HasKey(prop => prop.Id);
            builder.Property(prop => prop.TweetId).IsRequired();
            builder.Property(prop => prop.UserName).IsRequired();

        }
    }
}
