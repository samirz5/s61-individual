using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models.Config
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.BirthDay)
                .HasColumnType("TIMESTAMP(0)");

            builder.Property(prop => prop.UserName)
                .IsRequired().HasMaxLength(12);

            builder.Property(prop => prop.Bio).HasMaxLength(160);
            builder.Property(prop => prop.FirstName).IsRequired().HasMaxLength(20);
            builder.Property(prop => prop.LastName).HasMaxLength(20);

        }
    }
}
